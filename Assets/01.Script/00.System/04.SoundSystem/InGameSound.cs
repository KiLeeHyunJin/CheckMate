using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSound : MonoBehaviour
{
    public static InGameSound instance;
    AudioSource audioSource;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip doubleJumpClip;
    [SerializeField] AudioClip slideClip;
    [SerializeField] AudioClip feverClip;
    [SerializeField] AudioClip skilllClip;
    [SerializeField] AudioClip hitPlayer;

    bool isvolumeOn;
    int jumpCount = 0;
    //public AudioClip slideClip;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("SFXisOn"))
            if (PlayerPrefs.GetInt("SFXisOn") == 1)
                SoundOn(PlayerPrefs.GetFloat("SFXslider"));
            else
                SoundOff();
        else
            SoundOn(PlayerPrefs.GetFloat("SFXslider"));
    }
    public void JumpCountReset()
    {
        jumpCount = 0;
    }
    public void PlayOnCrashWithBody()
    {
        audioSource.PlayOneShot(hitPlayer);
    }
    public void PlayOnSlideclip()
    {
        audioSource.PlayOneShot(slideClip);
    }

    public void PlayOnJumpclip()
    {
        if(jumpCount == 0)
        {
            audioSource.PlayOneShot(jumpClip);
            jumpCount++;
        }
        else
        {
            audioSource.PlayOneShot(doubleJumpClip);
        }
    }
    public void SoundOn(float value)
    {
        isvolumeOn = true;
        audioSource.mute = false;
        PlayerPrefs.SetFloat("SFXslider", value);
        audioSource.volume = PlayerPrefs.GetFloat("SFXslider");
    }

    public void SoundOff()
    {
        isvolumeOn = false;
        audioSource.mute = true;
    }

    public void ChangeVolume(float value)
    {
        if (isvolumeOn)
        {
            audioSource.volume = value;
        }
    }
}
