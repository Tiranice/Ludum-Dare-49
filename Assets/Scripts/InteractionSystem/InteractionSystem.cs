using TirUtilities;
using TirUtilities.Extensions;
using TirUtilities.Signals;
using TMPro;
using UnityEngine;

namespace LudumDare49.Interaction
{
    ///<!--
    /// InteractionSystem.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 01, 2021
    /// Updated:  Oct 04, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class InteractionSystem : MonoBehaviour
    {
        #region Inspector Fields

        [Header("UI Refs")]
        [SerializeField] private RectTransform _interactionPanel;
        [SerializeField] private TMP_Text _interactionText;

        #endregion

        #region Interactable

        [SerializeField, DisplayOnly] private IInteractable _interactable;

        #endregion

        #region Events & Signals

        [Header("Signals")]
        [SerializeField] private BoolSignal _playerInteractSignal;
        [SerializeField] private GameObjectSignal _cameraLookSignal;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            //_interactionTriggerSignal.AddReceiver(TriggerReceiver);
            _playerInteractSignal.AddReceiver(InteractInputReceiver);
            _cameraLookSignal.AddReceiver(CameraLookReceiver);
        }

        private void OnDestroy()
        {
            //_interactionTriggerSignal.RemoveReceiver(TriggerReceiver);
            _playerInteractSignal.RemoveReceiver(InteractInputReceiver);
            _cameraLookSignal.RemoveReceiver(CameraLookReceiver);

        }

        #endregion

        #region Signal Receivers

        private void CameraLookReceiver(GameObject target)
        {
            _interactionPanel.gameObject.SetActive(target.NotNull());

            if (!target.NotNull())
            {
                _interactionText.SetText(string.Empty);
                return;
            }

            if (!target.TryGetComponent(out _interactable) || !_interactable.CanInteract) return;
            _interactionText.SetText($"E  {_interactable.InteractionType}");
        }


        private void TriggerReceiver(bool entered, GameObject target)
        {
            _interactionPanel.gameObject.SetActive(entered);

            if (!entered)
            {
                _interactionText.SetText(string.Empty);
                return;
            }

            if (!target.TryGetComponent(out _interactable) || !_interactable.CanInteract) return;
            _interactionText.SetText($"E  {_interactable.InteractionType}");
        }

        private void InteractInputReceiver(bool value)
        {
            if (!_interactionPanel.gameObject.activeSelf || !value || _interactable == null) 
                return;

            _interactable.Interact();
            _interactionPanel.gameObject.SetActive(false);
        }

        #endregion
    }
}