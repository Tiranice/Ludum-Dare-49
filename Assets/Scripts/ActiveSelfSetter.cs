using TirUtilities.Extensions;
using TirUtilities.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare49
{
    ///<!--
    /// ToggleOff.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 03, 2021
    /// Updated:  Oct 07, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class ActiveSelfSetter : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField] private bool _defaultState;
        [SerializeField] private BoolSignal _lockSignal;

        [SerializeField] private bool _isLocked = false;

        #endregion

        #region Private Fields

        private bool _isDead = false;

        #endregion

        #region Events & Signals

        public UnityEvent OnDisabled;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            if (_lockSignal.NotNull())
                _lockSignal.AddReceiver(LockReceiver);
            gameObject.SetActive(_defaultState);
        }

        private void OnDestroy()
        {
            if (_lockSignal.NotNull())
                _lockSignal.RemoveReceiver(LockReceiver);
        }

        #endregion

        #region Receivers

        private void LockReceiver(bool val)
        {
            _isLocked = val;
            ToggleOn();
        }

        #endregion

        public void ToggleOff()
        {
            if (_isLocked || _isDead) return;
            gameObject.SetActive(false);
            OnDisabled.SafeInvoke();
        }

        public void ToggleOn()
        {
            if (_isLocked || _isDead) return;
            gameObject.SetActive(true);
        }

        public void ToggleOffLocked()
        {
            if (_isLocked || _isDead) return;
            ToggleOff();
            _isLocked = true;
            _isDead = true;
            OnDisabled.SafeInvoke();
        }

        public void ToggleOnLocked()
        {
            if (_isLocked || _isDead) return;
            ToggleOn();
            _isLocked = true;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}