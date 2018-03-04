using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace RedChild
{
    public class EnemyHealth : BaseEnemyComponent
    {
        [SerializeField] private float m_hp = 300f;
        [SerializeField] private Image m_hpImage;

        private GameObject m_canvas;
        private float m_baseHp;

        protected override void OnStart()
        {
            m_canvas = m_hpImage.transform.parent.gameObject;
            m_canvas.SetActive(false);

            m_hpImage.fillAmount = 1f;
            m_baseHp = m_hp;

            m_core.m_isStart.Where(_ => _).Subscribe(_ => DOVirtual.DelayedCall(5f, () => m_canvas.SetActive(true))).AddTo(gameObject);

            m_core.m_damge.Where(_ => _ > 0f).Subscribe(_ =>
            {
                GetDamage(_);
            }).AddTo(gameObject);

        }

        private void GetDamage(float damage)
        {
            m_hp -= damage;
            m_hpImage.fillAmount = m_hp / m_baseHp;
        }
    }
}
