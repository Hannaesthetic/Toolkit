using UnityEngine;

namespace Game
{
	/// <summary>
	/// Everything you should need to define a "level" or "location" in your game
	/// Used by the GameManager injector to correctly identify which stage you're in when
	/// you run the editor directly in there
	/// </summary>
	[CreateAssetMenu(menuName = "Game/Stage", order = 0)] 
	public class Stage : ScriptableObject
	{
		// todo: CoolTool should be able to open all the scenes for every stage in the gameconfig
		// todo: this could really do with some nicer editor side layout 
		public SceneListAsset Scenes;
		public string         GetName() => name;
	}
}