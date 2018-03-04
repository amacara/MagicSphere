using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace RedChild
{
    [RequireComponent(typeof(GameTimeManager))]
    public class MainGameManager : MonoBehaviour, IGameStateProvider
    {
        public GameStateReactiveProperty CurrentState
            = new GameStateReactiveProperty(GameState.Initializing);

        public IReadOnlyReactiveProperty<GameState> CurrentGameState
        {
            get { return CurrentState; }
        }

        [SerializeField] private GameObject m_opEff;
        [SerializeField] private GameObject m_epEff;

        private GameTimeManager m_gameTimeManager;
        private GameUIManager m_gameUIManager;
        private PlayerCore m_playerCore;
        private EnemyCore m_enemyCore;

        readonly private string PLAYER_CORE_OBJ = "PlayerCore";
        readonly private string ENEMY_CORE_OBJ = "PurpleDragon";
        readonly private float TITLE_WAIT_TIME = 9f;
        readonly private string SCENE_NAME = "main";

        // Use this for initialization
        void Start()
        {
            m_gameTimeManager = GetComponent<GameTimeManager>();
            m_gameUIManager = GetComponent<GameUIManager>();
            m_playerCore = GameObject.Find(PLAYER_CORE_OBJ).GetComponent<PlayerCore>();
            m_enemyCore = GameObject.Find(ENEMY_CORE_OBJ).GetComponent<EnemyCore>();

            CurrentState.Subscribe(state =>
            {
                OnStateChanged(state);
            });
        }

        private void OnStateChanged(GameState nextState)
        {
            switch (nextState)
            {
                case GameState.Initializing:
                    StartCoroutine(Initilize());
                    break;
                case GameState.Title:
                    StartCoroutine(Title());
                    break;
                case GameState.Tutorial:
                    StartCoroutine(Tutorial());
                    break;
                case GameState.Ready:
                    StartCoroutine(Ready());
                    break;
                case GameState.Battle:
                    Battle();
                    break;
                case GameState.Judgement:
                    Judgement();
                    break;
                case GameState.Result:
                    StartCoroutine(Result());
                    break;
                case GameState.Finished:
                    Finished();
                    break;
                default:
                    break;
            }
        }

        private IEnumerator Initilize()
        {
            m_playerCore.m_isTorus.Value = false;
            TeleportCore.Instance.TeleportDisable();

            yield return null;
            CurrentState.Value = GameState.Title;
        }

        private IEnumerator Title()
        {
            AudioManager.Instance.BGMChange(AudioManager.Instance.m_bgmTitle);
            WorldEffect.Instance.MainEffectActivate();
            yield return new WaitForSeconds(TITLE_WAIT_TIME);
            yield return StartCoroutine(m_gameUIManager.TitleCall());

            CurrentState.Value = GameState.Tutorial;
        }

        private IEnumerator Tutorial()
        {
            yield return StartCoroutine(m_gameUIManager.TutorialCall());

            CurrentState.Value = GameState.Ready;
        }

        private IEnumerator Ready()
        {
            m_gameTimeManager.CountDownTime.Where(_ => _ == 4).Subscribe(_ => TeleportCore.Instance.TeleportEnable()).AddTo(gameObject);

            m_gameTimeManager.CountDownTime
                .FirstOrDefault(x => x == 0)
                .Delay(TimeSpan.FromSeconds(1))
                .Subscribe(_ => CurrentState.Value = GameState.Battle)
                .AddTo(gameObject);

            yield return null;

            m_gameTimeManager.StartCountDown();
            m_enemyCore.m_isStart.Value = true;
            m_gameUIManager.ReadySetup(true);
        }

        private void Battle()
        {
            m_gameUIManager.ReadySetup(false);
            m_playerCore.m_isTorus.Value = true;

            AudioManager.Instance.BGMChange(AudioManager.Instance.m_bgmBattle);

            m_gameTimeManager.RemainingTime.Where(_ => _ == 4).Subscribe(_ => 
            {
                m_enemyCore.m_isEnd.Value = true;
                m_playerCore.m_isEnd.Value = true;
            }).AddTo(gameObject);

            m_gameTimeManager.RemainingTime
                .FirstOrDefault(x => x == 0)
                .Delay(TimeSpan.FromSeconds(2))
                .Subscribe(_ => CurrentState.Value = GameState.Judgement);

            m_gameTimeManager.StartBattleTime();
        }

        private void Judgement()
        {
            AudioManager.Instance.BGMChange(AudioManager.Instance.m_bgmJudgement);
            m_playerCore.m_isJudge.Value = 1;
            m_playerCore.m_isJudge.Where(_ => _ == -1).Subscribe(_ => CurrentState.Value = GameState.Result);
        }

        private IEnumerator Result()
        {
            AudioManager.Instance.BGMChange(AudioManager.Instance.m_bgmResultWin);
            m_opEff.SetActive(false);
            m_epEff.SetActive(true);
            yield return StartCoroutine(m_gameUIManager.ResultCall());
            CurrentState.Value = GameState.Finished;
        }

        private void Finished()
        {
            SceneManager.LoadScene(SCENE_NAME);
        }
    }
}
