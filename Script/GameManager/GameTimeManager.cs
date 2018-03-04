using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Chronos;

namespace RedChild
{
    public class GameTimeManager : MonoBehaviour
    {
        [SerializeField]
        private IntReactiveProperty m_countDownTime = new IntReactiveProperty(5);

        [SerializeField]
        private IntReactiveProperty m_remainingTime = new IntReactiveProperty(120);

        public IReadOnlyReactiveProperty<int> CountDownTime
        {
            get { return m_countDownTime; }
        }

        public IReadOnlyReactiveProperty<int> RemainingTime
        {
            get { return m_remainingTime; }
        }

        private GlobalClock m_globalClock;
        public GlobalClock m_GlobalClock
        {
            get { return m_globalClock; }
        }

        private Timeline m_timeline;
        public Timeline m_Timeline
        {
            get { return m_timeline; }
        }

        private void Awake()
        {
            m_timeline = GetComponent<Timeline>();
            m_globalClock = GetComponentInChildren<GlobalClock>();
        }

        public void StartCountDown()
        {
            StartCoroutine(CountDown());
        }

        private IEnumerator CountDown()
        {
            yield return new WaitForSeconds(.5f);

            m_countDownTime.SetValueAndForceNotify(m_countDownTime.Value);

            yield return new WaitForSeconds(1f);
            while (m_countDownTime.Value > 0)
            {
                m_countDownTime.Value -= 1;
                yield return new WaitForSeconds(1f);
            }

        }

        public void StartBattleTime()
        {
            StartCoroutine(BattleTime());
        }

        private IEnumerator BattleTime()
        {
            while (m_remainingTime.Value > 0)
            {
                yield return m_timeline.WaitForSeconds(1f);
                m_remainingTime.Value--;
            }
        }

        public void TimeSlow()
        {
            m_globalClock.localTimeScale = 0.1f;
        }

        public void TimePause()
        {
            m_globalClock.paused = true;
        }

        public void TimeReset()
        {
            m_globalClock.localTimeScale = 1f;
            m_globalClock.paused = false;
        }
    }
}
