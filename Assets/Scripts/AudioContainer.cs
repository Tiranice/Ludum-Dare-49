using System.Collections.Generic;
using TirUtilities.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare49.Audio
{
    ///<!--
    /// AudioContainer.cs
    /// 
    /// Project:  TirBombardier — Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Phoenix Software
    /// Created:  Oct 15, 2021
    /// Updated:  Oct 15, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioContainer : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> _audioClips;

        #endregion

        #region Unity Messages

        private void Update()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame)
                PlayRandomClip();
        }

        #endregion

        #region Public Methods

        public void PlayRandomClip()
        {
            if (_audioSource.IsNull()) TryGetComponent(out _audioSource);
            if (_audioClips.IsNullOrEmpty()) return;
            _audioSource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Count - 1)]);
        }

        #endregion
    }
}