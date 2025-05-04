using UnityEngine.InputSystem;

namespace Game
{
	/// <summary>
	/// Input Action for binary buttons, including functionality for checking
	/// if its was pressed or released this frame
	/// </summary>
	public class InputActionButton : InputActionBase
	{

		#region Fields

		private bool m_isPressed;

		#endregion

		#region Properties

		public bool IsPressed            { get; protected set; }
		public bool WasPressedThisFrame  { get; protected set; }
		public bool WasReleasedThisFrame { get; protected set; }

		#endregion

		#region Constructors

		public InputActionButton(InputActionReference action) : base(action)
		{ }

		#endregion

		#region Protected Methods

		protected override void UpdateStateFromAction(float time, float deltaTime)
		{
			bool pressed = ActionRef.IsPressed();

			bool wasPressed = IsPressed;

			IsPressed            = pressed;
			WasPressedThisFrame  = pressed  && !wasPressed;
			WasReleasedThisFrame = !pressed && wasPressed;
		}

		#endregion

	}
}