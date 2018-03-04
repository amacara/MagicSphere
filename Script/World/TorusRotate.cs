using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace RedChild
{
    public class TorusRotate : MonoBehaviour
    {
        [SerializeField] private float m_angle = .3f;

        Coroutine m_coroutine;

        private void Start()
        {
            m_coroutine = StartCoroutine(RotateY());
        }

        private IEnumerator RotateY()
        {
            while (true)
            {
                yield return this.transform.DOLocalRotate(new Vector3(0f, m_angle, 0f), Time.deltaTime).SetRelative().WaitForCompletion();
            }
        }

        public void StopRotate()
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
        }
    }
}
