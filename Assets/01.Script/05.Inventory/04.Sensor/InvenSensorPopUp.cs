using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenSensorPopUp : MonoBehaviour
{
    public Button diaButton;
    public Button goldButton;
    [SerializeField] Sprite able;
    [SerializeField] Sprite dis;
    [SerializeField] Image gold;
    [SerializeField] Image dia;
    [SerializeField] Color disImageColor;
    [SerializeField] TextMeshProUGUI sensorName;
    [SerializeField] TextMeshProUGUI goldePrice;
    [SerializeField] TextMeshProUGUI diaPrice;
    [SerializeField] Image sensorImage;
    public TextMeshProUGUI Description;
    [SerializeField] TextMeshProUGUI CurrentLevelTxt;
    [SerializeField] TextMeshProUGUI NextLevelTxt;
    InvenSensorUI Owner;
    InvenSensorEntryUI Data;
    DataType.SensorType activeType;
    [SerializeField] Button DiaButton;
    [SerializeField] Button GoldButton;

    void Start()
    {
        Owner = FindObjectOfType<InvenSensorUI>();
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
    private void OnEnable()
    {
        if (Data == null)
            return;
        if (Data.Data == null)
            return;
        bool isMaxLevel = false;
        activeType = Data.Data.sensorType;
        if (sensorName != null)
        {
            sensorName.text = Data.Data.sensorName;
        }
        if(Description != null)
        {
            Description.text = EditSensorInfo();
        }
        if(sensorImage != null)
        {
            sensorImage.sprite = Data.Data.sensorSprite;
        }
        if(CurrentLevelTxt != null)
        {
            CurrentLevelTxt.text = Data.Data.sensorLevel.ToString();
        }
        if(NextLevelTxt != null)
        {
            if(Data.Data.sensorLevel < 7 )
            {
                NextLevelTxt.text = (Data.Data.sensorLevel + 1).ToString();
            }
            else
            {
                NextLevelTxt.text = "MaxLevel";
                isMaxLevel = true;
            }
        }
        if (isMaxLevel)
        {
            if (GoldButton != null)
                GoldButton.gameObject.SetActive(false);
            if (DiaButton != null)
                DiaButton.gameObject.SetActive(false);
        }
        else
        {
            if (goldePrice != null)
            {
                goldePrice.text = Data.Data.GoldPrice.ToString();
            }
            if (diaPrice != null)
            {
                diaPrice.text = Data.Data.DiaPrice.ToString();
            }
        }

    }
    string EditSensorInfo()
    {
        string sensorInfoString = ""; //전달할 변수
        string changeString = "99"; //바꿀 내용
        string addString = ""; //변경시킬 내용
        string origin = Data.Data.sensorInfo; //원래 내용

        int findIndex = origin.IndexOf(changeString); //찾았는지 못찾았는지
        switch (activeType)
        {
            case DataType.SensorType.Bonus:
                addString = Data.Data.percentScore.ToString();
                break;
            case DataType.SensorType.FightDog:
                addString = Data.Data.skillTime.ToString();
                break;
            case DataType.SensorType.Bluffing:
                addString = Data.Data.skillTime.ToString();
                break;
            case DataType.SensorType.BlackJack:
                addString = Data.Data.percentScore.ToString();
                break;
            case DataType.SensorType.Gambler:
                addString = Data.Data.skillTime.ToString();
                break;
            case DataType.SensorType.UnfairPlay:
                addString = Data.Data.skillTime.ToString();
                break;
            default:
                break;
        }

        if (findIndex != -1)
        {
            sensorInfoString = origin.Remove(findIndex, changeString.Length)
                                    .Insert(findIndex, addString);
        }
        else
            sensorInfoString = origin;
        return sensorInfoString;
    }
    private void OnDisable()
    {
        if (Data == null)
            return;
        if (Data.Data == null)
            return;
        if (sensorName != null)
        {
            sensorName.text = "";
        }
        if (Description != null)
        {
            Description.text = "";
        }
        if (sensorImage != null)
        {
            sensorImage.sprite = null;
        }
        if (CurrentLevelTxt != null)
        {
            CurrentLevelTxt.text = "99";
        }
        if (NextLevelTxt != null)
        {
            NextLevelTxt.text = "99";
        }
        if (GoldButton != null)
            GoldButton.gameObject.SetActive(true);
        if (DiaButton != null)
            DiaButton.gameObject.SetActive(true);
        if (goldePrice != null)
        {
            goldePrice.text = "0";
        }
        if (diaPrice != null)
        {
            diaPrice.text = "0";
        }
    }

    void Update()
    {
        if (Data == null)
            gameObject.SetActive(false);

        if (Data.Data.sensorLevel > 6)
        {
            return;
        }

        if (Data.Data.levelUpDiaPrice <= Data.Owner.Owner.m_Data.Cash)
        {
            diaButton.interactable = true; 
            if (able != null)
                diaButton.GetComponent<Image>().sprite = able;
            dia.color = Color.white;
        }
        else
        {
            diaButton.interactable = false;
            if (dis != null)
                diaButton.GetComponent<Image>().sprite = dis;
            dia.color = disImageColor;
        }

        if (Data.Data.levelUpGoldPrice <= Data.Owner.Owner.m_Data.Money)
        {
            goldButton.interactable = true;
            if (able != null)
                goldButton.GetComponent<Image>().sprite = able;
            gold.color = Color.white;
        }
        else
        {
            goldButton.interactable = false;
            if (dis != null)
                goldButton.GetComponent<Image>().sprite = dis;
            gold.color = disImageColor;
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
    public void SetSensorData(InvenSensorEntryUI _setData)
    {
        if (_setData != null)
            Data = _setData;
        else
            Debug.Log("캐릭터의 데이터가 정상적으로 전달되지 못하였습니다. _InvenCharacterPopUp_ Data : " + _setData);
    }
    public void RemoveCharacterData()
    {
        Data = null;
    }
}

