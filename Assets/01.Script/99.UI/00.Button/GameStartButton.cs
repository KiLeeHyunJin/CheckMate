using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    public void OnClick()
    {
        //SceneManager.LoadScene("mode_select");
        SceneManager.LoadScene("OvenBreak");
    }
}
