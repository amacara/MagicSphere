using System;
using UnityEngine;

namespace RedChild
{
    public class TeleportCore : SingletonMonoBehaviour<TeleportCore>
    {
        public event Action OnTeleportEnable;
        public event Action OnTeleportDisable;

        public Vector3 m_targetPos;
        public GameObject m_target;

        public void TeleportEnable() { if (OnTeleportEnable != null) OnTeleportEnable(); }
        public void TeleportDisable() { if (OnTeleportDisable != null) OnTeleportDisable(); }

        public void SetCollider(GameObject collider)
        {
            m_target = collider;
        }

        public void ChangeCollider(GameObject collider)
        {
            if (m_target != null) m_target.SetActive(true);
            m_target = collider;
            m_target.SetActive(false);
        }
    }
}
