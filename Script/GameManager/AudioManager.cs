using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace RedChild
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [SerializeField] private AudioSource m_bgmAudio;
        [SerializeField] private Transform m_sePoint;

        [SerializeField] private float m_bgmVolume = 0.5f;
        [SerializeField] private float m_bgmFadeTime = 3f;
        [SerializeField] private float m_seVolume = 0.35f;

        readonly private string m_bgmPath = "BGM/";
        readonly public string m_bgmTitle = "bgm_maoudamashii_orchestra03";
        readonly public string m_bgmTutorial = "bgm_maoudamashii_orchestra19";
        readonly public string m_bgmBattle = "bgm_maoudamashii_neorock66";
        readonly public string m_bgmJudgement = "bgm_maoudamashii_orchestra23";
        readonly public string m_bgmResultWin = "bgm_maoudamashii_orchestra03";
        readonly public string m_bgmResultLose = "bgm_maoudamashii_piano16";

        readonly private string m_sePath = "SE/";
        readonly private string m_sePlayer = "SEPlayer";
        readonly public string m_seTitle = "se_maoudamashii_system46";
        readonly public string m_seTutorial = "se_maoudamashii_onepoint09";
        readonly public string m_seRc1 = "se_maoudamashii_explosion02";
        readonly public string m_seRc2 = "se_maoudamashii_element_thunder02";
        readonly public string m_seRc3 = "se_maoudamashii_retro28";
        readonly public string m_seRc4 = "se_maoudamashii_explosion01";
        readonly public string m_seRc5 = "se_maoudamashii_magical27";
        readonly public string m_seRc6 = "se_maoudamashii_element_ice06";
        readonly public string m_seRc7 = "se_maoudamashii_element_wind01";
        readonly public string m_seRc8 = "se_maoudamashii_magical21";
        readonly public string m_seRcStart = "se_maoudamashii_element_wind02";

        readonly public string m_seJudge = "se_maoudamashii_effect06";
        readonly public string m_seJudge2 = "se_maoudamashii_explosion01";

        readonly public string m_seTimeStart = "se_maoudamashii_magical14";
        readonly public string m_seTimeEnd = "se_maoudamashii_magical13";

        readonly public string m_seMonsterVoice = "se_maoudamashii_voice_monster01";
        readonly public string m_seFireBall = "se_maoudamashii_element_fire09";

        public void BGMChange(string bgmName)
        {
            StartCoroutine(BGMChangeProcess(bgmName));
        }

        private IEnumerator BGMChangeProcess(string bgmName)
        {
            if (m_bgmAudio.clip != null)
            {
                yield return StartCoroutine(BGMFade(0f, m_bgmFadeTime));
            }
            m_bgmAudio.Stop();
            yield return null;
            m_bgmAudio.clip = Resources.Load<AudioClip>(m_bgmPath + bgmName);
            m_bgmAudio.Play();
            yield return StartCoroutine(BGMFade(m_bgmVolume, m_bgmFadeTime));
        }

        private IEnumerator BGMFade(float volume, float fadeTime)
        {
            yield return DOTween.To(() => m_bgmAudio.volume, (n) => m_bgmAudio.volume = n, volume, fadeTime).WaitForCompletion();
        }

        public void SEPlay(string seName)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(m_sePath + m_sePlayer), m_sePoint);
            var audio = obj.GetComponent<AudioSource>();
            audio.volume = m_seVolume;
            audio.clip = Resources.Load<AudioClip>(m_sePath + seName);
            audio.Play();
            Destroy(obj, audio.clip.length);
        }

    } 
}
