using UnityEngine;

[CreateAssetMenu(fileName = "CursorConfiguration", menuName = "ScriptableObjects/Configurations/CursorConfiguration", order = 0)]
public class CursorConfiguration : ScriptableObject
{
    public Texture2D DefaultCursor;
    public Texture2D ClickedCursor;
    [Space]
    public Vector2 Hotspot;
}
