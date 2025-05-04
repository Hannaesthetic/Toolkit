using UnityEngine;

namespace Game
{

	public class ManagerBase : MonoBehaviour
	{
		// having this data also owned by each manager means pieces of a manager
		// can access their owner without having to use the singleton
		// this would make it easier to move off a singleton gamemanager,
		// but non singleton gamemanager already sounds like a nightmare...
		public GameManager Manager { get; private set; }
		
		public void Init(GameManager manager)
		{
			Manager = manager;
			OnInit();
		}

		protected virtual void OnInit()           {}
		public virtual    void OnPreSceneUnload() {}
		public virtual    void OnScenesLoaded()   {}
	}
}