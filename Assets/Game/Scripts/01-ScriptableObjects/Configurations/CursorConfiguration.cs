using UnityEngine;

[CreateAssetMenu(fileName = "CursorConfiguration", menuName = "ScriptableObjects/Configurations/CursorConfiguration")]
public class CursorConfiguration : ScriptableObject
{
    [Header("Configuration")]
    public Texture2D DefaultCursor;
    public Texture2D ClickedCursor;
    [Space]
    public Vector2 Hotspot;
}
