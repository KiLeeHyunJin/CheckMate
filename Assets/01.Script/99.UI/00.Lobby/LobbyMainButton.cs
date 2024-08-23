using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMainButton : MonoBehaviour
{
    [SerializeField] GameObject bellButton;
    [SerializeField] GameObject settingButton;
    [SerializeField] GameObject MenuButton;
    [SerializeField] GameObject Black;
    List<GameObject> list = new List<GameObject>();
    
    void Start()
    {
        list.Add(bellButton);
        list.Add(settingButton);
        list.Add(MenuButton);
        AllClose();
    }
    public void ResetButton(int _num)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (i == _num)
                continue;
            list[i].SetActive(false);
            if(Black != null)
            Black.gameObject.SetActive(false);
        }
    }
    public void AllClose()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(false);
            if(Black != null)
            Black.gameObject.SetActive(false);
        }
    }
}
