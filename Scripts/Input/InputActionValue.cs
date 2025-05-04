using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>
    /// Casses which would be updating a single value type by reading
    /// input from the Unity Action
    /// </summary>
    public class InputActionValue<T> : InputActionBase where T : struct
    {

        #region Properties

        public T CurrentValue { get; protected set; }

        #endregion

        #region Constructors

        public InputActionValue( InputActionReference action ) : base( action )
        {
        }

        #endregion

        #region Protected Methods

        protected T ReadValue() => ActionRef.ReadValue<T>();

        protected override void UpdateStateFromAction( float time, float deltaTime )
        {
            CurrentValue = ReadValue();
        }

        #endregion

    }

    public class InputActionFloat : InputActionValue<float>
    {

        #region Constructors

        public InputActionFloat( InputActionReference action ) : base( action )
        {
        }

        #endregion

    }

    public class InputActionVector2 : InputActionValue<Vector2>
    {

        #region Statics and Constants

        private const float GATE_SIZE     = 0.3f;
        private const float GATE_SIZE_SQR = GATE_SIZE * GATE_SIZE;

        #endregion
        
        public bool WasPressedThisFrame { get; private set; }
        public bool IsPressed { get; private set; }

        #region Fields

        private readonly bool m_clamped;
        private readonly bool m_gated;

        #endregion

        #region Constructors

        public InputActionVector2( InputActionReference action, bool gated = false, bool clamped = true ) : base( action )
        {
            m_gated   = gated;
            m_clamped = clamped;
        }

        #endregion

        public EDirection GetRoundedDirection()
        {
            if (!IsPressed)
            {
                return EDirection.None;
            }

            if (Mathf.Abs(CurrentValue.x) > Mathf.Abs(CurrentValue.y))
            {
                if (CurrentValue.x > 0f)
                {
                    return EDirection.Right;
                }
                else
                {
                    return EDirection.Left;
                }
            }
            else
            {
                if (CurrentValue.y > 0f)
                {
                    return EDirection.Up;
                }
                else
                {
                    return EDirection.Down;
                }
            }
        }

        #region Protected Methods

        protected override void UpdateStateFromAction( float time, float deltaTime )
        {
            bool wasPressed = IsPressed;
            
            Vector2 readValue = ReadValue();
            if ( m_clamped )
            {
                readValue = Vector2.ClampMagnitude( readValue, 1f );
            }

            if (m_gated && readValue.sqrMagnitude > GATE_SIZE_SQR)
            {
                readValue = Vector2.zero;
            }
             
            IsPressed           = readValue.sqrMagnitude > 0f;
            CurrentValue        = readValue;
            WasPressedThisFrame = IsPressed && !wasPressed;
        }

        #endregion

    }

}