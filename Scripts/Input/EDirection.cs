using UnityEngine;

namespace Game
{
	/// <summary>
	/// Utils for converting too or from the four cardinal directions
	/// </summary>
	public enum EDirection
	{
		None,
		Right,
		Up,
		Left,
		Down,
	}

	public static class DirectionUtils
	{
		public static EDirection GetRandomDirection()
		{
			return (EDirection)Random.Range(1, 5);
		}
		
		public static EDirection GetRandomDirectionOtherThan(EDirection otherDirection)
		{
			int otherDirectionAsInt = (int)otherDirection;
			if (otherDirectionAsInt == 0)
			{
				return GetRandomDirection();
			} 
			
			int directionRoll = Random.Range(1, 4);
			
			if (otherDirectionAsInt <= directionRoll)
			{
				directionRoll += 1;
			}
			
			return (EDirection)directionRoll;
		}

		public static EDirection GetBestDirectionForVector(Vector2 direction)
		{
			if (direction.sqrMagnitude == 0f)
			{
				return EDirection.None;
			}
			// X wins in case of ties
			if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
			{
				if (direction.x > 0f)
				{
					return EDirection.Right;
				}

				return EDirection.Left;
			}

			if (direction.y > 0f)
			{
				return EDirection.Up;
			}

			return EDirection.Down;
		}

		public static Vector2Int GetVector(this EDirection direction)
		{
			switch (direction)
			{
				case EDirection.Down: return Vector2Int.down;
				case EDirection.Left: return Vector2Int.left;
				case EDirection.Right: return Vector2Int.right;
				case EDirection.Up: return Vector2Int.up;
				default: return Vector2Int.zero;
			} 
		}
	}
}