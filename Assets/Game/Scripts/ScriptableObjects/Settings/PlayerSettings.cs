using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Setting/PlayerSettings", order = 6)]
public class PlayerSettings : ScriptableObject
{
    [Header("Health")]
    [Range(1,5)]public int HealthPoints;
    [Space]
    public Sprite HealthPointSprite;
    public Sprite emptyHealtPointSprite;

    [Header("Shield")]
    public Sprite ShieldBarSprite;
    public Sprite FullShieldBarSprite;
    [Space]
    public int ShieldRecoveryTime;
    [Space]
    public Color Color;
    [Space]
    public float FadeInDuration;
    public float FadeOutDuration;
    [Space]
    public float BlickDuration;
    [Space]
    public float BlinkMinAlpha;
    public float BlinkMaxAlpha;
}
