using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadMoney : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyTxt;
    [SerializeField] TextMeshProUGUI diaTxt;
    UserDataController userData;
    void Start()
    {
        userData = FindObjectOfType<UserDataController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(userData != null)
        {
            if (userData.Money > 0)
                moneyTxt.text = string.Format("{0:#,###}", userData.Money);
            else
                moneyTxt.text = "0";

            if (userData.Cash > 0)
                diaTxt.text = string.Format("{0:#,###}", userData.Cash);
            else
                diaTxt.text = "0";
        }
    }
}
