using UnityEngine;

[CreateAssetMenu(fileName = "StageSettings", menuName = "ScriptableObjects/Setting/StageSettings", order = 5)]
public class StageSettings : ScriptableObject
{
    public int InitialAsteroids;
    public int AsteroidsIncrementPerRound;
    public int MaxPosibleAsteroids;
    [Space]
    public int DelayBeforeFirstRound_MS;
    public int DelayBetweenRounds_MS;
}
