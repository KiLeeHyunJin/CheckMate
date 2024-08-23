using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadUserData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] userNickName;
    [SerializeField] TextMeshProUGUI userLevel;
    [SerializeField] Image expImage;
    UserDataController userData;
    // Start is called before the first frame update
    void Start()
    {
        userData = FindObjectOfType<UserDataController>();
    }
    void Update()
    {
        InputLevel();
        InputExp();
        InputName();
    }

    void InputLevel()
    {
        if (userLevel != null)
        {
            if (userData != null)
            {
                userLevel.text = userData.UserLevel.ToString();
            }
        }
    }
    void InputExp()
    {
        if(expImage != null)
        {
            if (userData != null)
            {
                float current = userData.CurrentEXP;
                float max = userData.MaxEXP;
                float Temp = 0;
                if (max != 0 || current != 0)
                {
                    Temp = current / max;
                }
                expImage.fillAmount = Temp;
            }
            else 
                expImage.fillAmount = 0f;
        }
    }
    void InputName()
    {
        if(userNickName != null)
        {
            for (int i = 0; i < userNickName.Length; i++)
            {
                if(userNickName[i] != null)
                {
                    userNickName[i].text = userData.UserName;
                }
            }
        }
    }

    // Update is called once per frame

}
