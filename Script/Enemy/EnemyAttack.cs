using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace RedChild
{
    public class EnemyAttack : BaseEnemyComponent
    {
        [SerializeField] private GameObject m_appearPoint;
        [SerializeField] private GameObject m_target;

        readonly private string m_prefab = "Enemy/FireBall";

        protected override void OnStart()
        {
            m_core.m_attack.Where(_ => _ > 0f).Subscribe(_ => {
                for (int i = 0; i < (int)_; i++)
                {
                    var delay = i + Random.Range( .2f, .8f);
                    DOVirtual.DelayedCall(delay, () => AttackFireBall());
                }
            });
        }

        private void AttackFireBall()
        {
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(m_prefab), m_appearPoint.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.parent = null;
            AudioManager.Instance.SEPlay(AudioManager.Instance.m_seMonsterVoice);
            StartCoroutine(MovingFireBall(obj, m_target.transform.position + GetRandomVec()));
        }

        private IEnumerator MovingFireBall(GameObject obj, Vector3 pos)
        {
            float count = 0f;
            float maxCount = Random.Range(2.5f, 4f);
            Vector3 basePos = obj.transform.position;
            while (count < maxCount)
            {
                count += m_core.m_timeline.deltaTime * 2f;
                var vec = Vector3.Lerp(basePos, pos, Mathf.Clamp01( count / maxCount + .1f));
                obj.transform.DOMove(vec, m_core.m_timeline.deltaTime);
                yield return new WaitForSeconds(m_core.m_timeline.deltaTime);
            }
            Destroy(obj);
        }

        private Vector3 GetRandomVec()
        {
            if (Random.value >= 0.6f) return Vector3.zero;
            else return new Vector3(Random.value, Random.value, Random.value);
        }
    }
}
