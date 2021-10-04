using System.Collections;
using System.Collections.Generic;
using TirUtilities.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare49
{
    ///<!--
    /// Interactable.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 01, 2021
    /// Updated:  Oct 01, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _interactionType = "No Name";

        public UnityEvent OnInteracted;

        public virtual string InteractionType => _interactionType;

        public virtual bool CanInteract { get; } = true;

        public event System.Action<IInteractable> OnInteract;

        public virtual void Interact() => OnInteract?.Invoke(this);

        protected virtual void Awake() => OnInteract += _ => OnInteracted.SafeInvoke();
    }
}