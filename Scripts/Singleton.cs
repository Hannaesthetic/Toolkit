using UnityEngine;

namespace Game
{
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		public static T Instance
		{
			get
			{
				return m_instance;
			}
		}

		private static T    m_instance;
		public static  bool HasInstance { get; private set; }

		private void Awake()
		{
			Debug.Assert(!HasInstance);
			m_instance  = (T)this;
			HasInstance = true;
			OnAwake();
		}

		protected void OnDestroy()
		{
			m_instance  = null;
			HasInstance = false;
			OnDestroyed();
		}

		protected virtual void OnAwake()
		{
			
		}

		protected virtual void OnDestroyed()
		{
			
		}
	}
}