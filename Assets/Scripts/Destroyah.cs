using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare49
{
    ///<!--
    /// Destroyah.cs
    /// 
    /// Project:  TirBombardier — Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 05, 2021
    /// Updated:  Oct 05, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public class Destroyah : MonoBehaviour
    {
        public void Bip(bool entered, GameObject target) => Destroy(target);
    }
}