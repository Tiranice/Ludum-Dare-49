using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TirUtilities.Extensions;
using TirUtilities.Signals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare49.GameStateControl
{
    ///<!--
    /// GameState.cs
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
    public class GameState : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private List<Destructable> _destructables = new List<Destructable>();

        #endregion

        #region Events & Signals

        [Title("Signals")]
        [SerializeField] private Signal _victorySignal;
        [SerializeField] private GameObjectSignal _destructionSignal;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            _destructables = FindObjectsOfType<Destructable>().ToList();

            if (_playerInput.IsNull())
                _playerInput = FindObjectOfType<PlayerInput>();

            AssignReceivers();
        }

        private void OnDestroy()
        {
            if (_destructionSignal.NotNull())
                _destructionSignal.RemoveReceiver(DestrucitonReceiver);
        }

        #endregion

        #region Setup & Teardown

        private void AssignReceivers()
        {
            if (_destructionSignal.NotNull())
                _destructionSignal.AddReceiver(DestrucitonReceiver);
        }

        private void RemoveReceivers()
        {
            if (_destructionSignal.NotNull())
                _destructionSignal.RemoveReceiver(DestrucitonReceiver);
        }

        #endregion

        #region Receivers

        private void DestrucitonReceiver(GameObject destroyed)
        {
            if (!destroyed.TryGetComponent(out Destructable destructable))
                return;

            _ = destructable;

            if (AllDestroyed && _victorySignal.NotNull())
            {
                _playerInput.SwitchCurrentActionMap("UI");
                Cursor.lockState = CursorLockMode.None;
                _victorySignal.Emit();
            }
        }

        #endregion

        #region Helpers

        private bool AllDestroyed => !_destructables.Any(d => d.IsDestroyed is false);

        #endregion

        #region Debug

        [ContextMenu(nameof(TestAllDestroyed))]
        private void TestAllDestroyed()
        {
            Debug.Log($"{_destructables.Count}  {AllDestroyed}");
        }

        #endregion
    }
}