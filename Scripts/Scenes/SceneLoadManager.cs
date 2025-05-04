using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	public class SceneLoadManager : ManagerBase
	{
		private EState m_state;

		private ISceneList m_currentSceneList;

		public event Action<ISceneList> Evt_PreScenesUnloaded;
		public event Action<ISceneList> Evt_ScenesLoaded;
		
		public SceneLoadToken LoadScenes(ISceneList scenes)
		{
			if (m_state != EState.Idle)
			{
				Logging.Error($"Scenelist {scenes} attempted to load while another scene load was ongoing, cancelling", ELogCategory.Scenes);
				return null;
			}

			SceneLoadToken token = new SceneLoadToken();
			StartCoroutine(c_LoadScenes(scenes, token));
			return token;
		}

		private List<AsyncOperation> m_asyncOperations = new List<AsyncOperation>(); 
		
		private IEnumerator c_LoadScenes(ISceneList sceneList, SceneLoadToken token)
		{
			// let everything else finish, to avoid weird edge cases
			SetState(EState.AwaitingStart);
			yield return new WaitForEndOfFrame();

			Evt_PreScenesUnloaded?.Invoke(m_currentSceneList);
			m_currentSceneList = null;
			
			// unload all existing scenes
			SetState(EState.Unloading);
			

			// count all the scenes to unload
			string[] scenesToLoad              = sceneList.GetRequiredScenes();
			int      scenesToUnloadCount       = SceneManager.sceneCount;
			int      scenesToLoadCount         = scenesToLoad.Length;
			int      sceneUnloadsAndLoadsTotal = scenesToUnloadCount + scenesToLoadCount;
			int      progress                  = 0;

			token.progressTotal = sceneUnloadsAndLoadsTotal;
			
			void UpdateProgress()
			{
				token.progress = progress;
				SetProgress(progress, sceneUnloadsAndLoadsTotal);
			}
			
			// runs through all the async operations, to see if they're all done
			void CheckAllAsyncOperations()
			{
				for (int i = m_asyncOperations.Count - 1; i >= 0; i--)
				{
					if (!m_asyncOperations[i].isDone)
					{
						continue;
					}

					m_asyncOperations.RemoveAt(i);
					progress++;
					UpdateProgress();
				}
			}
			
			// empty scene - used to unload all scenes async, reload new scenes, & remove temp scene
			Scene placeholderScene = SceneManager.CreateScene( "temp_placeholder" );

			// request to unload them
			m_asyncOperations.Clear();
			for (int i = scenesToUnloadCount; i >= 0; i--)
			{
				Scene sceneToUnload = SceneManager.GetSceneAt(i);
				if (sceneToUnload == placeholderScene)
				{
					continue;
				}
				m_asyncOperations.Add(SceneManager.UnloadSceneAsync(sceneToUnload));
			}

			// wait for them all to unload
			while (true)
			{
				CheckAllAsyncOperations();
				if (m_asyncOperations.Count > 0)
				{
					yield return null;
				}
				else
				{
					break;
				}
 			}

			Debug.Assert(progress == scenesToUnloadCount);
			UpdateProgress();
			
			// load the scenes
			SetState(EState.Loading);
			
			for (int i = 0; i < scenesToLoadCount; i++)
			{
				string sceneNameToLoad = scenesToLoad[i];
				m_asyncOperations.Add(SceneManager.LoadSceneAsync(sceneNameToLoad));
			}
			
			// wait for them all to load
			while (true)
			{
				CheckAllAsyncOperations();
				if (m_asyncOperations.Count > 0)
				{
					yield return null;
				}
				else
				{
					break;
				}
			}
			
			// AsyncOperation unloadPlaceholderScene = SceneManager.UnloadSceneAsync( placeholderScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects );
			// while (!unloadPlaceholderScene.isDone)
			// {
			// 	yield return null;
			// }
			
			// announce that you're done
			m_currentSceneList = sceneList;
			SetState(EState.Idle);
			token.Complete();
			Evt_ScenesLoaded?.Invoke(m_currentSceneList);
		}

		private void SetProgress(int progress, int totalNeeded)
		{
			
		}

		private void SetState(EState state)
		{
			if (m_state == state)
			{
				return;
			}

			m_state = state;
		}

		public class SceneLoadToken
		{
			public int  progress;
			public int  progressTotal;
			public bool finished;

			public event Action Evt_OnComplete;

			public void Complete()
			{
				Evt_OnComplete?.Invoke();
			}
		}

		private enum EState
		{
			Idle,
			AwaitingStart,
			Unloading,
			Loading,
		}
	}
}