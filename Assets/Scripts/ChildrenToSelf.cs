using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LudumDare49
{
    ///<!--
    /// ChildrenToSelf.cs
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
    public class ChildrenToSelf : MonoBehaviour
    {
        #region Unity Messages

        private void Update()
        {
            GetComponentsInChildren<Transform>().ToList().ForEach(t =>
                t.SetPositionAndRotation(transform.position, transform.rotation)
            );
        }

        #endregion
    }
}