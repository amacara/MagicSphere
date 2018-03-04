using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Chronos;
using UnityEngine.PostProcessing;

namespace RedChild
{
    public class PlayerTime : BasePlayerComponent
    {
        public enum TimeType
        {
            Disable = 0, Enable, Pausing
        }

        [SerializeField] private GlobalClock m_globalClock;
        [SerializeField] private GameObject m_timeSphere;

        [SerializeField] private GameObject m_timeKeeperEff;

        [SerializeField] private PostProcessingBehaviour m_postProcessingBehaviour;

        private TimeType m_timeType = TimeType.Disable;
        private Vector3 m_tkBaseScale;

        readonly private float STOP_TIME = 12f;
        readonly private float RECAST_TIME = 10f;

        protected override void OnStart()
        {
            m_timeSphere.SetActive(false);
            m_timeKeeperEff.SetActive(false);
            m_tkBaseScale = m_timeKeeperEff.transform.localScale;
            m_postProcessingBehaviour.enabled = false;

            m_core.m_isTorus.Where(_ => _).Subscribe(_ => TimeSphereActivate());
            m_core.m_isEnd.Where(_ => _).Subscribe(_ => EndProcess());

            this.UpdateAsObservable().Where(_ => OVRInput.GetDown(OVRInput.RawButton.X)).
                Subscribe(_ => TimeControll());
        }

        private void TimeSphereActivate()
        {
            m_timeType = TimeType.Enable;
            m_timeSphere.SetActive(true);
        }

        private void EndProcess()
        {
            m_timeSphere.SetActive(false);
            m_timeKeeperEff.SetActive(false);
        }

        private void TimeControll()
        {
            if (m_timeType != TimeType.Enable || m_core.m_isEnd.Value) return;
            StartCoroutine(TimePausing());
        }

        private IEnumerator TimePausing()
        {
            AudioManager.Instance.SEPlay(AudioManager.Instance.m_seTimeStart);
            m_timeSphere.SetActive(false);
            m_timeKeeperEff.SetActive(true);
            m_timeType = TimeType.Pausing;
            m_globalClock.paused = true;
            m_postProcessingBehaviour.enabled = true;

            var scale2 = new Vector2(m_tkBaseScale.x, m_tkBaseScale.y);
            m_timeKeeperEff.transform.localScale = new Vector3( scale2.x, scale2.y, m_tkBaseScale.z);
            yield return new WaitForSeconds(.5f);

            float timeCount = 0f;
            while (timeCount < STOP_TIME)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.X)) break;

                var sc = Mathf.Lerp(scale2.x, 0.01f, timeCount / 20f);
                m_timeKeeperEff.transform.localScale = new Vector3(sc, sc, m_tkBaseScale.z);

                timeCount += Time.deltaTime;
                yield return null;
            }

            AudioManager.Instance.SEPlay(AudioManager.Instance.m_seTimeEnd);
            m_timeKeeperEff.SetActive(false);
            m_globalClock.paused = false;
            m_globalClock.localTimeScale = 1f;
            m_timeType = TimeType.Disable;
            m_postProcessingBehaviour.enabled = false;

            timeCount = 0f;
            while (timeCount < RECAST_TIME)
            {
                timeCount += Time.deltaTime;
                yield return null;
            }

            if (!m_core.m_isEnd.Value) TimeSphereActivate();
        }
    }
}
