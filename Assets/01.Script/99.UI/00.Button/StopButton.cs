using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StopButton : MonoBehaviour
{
    public void Onclick()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        InGameManager.instance.GameOver();
    }
}
