using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace RedChild
{
    public class PlayerJudge : BasePlayerComponent
    {
        [SerializeField] private GameObject m_judgeParent;
        [SerializeField] private GameObject m_judgeUltima;
        [SerializeField] private GameObject m_judgeBow;
        [SerializeField] private GameObject m_judgeBlue;
        [SerializeField] private GameObject m_judgeArrow;
        [SerializeField] private GameObject m_judgeNucleus;

        [SerializeField] private GameObject m_target;

        [SerializeField] private GameObject m_tails;
        [SerializeField] private GameObject m_ex;
        [SerializeField] private GameObject m_lazer;

        private bool m_isGrabbed = false;

        protected override void OnStart()
        {
            m_judgeParent.SetActive(false);
            m_judgeUltima.SetActive(false);
            m_judgeBow.SetActive(false);
            m_judgeBlue.SetActive(false);
            m_judgeArrow.SetActive(false);
            m_judgeNucleus.SetActive(false);

            m_tails.SetActive(false);
            m_ex.SetActive(false);
            m_lazer.SetActive(false);

            m_core.m_isJudge.Where(_ => _ == 2).Subscribe(_ => JudgeParentActivate());

            m_core.m_isJudge.Where(_ => _ == 4).Subscribe(_ => StartCoroutine(LastJudge()));
        }

        private void JudgeParentActivate()
        {
            m_judgeParent.SetActive(true);
            StartCoroutine(ScaleUp(m_judgeUltima, 1.5f));
        }

        public void JudgeBowGrabbed()
        {
            if (m_isGrabbed) return;
            StartCoroutine(JudgeArrowSetup());
        }

        private IEnumerator JudgeArrowSetup()
        {
            m_isGrabbed = true;
            m_judgeParent.transform.parent = null;
            StartCoroutine(ScaleUp(m_judgeBow, 1.5f));
            yield return StartCoroutine(ScaleUp(m_judgeBlue, 3f));

            yield return new WaitForSeconds(.5f);

            StartCoroutine(ScaleUp(m_judgeArrow, 1f));
            StartCoroutine(ScaleUp(m_judgeNucleus, 2.5f));
            m_judgeArrow.transform.DOLocalMove(Vector3.up, 5f).SetRelative();
        }

        private IEnumerator ScaleUp(GameObject obj, float time)
        {
            var objScale = obj.transform.localScale;
            obj.transform.localScale = Vector3.one * .01f;
            obj.SetActive(true);
            yield return obj.transform.DOScale(objScale, time).SetEase(Ease.InCubic).WaitForCompletion();
        }

        public void Judgement()
        {
            m_judgeArrow.transform.parent = null;
            StartCoroutine(Judge());
        }

        private IEnumerator Judge()
        {
            AudioManager.Instance.SEPlay(AudioManager.Instance.m_seRcStart);
            AudioManager.Instance.SEPlay(AudioManager.Instance.m_seJudge2);
            m_judgeArrow.transform.DOMove(m_target.transform.position, 2f);
            m_tails.SetActive(true);
            yield return new WaitForSeconds(2f);

            m_tails.SetActive(false);
            m_ex.SetActive(true);
            AudioManager.Instance.SEPlay(AudioManager.Instance.m_seJudge);
            yield return new WaitForSeconds(1.5f);

            m_lazer.SetActive(true);
            m_core.m_isJudge.Value = 3;
        }

        private IEnumerator LastJudge()
        {
            m_judgeArrow.transform.DOScale(Vector3.one * 0.1f, 5f);
            yield return new WaitForSeconds(5f);
            m_lazer.SetActive(false);
            m_core.m_isJudge.Value = -1;
        }
    }
}
