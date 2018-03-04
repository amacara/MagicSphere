using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace RedChild
{
    public class EnemyMover : BaseEnemyComponent
    {
        [SerializeField] private Transform m_rcTarget;
        [SerializeField] private Transform m_baseTarget;

        readonly private float APPEAR_TIME = 5f;
        readonly private float MOVE_TIME = 2f;
        readonly private Vector3 BASE_POS = new Vector3(0f, 0f, 23.5f);
        readonly private Vector3 BASE_SCALE = Vector3.one * 2f;

        protected override void OnStart()
        {
            m_core.m_isStart.Where(_ => _).Subscribe(_ => EnemyAppear()).AddTo(gameObject);

            m_core.m_move.Where(_ => _ != Vector3.zero).Subscribe(_ => EnemyMove(_)).AddTo(gameObject);

            m_core.m_isEnd.Where(_ => _).Subscribe(_ => EnemyMove(BASE_POS)).AddTo(gameObject);

            this.UpdateAsObservable().Where(_ => m_core.m_IsUpdate).Subscribe(_ => 
            {
                m_rcTarget.position = m_baseTarget.position;
                this.transform.DOLookAt(m_core.m_Player.transform.position, Time.deltaTime, AxisConstraint.Y);
            });
        }

        private void EnemyAppear()
        {
            this.transform.DOMove(BASE_POS, APPEAR_TIME).SetEase(Ease.InOutBack);
            this.transform.DOScale(BASE_SCALE, APPEAR_TIME).SetEase(Ease.InCubic);
        }

        private void EnemyMove(Vector3 vec)
        {
            this.transform.DOMove(vec, MOVE_TIME);
        }
    }
}
