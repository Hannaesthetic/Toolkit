using UnityEngine.InputSystem;

namespace Game
{
	/// <summary>
	/// A base form of InputAction, to be extended into different kinds of Input
	/// Acts as a layer between Unity's Input system and the game, meaning we can extend
	/// it with functionality such as input buffering more easily
	/// </summary>
	public abstract class InputActionBase
	{

		#region Properties

		public InputAction ActionRef { get; }

		#endregion

		#region Constructors

		public InputActionBase( InputActionReference action )
		{
			ActionRef = action;
			ActionRef.Enable();
		}

		#endregion

		#region Public Methods

		public void Update( float time, float deltaTime )
		{
			// don't if paused and cannot run when paused
			UpdateStateFromAction( time, deltaTime );
		}

		#endregion

		#region Protected Methods

		protected abstract void UpdateStateFromAction( float time, float deltaTime );

		#endregion

	}
}