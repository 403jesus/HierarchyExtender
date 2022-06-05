using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace HierarchyExtender.Utility
{
    public static class ReflectionHelper
    {
        public static Type[] GetAllUnityComponentTypes()
        {
            List<Type> types = GetAllTypesInAssembly("Unity");

            List<Type> componentTypes = new();
            foreach (var type in types)
                if (type.IsSubclassOf(typeof(Component)))
                    componentTypes.Add(type);

            return componentTypes.ToArray();
        }

        public static List<Type> GetAllTypesInAssembly(string assemblyName)
        {
            List<Type> results = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith(assemblyName))
                    results.AddRange(assembly.GetTypes());
            }

            return results;
        }
    }
}