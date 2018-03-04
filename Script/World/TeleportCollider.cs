using UnityEngine;
using DG.Tweening;

namespace RedChild
{
    public class TeleportCollider : MonoBehaviour
    {
        [SerializeField] private bool m_isFirst = false;

        private GameObject m_collider;

        private void Awake()
        {
            m_collider = GetComponent<CapsuleCollider>().gameObject;
            m_collider.SetActive(false);

            if (m_isFirst)
            {
                TeleportCore.Instance.SetCollider(m_collider);
            } else
            {
                DOVirtual.DelayedCall(0.1f, () => m_collider.SetActive(true));
            }
            TeleportCore.Instance.OnTeleportEnable += TeleportEnable;
            TeleportCore.Instance.OnTeleportDisable += TeleportDisable;
        }

        private void TeleportEnable()
        {
            if (m_isFirst) return;
            var parent = m_collider.transform.parent;
            parent.gameObject.SetActive(true);
            parent.DOScale(new Vector3(1.5f, 1f, 1.5f), 1f);
        }

        private void TeleportDisable()
        {
            if (m_isFirst) return;
            var parent = m_collider.transform.parent;
            parent.gameObject.SetActive(false);
            parent.localScale = Vector3.zero;
        }

        public void SetTargetPos()
        {
            TeleportCore.Instance.m_targetPos = this.transform.position;
            TeleportCore.Instance.ChangeCollider(m_collider);
        }
    }
}
