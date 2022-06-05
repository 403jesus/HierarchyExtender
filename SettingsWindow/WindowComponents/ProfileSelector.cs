using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace HierarchyExtender.SettingsWindow.WindowComponents
{
    public class ProfileSelector : BaseVisualWindowComponent
    {
        private readonly List<IWindowComponent> _windowComponents;
        
        public ProfileSelector(List<IWindowComponent> windowComponents)
        {
            _windowComponents = windowComponents;
        }

        protected override void InternalDrawWindowComponent()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Defaults"))
                {
                    if (EditorUtility.DisplayDialog("Load Defaults?",
                            "Are you sure you want to load Default settings? " +
                            "Any current unsaved changes will be lost!",
                            "Load defaults",
                            "Cancel"))
                    {
                        foreach (var wComp in _windowComponents)
                        {
                            wComp?.Initialize();
                            (wComp as IWindowData)?.SaveWindowData();
                        }

                        EditorGUI.FocusTextInControl(null);
                        EditorApplication.RepaintHierarchyWindow();
                    }
                }

                if (GUILayout.Button("Save To Profile"))
                {
                    if (EditorUtility.DisplayDialog("Save To Profile?",
                            "Are you sure you want to save current settings to a Profile? ",
                            "Save",
                            "Cancel"))
                    {
                        foreach (var wComp in _windowComponents)
                        {
                            (wComp as IWindowData)?.SaveWindowData(1);
                        }
                    }
                }

                if (GUILayout.Button("Load From Profile"))
                {
                    if (EditorUtility.DisplayDialog("Load From Profile?",
                            "Are you sure you want to load settings from a Profile? " +
                            "All current changes will be lost!",
                            "Load",
                            "Cancel"))
                    {
                        foreach (var wComp in _windowComponents)
                        {
                            (wComp as IWindowData)?.LoadWindowData(1);
                            (wComp as IWindowData)?.SaveWindowData();
                        }
                        
                        EditorApplication.RepaintHierarchyWindow();
                    }
                }
            }
        }
    }
}
