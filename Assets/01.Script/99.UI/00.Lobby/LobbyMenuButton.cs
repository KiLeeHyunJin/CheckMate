using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LobbyMenuButton : MonoBehaviour
{
    [SerializeField] GameObject menuButton;

    [SerializeField] GameObject userButton;
    [SerializeField] GameObject titleButton;
    [SerializeField] GameObject invenButton;

    List<GameObject> gameObjects = new List<GameObject>();


    void Start()
    {
        if(userButton != null)
        gameObjects.Add(userButton);
        if(titleButton != null)
        gameObjects.Add(titleButton);
        if (invenButton != null)
        gameObjects.Add(invenButton);
    }
    public void OnOffMenu()
    {

        if (menuButton.activeSelf == true)
        {
            SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);
            menuButton.SetActive(false);
            return;
        }
        else
            SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        menuButton.SetActive(true);
    }
    public void OpenMenu()  
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        ResetButton(); 
        if (menuButton != null)
            menuButton.SetActive(true); 
    }
    public void OpenUSer()  
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        ResetButton(); 
        if(userButton != null)
            userButton.SetActive(true);
    }
    public void OpenTitle() 
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        ResetButton(); 
        if(titleButton != null)
            titleButton.SetActive(true);
    }
    public void OpenInven()  
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        ResetButton(); 
        SceneManager.LoadScene("characters"); 
    }

    void ResetButton()
    {
        for (int i = 0; i < gameObjects.Count; i++)
            gameObjects[i].SetActive(false);
    }
}
