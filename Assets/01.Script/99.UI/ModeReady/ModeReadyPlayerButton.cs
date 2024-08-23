using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeReadyPlayerButton : MonoBehaviour
{
    [SerializeField] GameObject selectwindow;
    [SerializeField] GameObject readyWindow;
    [SerializeField] GameObject character;
    [SerializeField] GameObject sensor;

    [SerializeField] GameObject moneyUI;
    [SerializeField] GameObject OnOffUI;

    PlayerLoadSelectData selectData;

    public bool onSensor { get; private set; }
    public bool onCharacter { get; private set; }
    public bool onReady { get; private set; }
    public bool onStage { get; private set; }

    void SetOnOffUI(bool _state)
    {
        moneyUI.SetActive(_state);
        OnOffUI.SetActive(_state);
    }

    void Start()
    {
        selectData = FindObjectOfType<PlayerLoadSelectData>();
        sensor.SetActive(false);
        character.SetActive(true);
        SetOnOffUI(true);

        selectwindow.SetActive(false);
        readyWindow.SetActive(true);

        onStage = true;
        onReady = false;
        onCharacter = false;
        onSensor = false;
    }

    public void DefaultWinodw()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        sensor.SetActive(false);
        character.SetActive(true);
        SetOnOffUI(true);

        selectwindow.SetActive(false);
        readyWindow.SetActive(true);

        onStage = false;
        onReady = true;
        onCharacter = false;
        onSensor = false;
    }

    public void OpenWindow()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        selectwindow.SetActive(true);
        sensor.SetActive(false);
        SetOnOffUI(false);

        character.SetActive(true);
        readyWindow.SetActive(false);

        onReady = true;
        onCharacter = false;
        onSensor = false;
        onStage = false;
    }
    public void OpenSensor()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        selectwindow.SetActive(true);
        sensor.SetActive(true);
        SetOnOffUI(false);

        character.SetActive(false);
        readyWindow.SetActive(false);

        selectData.OnSensorIcon();

        onReady = false;
        onCharacter = false;
        onSensor = true;
        onStage = false;
    }
    public void OpenCharacter()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        selectwindow.SetActive(true);
        sensor.SetActive(false);
        SetOnOffUI(false);

        character.SetActive(true);
        readyWindow.SetActive(false);

        selectData.OffSensorIcon();

        onReady = false;
        onCharacter = true;
        onSensor = false;
        onStage = false;
    }
    public void CloseWindow()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);

        sensor.SetActive(false);
        character.SetActive(true);
        SetOnOffUI(true);

        selectwindow.SetActive(false);
        readyWindow.SetActive(true);

        onReady = true;
        onCharacter = false;
        onSensor = false;
        onStage = false;

    }
    public void SetOnStage(bool isStage)
    {
        //SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        sensor.SetActive(false);
        character.SetActive(false);
        selectwindow.SetActive(false);
        readyWindow.SetActive(false);

        onReady = false;
        onCharacter = false;
        onSensor = false;
        onStage = false;


        onStage = isStage;
    }
}
