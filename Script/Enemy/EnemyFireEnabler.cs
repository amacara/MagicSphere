using UnityEngine;

namespace RedChild
{
    public class EnemyFireEnabler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GetComponentInChildren<Transform>().gameObject.SetActive(false);
        }
    }
}
