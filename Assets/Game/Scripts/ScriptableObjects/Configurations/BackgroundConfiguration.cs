using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundConfiguration", menuName = "ScriptableObjects/Configurations/BackgroundConfiguration", order = 2)]
public class BackgroundConfiguration : ScriptableObject
{
    public Vector3 InitialPosition;
    public Vector3 FinalPosition;
    public float MovementDuration;
    public Ease MovementEase;
    [Space]
    public float InitialAlphaValue;
    public float AlphaValue;
    public float AlphaTransitionDuration;
}
