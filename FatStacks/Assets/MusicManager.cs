using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager i;

    bool looped = false;

    MusicTrack currTrack = null;
    MusicTrack nextTrack = null;

    private AudioSource source;
    private AudioSource tranSource;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        source = sources[0];
        tranSource = sources[1];
        if (!i)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void PlayTrack(MusicTrack track)
    {
        currTrack = track;
        StartCoroutine(ManageMusic());
    }

    public IEnumerator ManageMusic()
    {
        while (true)
        {
            yield return new WaitUntil(() => source.isPlaying == false);
            if (nextTrack != null)
            {
                source.clip = nextTrack.cut;
                looped = false;
                nextTrack = null;
            }
            else
            {
                if(looped == false)
                {
                    source.clip = currTrack.cut;
                }
                else
                {
                    source.clip = currTrack.overlap;
                }
                looped = true;
            }

            source.Play();
        }
    }

    public void SwitchTrack(MusicTrack track, bool immediate = false)
    {
        if (immediate)
        {
            tranSource.clip = source.clip;
            tranSource.volume = source.volume;
            tranSource.time = source.time;
            source.clip = track.cut;
            source.time = tranSource.time;
            source.volume = 0;
            StartCoroutine(FadeOut(tranSource, 0.1f));
            StartCoroutine(FadeIn(source, 0.1f,tranSource.volume));
        }
        else
        {
            nextTrack = track;
        }
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float volume)
    {
        float startVolume = 0.01f;
        while (audioSource.volume < volume)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
