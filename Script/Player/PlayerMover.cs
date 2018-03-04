using UnityEngine;
using DG.Tweening;

namespace RedChild
{
    public class PlayerMover : BasePlayerComponent
    {
        [SerializeField] private Transform m_moveTarget;
        [SerializeField] private float m_teleportTime;

        protected override void OnStart()
        {
            base.OnStart();
        }

        public void DashTeleport()
        {
            DOVirtual.DelayedCall(.1f, () => m_moveTarget.DOMove(TeleportCore.Instance.m_targetPos, m_teleportTime));
        }
    }
}
