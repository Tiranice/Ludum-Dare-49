using Sirenix.OdinInspector;
using TirUtilities.Extensions;
using TirUtilities.Signals;
using UnityEngine;
using UnityEngine.Events;

using static TirUtilities.TirLogger;

namespace LudumDare49
{
    ///<!--
    /// Destructable.cs
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
    public class Destructable : MonoBehaviour
    {
        #region Inspector Fields

        [Title("Parts")]
        [SerializeField] private GameObject _solid;
        [SerializeField] private GameObject _composite;

        [Title("Settings")]
        [SerializeField] private float _breakForce = 20.0f;

        #endregion

        #region Events & Signals

        [Title("Events")]
        public UnityEvent OnDestroyed;

        [Title("Signals")]
        [SerializeField] private GameObjectSignal _destructionSignal;

        #endregion

        #region Public Methods

        public void CheckExplosion(Collision collision)
        {
            LogCall();
            if (!collision.gameObject.TryGetComponent(out Bomb bomb) || bomb.ExplosiveForce < _breakForce)
                return;

            Debug.Log($"Exploding {gameObject.name}");

            _solid.SetActive(false);
            _composite.SetActive(true);
            var rigidbodies = _composite.GetComponentsInChildren<Rigidbody>();
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.AddExplosionForce(bomb.ExplosiveForce,
                                            bomb.transform.position,
                                            bomb.ExplosionRadius,
                                            bomb.UpwardsForce);
            }

            if (_destructionSignal.NotNull())
                _destructionSignal.Emit(gameObject);

            OnDestroyed.SafeInvoke();
        }

        #endregion

        #region Private Properties

        #endregion

        #region Public Properties

        public bool IsDestroyed => _composite.gameObject.activeSelf;

        #endregion

        #region Debug

        [ContextMenu(nameof(CheckIsDestroyed))]
        private void CheckIsDestroyed() => Debug.Log($"{name} : {IsDestroyed}");

        #endregion
    }
}