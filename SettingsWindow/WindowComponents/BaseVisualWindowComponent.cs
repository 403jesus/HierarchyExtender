using System;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable SuspiciousTypeConversion.Global

namespace HierarchyExtender.SettingsWindow.WindowComponents
{
    public abstract class BaseVisualWindowComponent : IWindowComponent
    {
        private bool _runOnce = false;
        public void DrawWindowComponent()
        {
            if (_runOnce == false)
            {
                (this as IWindowComponent).Initialize();
                (this as IWindowData)?.LoadWindowData();
                _runOnce = true;
            }

            using (new EditorGUILayout.VerticalScope("box"))
                InternalDrawWindowComponent();
        }

        /// <summary>
        /// This method is part of a Template Design Pattern
        /// </summary>
        protected abstract void InternalDrawWindowComponent();
        
        public virtual void Initialize(){}
    }

    public interface IWindowData
    {
        public void LoadWindowData(int profile = 0);
        public void SaveWindowData(int profile = 0);
    }

    public interface IWindowComponent
    {
        public virtual void Initialize(){}
    }
}