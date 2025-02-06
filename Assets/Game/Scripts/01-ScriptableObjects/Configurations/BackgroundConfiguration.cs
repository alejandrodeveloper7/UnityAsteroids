using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundConfiguration", menuName = "ScriptableObjects/Configurations/BackgroundConfiguration")]
public class BackgroundConfiguration : ScriptableObject
{
    [Header("Positions")]
    public Vector3 InitialPosition;
    public Vector3 FinalPosition;

    [Header("Movement")]
    public float MovementDuration;
    public Ease MovementEase;

    [Header("Aparition")]
    public float InitialAlphaValue;
    public float FinalAlphaValue;
    public float AlphaTransitionDuration;
}
