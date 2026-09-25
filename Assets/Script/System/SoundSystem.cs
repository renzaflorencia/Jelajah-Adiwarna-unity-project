using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instance { get; set; }

    //sound effect
    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolsSound;
    public AudioSource chopSound;
    public AudioSource pickUpSound;
    public AudioSource walkSound;

    //music
    public AudioSource backsoundZone;
    public AudioClip zoneA_Music;
    public AudioClip zoneB_Music;
    public AudioClip zoneC_Music;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    public void PlaySound(AudioSource soundToPlay) {
        if (!soundToPlay.isPlaying) {
            soundToPlay.Play(); 
        }
    }
    public void ChangeMusicZone(AudioClip newMusic)
    {
        if (backsoundZone.clip != newMusic)
        {
            backsoundZone.clip = newMusic;
            backsoundZone.Play();
        }
    }
    public void StopZoneMusic()
    {
        StartCoroutine(FadeOut(backsoundZone, 1f));
    }

    IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume agar siap dipakai lagi
    }
}
