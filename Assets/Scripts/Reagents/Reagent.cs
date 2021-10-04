using TirUtilities.Core.Experimental;
using TirUtilities.Signals;
using UnityEngine;

namespace LudumDare49
{
    ///<!--
    /// Reagent.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 02, 2021
    /// Updated:  Oct 02, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class Reagent : Interactable, IPoolable
    {
        #region Inspector Fields

        [SerializeField] private ReagentData _data;

        #endregion

        #region Events & Signals

        [Header("Events")]
        // Later
        //TODO : Make this work... somewhere.
        //public UnityEvent OnReagentStored;
        //TODO : Make sure that this event is invoked when the crafting system consumes the reagent!!!
        //public UnityEvent OnReagentUsed;

        [Header("Signals")]
        [SerializeField] private GameObjectSignal _carrySocketSignal;

        #endregion

        #region Interaction & Poolable

        public event System.Action ReturnAction;

        public override void Interact()
        {
            base.Interact();

            if (FindObjectOfType<CarrySocket>().IsCarrying) return;

            TryGetComponent(out Collider collider);
            collider.enabled = false;
            _carrySocketSignal.Emit(gameObject);
        }

        public void Return()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            ReturnAction?.Invoke();
        }

        #endregion

        public override string InteractionType => $"{base.InteractionType} {Data.Name}";
        public ReagentData Data => _data;
    }
}