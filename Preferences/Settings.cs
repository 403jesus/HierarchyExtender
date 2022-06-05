using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
// ReSharper disable CheckNamespace

namespace HierarchyExtender.Preferences
{
    public class Settings : IDisposable
    {
        private static string PrefsPrefix => "HierarchyExtender_";
        
        private static int _profile = 0;

        public Settings(int profile = 0)
        {
            SetProfile(profile);
        }

        private static bool _isSetProfile = false;

        private static void SetProfile(int p)
        {
            _profile = p;
            _isSetProfile = true;
        }

        public static void ResetKeys()
        {
            //Todo: use reflection to reset every public property to default in this class

            Enabled = false;
            Selection = 0;
            PreviewName = "";
            PreviewType = "";
            Prefix = "";
            Postfix = "";
            PrioritizedDefaultTypes = new List<string>();
        }

        private static string KeysPrefix
        {
            get
            {
                if (_isSetProfile == false)
                    _profile = 0;
                return PrefsPrefix + _profile;
            }
        }
        
        public static bool Enabled
        {
            get => EditorPrefs.GetBool(KeysPrefix + nameof(Enabled));
            set => EditorPrefs.SetBool(KeysPrefix + nameof(Enabled), value);
        }

        public static int Selection
        {
            get => EditorPrefs.GetInt(KeysPrefix + nameof(Selection));
            set => EditorPrefs.SetInt(KeysPrefix + nameof(Selection), value);
        }

        public static string PreviewName
        {
            get => EditorPrefs.GetString(KeysPrefix + nameof(PreviewName));
            set => EditorPrefs.SetString(KeysPrefix + nameof(PreviewName), value);
        }

        public static string PreviewType
        {
            get => EditorPrefs.GetString(KeysPrefix + nameof(PreviewType));
            set => EditorPrefs.SetString(KeysPrefix + nameof(PreviewType), value);
        }

        public static string Prefix
        {
            get => EditorPrefs.GetString(KeysPrefix + nameof(Prefix));
            set => EditorPrefs.SetString(KeysPrefix + nameof(Prefix), value);
        }

        public static string Postfix
        {
            get => EditorPrefs.GetString(KeysPrefix + nameof(Postfix));
            set => EditorPrefs.SetString(KeysPrefix + nameof(Postfix), value);
        }
        
        public static List<string> PrioritizedDefaultTypes
        {
            get
            {
                var x = EditorPrefs.GetString(KeysPrefix + nameof(PrioritizedDefaultTypes));
                return x == "" ? new List<string>() : Utility.SerializationUtility.DeSerialize(x).ToList();
            }
            set =>
                EditorPrefs.SetString(KeysPrefix + nameof(PrioritizedDefaultTypes),
                    Utility.SerializationUtility.Serialize(value));
        }

        public void Dispose()
        {
            //We don't actually use any un-managed resources so this can just be empty
            _isSetProfile = false;
        }
    }
}
