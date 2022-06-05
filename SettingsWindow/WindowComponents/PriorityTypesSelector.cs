using System.Collections.Generic;
using System.Linq;
using HierarchyExtender.Preferences;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace HierarchyExtender.SettingsWindow.WindowComponents
{
    public class PriorityTypesSelector : BaseVisualWindowComponent, IWindowData
    {
        private bool _drawIgnoredDefaultTypesList = true;
        
        private Vector2 _propertyFieldScrollPos;
        private Vector2 _propertyPriorityFieldScrollPos;
        
        private List<string> _prioritizedDefaultTypes;
        private List<string> _unusedDefaultTypes = new();

        private string _defaultTypesSearchString = "";
        private string _prioritizedTypesSearchString = "";


        private readonly List<string> _unityDefaultTypes;
        public PriorityTypesSelector(List<string> unityDefaultTypes)
        {
            _unityDefaultTypes = unityDefaultTypes;
        }
        

        public override void Initialize()
        {
            _prioritizedDefaultTypes = new List<string>
            {
                "Canvas",
            };
            
            RemovePrioritizedTypesFromUnusedDefaultTypesList();
        }

        private void RemovePrioritizedTypesFromUnusedDefaultTypesList()
        {
            _unusedDefaultTypes = new List<string>(_unityDefaultTypes);
            foreach (var unusedDefaultType in _unusedDefaultTypes.ToList())
            {
                foreach (var prioritizedType in _prioritizedDefaultTypes)
                {
                    if (unusedDefaultType == prioritizedType)
                    {
                        _unusedDefaultTypes.Remove(unusedDefaultType);
                    }
                }
            }
        }
        
        private bool _hasRun = false;
        private void RunOnceInternal()
        {
            if (_hasRun == true) return;

            RemovePrioritizedTypesFromUnusedDefaultTypesList();
            _hasRun = true;
        }

        protected override void InternalDrawWindowComponent()
        {
            RunOnceInternal();
            
            _drawIgnoredDefaultTypesList =
                EditorGUILayout.Foldout(_drawIgnoredDefaultTypesList, "Prioritized Unity's Default Types");

            GUILayout.Space(3);

            if (_drawIgnoredDefaultTypesList == false) return;


            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(155)))
                {
                    GUILayout.Space(3);
                    
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        _defaultTypesSearchString = EditorGUILayout.TextField("", _defaultTypesSearchString, GUILayout.Width(155));
                        GUILayout.Button("Search");
                    }

                    GUILayout.Label("All Default Unity Types");

                    using (var scrollView = new EditorGUILayout.ScrollViewScope(_propertyFieldScrollPos))
                    {
                        _propertyFieldScrollPos = scrollView.scrollPosition;
                        
                        EditorGUI.BeginChangeCheck();

                        List<string> list = new(_unusedDefaultTypes);

                        if (_defaultTypesSearchString != "")
                            list = _unusedDefaultTypes.Where(x => x.ToLower().Contains(_defaultTypesSearchString.ToLower())).ToList();

                        var x = _defaultTypesSearchString == "" ? _unusedDefaultTypes : list;

                        for (int i = 0; i < x.Count; i++)
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                string o = x[i];
                                o = EditorGUILayout.TextField("", o, GUILayout.Width(172));

                                if (GUILayout.Button("►", GUILayout.Width(20)))
                                {
                                    _prioritizedDefaultTypes.Add(x[i]);
                                    _unusedDefaultTypes.Remove(x[i]);
                                }
                            }
                        }

                        if (EditorGUI.EndChangeCheck())
                            OnChange();
                    }
                }

                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(287)))
                {
                    GUILayout.Space(3);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        _prioritizedTypesSearchString = EditorGUILayout.TextField("", _prioritizedTypesSearchString, GUILayout.Width(200));
                        GUILayout.Button("Search");
                    }

                    GUILayout.Label("Prioritized Default Unity Types");
                    GUILayout.Label("Mode: Top most priority", EditorStyles.miniLabel);

                    using (var scrollView = new EditorGUILayout.ScrollViewScope(_propertyPriorityFieldScrollPos))
                    {
                        _propertyPriorityFieldScrollPos = scrollView.scrollPosition;

                        EditorGUI.BeginChangeCheck();
                        
                        List<string> list = new(_prioritizedDefaultTypes);

                        if (_prioritizedTypesSearchString != "")
                            list = _prioritizedDefaultTypes.Where(x => x.ToLower().Contains(_prioritizedTypesSearchString.ToLower())).ToList();

                        var x = _prioritizedTypesSearchString == "" ? _prioritizedDefaultTypes : list;
                        
                        for (int i = 0; i < x.Count; i++)
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                string o = x[i];
                                o = EditorGUILayout.TextField("", o, GUILayout.Width(185));

                                if (GUILayout.Button("◄", GUILayout.Width(20)))
                                {
                                    _unusedDefaultTypes.Add(x[i]);
                                    _prioritizedDefaultTypes.Remove(x[i]);
                                }


                                if (GUILayout.Button("▲"))
                                {
                                    int nextIndex = Mathf.Clamp(i - 1, 0, _prioritizedDefaultTypes.Count - 1);
                                    var item = _prioritizedDefaultTypes[i];
                                    _prioritizedDefaultTypes.RemoveAt(i);
                                    _prioritizedDefaultTypes.Insert(nextIndex, item);
                                }
                                if (GUILayout.Button("▼"))
                                {
                                    int nextIndex = Mathf.Clamp(i + 1, 0, _prioritizedDefaultTypes.Count - 1);
                                    var item = _prioritizedDefaultTypes[i];
                                    _prioritizedDefaultTypes.RemoveAt(i);
                                    _prioritizedDefaultTypes.Insert(nextIndex, item);
                                }
                            }
                        }

                        if (EditorGUI.EndChangeCheck())
                            OnChange();
                    }
                }
            }
        }

        private void OnChange()
        {
            //Debug.Log("ChangeOnPriotityTypes");
            EditorApplication.RepaintHierarchyWindow();
            SaveWindowData();
        }

        public void LoadWindowData(int profile = 0)
        {
            using (new Settings(profile))
                _prioritizedDefaultTypes = Settings.PrioritizedDefaultTypes;
        }

        public void SaveWindowData(int profile = 0)
        {
            using (new Settings(profile))
                Settings.PrioritizedDefaultTypes = _prioritizedDefaultTypes;
        }
    }
}
