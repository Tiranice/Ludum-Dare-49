using System.Collections;
using System.Collections.Generic;
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
    /// Updated:  Oct 03, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class ActiveSelfSetter : MonoBehaviour
    {
        [SerializeField] private bool _defaultState;
        [SerializeField] private BoolSignal _lockSignal;

        [SerializeField] private bool _isLocked = false;

        private bool _isDead = false;

        public UnityEvent OnDisabled;

        private void Awake()
        {
            if (_lockSignal.NotNull())
                _lockSignal.AddReceiver(val => 
                { 
                    _isLocked = val;
                    ToggleOn();
                });
            gameObject.SetActive(_defaultState);
        }

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