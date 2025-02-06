using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    private Dictionary<string, ScriptableObject> _scriptableObjects = new Dictionary<string, ScriptableObject>();

    #region Properties
 
    public static ResourcesManager Instance { get; } = new ResourcesManager();

    //Settings

    public GameSettings GameSettings { get { return GetScriptableObject<GameSettings>("GameSettings"); } }
    public InputSettings InputSettings { get { return GetScriptableObject<InputSettings>("InputSettings"); } }
    public StageSettings StageSettings { get { return GetScriptableObject<StageSettings>("StageSettings"); } }
    public PlayerSettings PlayerSettings { get { return GetScriptableObject<PlayerSettings>("PlayerSettings"); } }


    //Configurations

    public AsteroidsConfiguration AsteroidsConfiguration { get { return GetScriptableObject<AsteroidsConfiguration>("AsteroidsConfiguration"); } }
    public ShipsConfiguration ShipsConfiguration { get { return GetScriptableObject<ShipsConfiguration>("ShipsConfiguration"); } }
    public BulletsConfiguration BulletsConfiguration { get { return GetScriptableObject<BulletsConfiguration>("BulletsConfiguration"); } }
    public MusicConfiguration MusicConfiguration { get { return GetScriptableObject<MusicConfiguration>("MusicConfiguration"); } }

    public PoolsConfiguration PoolsConfiguration { get { return GetScriptableObject<PoolsConfiguration>("PoolsConfiguration"); } }
    public BackgroundConfiguration BackgroundConfiguration { get { return GetScriptableObject<BackgroundConfiguration>("BackgroundConfiguration"); } }
    public CameraConfiguration CameraConfiguration { get { return GetScriptableObject<CameraConfiguration>("CameraConfiguration"); } }
    public CursorConfiguration CursorConfiguration { get { return GetScriptableObject<CursorConfiguration>("CursorConfiguration"); } }

    #endregion

    #region Initialization

    private ResourcesManager()
    {
        Initialize();
    }

    private void Initialize()
    {
        LoadScriptablesFromFolder("Settings");
        LoadScriptablesFromFolder("Configurations");
    }

    #endregion

    #region ScriptableObject managements

    private void LoadScriptablesFromFolder(string folderName)
    {
        ScriptableObject[] loadedObjects = Resources.LoadAll<ScriptableObject>(folderName);
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

    #endregion
}
