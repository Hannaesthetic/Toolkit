using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	/// <summary>
	///     Big "has everything" manager, ideally the only singleton, which finds you everything else
	/// </summary>
	public class GameManager : Singleton<GameManager>
	{
		#region Serialized
		// when you add a new manager, also add a public call for it, and add it to OnAwake to bind the events
		[SerializeField] private GameConfig       m_Config;
		[SerializeField] private FlowManager      m_Flow;
		[SerializeField] private SceneLoadManager m_SceneLoad;
		// this won't exist when you first import the project
		[SerializeField] private InputManager_YOURGAMENAME m_Input;
		#endregion

		#region Fields
		private ManagerBase[] m_managers;
		#endregion

		#region Properties
		public GameConfig              Config          => m_Config;
		public FlowManager             Flow            => m_Flow;
		public SceneLoadManager        SceneLoad       => m_SceneLoad;
		public InputManager_YOURGAMENAME       Input           => m_Input;

		public EGameState State { get; private set; }
		#endregion

		#region Protected Methods
		protected override void OnAwake()
		{
			DontDestroyOnLoad(this);
			
			m_managers = new ManagerBase[]
			{
				m_Flow,
				m_SceneLoad,
				m_Input,
			};

			for (int i = 0; i < m_managers.Length; i++)
			{
				m_managers[i].Init(this);
			}

			SceneLoad.Evt_PreScenesUnloaded += OnPreSceneUnload;
			SceneLoad.Evt_ScenesLoaded      += OnScenesLoaded;
		}

		protected override void OnDestroyed()
		{
			SceneLoad.Evt_PreScenesUnloaded -= OnPreSceneUnload;
			SceneLoad.Evt_ScenesLoaded      -= OnScenesLoaded;
		}
		#endregion

		#region Private Methods
		private void SetState(EGameState newState)
		{
			Debug.Assert(newState != State);

			// unset the old state
			switch (State)
			{ }

			State = newState;

			// setup the new state
			switch (State)
			{
			}
		}

		private void OnScenesLoaded(ISceneList sceneList)
		{
			EGameState nextState = EGameState.Unknown;

			// set up whatever kind of scenes we loaded in
			switch (sceneList.GetSceneType())
			{
				case ISceneList.EScenesType.Stage:
					nextState = EGameState.Playing;
					break;

				case ISceneList.EScenesType.MainMenu:
					nextState = EGameState.MainMenu;
					break;
			}

			foreach (ManagerBase manager in m_managers)
			{
				manager.OnScenesLoaded();
			}

			// this might have to be moved above the OnScenesLoaded?
			if (nextState != EGameState.Unknown)
			{
				SetState(nextState);
			}
		}
		
		private void OnPreSceneUnload(ISceneList sceneList)
		{
			foreach (ManagerBase manager in m_managers)
			{
				manager.OnPreSceneUnload();
			}

			SetState(EGameState.LoadingScreen);
		}

		#endregion

		#if UNITY_EDITOR
		#region GameManager injection
		/// <summary>
		/// To be run if you inject a GameManager into a scene that you start from a "non natural" flow
		/// Should do everything it can to guess what "from boot" state its missing, and restore that
		/// Eg, if it detects that its in a microgame, it should tell the managers to set their states to
		/// being in a microgame
		/// </summary>
		public void Editor_InitialiseStateFromInjection()
		{
			string[] openScenes = new string[SceneManager.sceneCount];
			for (int i = 0; i < openScenes.Length; i++)
			{
				openScenes[i] = SceneManager.GetSceneAt(i).name;
			}

			bool MatchesOpenScenes(ISceneList sceneList)
			{
				return sceneList.MatchesSceneList(openScenes);
			}

			if (Config.MainMenu != null && MatchesOpenScenes(Config.MainMenu))
			{
				Editor_InitialiseIntoMainMenu();
				return;
			}

			for (int i = 0; i < Config.Stages.Length; i++)
			{
				Stage stage = Config.Stages[i];
				if (MatchesOpenScenes(stage.Scenes))
				{
					Editor_InitialiseIntoStage(stage);
					return;
				}
			}
		}

		private void Editor_InitialiseIntoMainMenu()
		{
			SetState(EGameState.MainMenu);
			Logging.Log("Booted into MainMenu", ELogCategory.Editor);
		}

		private void Editor_InitialiseIntoStage(Stage stage)
		{
			// when we have progress data, you should try load it, or best approximate a new one here
			SetState(EGameState.Playing);
			Logging.Log($"Booted into {stage}", ELogCategory.Editor);
		}
		#endregion
		#endif
	}

	public enum EGameState
	{
		Unknown,
		MainMenu,
		LoadingScreen,
		Playing,
	}
}