using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class LobbySet : MonoBehaviour
{
    [SerializeField] GameObject Black;
    [SerializeField] GameObject settingButton;

    [SerializeField] GameObject userWindow;
    [SerializeField] GameObject gameWindow;

    [SerializeField] Image setting;
    [SerializeField] Sprite soundImage;
    [SerializeField] Sprite userImage;

    [SerializeField] Image userButton;
    [SerializeField] Image gameButton;

    [SerializeField] Sprite userDisableImage;
    [SerializeField] Sprite gameDisableImage;
    [SerializeField] Sprite userAbleImage;
    [SerializeField] Sprite gameAbleImage;
    void Start()
    {
        if (setting != null)
        {
            if (userImage != null)
                setting.sprite = userImage;
        }
        gameWindow.SetActive(false);
        userWindow.SetActive(true);
        userButton.sprite = userAbleImage;
        gameButton.sprite = gameDisableImage;
    }
    public void OpenSetting()
    {
        if (Black != null)
            Black.SetActive(true);
        settingButton.SetActive(true);
        OpenUser();
    }
    public void OpenUser()
    {
        if (setting != null)
        {
            if (userImage != null)
                setting.sprite = userImage;
        }
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        gameWindow.SetActive(false);
        userWindow.SetActive(true);
        userButton.sprite = userAbleImage;
        gameButton.sprite = gameDisableImage;

    }
    public void OpenGame()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        if (setting != null)
        {
            if (soundImage != null)
                setting.sprite = soundImage;
        }
        userWindow.SetActive(false);
        gameWindow.SetActive(true);
        userButton.sprite = userDisableImage;
        gameButton.sprite = gameAbleImage;
    }
    public void CloseSetting()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);
        settingButton.GetComponent<Animator>().SetTrigger("IsClose");
        //settingButton.SetActive(false);
        if (Black != null)
            Black.SetActive(false);
    }

    public void CloseUser()
    {
        userWindow.SetActive(false);
    }

    public void CloseGame()
    {
        gameWindow.SetActive(false);
    }
}
