using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
	/// <summary>
	/// An example implementation for your input manager
	/// Rename it and move it into your own project files when you start up
	/// </summary>
	public class InputManager_YOURGAMENAME : InputManager
	{
		#region Serialized
		[Tooltip("Input action for moving, should be a Vector 2")] [SerializeField]
		private InputActionReference m_Move;

		[Tooltip("Input action for the primary action")] [SerializeField]
		private InputActionReference m_Primary;

		[Tooltip("Input action for the secondary action")] [SerializeField]
		private InputActionReference m_Secondary;
		#endregion

		#region Properties
		public InputActionVector2 Move      { get; private set; }
		public InputActionButton  Primary   { get; private set; }
		public InputActionButton  Secondary { get; private set; }
		#endregion

		#region Protected Methods
		protected override InputActionBase[] GenerateActionList()
		{
			return new InputActionBase[]
			{
				Move,
				Primary,
				Secondary
			};
		}

		protected override void InitializeActions()
		{
			Move      = new InputActionVector2(m_Move);
			Primary   = new InputActionButton(m_Primary);
			Secondary = new InputActionButton(m_Secondary);
		}
		#endregion
	}
}