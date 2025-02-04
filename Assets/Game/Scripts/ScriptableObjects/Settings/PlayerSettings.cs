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
    public float ShieldSliderMinValue;
    public float ShieldRecoveryTime;
}
