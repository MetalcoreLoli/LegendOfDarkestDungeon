using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource efxSource;
    public AudioSource musicSource;
    public AudioSource menuMusicSource;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    public static SoundManager instance = null;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy (gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySinlge(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }


    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randIndex];
        efxSource.Play();
    }


}
