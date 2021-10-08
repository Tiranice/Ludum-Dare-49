using TirUtilities.Experimental;
using TirUtilities.Extensions;
using TirUtilities.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare49.GameStateControl
{
    ///<!--
    /// VictoryScreen.cs
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
    public class VictoryScreen : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField] private RectTransform _panel;

        private ApplicationStateMachine _applicationStateMachine;

        #endregion

        #region Events & Signals

        [Header("Signals")]
        [SerializeField] private Signal _victorySignal;

        [Header("Events")]
        public UnityEvent OnOpened;
        public UnityEvent OnClosed;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            _applicationStateMachine = FindObjectOfType<ApplicationStateMachine>();
            AssignReceivers();
        }

        private void OnDestroy() => RemoveReceivers();

        #endregion

        #region Setup & Teardown

        private void AssignReceivers()
        {
            if (_victorySignal.NotNull())
                _victorySignal.AddReceiver(Open);

            OnOpened.AddListener(_applicationStateMachine.BlockPause);
            OnClosed.AddListener(_applicationStateMachine.AllowPause);
        }

        private void RemoveReceivers()
        {
            if (_victorySignal.NotNull())
                _victorySignal.RemoveReceiver(Open);

            OnOpened.RemoveListener(_applicationStateMachine.BlockPause);
            OnClosed.RemoveListener(_applicationStateMachine.AllowPause);
        }

        #endregion

        #region Public Methods

        public void Open() { _panel.gameObject.SetActive(true); OnOpened.SafeInvoke(); }

        public void Close() { _panel.gameObject.SetActive(false); OnClosed.SafeInvoke(); }

        public void Toggle() { if (_panel.gameObject.activeSelf) Close(); else Open(); }

        public void SetOpen(bool state) { if (state) Open(); else Close(); }

        #endregion

        #region Public Properties

        public bool IsOpen => _panel.gameObject.activeSelf;

        #endregion
    }
}
