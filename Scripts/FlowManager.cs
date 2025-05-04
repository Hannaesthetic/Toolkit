using System;
using UnityEngine;

namespace Game
{
	/// <summary>
	///     Class which handles all calls to navigate game flow
	///     Eg, quitting mid game, pressing new game, etc...
	///     Keeps all these functions in one place, UI and the like should be calling this
	/// </summary>
	public class FlowManager : ManagerBase
	{
		#region Enums
		private enum EState
		{
			Idle,
			LoadingStage,
			LoadingMenu,
		}
		#endregion

		#region Fields
		private Stage  m_loadingStage;
		private EState m_state;
		#endregion

		public bool IsLoading => m_state is EState.LoadingMenu or EState.LoadingStage;

		#region Public Methods
		/// <summary>
		///     Should take you to the start of the game
		/// </summary>
		public void StartGame()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Should take you from whatever state to the main menu
		/// </summary>
		[ContextMenu("Quit to main menu")]
		public void QuitToMainMenu()
		{
			LoadSceneList(GameManager.Instance.Config.MainMenu);
		}

		/// <summary>
		///     Take you back to desktop
		/// </summary>
		public void QuitGame()
		{
			Application.Quit();
		}

		public void SelectStage(Stage stage)
		{
			if (IsLoading)
			{
				Logging.Error("Attempted to load stage while FlowManager was not idle", ELogCategory.Scenes, this);
				return;
			}

			m_loadingStage = stage;
			LoadSceneList(stage.Scenes);
		}

		private void LoadSceneList(ISceneList sceneList)
		{
			switch (sceneList.GetSceneType())
			{
				case ISceneList.EScenesType.Stage:
					SetState(EState.LoadingStage);
					break;
				case ISceneList.EScenesType.MainMenu:
					SetState(EState.LoadingMenu);
					break;
			}
			
			SceneLoadManager.SceneLoadToken token = Manager.SceneLoad.LoadScenes(sceneList);
			token.Evt_OnComplete += SceneLoadToken_OnStageLoaded;
		}

		public bool RequestLoadedStage(out Stage loadedStage)
		{
			if (m_loadingStage != null)
			{
				loadedStage    = m_loadingStage;
				m_loadingStage = null;
				return true;
			}

			loadedStage = null;
			return false;
		}
		#endregion

		#region Private Methods

		private void SceneLoadToken_OnStageLoaded()
		{
			Debug.Assert(IsLoading);
			SetState(EState.Idle);
		}

		private void SetState(EState state)
		{
			if (m_state == state)
			{
				return;
			}

			m_state = state;
		}
		#endregion
	}
}