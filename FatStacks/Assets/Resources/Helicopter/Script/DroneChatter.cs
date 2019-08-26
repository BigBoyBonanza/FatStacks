using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drone Chatter", fileName = "New Drone Chatter")]
public class DroneChatter : ScriptableObject
{
    public AudioClip[] Idle;
    public AudioClip[] Bumping;
    public AudioClip[] Death;
    public AudioClip[] Pushing;
    private static int seed;
    System.Random random = new System.Random(++seed);
    public enum ChatterCategory
    {
        idle,
        bumping,
        death,
        pushing
    }
    public void PlayRandomClip(ChatterCategory category, AudioSource source)
    {
        AudioClip[] clips = GetAudioClips(category);
        source.clip = clips[random.Next() % clips.Length];
        source.Play();

    }
    public void PlayClip(ChatterCategory category, AudioSource source, int index)
    {

    }
    private AudioClip[] GetAudioClips(ChatterCategory category)
    {
        switch (category)
        {
            case ChatterCategory.bumping:
                return Bumping;
            case ChatterCategory.idle:
                return Idle;
            case ChatterCategory.death:
                return Death;
            case ChatterCategory.pushing:
                return Pushing;
            default:
                return null;
        }
    }
}
