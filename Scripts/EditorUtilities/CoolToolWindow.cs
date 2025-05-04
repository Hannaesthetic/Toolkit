#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game
{
	/// <summary>
	///     Editor window for putting utilities onto
	///     Ideally should allow things such as opening different sets of scenes,
	///     buttons that open up important assets such as your UI windows, game manager, core data files
	/// </summary>
	public class CoolToolWindow : EditorWindow
	{
		#region Statics and Constants
		private const string WINDOW_NAME    = "Cool Tool";
		private const string WINDOW_TOOLTIP = "One Shop Stop for every big name utility button we want to add";
		private const string WINDOW_ICON    = "d_BuildSettings.Lumin";

		private static readonly Color LineBreakColor = new(0.5f, 0.5f, 0.5f, 0.5f);
		#endregion

		#region Fields
		private GameManager m_gameManagerRef;

		private Vector2  m_scrollPos;
		private string[] m_locationNames;
		#endregion

		#region Unity Functions
		/// <summary>
		///     This is where all the rendering happens
		///     If you want to add a new button, add it to this list
		/// </summary>
		private void OnGUI()
		{
			if (m_gameManagerRef == null)
			{
				m_gameManagerRef = EditorConsts.GetGameManagerPrefabFromConfig();
			}

			// scroll area for all the contents
			using (EditorGUILayout.ScrollViewScope scrollViewScope =
			       new(m_scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar))
			{
				DrawLoggingSettings();

				// update the scroll position
				m_scrollPos = scrollViewScope.scrollPosition;
			}
		}
		#endregion

		#region Public Methods
		[MenuItem("Tools/AUKM/" + WINDOW_NAME)]
		public static void OpenTool()
		{
			CoolToolWindow window = GetWindow<CoolToolWindow>();
			window.titleContent = GetTitle();
			window.Show();
		}
		#endregion

		#region Private Methods
		private static void DrawLoggingSettings()
		{
			EditorGUILayout.LabelField("Logs", EditorStyles.boldLabel);
			string[] loggingEnumDisplayNames = Enum.GetNames(typeof(ELogCategory));

			// show dropdown for setting an enum mask, in this case to filter logs
			void ShowLoggingFilter(string labelText, int currentMask, Action<int> OnMaskChanged)
			{
				using (new EditorGUILayout.HorizontalScope())
				{
					EditorGUILayout.LabelField(labelText);
					// show the dropdown mask
					int newMask = EditorGUILayout.MaskField(currentMask, loggingEnumDisplayNames);
					if (newMask != currentMask)
					{
						OnMaskChanged(newMask);
					}
				}
			}

			ShowLoggingFilter("Logs filter", Logging.GetLogFilter(), Logging.SetLogFilter);
			ShowLoggingFilter("Warn filter", Logging.GetWarnFilter(), Logging.SetWarnFilter);
		}

		#region Tools
		private void LoadSceneList(ISceneList sceneList)
		{
			string[] scenes = sceneList.GetRequiredScenes();
			EditorSceneManager.OpenScene(scenes[0], OpenSceneMode.Single);

			for (int i = 1; i < scenes.Length; ++i)
			{
				EditorSceneManager.OpenScene(scenes[i], OpenSceneMode.Additive);
			}
		}
		#endregion

		private static GUIContent GetTitle()
		{
			GUIContent guiTitle = new(EditorGUIUtility.IconContent(WINDOW_ICON));

			guiTitle.text    = WINDOW_NAME;
			guiTitle.tooltip = WINDOW_TOOLTIP;
			return guiTitle;
		}
		#endregion

		#region Utilities
		/// <summary>
		///     Draw a horizontal line taking up GUI Layout
		/// </summary>
		public static void DrawHorizontalLine(int height = 1)
		{
			Rect lineRect = EditorGUILayout.GetControlRect(false, height);
			lineRect.height = height;
			EditorGUI.DrawRect(lineRect, LineBreakColor);
		}

		public static bool DrawDropdownSelection(string header, IList<string> options, ref string[] premadeOptionsList,
			out int                                     selectedIndex)
		{
			int targetLength = options.Count + 2;
			// only remake list if length is different
			if (premadeOptionsList == null || premadeOptionsList.Length != targetLength)
			{
				premadeOptionsList    = new string[targetLength];
				premadeOptionsList[0] = header;
				premadeOptionsList[1] = string.Empty; // this causes a separator
				for (int i = 0; i < options.Count; i++)
				{
					premadeOptionsList[i + 2] = options[i];
				}
			}

			selectedIndex =  EditorGUILayout.Popup(0, premadeOptionsList);
			selectedIndex -= 2;
			return selectedIndex >= 0;
		}
		#endregion
	}
}
#endif