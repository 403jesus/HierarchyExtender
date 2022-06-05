using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using HierarchyExtender.SettingsWindow.WindowComponents;

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace HierarchyExtender.SettingsWindow
{
    public class SettingsWindowCore : EditorWindow
    {
        /// <summary>
        /// Represents the On or Off state of this Plugin for internal use
        /// </summary>
        private bool _enabled;


        /// <summary>
        /// Used to Store all possible Unity default component types
        /// </summary>
        private string[] _unityComponentTypes;


        /// <summary>
        /// References to Visual Window Components used in the this window
        /// </summary>

        #region Window Components

        private Stylization _stylization;
        private ProfileSelector _profileSelector;
        private PriorityTypesSelector _priorityTypeSelector;
        private TitleAndControls _titleAndControls;

        #endregion


        /// <summary>
        /// A single list holding references to all window components
        /// </summary>
        public List<IWindowComponent> windowComponents = new();


        [MenuItem("Extensions/HierarchyExtentor")]
        public static void InitializeWindow()
        {
            SettingsWindowCore settingsWindow = (SettingsWindowCore) EditorWindow.GetWindow(typeof(SettingsWindowCore));
            settingsWindow.maxSize = new Vector2(525f, 525f);
            settingsWindow.minSize = settingsWindow.maxSize;
            settingsWindow.Show();
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            _unityComponentTypes = Utility.ReflectionHelper.GetAllUnityComponentTypes().Select(item => item.Name).ToArray();
            if (_unityComponentTypes.Length == 0)
                throw new Exception($"{nameof(_unityComponentTypes)} can not be empty!");

            _stylization = new Stylization();
            _profileSelector = new ProfileSelector(windowComponents);
            _priorityTypeSelector = new PriorityTypesSelector(_unityComponentTypes.ToList());
            _titleAndControls = new TitleAndControls();

            windowComponents.Add(_stylization);
            windowComponents.Add(_profileSelector);
            windowComponents.Add(_priorityTypeSelector);
            windowComponents.Add(_titleAndControls);
        }


        private void OnGUI()
        {
            _titleAndControls.DrawWindowComponent();
            _enabled = _titleAndControls.enabled;

            if (_enabled == false)
                return;

            _profileSelector.DrawWindowComponent();

            _stylization.DrawWindowComponent();

            _priorityTypeSelector.DrawWindowComponent();
        }
    }
}