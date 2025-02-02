using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    public static ResourcesManager Instance { get; } = new ResourcesManager();

    private Dictionary<string, ScriptableObject> _scriptableObjects = new Dictionary<string, ScriptableObject>();

    public AsteroidSettings AsteroidSettings { get { return GetScriptableObject<AsteroidSettings>("AsteroidSettings"); } }
    public BulletSettings BulletSettings { get { return GetScriptableObject<BulletSettings>("BulletSettings"); } }
    public InputSettings InputSettings { get { return GetScriptableObject<InputSettings>("InputSettings"); } }
    public PoolSettings PoolSettings { get { return GetScriptableObject<PoolSettings>("PoolSettings"); } }
    public ShipSettings ShipSettings { get { return GetScriptableObject<ShipSettings>("ShipSettings"); } }
    public StageSettings StageSettings { get { return GetScriptableObject<StageSettings>("StageSettings"); } }
    public PlayerSettings PlayerSettings { get { return GetScriptableObject<PlayerSettings>("PlayerSettings"); } }

    private ResourcesManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        ScriptableObject[] loadedObjects = Resources.LoadAll<ScriptableObject>("Settings");
        foreach (var obj in loadedObjects)        
            _scriptableObjects[obj.name] = obj;        
    }

    public T GetScriptableObject<T>(string pName) where T : ScriptableObject
    {
        if (_scriptableObjects.TryGetValue(pName, out ScriptableObject obj))
            return obj as T;

        Debug.LogError(string.Format("{0} not found", pName));
        return null;
    }
}
