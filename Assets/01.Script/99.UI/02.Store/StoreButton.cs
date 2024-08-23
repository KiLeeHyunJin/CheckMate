using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour
{
    [SerializeField] GameObject characterWindow;
    [SerializeField] GameObject sensorWindow;
    [SerializeField] GameObject pickWindow;

    [SerializeField] Image sensorButton;
    [SerializeField] Image characterButton;

    [SerializeField] Sprite clickSensor;
    [SerializeField] Sprite clickCharcter;
    [SerializeField] Sprite nonClickSensor;
    [SerializeField] Sprite nonClickCharcter;

    [SerializeField] Image backGround;
    [SerializeField] Sprite characterImage;
    [SerializeField] Sprite sensorImage;
    void Start()
    {
        sensorWindow.SetActive(false);
        pickWindow.SetActive(false);
        characterWindow.SetActive(true);
        characterButton.sprite = clickCharcter;
        sensorButton.sprite = nonClickSensor;
        backGround.sprite = characterImage;
    }

    public void OpenChar()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        sensorWindow.SetActive(false);
        pickWindow.SetActive(false);
        characterWindow.SetActive(true);
        characterButton.sprite = clickCharcter;
        sensorButton.sprite = nonClickSensor;
        backGround.sprite = characterImage;
    }
    public void OpenSensor()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        characterWindow.SetActive(false);
        pickWindow.SetActive(false);
        sensorWindow.SetActive(true);
        characterButton.sprite = nonClickCharcter;
        sensorButton.sprite = clickSensor;
        backGround.sprite = sensorImage;
    }
    public void OpenPick()
    {
        characterWindow.SetActive(false);
        sensorWindow.SetActive(false);
        pickWindow.SetActive(true);
    }
}
