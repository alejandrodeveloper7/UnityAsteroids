using UnityEngine;

[CreateAssetMenu(fileName = "StageSettings", menuName = "ScriptableObjects/Settings/StageSettings")]
public class StageSettings : ScriptableObject
{
    [Header("Asteroids Amount")]
    public int InitialAsteroids;
    public int AsteroidsIncrementPerRound;
    public int MaxPosibleAsteroids;

    [Header("Times")]
    public float DelayBeforeDecorationAsteroids;
    public float DelayBeforeFirstRound;
    public float DelayBetweenRounds;
}
