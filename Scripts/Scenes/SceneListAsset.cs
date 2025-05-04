using UnityEngine;

namespace Game
{
	[CreateAssetMenu(menuName = "Game/Scene List")]
	public class SceneListAsset : ScriptableObject, ISceneList
	{
		[SerializeField] private string[]               m_SceneList;
		[SerializeField] private ISceneList.EScenesType m_ScenesType;

		public string[] GetRequiredScenes() => m_SceneList;

		public ISceneList.EScenesType GetSceneType() => m_ScenesType;
	}
}