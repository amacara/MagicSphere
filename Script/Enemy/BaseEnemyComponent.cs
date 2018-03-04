using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedChild
{
    public abstract class BaseEnemyComponent : MonoBehaviour
    {

        protected EnemyCore m_core;

        private void Start()
        {
            m_core = GetComponent<EnemyCore>();
            OnStart();
        }

        protected virtual void OnStart()
        {

        }
    }
}
