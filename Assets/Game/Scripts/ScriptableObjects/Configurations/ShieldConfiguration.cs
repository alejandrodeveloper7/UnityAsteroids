using UnityEngine;

[CreateAssetMenu(fileName = "ShieldConfiguration", menuName = "ScriptableObjects/Configurations/ShieldConfiguration", order = 0)]
public class ShieldConfiguration : ScriptableObject
{
    public Color Color;
    [Space]
    public float FadeInDuration;
    public float FadeOutDuration;
    [Space]
    public float BlickDurationSpeed;
    public float BlinkMinAlpha;  
    public float BlinkMaxAlpha;    
}
