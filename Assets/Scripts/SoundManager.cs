using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }


    [Header("Sound Effects")]
    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource axeHitTreeSound;
    public AudioSource pickupItemSound;
    public AudioSource BGM;
    //public AudioSource grassWalkSound;

    //Music
    //public AudioSource ... 






    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {

            soundToPlay.Play();
        }


    }

}
