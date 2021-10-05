using TirUtilities.Extensions;
using TirUtilities.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare49.TirUtilitiesWrapper
{
    ///<!--
    /// TriggerVolumeSignal.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 01, 2021
    /// Updated:  Oct 04, 2021
    /// -->
    /// <summary>
    /// A <see cref="Signal"/> that emits a bool and a game object.
    /// </summary>
    [CreateAssetMenu(menuName = "Signals/Trigger Volume Signal", order = 30)]
    public class TriggerVolumeSignal : SignalBase, ISignal<bool, GameObject>
    {
        #region Actions

        /// <summary>
        /// Invoked in <see cref="Emit(bool, GameObject)"/>, calling receivers.
        /// </summary>
        [SerializeField]
        protected UnityAction<bool, GameObject> _OnEmit;

        #endregion

        #region Public Methods

        /// <summary>
        /// Register a callback function to be invoked when <see cref="Emit(bool, GameObject)"/> is called.
        /// </summary>
        /// <param name="receiver">The callback to be invoked.</param>
        public virtual void AddReceiver(UnityAction<bool, GameObject> receiver) => _OnEmit += receiver;

        /// <summary> Unregister a callback function. </summary>
        /// <param name="receiver">The callback function.</param>
        public virtual void RemoveReceiver(UnityAction<bool, GameObject> receiver) => _OnEmit -= receiver;

        /// <summary>
        /// Emit this signal to all receivers, calling methods registered with 
        /// <see cref="AddReceiver(UnityAction{bool, GameObject})"/>.
        /// </summary>
        public virtual void Emit(bool entered, GameObject target) => _OnEmit.SafeInvoke(entered, target);

        #endregion
    }
}