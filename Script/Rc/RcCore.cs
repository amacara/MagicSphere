using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using VrGrabber;

namespace RedChild
{
    public class RcCore : MonoBehaviour
    {
        [SerializeField] private RcList m_rcType;

        private PlayerCore m_playerCore;
        private GameObject m_target;
        private GameTimeManager m_timeManager;

        private ParticleSystem[] m_particles;
        private VrgGrabbable m_vrgGrabbable;

        public RcList m_RcType
        {
            get { return m_rcType; }
        }
        public GameObject m_Target
        {
            get { return m_target; }
        }
        public GameTimeManager m_TimeManager
        {
            get { return m_timeManager; }
        }
        public ParticleSystem[] m_Particles
        {
            get { return m_particles; }
        }

        public BoolReactiveProperty m_grabbed = new BoolReactiveProperty(false);
        public BoolReactiveProperty m_released = new BoolReactiveProperty(false);
        public BoolReactiveProperty m_attackAction = new BoolReactiveProperty(false);

        readonly private string PLAYER_CORE_OBJ = "PlayerCore";
        readonly private string BOSS_TARGET_OBJ = "BossTarget";
        readonly private string GAME_MANAGER_OBJ = "GameManager";

        void Start()
        {
            m_playerCore = GameObject.Find(PLAYER_CORE_OBJ).GetComponent<PlayerCore>();
            m_target = GameObject.Find(BOSS_TARGET_OBJ);
            m_timeManager = GameObject.Find(GAME_MANAGER_OBJ).GetComponent<GameTimeManager>();

            m_particles = GetComponentsInChildren<ParticleSystem>(true);
            m_vrgGrabbable = GetComponent<VrgGrabbable>();
            m_vrgGrabbable.onGrabbed.AddListener(GrabbedCallback);
            m_vrgGrabbable.onReleased.AddListener(ReleasedCallback);
        }

        public void GrabbedCallback()
        {
            m_grabbed.SetValueAndForceNotify(true);
        }

        public void ReleasedCallback()
        {
            this.transform.parent = null;
            m_vrgGrabbable = null;
            Destroy(GetComponent<VrgGrabbable>());
            m_playerCore.RecastRc.SetValueAndForceNotify(m_rcType);
            m_grabbed.SetValueAndForceNotify(false);
            m_released.SetValueAndForceNotify(true);
        }
    }
}
