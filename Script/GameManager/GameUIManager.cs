using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace RedChild
{
    public class GameUIManager : MonoBehaviour
    {
        [System.Serializable]
        public struct TitleCanvas
        {
            public Canvas m_canvas;
            public Image m_logo;
            public Text m_text;
        }
        [SerializeField] private TitleCanvas m_titleCanvas;

        [System.Serializable]
        public struct TutorialCanvas
        {
            public Canvas m_canvas;
            public GameObject m_teleport;
            public GameObject m_attack_1;
            public GameObject m_attack_2;
            public GameObject m_attack_3;
        }
        [SerializeField] private TutorialCanvas m_tutorialCanvas;

        [System.Serializable]
        public struct ReadyCanvas
        {
            public Canvas m_canvas;
            public Image m_frame;
            public Text m_text;
        }
        [SerializeField] private ReadyCanvas m_readyCanvas;

        [System.Serializable]
        public struct ResultCanvas
        {
            public Canvas m_canvas;
            public Image m_logo;
        }
        [SerializeField] private ResultCanvas m_resultCanvas;

        private GameTimeManager m_timeManager;

        readonly private float FADE_TIME = 5f;

        private void Awake()
        {
            m_titleCanvas.m_canvas.gameObject.SetActive(false);
            m_titleCanvas.m_logo.DOFade(0f, Time.deltaTime);
            m_titleCanvas.m_text.DOFade(0f, Time.deltaTime);

            m_tutorialCanvas.m_canvas.gameObject.SetActive(false);
            m_tutorialCanvas.m_teleport.SetActive(false);
            m_tutorialCanvas.m_attack_1.SetActive(false);
            m_tutorialCanvas.m_attack_2.SetActive(false);
            m_tutorialCanvas.m_attack_3.SetActive(false);

            m_readyCanvas.m_canvas.gameObject.SetActive(false);

            m_resultCanvas.m_canvas.gameObject.SetActive(false);
            m_resultCanvas.m_logo.DOFade(0f, Time.deltaTime);

            m_timeManager = GetComponent<GameTimeManager>();
        }

        private void Start()
        {
            m_timeManager.CountDownTime.Subscribe(_ => {
                m_readyCanvas.m_text.text = _.ToString();
                m_readyCanvas.m_frame.fillAmount = 0f;
                DOTween.To(() => m_readyCanvas.m_frame.fillAmount, (n) => m_readyCanvas.m_frame.fillAmount = n, 1f, 1f);
            }).AddTo(this.gameObject);
        }

        public IEnumerator TitleCall()
        {
            m_titleCanvas.m_canvas.gameObject.SetActive(true);
            yield return m_titleCanvas.m_logo.DOFade(1f, FADE_TIME).WaitForCompletion();
            yield return m_titleCanvas.m_text.DOFade(1f, FADE_TIME / 2f).WaitForCompletion();

            yield return StartCoroutine(WaitingInput(AudioManager.Instance.m_seTitle));
            m_titleCanvas.m_canvas.gameObject.SetActive(false);
        }

        public IEnumerator TutorialCall()
        {
            m_tutorialCanvas.m_canvas.gameObject.SetActive(true);
            m_tutorialCanvas.m_teleport.SetActive(true);
            yield return StartCoroutine(WaitingInput(AudioManager.Instance.m_seTutorial));

            m_tutorialCanvas.m_teleport.SetActive(false);
            m_tutorialCanvas.m_attack_1.SetActive(true);
            yield return StartCoroutine(WaitingInput(AudioManager.Instance.m_seTutorial));

            m_tutorialCanvas.m_attack_1.SetActive(false);
            m_tutorialCanvas.m_attack_2.SetActive(true);
            yield return StartCoroutine(WaitingInput(AudioManager.Instance.m_seTutorial));

            m_tutorialCanvas.m_attack_2.SetActive(false);
            m_tutorialCanvas.m_attack_3.SetActive(true);
            yield return StartCoroutine(WaitingInput(AudioManager.Instance.m_seTutorial));

            m_tutorialCanvas.m_canvas.gameObject.SetActive(false);
        }

        private IEnumerator WaitingInput(string seName)
        {
            while (!OVRInput.GetDown(OVRInput.RawButton.A))
            {
                yield return null;
            }
            AudioManager.Instance.SEPlay(seName);
            yield return new WaitForSeconds(.1f);
        }

        public void ReadySetup(bool isTrue = false)
        {
            m_readyCanvas.m_canvas.gameObject.SetActive(isTrue);
        }

        public IEnumerator ResultCall()
        {
            m_resultCanvas.m_canvas.gameObject.SetActive(true);
            yield return m_resultCanvas.m_logo.DOFade(1f, FADE_TIME).WaitForCompletion();

            yield return StartCoroutine(WaitingInput(AudioManager.Instance.m_seTitle));
            m_resultCanvas.m_canvas.gameObject.SetActive(false);
        }
    }
}
