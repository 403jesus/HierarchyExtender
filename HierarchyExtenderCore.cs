using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using HierarchyExtender.Utility;
using HierarchyExtender.Preferences;

// ReSharper disable StringIndexOfIsCultureSpecific.1
// ReSharper disable CheckNamespace

namespace HierarchyExtender
{
    [InitializeOnLoad]
    public class HierarchyExtenderCore
    {
        #region Private Variables

        private static bool Enabled => Settings.Enabled;
        
        private static readonly List<Type> UnityDefaultTypes = Utility.ReflectionHelper.GetAllUnityComponentTypes().ToList();
        
        private static List<string> PrioritizedDefaultTypes => Settings.PrioritizedDefaultTypes;

        #endregion


        /// <summary>
        /// Main Constructor, called when editor first launches
        /// </summary>
        static HierarchyExtenderCore()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }


        private static void HierarchyWindowItemOnGUI (int instanceID, Rect selectionRect) 
        {			
            if (Enabled == false) return;

            var hierarchyItemGameObject = EditorUtility.InstanceIDToObject (instanceID) as GameObject;
            if (hierarchyItemGameObject == null) return;

            Component component = SearchForComponent(hierarchyItemGameObject);
            if (component == null) return;

            var componentTypeName = component.GetType().Name;

            var prefix = Settings.Prefix;
            var postfix = Settings.Postfix;

            var capitalizationSelection = Settings.Selection;
            var capitalized = capitalizationSelection switch
            {
                0 => componentTypeName.ToPascal(),
                1 => componentTypeName.ToCamel(),
                2 => componentTypeName.ToUpper(),
                3 => componentTypeName.ToLower(),
                _ => throw new ArgumentOutOfRangeException()
            };

            componentTypeName = capitalized;
            
            var finalString = $"{prefix}{componentTypeName}{postfix}";
            
            var calculatedLabelSize = GUI.skin.label.CalcSize(new GUIContent(finalString));
            var rect = new Rect(selectionRect.max - new Vector2(calculatedLabelSize.x, 0) - new Vector2(0, selectionRect.height),
                new Vector2(calculatedLabelSize.x, selectionRect.size.y));
            
            EditorGUI.LabelField(rect, finalString);
        }


        /// <summary>
        /// Priority Get Component Order:
        ///     #0 - Custom Types
        ///     #1 - User Prioritized Unity's Default Types
        ///     #2 - Unity's Default Types
        /// </summary>
        private static Component SearchForComponent(GameObject gameObject)
        {
            Component[] gameObjectsChildren = gameObject.GetComponentsInChildren<Component>(true);
            
            List<Component> prioritizedComponents = new List<Component>();
            foreach (Component component in gameObjectsChildren)
            {
                //Return if it doesn't exist inside "UnityDefaultTypes" because it means it's of a Custom Type
                if (!UnityDefaultTypes.Exists(type => type == component.GetType()))
                    return component;

                //Cache the component if it exists, can't return yet because there could
                //still be a Custom Type Component after this one, which of course takes priority
                if (PrioritizedDefaultTypes.Exists(type => type == component.GetType().Name))
                    prioritizedComponents.Add(component);
            }
            
            if (prioritizedComponents.Count != 0)
            {
                //If multiple priority components are found, have
                //to return them based on user defined component-Priority
                //;Order in which they are inside "PrioritizedDefaultTypes" list
                foreach (string prioritizedDefaultType in PrioritizedDefaultTypes)
                {
                    if (prioritizedComponents.Exists(x => x.GetType().Name == prioritizedDefaultType))
                        return prioritizedComponents.Find(x => x.GetType().Name == prioritizedDefaultType);
                }
            }

            return gameObjectsChildren[0];
        }
    }
}