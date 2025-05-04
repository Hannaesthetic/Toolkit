using System;
using System.Collections.Generic;

namespace Game
{
	public static class Helpers
	{
		public static string Combine(this IList<string> strings, char separator = ',')
		{
			string output = "{";
			for (int i = 0; i < strings.Count; i++)
			{
				if (i != 0)
				{
					output += separator;
				}

				output += strings[i];
			}

			output += "}";
			return output;
		}

		/// <summary>
		/// Checks if two arrays have the same set of values, regardless of order
		/// </summary>
		public static bool ContainsSame<T>(this IList<T> a, IList<T> b)
		{
			// cheap early exit if the lengths are different
			if (a.Count != b.Count)
			{
				return false;
			}

			// check if every item in A is contained in B
			// while this step alone only detects that B is the superset of A
			// them being the same length combines with this to guarentee that they're the same
			for (int i = 0; i < a.Count; i++)
			{
				bool found = false;
				for (int j = 0; j < b.Count; j++)
				{
					if (a[i].Equals(b[j]))
					{
						found = true;
						break;
					}
				}
				
				if (!found)
				{
					// a string in A has not been found in B
					return false;
				}
			}
			
			return true;
		}
		
		private static readonly Random RANDOM_GENERATOR = new Random();

		// fisher-yates shuffle, stolen shamelessly from https://stackoverflow.com/questions/273313/randomize-a-listt
		public static void Shuffle<T>(this IList<T> list)
			=> list.Shuffle(RANDOM_GENERATOR);
		
		public static void Shuffle<T>(this IList<T> list, Random generator)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k     = RANDOM_GENERATOR.Next(n + 1);
				T   value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}