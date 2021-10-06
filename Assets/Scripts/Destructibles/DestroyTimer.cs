using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LudumDare49
{
    ///<!--
    /// DestroyTimer.cs
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
    public class DestroyTimer : MonoBehaviour
    {
        [SerializeField, MinMaxSlider(0.0f, 600.0f, true)]
        private Vector2 _timeRange = new Vector2(30.0f, 60.0f);

        private void OnEnable()
        {
            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(Random.Range(_timeRange.x, _timeRange.y));

            Destroy(gameObject);
        }
    }
}