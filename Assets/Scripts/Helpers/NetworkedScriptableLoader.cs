using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Helpers
{
    public static class NetworkedScriptableLoader
    {
        private static Dictionary<string, SerializedScriptableObject> _loadedObjects = new();
        
        public static T GetScriptable<T>(string scriptableName) where T : SerializedScriptableObject
        {
            Initialize();
            
            if (_loadedObjects.ContainsKey(scriptableName))
            {
                return (T) _loadedObjects[scriptableName];
            }
            
            var scriptable = Resources.Load<T>($"NetworkedScriptables/{scriptableName}");
            
            if (scriptable == null)
            {
                Debug.LogError($"Scriptable {scriptableName} not found");
                return null;
            }
            
            _loadedObjects.Add(scriptableName, scriptable);
            return scriptable;
        }
        
        private static void Initialize()
        {
            if (_loadedObjects != null)
                return;
            
            var loadedObjects = Resources.LoadAll<SerializedScriptableObject>("NetworkedScriptables");
            _loadedObjects = new Dictionary<string, SerializedScriptableObject>();
            
            foreach (var loadedObject in loadedObjects)
            {
                _loadedObjects.Add(loadedObject.name, loadedObject);
            }
        }
    }
}