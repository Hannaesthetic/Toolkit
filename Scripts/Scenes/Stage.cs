using UnityEngine;

namespace Game
{
	/// <summary>
	/// One of the game's level locations, should there be one
	/// Idk, populate some stuff here
	/// </summary>
	[CreateAssetMenu(menuName = "Game/Stage", order = 0)] 
	public class Stage : ScriptableObject
	{
		public SceneListAsset Scenes;
		public string         GetName() => name;
	}
}