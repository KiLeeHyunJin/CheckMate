using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject pauseMenu;

    public void OnClick()
    {
        Time.timeScale = 1;
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);
        pauseMenu.SetActive(false);
    }
}
