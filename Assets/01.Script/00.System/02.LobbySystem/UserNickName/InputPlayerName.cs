using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPlayerName : MonoBehaviour
{
    public InputField playerNameInput;
    private string playerName = null;

    private void Awake()
    {
        if(playerName == null)
        {
            playerName = playerNameInput.GetComponent<InputField>().text;
        }
    }

    //마우스
    public void InputName()
    {
        if (playerNameInput == null)
            return;
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        if (playerName != null)
        {
            playerName = playerNameInput.text;

            if (playerName.Length > 0)
            {
                if(playerName == "GiveMeMoney")
                    UserDataController.Instance.isGiveMoney = false;
                else if(playerName == "Reset")
                    UserDataController.Instance.gameData.ResetData();
                else
                    UserDataController.Instance.UserName = playerName;

                playerNameInput.GetComponent<InputField>().text = "";
            }
        }
    }
}
