using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace TirUtilities.Controllers.Experimental
{
    using TirUtilities.Extensions;
    using TirUtilities.Signals;
    ///<!--
    /// InputSignals.cs
    /// 
    /// Project:  TirUtilities
    ///        
    /// Author :  Devon Wilson
    /// Company:  BlackPheonixSoftware
    /// Created:  Sep 26, 2021
    /// Updated:  Oct 04, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class InputSignalReceiver : MonoBehaviour
    {
        #region Inspector Fields

        [Header("Debug")]
        [SerializeField] private bool _verboseLogging = false;

        [Header("Player Input Values")]
        [SerializeField] private PlayerInput _playerInput;
        [Space]
        [SerializeField, DisplayOnly] private Vector2 _move;
        [SerializeField, DisplayOnly] private Vector2 _look;
        [SerializeField, DisplayOnly] private bool _isJumping;
        [SerializeField, DisplayOnly] private bool _isSprinting;
        [SerializeField, DisplayOnly] private bool _isInteracing;
        [SerializeField, DisplayOnly] private bool _shouldThrow;


        //[Header("Movement Settings")]
        //[SerializeField] private bool _analogMovement;

        #endregion

        #region Events & Signals

        [Header("Signals")]
        [SerializeField] private Vector2Signal _moveSignal;
        [SerializeField] private Vector2Signal _lookSignal;
        [SerializeField] private BoolSignal _jumpSignal;
        [SerializeField] private BoolSignal _sprintSignal;
        [SerializeField] private BoolSignal _interactInputSignal;
        [SerializeField] private BoolSignal _throwSignal;
        [SerializeField] private Signal _pauseSignal;

        public static event System.Action<PlayerInput> OnActionMapChanged;

        #endregion

        private void Awake()
        {
            _playerInput.onControlsChanged += _playerInput_onControlsChanged;
        }

        private void _playerInput_onControlsChanged(PlayerInput obj) => OnActionMapChanged?.Invoke(obj);

        #region Input Messages

        public void OnMove(InputValue value) => MoveInput(value.Get<Vector2>());
        public void OnLook(InputValue value) => LookInput(value.Get<Vector2>());
        public void OnJump(InputValue value) => JumpInput(value.isPressed);
        public void OnSprint(InputValue value) => SprintInput(value.isPressed);
        public void OnInteract(InputValue value) => InteractInput(value.isPressed);
        public void OnThrow(InputValue value) => ThrowInput(value.isPressed);
        public void OnPause(InputValue value) => PauseInput();

        #endregion

        #region Input Methods

        public void MoveInput(Vector2 moveDirection)
        {
            _move = moveDirection;
            _moveSignal.Emit(moveDirection);

            if (_verboseLogging) Debug.Log($"Move Input => {moveDirection}");
        }

        public void LookInput(Vector2 lookDirection)
        {
            _look = lookDirection;
            _lookSignal.Emit(lookDirection);

            if (_verboseLogging) Debug.Log($"Look Input => {lookDirection}");
        }

        public void JumpInput(bool jumpState)
        {
            _isJumping = jumpState;
            _jumpSignal.Emit(jumpState);

            if (_verboseLogging) Debug.Log($"Jump Input => {jumpState}");
        }

        public void SprintInput(bool sprintState)
        {
            _isSprinting = sprintState;
            _sprintSignal.Emit(sprintState);

            if (_verboseLogging) Debug.Log($"Sprint Input => {sprintState}");
        }

        public void InteractInput(bool interactState)
        {
            _isInteracing = interactState;
            _interactInputSignal.Emit(interactState);

            if (_verboseLogging) Debug.Log($"Sprint Input => {interactState}");
        }

        public void ThrowInput(bool shouldThrow)
        {
            _shouldThrow = shouldThrow;
            _throwSignal.Emit(shouldThrow);

            if (_verboseLogging) Debug.Log($"Sprint Input => {shouldThrow}");
        }

        public void PauseInput()
        {
            _pauseSignal.Emit();

            //if (_verboseLogging) Debug.Log($"Sprint Input => {}");
        }

        #endregion

        #region Cursor Lock
#if !UNITY_IOS || !UNITY_ANDROID
        private void OnApplicationFocus(bool hasFocus) => SetCursorLockState(hasFocus);

        private void SetCursorLockState(bool hasFocus)
        {
            Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
        }
#endif
        #endregion
    }
}
