using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/Settings/GameSettings")]
public class GameSettings : ScriptableObject
{   
    public List<string> _desktopSceneDependencys;
    public List<string> _MobileSceneDependencys;
}
