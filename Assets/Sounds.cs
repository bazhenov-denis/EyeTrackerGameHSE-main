using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioSrc => GetComponent<AudioSource>();

    public void PlaySound(AudioClip clip, float volume = 1f, bool destroyed = false)
    {
        audioSrc.PlayOneShot(clip, volume);
    }
}
