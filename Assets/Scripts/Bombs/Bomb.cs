using Sirenix.OdinInspector;
using System.Collections;
using TirUtilities.Extensions;
using TirUtilities.Signals;
using UnityEngine;

namespace LudumDare49
{
    using LudumDare49.Interaction;
    using UnityEngine.Events;

    ///<!--
    /// Bomb.cs
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
    public class Bomb : Interactable
    {
        #region Inspector Fields

        [Title("Explosion Settings")]
        [SerializeField] private float _forceToDetinate = 1.0f;
        [SerializeField] private float _explosiveForce = 10.0f;
        [SerializeField] private float _explosionRadius = 5.0f;
        [SerializeField] private float _upwardsForce = 3.0f;
        [Space]
        [SerializeField, Range(0, 30)] private int _timeToDetonation = 0;
        [SerializeField] private Color _timerColor = Color.red;
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private bool _explodeOnSprint = false;
        
        [Title("Mesh & Collision")]
        [SerializeField] private GameObject _bombMesh;
        [SerializeField] private GameObject _bombTransparent;
        [SerializeField] private LayerMask _targetLayers;

        #endregion

        #region Private Fields

        private bool _isSprinting = false;
        private bool _isMoving = false;
        private bool _isCarried = false;

        private CarrySocket _carrySocket;

        private int _colorId = Shader.PropertyToID("_BaseColor");
        private MaterialPropertyBlock _timerColorBlock;

        private Vector3 _hitPosition = Vector3.zero;

        #endregion

        #region Events & Signals

        [Header("Events")]
        public UnityEvent OnExploded;

        [Header("Signals")]
        [SerializeField] private GameObjectSignal _carrySocketSignal;
        [SerializeField] private Vector2Signal _playerMoveSignal;
        [SerializeField] private BoolSignal _playerSprintSignal;

        #endregion

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();
            _carrySocket = FindObjectOfType<CarrySocket>();
        }

        private void Start() => AssignReceivers();

        private void Update() => ExplodeOnSprint();

        private void OnCollisionEnter(Collision collision) => CheckCollision(collision);

        private void OnDestroy() => RemoveReceivers();

        #endregion

        #region Setup & Teardown

        private void AssignReceivers()
        {
            _playerSprintSignal.AddReceiver(SprintReceiver);
            _playerMoveSignal.AddReceiver(SetIsMoving);
        }

        private void RemoveReceivers()
        {
            _playerSprintSignal.RemoveReceiver(SprintReceiver);
            _playerMoveSignal.RemoveReceiver(SetIsMoving);
        }

        #endregion

        #region Receivers

        private void SetIsMoving(Vector2 val) => _isMoving = val != Vector2.zero;
        private void SprintReceiver(bool val) => _isSprinting = val;

        #endregion

        #region Model State

        public void SetOpaque()
        {
            _bombMesh.SetActive(true);
            _bombTransparent.SetActive(false);
            GetComponent<Collider>().enabled = true;
        }

        public void SetTransparent()
        {
            _bombTransparent.SetActive(true);
            _bombMesh.SetActive(false);
            GetComponent<Collider>().enabled = false;
        }

        #endregion

        #region Explosion

        private void CheckCollision(Collision collision)
        {
            if (!LayerInMask(collision.gameObject.layer)) return;
            if (collision.relativeVelocity.magnitude >= _forceToDetinate)
            {
                _hitPosition = collision.collider.ClosestPointOnBounds(transform.position);
                StartCoroutine(Explode());
            }
        }


        private void ExplodeOnSprint()
        {
            _hitPosition = transform.position + _carrySocket.transform.forward * 1.5f;
            if (_explodeOnSprint && _isSprinting && _isMoving && _isCarried)
                StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            //if (_timeToDetonation > 0)
            //{
            //    StartTimerVisual();
            //    yield return new WaitForSeconds(_timeToDetonation);
            //}

            TryGetComponent(out Rigidbody rigidbody);
            rigidbody.CancelAllVelocity();
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Collider>().enabled = false;

            _explosionEffect.transform.SetParent(null);
            _explosionEffect.transform.SetPositionAndRotation(_hitPosition, Quaternion.identity);
            _explosionEffect.Play();
            _bombMesh.SetActive(false);

            var colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

            foreach (var hit in colliders)
            {
                if (hit.attachedRigidbody.NotNull())
                    hit.attachedRigidbody.AddExplosionForce(_explosiveForce,
                                                            transform.position,
                                                            _explosionRadius,
                                                            _upwardsForce); 
            }
            
            if (_carrySocket.CarriedObject == gameObject)
                _carrySocket.DropCarriedObject();

            OnExploded.SafeInvoke();

            yield return new WaitForSeconds(3.0f);

            Destroy(_explosionEffect.gameObject);
            Destroy(gameObject);
        }

        private void StartTimerVisual() { }

        #endregion

        #region Interaction

        public void SetIsCarried() => _isCarried = true;

        public override void Interact()
        {
            base.Interact();

            if (_carrySocket.IsCarrying) return;

            TryGetComponent(out Collider collider);
            collider.enabled = false;
            TryGetComponent(out Rigidbody rigidbody);
            rigidbody.constraints = RigidbodyConstraints.None;
            _carrySocketSignal.Emit(gameObject);
        }

        #endregion

        #region Public Properties

        public float ExplosiveForce => _explosiveForce;
        public float ExplosionRadius => _explosionRadius;
        public float UpwardsForce => _upwardsForce; 

        #endregion

        public override string InteractionType => $"{base.InteractionType}";
        private bool LayerInMask(int layer) => (1 << layer & _targetLayers) != 0;

        #region Gizmo

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawSphere(transform.position, _explosionRadius);
        }

        #endregion
    }
}