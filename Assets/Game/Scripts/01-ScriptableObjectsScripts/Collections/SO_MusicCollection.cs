using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicCollection", menuName = "ScriptableObjects/Collections/MusicCollection")]
public class SO_MusicCollection : ScriptableObject
{
    public List<SO_Sound> Musics;
}
