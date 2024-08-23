using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    public GameObject SettingPanel;
    public void OnClick()
    {
        SettingPanel.SetActive(true);
    }
}
