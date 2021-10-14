using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using TirUtilities.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using TirUtilities;
using UnityEditor.AnimatedValues;

namespace LudumDare49
{
    ///<!--
    /// LinkButton.cs
    /// 
    /// Project:  TirBombardier — Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 10, 2021
    /// Updated:  Oct 10, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public sealed class LinkButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Data Structures
        
        [System.Serializable]
        private struct Route
        {
            [SerializeField, DisplayOnly] private string _name;
            [SerializeField, DisplayOnly] private string _websiteRoute;

            public Route(string name, string websiteRoute)
            {
                _name = name;
                _websiteRoute = websiteRoute;
            }

            public string Name => _name;
            public string WebsiteRoute => _websiteRoute;
        }

        private static readonly IEnumerable _routes = new ValueDropdownList<Route>()
        {
            { "Ludum Dare 48", new Route("Ludum Dare 48", "ldj.am/$242513") },
            { "GitHub", new Route("Github – Tiranice", "https://github.com/Tiranice") },
            {  "My Itch.io Page", new Route("Tiranice.Itch.io", "https://tiranice.itch.io/")},
            { "TirUtilites", new Route("TirUtilities v0.0.0-alpha.9.3", "https://github.com/Tiranice/TirUtilities") },
            { "POLYGON Prototype", new Route("POLYGON Prototype - Low Poly 3D Art by Synty", "https://assetstore.unity.com/packages/3d/props/exterior/polygon-prototype-low-poly-3d-art-by-synty-137126") },
            { "Low Poly Weapons", new Route("Bombs - Low Poly Weapons", "https://assetstore.unity.com/packages/3d/props/weapons/low-poly-weapons-71680") },
            { "Free HDR Sky", new Route("Skybox - Free HDR Sky", "https://assetstore.unity.com/packages/2d/textures-materials/sky/free-hdr-sky-61217") },
            { "Prototype Textures", new Route("Kenney Prototype Textures", "https://www.kenney.nl/assets/prototype-textures") },
            { "Fullscreen Editor", new Route("Fullscreen Editor", "https://assetstore.unity.com/packages/tools/utilities/fullscreen-editor-69534") },
            { "Odin Inspector", new Route("Odin Inspector", "https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041") },
            { "Rainbow Folders", new Route("Rainbow Folders", "https://assetstore.unity.com/packages/tools/utilities/rainbow-folders-2-143526") },
            { "Inkscape", new Route("Inkscape", "https://inkscape.org/") },
            { "Masterplan", new Route("Masterplan", "https://solarlune.itch.io/masterplan") },
            { "Unity Cloud Build", new Route("Unity Cloud Build", "https://unity.com/features/cloud-build") },
            { "Cinemachine 2.8.1", new Route("Cinemachine 2.8.1", "https://unity.com/unity/features/editor/art-and-design/cinemachine") },
            { "Input System 1.1.0-preview.3", new Route("Input System 1.1.0-preview.3", "https://docs.unity3d.com/Packages/com.unity.inputsystem@1.1/manual/index.html") },
            { "Probuilder 5.0.3", new Route("Probuilder 5.0.3", "https://unity.com/features/probuilder") },
            { "ProGrids 3.0.3-preview.6", new Route("ProGrids 3.0.3-preview.6", "https://docs.unity3d.com/Packages/com.unity.progrids@3.0/manual/index.html") },
            { "Unity Recorder 2.5.5", new Route("Unity Recorder 2.5.5", "https://docs.unity3d.com/Packages/com.unity.recorder@2.5/manual/index.html") },
            { "Itch.io", new Route("Itch.io", "https://itch.io/") },
            { "Unity Play", new Route("Unity Play", "https://play.unity.com/") },
        };

        #endregion

        #region Inspector Fields

        [ValueDropdown("@_routes"), OnValueChanged(nameof(UpdateButton)), OdinSerialize]
        [SerializeField]
        private Route _selectedRoute = new Route(); 

        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _tooltip;
        [SerializeField] private float _tooltipDelay = 1.0f;
        [SerializeField, ColorPalette] private Color _linkColor;
        [SerializeField, ColorPalette] private Color _linkHoverColor;

        #endregion

        [SerializeField, DisplayOnly] private AnimBool _isTooltipShown;
        [SerializeField, DisplayOnly] private bool _timerIsRunning = false;

        private void OnEnable()
        {
            _isTooltipShown = new AnimBool(false);
            _isTooltipShown.valueChanged.AddListener(Tooltip);
        }

        private void UpdateButton()
        {
            if (_text.IsNull()) TryGetComponent(out _text);
            _text.color = _linkColor;
            _text.SetText($"<link=\"{ _selectedRoute.WebsiteRoute}\">{_selectedRoute.Name}</link>");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (TMP_TextUtilities.FindIntersectingLink(_text, eventData.position, null) is int index && index > -1)
            {
                var linkID = _text.textInfo.linkInfo[index].GetLinkID();
                Application.OpenURL(linkID);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetLinkColor(_linkHoverColor, eventData);
            if (_timerIsRunning || _isTooltipShown.value) return;
            StartCoroutine(DelayedShowTooltip(eventData));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isTooltipShown.target = false;
            _timerIsRunning = false;
            StopAllCoroutines();
            SetLinkColor(_linkColor, eventData);
        }

        private void SetLinkColor(Color color, PointerEventData eventData)
        {
            var linkIndex = TMP_TextUtilities.FindNearestLink(_text, eventData.position, null);
            if (linkIndex != -1)
            {
                _text.textInfo.linkInfo[linkIndex].textComponent.color = color;
            }
        }

        private TMP_LinkInfo GetLinkInfo(PointerEventData eventData)
        {
            var linkIndex = TMP_TextUtilities.FindNearestLink(_text, eventData.position, null);
            return linkIndex != -1 ? _text.textInfo.linkInfo[linkIndex] : default;
        }

        private void Tooltip()
        {
            _tooltip.alpha = _isTooltipShown.faded;
        }

        private IEnumerator DelayedShowTooltip(PointerEventData eventData)
        {
            _timerIsRunning = true;
            _tooltip.transform.position = new Vector2(eventData.position.x * 0.5f, eventData.position.y);
            _tooltip.GetComponentInChildren<TMP_Text>().SetText(string.Empty);
            yield return new WaitForSeconds(_tooltipDelay);
            _tooltip.GetComponentInChildren<TMP_Text>().SetText(GetLinkInfo(eventData).GetLinkID());
            LayoutRebuilder.ForceRebuildLayoutImmediate(_tooltip.GetComponent<RectTransform>());
            _isTooltipShown.target = true;
        }
    }
}