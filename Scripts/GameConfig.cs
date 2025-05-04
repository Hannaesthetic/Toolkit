using UnityEngine;

namespace Game
{
	/// <summary>
	/// Game config asset
	/// Should be set up so that different versions of these should be created for different kinds of builds you might make
	/// Eg, if you were to make a demo version of the game, you would make a new one of these with
	/// a few values changed, then you'd be able to swap between making demos and full released
	/// by just swapping in the right GameConfig
	/// </summary>
	[CreateAssetMenu(menuName = "Game/Config/Game Config", order = -200)]
	public class GameConfig : ScriptableObject
	{
		public CombatConfig   Combat;
		public SceneListAsset MainMenu;
		public Stage[]        Stages;
	}
}