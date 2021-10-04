using Sirenix.OdinInspector;
using TirUtilities;
using TirUtilities.Extensions;
using TirUtilities.Signals;
using UnityEngine;

namespace LudumDare49
{
    ///<!--
    /// CarrySocket.cs
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
    public class CarrySocket : MonoBehaviour
    {
        #region Inspector Fields

        [Title("Settings")]
        [SerializeField] private float _throwPower = 10.0f;
        [SerializeField] private float _holdForce = 5.0f;
        [SerializeField] private float _carriedDrag = 20.0f;
        [SerializeField] private Transform _cameraTransform;

        [Header("Debug")]
        [SerializeField, DisplayOnly] private GameObject _carriedObject;

        #endregion


        #region Events & Signals

        [Header("Signals")]
        [SerializeField] private GameObjectSignal _socketSignal;
        [SerializeField] private BoolSignal _playerThrowSignal;

        public static event System.Action<bool> OnCarrying;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            if (_cameraTransform.IsNull()) _cameraTransform = Camera.main.transform;

            _socketSignal.AddReceiver(CarryObject);
            _playerThrowSignal.AddReceiver(ThrowReceiver);
        }

        private void FixedUpdate()
        {
            if (!IsCarrying) return;

            var target = _cameraTransform.position + _cameraTransform.forward * transform.localPosition.z;
            target.y += transform.localPosition.y;

            var direction = target - _carriedObject.transform.position;

            _carriedObject.GetComponent<Rigidbody>().AddForce(direction * _holdForce, ForceMode.VelocityChange);
        }

        private void OnDestroy()
        {
            _socketSignal.RemoveReceiver(CarryObject);
            _playerThrowSignal.AddReceiver(ThrowReceiver);
        }

        #endregion

        #region Signal Receivers

        private void ThrowReceiver(bool shouldThrow)
        {
            if (!IsCarrying) return;

            _carriedObject.TryGetComponent(out Rigidbody rigidbody);
            _carriedObject.TryGetComponent(out Collider collider);

            collider.enabled = true;

            var forceVector = _cameraTransform.forward * _throwPower;

            DropCarriedObject();
            rigidbody.AddForce(forceVector);
        }

        #endregion

        #region Carry Methods

        private void CarryObject(GameObject target)
        {
            if (IsCarrying) return;

            _carriedObject = target;
            _carriedObject.TryGetComponent(out Rigidbody rigidbody);
            rigidbody.useGravity = false;
            rigidbody.drag = _carriedDrag;
            rigidbody.angularDrag = _carriedDrag;
            OnCarrying?.Invoke(true);
        }

        public void DropCarriedObject()
        {
            if (!IsCarrying) return;

            _carriedObject.TryGetComponent(out Rigidbody rigidbody);
            rigidbody.useGravity = true;
            rigidbody.drag = 0.0f;
            rigidbody.angularDrag = 0.05f;
            _carriedObject = null;
            OnCarrying?.Invoke(false);
        }

        #endregion

        #region Public Properties

        public GameObject CarriedObject => _carriedObject;
        public bool IsCarrying => _carriedObject.NotNull();

        #endregion
    }
}