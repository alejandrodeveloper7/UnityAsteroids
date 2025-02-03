using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicConfiguration", menuName = "ScriptableObjects/Configurations/MusicConfiguration", order = 1)]
public class MusicConfiguration : ScriptableObject
{
    public List<SO_Sound> Musics;
}
