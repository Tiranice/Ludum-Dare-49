using System;
using System.Collections.Generic;
using TirUtilities.Controllers.Experimental;
using TirUtilities.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace LudumDare49
{
    ///<!--
    /// ControlsUI.cs
    /// 
    /// Project:  TirBombardier — Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 04, 2021
    /// Updated:  Oct 04, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class ControlsUI : MonoBehaviour
    {
        #region Data Structures

        #endregion

        #region Inspector Fields

        [SerializeField] private RectTransform _move;
        [Space]
        [SerializeField] private RectTransform _look;
        [Space]
        [SerializeField] private RectTransform _throw;
        [Space]
        [SerializeField] private RectTransform _sprint;
        [Space]
        [SerializeField] private RectTransform _interact;
        [Space]
        [SerializeField] private List<GameObject> _controllerSprites;
        [SerializeField] private List<GameObject> _keyboardSprites;

        #endregion

        #region Events & Signals

        [SerializeField] private BoolSignal _playerSprintSignal;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            CarrySocket.OnCarrying += CarrySocket_OnCarrying;
            _playerSprintSignal.AddReceiver(SprintReceiver);
            InputUser.onChange += InputUser_onChange;
        }

        private void InputUser_onChange(InputUser user, InputUserChange change, InputDevice device)
        {
            if (change == InputUserChange.ControlSchemeChanged)
            {
                UpdateSprites(user.controlScheme.Value.name);
            }
        }

        private void InputSystem_onDeviceChange(InputDevice arg1, InputDeviceChange arg2)
        {
            Debug.Log(arg1);
            Debug.Log(arg2);
        }
        private void SprintReceiver(bool val) => _sprint.gameObject.SetActive(!val);

        private void CarrySocket_OnCarrying(bool val)
        {
            _throw.gameObject.SetActive(val);
            _interact.gameObject.SetActive(!val);
        }

        private void UpdateSprites(string name)
        {
            Debug.Log(name);
            _keyboardSprites.ForEach(g => g.SetActive(name == "Keyboard&Mouse"));
            _controllerSprites.ForEach(g => g.SetActive(name == "Gamepad"));
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion

        #region Private Properties

        #endregion

        #region Public Properties

        #endregion
    }
}