using System.Collections.Concurrent;
using TirUtilities.Detection.Experimental;
using TirUtilities.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare49.Crafting
{
    using LudumDare49.Interaction;
    ///<!--
    /// StorageZone.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 02, 2021
    /// Updated:  Oct 04, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class StorageZone : Interactable
    {
        #region Inspector Fields

        [SerializeField] private Zone _zoneSettings = (10.0f, 20.0f);
        [Space]
        [SerializeField] private int _storageLimit = 1;
        [SerializeField] private ReagentData _boundReagentType;
        public ReagentData BoundReagentType => _boundReagentType;
        [Space]
        [SerializeField] private CarrySocket _carrySocket;
        [SerializeField] private Transform _placementSocket;
        [Space]
        [SerializeField] private GameObject _selectButton;

        #endregion

        #region Private Fields

        private readonly ConcurrentBag<GameObject> _storedObjects = new ConcurrentBag<GameObject>();
        private readonly int _colorId = Shader.PropertyToID("_BaseColor");
        private MaterialPropertyBlock _selectColor; 

        #endregion


        #region Events & Signals

        //TODO : Play a sound effect from this.
        public UnityEvent OnWrongStorageZone;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _selectColor = new MaterialPropertyBlock();
            _selectColor.SetColor(_colorId, Color.green);
            if (_carrySocket.IsNull())
                _carrySocket = FindObjectOfType<CarrySocket>();
        }

        private void Update()
        {
            _selectButton.SetActive(!_storedObjects.IsEmpty);
        }

        public override string InteractionType => !_storedObjects.IsEmpty
            ? $"{_boundReagentType.Name} ({_storedObjects.Count} / {_storageLimit})"
            : _carrySocket.IsCarrying 
                ? base.InteractionType + _boundReagentType.Name 
                : $"Storage for {_boundReagentType.Name}";

        public override bool CanInteract => true;

        public override void Interact()
        {
            base.Interact();

            if (_storedObjects.Count >= _storageLimit || !_carrySocket.IsCarrying) return;

            var reagent = _carrySocket.CarriedObject.GetComponent<Reagent>();

            // Wrong reagent type.
            if (reagent.Data != _boundReagentType)
            {
                OnWrongStorageZone.SafeInvoke();
                return;
            }

            _carrySocket.DropCarriedObject();
            reagent.TryGetComponent(out Rigidbody rigidbody);
            
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            _storedObjects.Add(reagent.gameObject);
            reagent.transform.SetPositionAndRotation(_placementSocket.position, _placementSocket.rotation);
        }

        public void SelectReagent()
        {
            var craftingSystem = FindObjectOfType<CraftingSystem>();
            if (!craftingSystem.SelectedReagents.Contains(_boundReagentType))
            {
                craftingSystem.SelectedReagents.Add(_boundReagentType);
                craftingSystem.LookupRecipe();
                _selectButton.GetComponent<Renderer>().SetPropertyBlock(_selectColor);
            }
        }

        public void ReturnReagent()
        {
            if (_storedObjects.TryTake(out var reagent))
            {
                reagent.GetComponent<Reagent>().Return();
                _selectButton.GetComponent<Renderer>().SetPropertyBlock(null);
            }
        }

        #region Gizmo
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_zoneSettings.DrawGizmo) return;

            Gizmos.color = new Color(0.25f, 1.0f, 0.0f, 0.5f);
            Gizmos.DrawMesh(mesh: _zoneSettings.GizmoMesh,
                            position: transform.position,
                            rotation: Quaternion.identity,
                            scale: new Vector3(_zoneSettings.Diameter,
                                               _zoneSettings.HalfHeight,
                                               _zoneSettings.Diameter)
                            );
        }
#endif     
        #endregion
    }
}