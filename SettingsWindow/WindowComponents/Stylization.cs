using System;
using System.Linq;
using HierarchyExtender.Preferences;
using HierarchyExtender.Utility;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace HierarchyExtender.SettingsWindow.WindowComponents
{
    public class Stylization : BaseVisualWindowComponent, IWindowData
    {
        private bool _drawStylization = true;
        
        private readonly string[] _selectionOptions = {"Default", "camelCase", "UPPERCASE", "lowercase"};
        private int _selection;
        
        private string _previewName;
        private string _previewType;
        private string _prefix;
        private string _postfix;
        

        public override void Initialize()
        {
            _selection = 0;
            _previewName = "New GameObject";
            _previewType = "CameraController";
            _prefix = "[";
            _postfix = "]";
        }

        protected override void InternalDrawWindowComponent()
        {
            using (new EditorGUILayout.HorizontalScope())
                _drawStylization = EditorGUILayout.Foldout(_drawStylization, "Stylization");

            GUILayout.Space(3);

            if (_drawStylization == false) return;
            
            EditorGUI.BeginChangeCheck();
            
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    _prefix = EditorGUILayout.TextField("Prefix", _prefix);
                    _postfix = EditorGUILayout.TextField("Postfix", _postfix);
                }
                
                _selection = GUILayout.SelectionGrid(_selection, _selectionOptions, 4, EditorStyles.radioButton);

                GUILayout.Space(5);

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Preview", EditorStyles.miniLabel);
                    
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Preview Name", GUILayout.Width(85));
                        _previewName = EditorGUILayout.TextField("", _previewName, GUILayout.Width(135));
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Preview Type", GUILayout.Width(85));
                        _previewType = EditorGUILayout.TextField("", _previewType, GUILayout.Width(135));
                    }
                }

                //Preview area
                using (new EditorGUILayout.VerticalScope("box"))
                {

                    var transformedStringType = "";

                    if (_previewType != "")
                        transformedStringType = _selection switch
                        {
                            0 => _previewType,
                            1 => _previewType.ToCamel(),
                            2 => _previewType.ToUpper(),
                            3 => _previewType.ToLower(),
                            _ => throw new ArgumentOutOfRangeException($"{nameof(_selection)}", $"{_selection} = val")
                        };
                    

                    var resultString = _previewName
                                       + _prefix 
                                       + transformedStringType 
                                       + _postfix;

                    GUILayout.Label(resultString, EditorStyles.whiteLargeLabel);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                SaveWindowData();
                EditorApplication.RepaintHierarchyWindow();
                Debug.Log("SaveCheck");
            }
        }
        

        public void LoadWindowData(int profile = 0)
        {
            using (new Settings(profile))
            {
                _selection = Settings.Selection;
                _previewName = Settings.PreviewName;
                _previewType = Settings.PreviewType;
                _prefix = Settings.Prefix;
                _postfix = Settings.Postfix;
            }
        }

        public void SaveWindowData(int profile = 0)
        {
            using (new Settings(profile))
            {
                Settings.Selection = _selection;
                Settings.PreviewName = _previewName;
                Settings.PreviewType = _previewType;
                Settings.Prefix = _prefix;
                Settings.Postfix = _postfix;
            }
        }
    }
}
