using UnityEngine;

namespace RedChild
{
    public abstract class BaseRcComponent : MonoBehaviour
    {

        protected RcCore m_core;

        private void Start()
        {
            m_core = GetComponent<RcCore>();
            OnStart();
        }

        protected virtual void OnStart()
        {

        }
    }
}
