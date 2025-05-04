#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game
{
	/// <summary>
	///     Class for ensuring a game manager exists wherever you run the game from
	///     Saves you from having to populate one in each scene you'd ever wanna test in
	///     Editor only, so your boot sequence will need to introduce one
	///		Does this by injecting itself into the game starting to run
	/// </summary>
	[InitializeOnLoad]
	public class EditorStartBootstrap
	{
		static EditorStartBootstrap()
		{
			// binds an event to any editor play state
			// may run several times so we make sure its only ever on once by removing it first
			EditorApplication.playModeStateChanged -= EditorApplication_OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += EditorApplication_OnPlayModeStateChanged;
		}

		private static void EditorApplication_OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
		{
			switch (playModeStateChange)
			{
				case PlayModeStateChange.EnteredPlayMode:
					InjectInitialPrefabs();
					break;
			}
		}

		private static void InjectInitialPrefabs()
		{
			// check for GameManagers in open scenes
			if (Object.FindObjectOfType<GameManager>() == null)
			{
				GameManager gameManagerPrefab = EditorConsts.GetGameManagerPrefabFromConfig();
				if (gameManagerPrefab == null)
				{
					Logging.Error("Attempted to inject GameManager prefab, but none could be found", ELogCategory.Editor);
				}
				else
				{
					GameManager manager = Object.Instantiate(gameManagerPrefab);
					manager.name = manager.GetType().ToString();
					manager.Editor_InitialiseStateFromInjection();
				}
			}
		}
	}
}
#endif