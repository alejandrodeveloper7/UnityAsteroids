using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    public static ResourcesManager Instance { get; } = new ResourcesManager();

    private Dictionary<string, ScriptableObject> _scriptableObjects = new Dictionary<string, ScriptableObject>();

    private ResourcesManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        ScriptableObject[] loadedObjects = Resources.LoadAll<ScriptableObject>("Settings");
        foreach (var obj in loadedObjects)
        {
            _scriptableObjects[obj.name] = obj;
        }
    }

    public T GetScriptableObject<T>(string pName) where T : ScriptableObject
    {
        if (_scriptableObjects.TryGetValue(pName, out ScriptableObject obj))
            return obj as T;

        Debug.LogError(string.Format("{0} not found",pName));
        return null;
    }



    public AsteroidSettings GetAsteroidSettings()
    {
        if (_scriptableObjects.TryGetValue("AsteroidSettings", out ScriptableObject response))
            return response as AsteroidSettings;

        Debug.LogError("AsteroidSettings not found");
        return null;
    }
    public BulletSettings GetBulletSettings()
    {
        if (_scriptableObjects.TryGetValue("BulletSettings", out ScriptableObject response))
            return response as BulletSettings;

        Debug.LogError("BulletSettings not found");
        return null;
    }
    public InputSettings GetInputSettings()
    {
        if (_scriptableObjects.TryGetValue("InputSettings", out ScriptableObject response))
            return response as InputSettings;

        Debug.LogError("InputSettings not found");
        return null;
    }
    public PoolSettings GetPoolSettings()
    {
        if (_scriptableObjects.TryGetValue("PoolSettings", out ScriptableObject response))
            return response as PoolSettings;

        Debug.LogError("PoolSettings not found");
        return null;
    }
    public ShipSettings GetShipSettings()
    {
        if (_scriptableObjects.TryGetValue("ShipSettings", out ScriptableObject response))
            return response as ShipSettings;

        Debug.LogError("ShipSettings not found");
        return null;
    }
}
