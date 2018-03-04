using UnityEngine;

namespace RedChild
{
    public class EnemyCoreProvider : MonoBehaviour
    {
        [SerializeField] private EnemyCore m_core;

        readonly private string m_tag = "Rc";

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(m_tag)) m_core.m_damge.Value = Random.Range(1f, 3f);
        }
    }
}
