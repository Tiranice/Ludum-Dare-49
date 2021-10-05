using System.Collections.Generic;
using TirUtilities.Extensions;
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

        private void OnDestroy()
        {
            CarrySocket.OnCarrying -= CarrySocket_OnCarrying;
            _playerSprintSignal.RemoveReceiver(SprintReceiver);
            InputUser.onChange -= InputUser_onChange;
        }

        private void InputUser_onChange(InputUser user, InputUserChange change, InputDevice device)
        {
            if (change == InputUserChange.ControlSchemeChanged)
                UpdateSprites(user.controlScheme.Value.name);
        }

        private void SprintReceiver(bool val) => _sprint.gameObject.SetActive(!val);

        private void CarrySocket_OnCarrying(bool val)
        {
            if (_throw.IsNull() || _interact.IsNull()) return;
            _throw.gameObject.SetActive(val);
            _interact.gameObject.SetActive(!val);
        }

        private void UpdateSprites(string name)
        {
            if (_keyboardSprites.IsNullOrEmpty() || _controllerSprites.IsNullOrEmpty())
                return;

            _keyboardSprites.ForEach(g => { if (g.NotNull()) g.SetActive(name == "Keyboard&Mouse"); });
            _controllerSprites.ForEach(g => { if (g.NotNull()) g.SetActive(name == "Gamepad"); });
        }

        #endregion
    }
}