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

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
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
                currTrack = nextTrack;
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
            float startTime = source.time;
            currTrack = track;
            source.clip = track.overlap;
            source.time = startTime;
        }
        else
        {
            nextTrack = track;
        }
    }
}
