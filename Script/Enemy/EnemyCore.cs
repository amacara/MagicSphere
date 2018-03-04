using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Chronos;

namespace RedChild
{
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField] private PlayerCore m_playerCore;
        [SerializeField] private GameObject m_player;

        public BoolReactiveProperty m_isStart = new BoolReactiveProperty(false);
        public BoolReactiveProperty m_isEnd = new BoolReactiveProperty(false);
        public FloatReactiveProperty m_damge = new FloatReactiveProperty(0f);
        public FloatReactiveProperty m_attack = new FloatReactiveProperty(0f);
        public Vector3ReactiveProperty m_move = new Vector3ReactiveProperty(Vector3.zero);

        public PlayerCore m_PlayerCore { get { return m_playerCore; } }
        public GameObject m_Player { get { return m_player; } }
        public Timeline m_timeline { get; set; }
        public bool m_IsUpdate { get; set; }

        private Coroutine m_coroutine;

        private void Start()
        {
            this.transform.localScale = Vector3.zero;
            m_timeline = GetComponent<Timeline>();

            m_isStart.Where(_ => _).Subscribe(_ => m_coroutine = StartCoroutine(EnemyBehaviour())).AddTo(gameObject);
            m_isEnd.Where(_ => _).Subscribe(_ => StopCoroutine(m_coroutine)).AddTo(gameObject);
        }

        private IEnumerator EnemyBehaviour()
        {
            m_IsUpdate = true;
            yield return m_timeline.WaitForSeconds(7f);

            float timeInterval = 5f;
            float ifBehaviour = .7f;

            while (true)
            {
                if (Random.value > ifBehaviour)
                {
                    m_attack.Value = Random.Range(1f, 4f);
                } else
                {
                    var vec = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 5f), Random.Range(15f, 25f));
                    m_move.Value = m_Player.transform.position + vec;
                }
                yield return m_timeline.WaitForSeconds(timeInterval);
                timeInterval -= .3f * Random.value;
                if (timeInterval <= 2f) timeInterval = 2f;
                ifBehaviour -= .08f * Random.value;
                if (ifBehaviour <= .4f) ifBehaviour = .4f;
            }
        }
    }
}
