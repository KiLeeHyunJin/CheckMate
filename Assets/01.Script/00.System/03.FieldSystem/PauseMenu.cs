using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
    }
}