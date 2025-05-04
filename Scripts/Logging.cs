using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
	/// <summary>
	/// Logging wrapper
	/// Using this, we can allow tools to filter the log types
	/// We could also at compile time, cut all logging out of the project,
	/// which can be useful in builds for lower power consoles
	/// </summary>
	public static class Logging
	{
		private const string LOG_FILTER = "Logging_LogFilter";
		private const string WARN_FILTER = "Logging_WarnFilter";
		
		public static void Log(string message, ELogCategory category, object context = null)
		{
			if (IsFiltered(category, GetLogFilter()))
			{
				return;
			}
			
			Debug.Log($"[{category.ToString()} {message}]");
		}
		
		public static void Warn(string message, ELogCategory category, object context = null)
		{
			if (IsFiltered(category, GetWarnFilter()))
			{
				return;
			}
			
			Debug.LogWarning($"[{category.ToString()} {message}]");
		}
		
		public static void Error(string message, ELogCategory category, object context = null)
		{
			// we don't mute errors because we always want those showing
			Debug.LogError($"[{category.ToString()} {message}]");
		}
		
		private static void SetFilter( int filter, string key )
		{
			#if UNITY_EDITOR
			EditorPrefs.SetInt( key, filter );
			#endif
		}
		private static int GetFilter( string key )
		{
			#if UNITY_EDITOR
			return EditorPrefs.GetInt( key );
			#else
			return 0;
			#endif
		}

		/// <summary>
		///  uses bitwise operations to check if logCategory should be filtered out
		/// </summary>
		private static bool IsFiltered(ELogCategory logCategory, int filter)
		{
			int logIndex = (int)logCategory;
			// a number that when shown in binary, is all 0s except for the [logIndex]th value from the right
			// eg if logIndex is 3, logMask will be 0000 0100
			int logMask  = 1 << logIndex;
			// if both values are written in binary, AND them so that the result will be a number that, in binary,
			// every digit is 0 unless that digit is 1 in both filter and logMask
			// if none of them are 0, that means there are no digits in common between logMask and filter
			return (filter & logMask) == 0;
		}

		public static int  GetLogFilter()           => GetFilter(LOG_FILTER);
		public static void SetLogFilter(int filter) => SetFilter(filter, LOG_FILTER);
		public static int  GetWarnFilter()          => GetFilter(WARN_FILTER);
		public static void SetWarnFilter(int filter) => SetFilter(filter, WARN_FILTER);

	}

	// this is so later we can add some log filtering
	public enum ELogCategory
	{
		Default,
		Editor,
		Scenes,
		// Add more values here to let you filter out your logs
	}
}