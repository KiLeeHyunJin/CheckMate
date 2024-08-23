using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXmanager : MonoBehaviour
{
    public static SFXmanager instance;
    [SerializeField] InGameSound inGameSound;
    public enum SFXClip
    { 
        Buy,
        Check,
        Back,
        Coin,
        Jelly,
        Swallow,
        END
    }



    AudioSource audioSource;
    public AudioClip BuyButton;
    public AudioClip BackButton;
    public AudioClip CheckButton;
    public AudioClip []Jelly;
    public AudioClip Coin;
    public AudioClip []Swallow;



    public AudioClip[] getFlyingBearJelly;

    public AudioClip GameStart;
    public AudioClip GameEnd;
    public AudioClip Result;

    public AudioClip getAlphabatJelly;
    public AudioClip getBigBearJelly;
    //public AudioClip getBigCoinJelly;
    //public AudioClip getGoldJelly;
    public AudioClip getItemJelly;
    public AudioClip getJelly;

    public AudioClip crashWithBody;
    public AudioClip crashWithPower;
    public AudioClip bounceWithObstacle;

    public AudioClip giganticStart;
    public AudioClip giganticEnd;
    public AudioClip giganticLanding;

    public AudioClip jumpClip;
    public AudioClip slideClip;

    public AudioClip uibutton;

    bool isvolumeOn;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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

        if (inGameSound == null)
            inGameSound = InGameSound.instance;
    }

    public void PlayOnSFX(SFXClip _state)
    {
        AudioClip audio = null;
        switch (_state)
        {
            case SFXClip.Buy:
                audio = BuyButton;
                break;
            case SFXClip.Check:
                audio = CheckButton;
                break;
            case SFXClip.Back:
                audio = BackButton;
                break;
            case SFXClip.Coin:
                audio = Coin;
                break;
            case SFXClip.Jelly:
                audio = RandomClip(Jelly);
                break;
            case SFXClip.Swallow:
                audio = RandomClip(Swallow);
                break;
            case SFXClip.END:
                break;
        }
        if(audio != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(audio);
        }
    }

    AudioClip RandomClip(AudioClip[] audioClips)
    {
        if(audioClips != null)
        {
            if(audioClips.Length >= 2)
            {
                int num = Random.Range(0, audioClips.Length - 1);
                return audioClips[num];
            }
            else
                return audioClips[0];
        }
        return null;
    }

    public void CrashWithObstacle()
    {
        audioSource.PlayOneShot(bounceWithObstacle);
    }

    public void PlayOnCrashWithBody()
    {
        inGameSound.PlayOnCrashWithBody();
        //audioSource.PlayOneShot(crashWithBody);
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


    //public void PlayOnGameEnd()
    //{
    //    audioSource.Stop();
    //    audioSource.PlayOneShot(GameEnd);
    //}

    //public void PlayOnResult()
    //{
    //    audioSource.Stop();
    //    audioSource.PlayOneShot(Result);
    //}

    //public void PlayOnGetJelly()
    //{
    //    audioSource.PlayOneShot(getJelly);
    //}

    //public void PlayOnGetFlyingBearJelly()
    //{
    //    if (flyingjellyidx >= 6) flyingjellyidx = 0;
    //    audioSource.PlayOneShot(getFlyingBearJelly[flyingjellyidx++]);
    //}

    //public void PlayOnGetBigBearJelly()
    //{
    //    audioSource.PlayOneShot(getBigBearJelly);
    //}

    //public void PlayOnGetItemJelly()
    //{
    //    audioSource.PlayOneShot(getItemJelly);
    //}

    //public void PlayOnGiganticStart()
    //{
    //    audioSource.PlayOneShot(giganticStart);
    //}

    //public void PlayOnGiganticEnd()
    //{
    //    audioSource.PlayOneShot(giganticEnd);
    //}

    //public void PlayOnGiganticLanding()
    //{
    //    audioSource.PlayOneShot(giganticLanding);
    //}


    //public void PlayOnuibutton()
    //{
    //    audioSource.PlayOneShot(uibutton);
    //}
}
