using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMmanager : MonoBehaviour
{
    public enum MusicName
    {

        bgm_Intro,
        bgm_Lobby,
        bgm_Market,
        bgm_Inven,
        bgm_Chapter,
        bgm_Stage,
        bgm_Ready,
        bgm_InGame,
        bgm_Result,
        END
    }

    public static BGMmanager instance;

    AudioSource audioSource;

    public AudioClip bgm_Mainlobby;
    public AudioClip bgm_The_Witchs_House;
    public AudioClip bgm_Twisted_Maze_Grove;

    public AudioClip bgm_Intro;
    public AudioClip bgm_Lobby;
    public AudioClip bgm_Market;
    public AudioClip bgm_Inven;
    public AudioClip bgm_Chapter;
    public AudioClip bgm_Stage;
    public AudioClip bgm_Ready;
    public AudioClip bgm_InGame;
    public AudioClip bgm_Result;




    bool isvolumeOn;

    private void Awake()
    {
        var objs = FindObjectsOfType<BGMmanager>();
        if (objs.Length == 1)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (instance == this)
                    continue;
                else
                    Destroy(gameObject);
            }
        }
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("BGMisOn"))
            if (PlayerPrefs.GetInt("BGMisOn") == 1)
                SoundOn(PlayerPrefs.GetFloat("BGMslider"));
            else
                SoundOff();
        else
            SoundOn(PlayerPrefs.GetFloat("BGMslider"));
    }
    void SoundRePlay(AudioClip name)
    {
        audioSource.clip = name;
        audioSource.Stop();
        audioSource.Play();
    }
    public void PlayOnMainlobby()
    {
        if(audioSource.clip != null)
        {
            if (audioSource.clip.name != bgm_Mainlobby.name)
                SoundRePlay(bgm_Mainlobby);
        }
        else
            SoundRePlay(bgm_Mainlobby);
    }
    public void PlayOnOvenBreak()
    {
        SoundRePlay(bgm_Twisted_Maze_Grove);
    }

    public void PlayOnMusic(MusicName type)
    {
        AudioClip clip = null;
        switch (type)
        {
            case MusicName.bgm_Intro:
                clip = bgm_Intro;
                break;
            case MusicName.bgm_Lobby:
                clip = bgm_Lobby;
                break;
            case MusicName.bgm_Market:
                clip = bgm_Market;
                break;
            case MusicName.bgm_Inven:
                clip = bgm_Inven;
                break;
            case MusicName.bgm_Chapter:
                clip = bgm_Chapter;
                break;
            case MusicName.bgm_Stage:
                clip = bgm_Stage;
                break;
            case MusicName.bgm_Ready:
                clip = bgm_Ready;
                break;
            case MusicName.bgm_InGame:
                clip = bgm_InGame;
                break;
            case MusicName.bgm_Result:
                clip = bgm_Result;
                break;
            case MusicName.END:
                break;
        }
        if(clip != null)
            SoundRePlay(clip);
    }


    public void SoundOn(float value)
    {
        isvolumeOn = true;
        audioSource.mute = false;
        PlayerPrefs.SetFloat("BGMslider", value);
        audioSource.volume = PlayerPrefs.GetFloat("BGMslider");
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