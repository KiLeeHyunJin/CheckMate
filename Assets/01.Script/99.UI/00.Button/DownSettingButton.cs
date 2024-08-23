using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownSettingButton : MonoBehaviour
{
    public GameObject SettingPanel;
    public void OnClick()
    {
        SettingPanel.SetActive(false);
    }
}
