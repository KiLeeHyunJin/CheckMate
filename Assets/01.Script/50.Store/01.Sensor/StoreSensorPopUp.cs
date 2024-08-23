using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSensorPopUp : MonoBehaviour
{
    public Button diaButton;
    public Button goldButton;

    public TextMeshProUGUI goldPrice;
    public TextMeshProUGUI diaPrice;
    [SerializeField] Image buySensorImage;
    [SerializeField] TextMeshProUGUI sensorName;


    //public TextMeshProUGUI Description;
    SensorStore Owner;
    StoreSensorEntry Data;
    void Start()
    {
        Owner = FindObjectOfType<SensorStore>();
    }
    //private void OnEnable()
    //{
    //    if (Data != null)
    //    {
    //        DiaButtonClick();
    //        GoldButtonClick();
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        if (Data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (Data.data.DiaPrice <= Data.Owner.dataController.Cash)
            diaButton.interactable = true;
        else
            diaButton.interactable = false;

        if (Data.data.GoldPrice <= Data.Owner.dataController.Money)
            goldButton.interactable = true;
        else
            goldButton.interactable = false;
        if(diaPrice != null)
        {
            diaPrice.text = string.Format("{0:#,###}", Data.data.DiaPrice);
        }
        if(goldPrice != null)
        {
            goldPrice.text = string.Format("{0:#,###}", Data.data.GoldPrice);
        }
    }

    public void DiaButtonClick()
    {
        if (Data != null)
            Data.DiaClick();
    }
    public void GoldButtonClick()
    {
        if (Data != null)
            Data.GoldClick();
    }
    public void SetSensorData(StoreSensorEntry _setData, Sprite sprite)
    {
        if (_setData != null)
            Data = _setData;
        else
            Debug.Log("캐릭터의 데이터가 정상적으로 전달되지 못하였습니다. _InvenCharacterPopUp_ Data : " + _setData);
        if (sprite != null && buySensorImage != null)
        {
            buySensorImage.sprite = sprite;
            buySensorImage.SetNativeSize();
            sensorName.text = "직감-" + Data.data.sensorName;
        }
    }
    public void RemoveCharacterData()
    {
        Data = null;
    }
}
