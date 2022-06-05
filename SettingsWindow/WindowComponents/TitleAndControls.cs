using HierarchyExtender.Preferences;
using UnityEditor;
using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace HierarchyExtender.SettingsWindow.WindowComponents
{
    public class TitleAndControls : BaseVisualWindowComponent
    {
        public bool enabled { get; private set; }
        
        public override void Initialize()
        {
            using (new Settings(0))
                enabled = Settings.Enabled;
        }

        protected override void InternalDrawWindowComponent()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Hierarchy Extended Plugin", EditorStyles.boldLabel);

                if (GUILayout.Button("Refresh", GUILayout.Width(85)))
                    EditorApplication.RepaintHierarchyWindow();
                

                var tempEnabled = enabled;
                enabled = EditorGUILayout.Toggle("Enabled", enabled, GUILayout.Width(170));
                if (tempEnabled != enabled)
                {
                    using (new Settings(0))
                        Settings.Enabled = enabled;
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    }
    
}
