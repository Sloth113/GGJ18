using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    public AudioClip newMusic;

    void Start()
    {
        ChangeMusic();
    }

    public void ChangeMusic()
    {
        SoundManager.instance.musicSource.clip = newMusic;
        SoundManager.instance.musicSource.Play();
    }
}