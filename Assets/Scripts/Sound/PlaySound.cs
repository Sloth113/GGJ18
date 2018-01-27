using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundV2 : MonoBehaviour
{

    public AudioClip[] music;



    public void PlayNoOverlap(int index)
    {
        SoundManager.instance.PlaySingle(music[index]);
    }

    public void PlayV2(int index)
    {
        SoundManager.instance.PlaySfx(music[index]);
    }

    public void PlayDetailNoOverlap(AnimationEvent index)
    {
        SoundManager.instance.PlaySingle(music[index.intParameter]);
    }

    public void PlayDetailV2(AnimationEvent index)
    {
        SoundManager.instance.PlaySfx(music[index.intParameter]);
    }


}
