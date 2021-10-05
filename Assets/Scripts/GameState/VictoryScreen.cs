using TirUtilities.Extensions;
using TirUtilities.Signals;
using UnityEngine;

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
    /// Updated:  Oct 04, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class VictoryScreen : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField] private RectTransform _panel;

        #endregion

        #region Events & Signals

        [SerializeField] private Signal _victorySignal;

        #endregion

        #region Unity Messages

        private void Awake() => AssignReceivers();

        private void OnDestroy() => RemoveReceivers();

        #endregion

        #region Setup & Teardown

        private void AssignReceivers()
        {
            if (_victorySignal.NotNull())
                _victorySignal.AddReceiver(Open);
        }

        private void RemoveReceivers()
        {
            if (_victorySignal.NotNull())
                _victorySignal.RemoveReceiver(Open);
        }

        #endregion

        #region Public Methods

        public void Open() => _panel.gameObject.SetActive(true);
        public void Close() => _panel.gameObject.SetActive(false);
        public void Toggle() => _panel.gameObject.SetActive(!_panel.gameObject.activeSelf);
        public void SetOpen(bool state) => _panel.gameObject.SetActive(state);

        #endregion

        #region Public Properties

        public bool IsOpen => _panel.gameObject.activeSelf;

        #endregion
    }
}