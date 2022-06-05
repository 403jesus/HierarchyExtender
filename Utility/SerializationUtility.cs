using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace HierarchyExtender.Utility
{
    public static class SerializationUtility
    {
        public static string Serialize(IEnumerable<string> array, char delimiter = ',')
        {
            return string.Join(delimiter, array);
        }
    
        public static string[] DeSerialize(string @string, char delimiter = ',')
        {
            return @string.Split(delimiter);
        }
    }
}

