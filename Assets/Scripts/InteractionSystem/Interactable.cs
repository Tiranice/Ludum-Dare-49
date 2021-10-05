using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TirUtilities.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare49.Interaction
{
    ///<!--
    /// Interactable.cs
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
    public class Interactable : MonoBehaviour, IInteractable
    {
        #region Inspector Fields

        [SerializeField] private string _interactionType = "No Name";

        #endregion

        #region Events & Signals

        [Title("Events")]
        public UnityEvent OnInteracted;

        #endregion

        #region Interaction Properties

        public virtual string InteractionType => _interactionType;

        public virtual bool CanInteract { get; } = true;

        #endregion

        #region Interaction Events

        public event System.Action<IInteractable> OnInteract;

        #endregion

        #region Interaction Methods

        public virtual void Interact() => OnInteract?.Invoke(this);

        #endregion

        #region Unity Messages

        protected virtual void Awake() => OnInteract += _ => OnInteracted.SafeInvoke(); 
        
        #endregion
    }
}