using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int TargetFrameRate;
    [Space]
    public List<string> _sceneDependencys;
}
