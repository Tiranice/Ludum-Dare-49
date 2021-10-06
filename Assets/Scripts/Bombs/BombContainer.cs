using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare49
{
    ///<!--
    /// BombContainer.cs
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
    public class BombContainer : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField] private List<GameObject> _bombBag = new List<GameObject>();
        [SerializeField] private float _waitTime = 0.1f;

        #endregion

        #region Public Methods

        public void StartActivation() => StartCoroutine(ActivateBombs());

        public IEnumerator ActivateBombs()
        {
            yield return new WaitForSeconds(_waitTime);

            foreach (var bomb in _bombBag)
                bomb.SetActive(true);
        }

        #endregion
    }
}