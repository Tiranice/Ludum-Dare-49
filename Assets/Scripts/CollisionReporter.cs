using System.Collections;
using System.Collections.Generic;
using TirUtilities.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare49
{
    ///<!--
    /// CollisionReporter.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 03, 2021
    /// Updated:  Oct 03, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider))]
    public class CollisionReporter : MonoBehaviour
    {
        public CollisionEvent OnCollisionEntered;

        private void OnCollisionEnter(Collision collision) => 
            OnCollisionEntered.SafeInvoke(collision);
    }

    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }
}