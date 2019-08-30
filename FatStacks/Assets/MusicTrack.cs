using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMusicTrack")]
public class MusicTrack : ScriptableObject
{
    public AudioClip cut;
    public AudioClip overlap;
    public AudioClip tail;
}
