using Sirenix.OdinInspector;
//using Sirenix.OdinInspector.Editor;
using System.Collections;
using TirUtilities;
using TirUtilities.Experimental;
using TirUtilities.Extensions;
using TirUtilities.Signals;
using TirUtilities.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare49.LevelManagment
{
    ///<!--
    /// RouterButton.cs
    /// 
    /// Project:  TirBombardier — Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 09, 2021
    /// Updated:  Oct 09, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class RouterButton : MonoBehaviour
    {
        #region Data Structures
        
        [System.Serializable]
        private enum RouteType { LevelSignal, Application, MenuState, }

        [System.Serializable]
        private enum ButtonType { PlayPause, QuitGame, }

        #endregion

        #region Level Routing

        [SerializeField, EnumToggleButtons, HideLabel]
        private RouteType _routeType = RouteType.LevelSignal;

        [SerializeField, ShowIf(nameof(_routeType), RouteType.LevelSignal), 
            ValueDropdown(nameof(FetchLevelLoadSignals))]
        private LevelLoadSignal _targetLevel;
        private static IEnumerable FetchLevelLoadSignals() =>
            Resources.FindObjectsOfTypeAll<LevelLoadSignal>();

        private void AssignLevelListeners()
        {
            if (_routeType != RouteType.LevelSignal || _targetLevel.IsNull()) return;

            _button.onClick.AddListener(_targetLevel.Emit);
        }

        private void RemoveLevelListeners()
        {
            if (_routeType != RouteType.LevelSignal || _targetLevel.IsNull()) return;

            _button.onClick.AddListener(_targetLevel.Emit);
        }

        #endregion

        #region Application Routing

        [SerializeField, ShowIf(nameof(_routeType), RouteType.Application), SceneObjectsOnly]
        private ApplicationStateMachine _applicationStateMachine;
        [SerializeField, ShowIf(nameof(_routeType), RouteType.Application), SceneObjectsOnly]
        private ButtonType _buttonType = ButtonType.QuitGame;

        private void AssignAppListeners()
        {
            if (_routeType != RouteType.Application) return;

            if (_buttonType == ButtonType.PlayPause)
                _button.onClick.AddListener(_applicationStateMachine.TogglePaused);

            else if (_buttonType == ButtonType.QuitGame)
                _button.onClick.AddListener(_applicationStateMachine.QuitGame);
        }

        private void RemoveAppListeners()
        {
            if (_routeType != RouteType.Application) return;

            if (_buttonType == ButtonType.PlayPause)
                _button.onClick.RemoveListener(_applicationStateMachine.TogglePaused);

            else if (_buttonType == ButtonType.QuitGame)
                _button.onClick.RemoveListener(_applicationStateMachine.QuitGame);
        }

        #endregion

        #region Menu Routing

        [SerializeField, ShowIf(nameof(_routeType), RouteType.MenuState), SceneObjectsOnly]
        private MenuStateMachine _menuStateMachine;
        [SerializeField, ShowIf(nameof(_routeType), RouteType.MenuState), 
            AssetSelector(FlattenTreeView = false)]
        private TirUtilities.UI.MenuState _boundState;

        private void AssignMenuListeners()
        {
            if (_routeType != RouteType.MenuState) return;
            if (_menuStateMachine.IsNull() || _boundState.IsNull()) return;

            _button.onClick.AddListener(Transition);

            void Transition() => _menuStateMachine.TransitionTo(_boundState);
        }

        private void RemoveMenuListeners()
        {
            if (_routeType != RouteType.MenuState) return;
            if (_menuStateMachine.IsNull() || _boundState.IsNull()) return;

            _button.onClick.RemoveListener(Transition);

            void Transition() => _menuStateMachine.TransitionTo(_boundState);
        }

        #endregion

        #region Button Settings

        [Title("Button Colors")]
        [SerializeField,  ColorPalette("TirBombardier Core Pallet", ShowAlpha = true)]
        private Color _normalColor;

        [SerializeField, ColorPalette("TirBombardier Core Pallet", ShowAlpha = true)]
        private Color _highlightedColor;

        [SerializeField, ColorPalette("TirBombardier Core Pallet", ShowAlpha = true)]
        private Color _pressedColor;

        [SerializeField, ColorPalette("TirBombardier Core Pallet", ShowAlpha = true)]
        private Color _selectedColor;

        [SerializeField, ColorPalette("TirBombardier Core Pallet", ShowAlpha = true)]
        private Color _disabledColor;

        [SerializeField, DisplayOnly] Button _button;

        [Button(ButtonSizes.Medium)]
        private void ApplyButtonParams()
        {
            if (_button.IsNull()) TryGetComponent(out _button);
            _button.colors = new ColorBlock()
            {
                normalColor = _normalColor,
                highlightedColor = _highlightedColor,
                pressedColor = _pressedColor,
                selectedColor = _selectedColor,
                disabledColor = _disabledColor,
                colorMultiplier = _button.colors.colorMultiplier,
                fadeDuration = _button.colors.fadeDuration
            };
        }

        #endregion

        #region Unity Messages

        private void OnEnable()
        {
            if (_button.IsNull()) return;

            AssignAppListeners();
            AssignLevelListeners();
            AssignMenuListeners();
        }
        private void OnDisable()
        {
            if (_button.IsNull()) return;

            RemoveAppListeners();
            RemoveLevelListeners();
            RemoveMenuListeners();
        } 

        #endregion
    }
}