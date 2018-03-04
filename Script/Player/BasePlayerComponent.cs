using UnityEngine;

namespace RedChild {

	public abstract class BasePlayerComponent : MonoBehaviour {

		protected PlayerCore m_core;

		void Start() {
			m_core = GetComponent<PlayerCore> ();
			OnStart ();
		}

		protected virtual void OnStart() {}

	}
}
