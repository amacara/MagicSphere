using UnityEngine;
using DG.Tweening;

namespace RedChild
{
    public class WorldEffect : SingletonMonoBehaviour<WorldEffect>
    {
        [SerializeField] private GameObject m_mainEffect;

        readonly private Vector3 BASE_SCALE = Vector3.one * 30f;

        protected override void Init()
        {
            m_mainEffect.SetActive(false);
            m_mainEffect.transform.DOScale(BASE_SCALE, Time.deltaTime);
        }

        public void MainEffectActivate()
        {
            m_mainEffect.SetActive(true);
        }
    }
}
