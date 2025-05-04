namespace Game
{
	/// <summary>
	/// List of scenes, which can be loaded in one at a time
	/// </summary>
	public interface ISceneList
	{
		// all the scenes that should be loaded when this is loaded
		public string[]    GetRequiredScenes();
		public EScenesType GetSceneType();

		public string ToString()
		{
			string[] scenes = GetRequiredScenes();
			string   name   = "{";
			for (int i = 0; i < scenes.Length; i++)
			{
				if (i != 0)
				{
					name += ", ";
				}

				name += scenes[i];
			}

			name += "}";
			return name;
		}

		public bool MatchesSceneList(string[] scenes)
		{
			return GetRequiredScenes().ContainsSame(scenes);
		}

		public enum EScenesType
		{
			MainMenu,
			Stage,
		}
	}
}