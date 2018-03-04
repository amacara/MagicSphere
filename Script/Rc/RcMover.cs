using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System.Linq;

namespace RedChild
{
    public class RcMover : BaseRcComponent
    {
        [System.Serializable]
        public struct VelocityData
        {
            public Vector3 vec;
            public float mag;

        }
        private VelocityData[] m_velos = new VelocityData[10];
        private Rigidbody m_rigid;

        private Tween m_tweenPath;

        protected override void OnStart()
        {
            m_rigid = GetComponent<Rigidbody>();

            this.UpdateAsObservable().Where(_ => m_core.m_grabbed.Value).Subscribe(_ =>
            {
                VelocityUpdate();
            }).AddTo(this.gameObject);

            m_core.m_released.Where(_ => _).Subscribe(_ => 
            {
                StartCoroutine(RcMoveAction());
            }).AddTo(this.gameObject);

            m_core.m_attackAction.Where(_ => _).Subscribe(_ =>
             {
                 if (m_tweenPath != null) m_tweenPath.Kill();
             }).AddTo(this.gameObject);
        }

        void VelocityUpdate()
        {

            for (int i = m_velos.Length - 2; i >= 0; i--)
            {
                m_velos[i + 1].vec = m_velos[i].vec;
                m_velos[i + 1].mag = m_velos[i].mag;
            }
            m_velos[0].vec = m_rigid.velocity;
            m_velos[0].mag = m_rigid.velocity.sqrMagnitude;

        }

        public IEnumerator RcMoveAction()
        {
            if (m_core.m_TimeManager.m_GlobalClock.paused)
            {
                yield return m_core.m_TimeManager.m_Timeline.WaitForSeconds(Random.value * .5f);
            }
            yield return null;

            AudioManager.Instance.SEPlay(AudioManager.Instance.m_seRcStart);
            float moveTime = 2f;

            var dots = m_velos.Where(x => Vector3.Dot(x.vec.normalized, m_velos[9].vec.normalized) >= 0f).OrderByDescending(x => x.mag).Select(x => x);
            Debug.Log(dots.Count());
            if (dots.Count() > 0)
            {
                var velo = dots.First();
                var halfDist = Mathf.Abs(Vector3.Distance(m_core.m_Target.transform.position, this.transform.position)) / 2f;
                var halfPos = this.transform.position + velo.vec.normalized * halfDist;

                moveTime -= velo.mag / halfDist / 10f;
                if (moveTime <= .5f) moveTime = Mathf.Abs(moveTime) + Random.value;
                m_tweenPath = this.transform.DOLocalPath(new Vector3[] { halfPos, m_core.m_Target.transform.position }, moveTime, PathType.CatmullRom).SetEase(Ease.OutQuad);
            } else
            {
                moveTime -= Random.value * 0.2f;
                m_tweenPath = this.transform.DOLocalPath(new Vector3[] { m_core.m_Target.transform.position }, moveTime, PathType.CatmullRom).SetEase(Ease.OutCubic);
            }

            yield return m_core.m_TimeManager.m_Timeline.WaitForSeconds(moveTime);
            m_core.m_attackAction.Value = true;
            Destroy(this.gameObject, 3f);
        }
    }
}
