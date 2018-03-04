using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace RedChild
{
    public class RcAttack : BaseRcComponent
    {
        private bool m_isAttacked = false;

        private float m_effectScale = 20f;

        readonly private string TAG_ENEMY = "Enemy";
        readonly private string TAG_FIRE = "EnemyFire";

        protected override void OnStart()
        {
            m_core.m_attackAction.Where(_ => _).Subscribe(_ => { if (!m_isAttacked) StartCoroutine(OnAttack()); }).AddTo(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_core.m_released.Value || m_isAttacked) return;
            if (other.gameObject.CompareTag(TAG_ENEMY)) StartCoroutine(OnAttack());
            if (other.gameObject.CompareTag(TAG_FIRE))
            {
                m_effectScale = Random.Range(5f, 8f);
                m_core.m_attackAction.Value = true;
            }
        }

        private IEnumerator OnAttack()
        {
            m_isAttacked = true;
            SeStart();
            Destroy(GetComponent<Rigidbody>());
            yield return m_core.m_TimeManager.m_Timeline.WaitForSeconds(.05f);

            foreach (var p in m_core.m_Particles)
            {
                var main = p.main;
                main.loop = false;
                p.Stop();
            }

            GetComponent<MeshRenderer>().enabled = false;
            this.transform.localScale = Vector3.one * m_effectScale;

            foreach (var p in m_core.m_Particles)
            {
                p.Play();
            }
        }

        private void SeStart()
        {
            string seName = "";
            switch (m_core.m_RcType)
            {
                case RcList.solar:
                    seName = AudioManager.Instance.m_seRc1;
                    break;
                case RcList.thundershock:
                    seName = AudioManager.Instance.m_seRc2;
                    break;
                case RcList.megaflare2:
                    seName = AudioManager.Instance.m_seRc3;
                    break;
                case RcList.fantasticExplosion:
                    seName = AudioManager.Instance.m_seRc4;
                    break;
                case RcList.ultima2:
                    seName = AudioManager.Instance.m_seRc5;
                    break;
                case RcList.ritual:
                    seName = AudioManager.Instance.m_seRc6;
                    break;
                case RcList.twister:
                    seName = AudioManager.Instance.m_seRc7;
                    break;
                case RcList.atomic:
                    seName = AudioManager.Instance.m_seRc8;
                    break;
            }
            if (seName != "") AudioManager.Instance.SEPlay(seName);
        }
    }
}
