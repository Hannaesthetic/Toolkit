using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Game
{
    /// <summary>
    /// Input manager! Has and updates inputs!
    /// Has been set to update early in custom execution order, to avoid weird edge cases
    /// where some objects run Update before this, and some after
    /// https://docs.unity3d.com/6000.0/Documentation/Manual/class-MonoManager.html
    /// </summary>
    [DefaultExecutionOrder( -300 )]
    public abstract class InputManager : ManagerBase
    {

        #region Enums

        public enum EInputType
        {
            None,
            MouseAndKeyboard,
            Controller,
        }

        #endregion

        #region Fields

        protected InputActionBase[] inputs;

        #endregion

        #region Properties

        // For each action, have it
        // 1. defined here with both a an InputActionReference and an Action to be accessed (see example below)
        // 2. initialized in InitializeActions()
        // 3. added to the list in GenerateActionList()

        // [SerializeField] private InputActionReference m_ExampleAction;
        // public Action ExampleAction { get; private set; }

        public InputDevice CurrentDevice { get; private set; }
        public EInputType  InputType     { get; private set; }

        #endregion

        #region Updates

        public void Update()
        {
            float time      = Time.time;
            float deltaTime = Time.deltaTime;
            for ( int i = 0; i < inputs.Length; i++ )
            {
                inputs[ i ].Update( time, deltaTime );
            }
        }

        #endregion

        #region Public Methods

        private static Plane ORIGIN_PLANE = new Plane(Vector3.back, Vector3.zero);
        
        public bool TryGetCursorPosition( out Vector2 position )
        {
            if ( Mouse.current == null )
            {
                position = Vector2.zero;
                return false;
            }

            position = Mouse.current.position.value;
            return true;
        }

        #endregion

        #region Protected Methods

        /// <summary> should return a list of all actions to be managed </summary>
        protected abstract InputActionBase[] GenerateActionList();

        protected override void OnInit()
        {
            InputUser.onChange += InputUserEvt_OnChange;

            InitializeActions();
            inputs = GenerateActionList();

            // TODO: Uhhhhh
            InputType = EInputType.MouseAndKeyboard;
        }

        /// <summary> Should create new instances for all actions </summary>
        protected abstract void InitializeActions();

        #endregion

        #region Event Listeners

        private void InputUserEvt_OnChange( InputUser user, InputUserChange userChange, InputDevice device )
        {
            if ( userChange == InputUserChange.ControlSchemeChanged )
            {
                string name = user.controlScheme != null ? user.controlScheme.Value.name : "NONE";
                Debug.LogError( $"Switched to control scheme: {name}" );
                CurrentDevice = device;
            }
        }

        #endregion

    }
}