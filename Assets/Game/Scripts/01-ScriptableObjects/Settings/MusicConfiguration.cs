using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicConfiguration", menuName = "ScriptableObjects/Configurations/MusicConfiguration")]
public class MusicConfiguration : ScriptableObject
{
    public List<SO_Sound> Musics;
}
