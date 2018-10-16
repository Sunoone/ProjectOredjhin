using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    public AudioSource efxSource;



    public void Playsingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

}
