using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenChangerButton : MonoBehaviour
{
    [SerializeField] GameObject inventoryWindow;
    [SerializeField] GameObject characterWindow;

    [SerializeField] Image sensorButton;
    [SerializeField] Image characterButton;

    [SerializeField] Sprite clickSensor;
    [SerializeField] Sprite clickCharcter;
    [SerializeField] Sprite nonClickSensor;
    [SerializeField] Sprite nonClickCharcter;
    // Start is called before the first frame update
    void Start()
    {
        characterWindow.SetActive(false);
        inventoryWindow.SetActive(true);
        characterButton.sprite = nonClickCharcter;
        sensorButton.sprite = clickSensor;
    }

    public void OpenInven()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        characterWindow.SetActive(false);
        inventoryWindow.SetActive(true);
        characterButton.sprite = nonClickCharcter;
        sensorButton.sprite = clickSensor;

    }
    public void OpenChar()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        inventoryWindow.SetActive(false);
        characterWindow.SetActive(true);
        characterButton.sprite = clickCharcter;
        sensorButton.sprite = nonClickSensor;
    }
}
