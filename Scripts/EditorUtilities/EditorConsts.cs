using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Game
{
	public static class EditorConsts
	{
		#region Statics and Constants
		public const string CONFIG_FOLDER_NAME = "Assets/Config";

		private const string GAMEOBJECT_FILTER = "t:GameObject";

		public static readonly string[] CONFIG_FOLDERS_TO_SEARCH = { CONFIG_FOLDER_NAME };
		#endregion

		#region Public Methods
		/// <summary>
		///     Finds a GameManager prefab in the prefab folders
		/// </summary>
		/// <returns></returns>
		public static GameManager GetGameManagerPrefabFromConfig()
		{
			List<GameManager> prefabsList = GetPrefabsOfType<GameManager>(CONFIG_FOLDERS_TO_SEARCH);
			if (prefabsList.Count == 0)
			{
				Logging.Error($"No GameManager prefab found in the folders {CONFIG_FOLDERS_TO_SEARCH.Combine()}",
					ELogCategory.Editor);
				return null;
			}

			if (prefabsList.Count > 1)
			{
				Logging.Warn(
					$"There are multiple GameManager prefabs in the config folders, {prefabsList[0].name} will be used",
					ELogCategory.Editor);
			}

			// if you want a smarter selection process, it goes here
			return prefabsList[0];
		}

		/// <summary>
		///     Finds all GameObject assets (prefabs) at a given path, which have a component of a given type
		///     Returns them in a list
		/// </summary>
		public static List<T> GetPrefabsOfType<T>(string[] folderPaths) where T : MonoBehaviour
		{
			string[] prefabGUIDs  = AssetDatabase.FindAssets(GAMEOBJECT_FILTER, folderPaths);
			List<T>  foundPrefabs = new();

			if (prefabGUIDs.Length == 0)
			{
				Logging.Warn($"Found no prefabs of type {typeof(T)} in {folderPaths.Combine()}", ELogCategory.Editor);
				return foundPrefabs;
			}

			foreach (string guid in prefabGUIDs)
			{
				GameObject prefabObj = GetAssetFromGUID<GameObject>(guid);
				if (prefabObj != null && prefabObj.TryGetComponent(out T prefab))
				{
					foundPrefabs.Add(prefab);
				}
			}

			return foundPrefabs;
		}

		public static T GetAssetFromGUID<T>(string guid) where T : Object
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			return AssetDatabase.LoadAssetAtPath<T>(path);
		}
		#endregion
	}
}
#endif