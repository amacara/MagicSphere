using UnityEngine;
using UniRx;

namespace RedChild {

	public class PlayerCore : MonoBehaviour {

        [SerializeField] private Transform m_mainCamera;
        [SerializeField] private GameObject m_playerUI;

        public RcListReactiveProperty RecastRc
            = new RcListReactiveProperty(RcList.none);

        public IReadOnlyReactiveProperty<RcList> RecastRcData
        {
            get { return RecastRc; }
        }

        public BoolReactiveProperty m_isTorus = new BoolReactiveProperty(false);
        public BoolReactiveProperty m_isEnd = new BoolReactiveProperty(false);
        public IntReactiveProperty m_isJudge = new IntReactiveProperty(0);

        void Update () {

            m_playerUI.transform.position = m_mainCamera.transform.position;

		}
	}
}
