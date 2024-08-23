using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmButton : MonoBehaviour
{
    public void Onclick()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        SceneManager.LoadScene("login");
    }
}
