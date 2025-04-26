using UnityEngine;

public static class ScriptableObjectKeys
{
    [Header("Settings")]
    public const string GAME_SETTINGS_KEY = "GameSettings";
    public const string INPUT_SETTINGS_KEY = "InputSettings";
    public const string STAGE_SETTINGS_KEY = "StageSettings";
    public const string PLAYER_SETTINGS_KEY = "PlayerSettings";
    public const string FACTORY_SETTINGS_KEY = "FactorySettings";

    [Header("Configurations")]
    public const string CURSOR_CONFIGURATION_KEY = "CursorConfiguration";
    public const string BACKGROUND_CONFIGURATION_KEY = "BackgroundConfiguration";
    public const string CAMERA_CONFIGURATION_KEY = "CameraConfiguration";

    [Header("Collections")]
    public const string SHIP_COLLECTION_KEY = "ShipsCollection";
    public const string BULLET_COLLECTION_KEY = "BulletsCollection";
    public const string ASTEROID_COLLECTION_KEY = "AsteroidsCollection";
    public const string MUSIC_COLLECTION_KEY = "MusicCollection";
}
