using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicCoollection", menuName = "ScriptableObjects/Collections/MusicCollection")]
public class MusicCollection : ScriptableObject
{
    public List<SO_Sound> Musics;
}
