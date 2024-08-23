using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenCharacterPopUp : MonoBehaviour
{
    public Button diaButton;
    public Button goldButton;
    [SerializeField] Sprite dis;
    [SerializeField] Sprite able;
    [SerializeField] TextMeshProUGUI CharacterName;
    [SerializeField] TextMeshProUGUI diaPrice;
    [SerializeField] TextMeshProUGUI goldPrice;
    [SerializeField] Image CharacterImage;
    [SerializeField] Image gold;
    [SerializeField] Image dia;
    [SerializeField] Color disPriceColor;
    public TextMeshProUGUI Description;
    [SerializeField] TextMeshProUGUI CurrentLevelTxt;
    [SerializeField] TextMeshProUGUI NextLevelTxt;

    InvenCharacterUI Owner; //ui 정보(상위 데이터)
    InvenCharacterEntryUI Data; //아이템 정보
    [SerializeField] DataType.PriceType PriceType;

    [SerializeField] Button DiaButton;
    [SerializeField] Button GoldButton;

    bool isSet;
    void Awake() //초기화
    {
        Owner = FindObjectOfType<InvenCharacterUI>();

        if (CharacterName != null)
        {
            CharacterName.text = "Not found data";
        }
        if (Description != null)
        {
            Description.text = "Not found data"; ;
        }
        if (CharacterImage != null)
        {
            CharacterImage.sprite = null;
        }
        if (CurrentLevelTxt != null)
        {
            CurrentLevelTxt.text = "Not found data"; 
        }
        if(NextLevelTxt != null)
        {
            NextLevelTxt.text = "Not found data";
        }
        if (goldPrice != null)
        {
            goldPrice.text = "0";
        }
        if (diaPrice != null)
        {
            diaPrice.text = "0";
        }
    }

    private void OnEnable()
    {
        isSet = true;
        if (Data == null)
            return;
        if (Data.Data == null)
            return;

        bool isMaxLevel = false;
        if (CharacterName != null)
        {
            CharacterName.text = Data.Data.CharacterName;
        }
        if (Description != null)
        {
            Description.text = Data.Data.CharacterDescription;
        }
        if (CharacterImage != null)
        {
            CharacterImage.sprite = Data.Data.MarketCharacterSprite;
        }
        if (CurrentLevelTxt != null)
        {
            CurrentLevelTxt.text = Data.Data.currentLevel.ToString();
        }
        if (NextLevelTxt != null)
        {
            if (Data.Data.currentLevel < 7)
            {
                NextLevelTxt.text = (Data.Data.currentLevel + 1).ToString();
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
            if (goldPrice != null)
            {
                goldPrice.text = Data.Data.levelGoldPrice.ToString();
            }
            if (diaPrice != null)
            {
                diaPrice.text = Data.Data.levelDiaPrice.ToString();
            }
        }
    } //셋팅
    private void OnDisable() //초기화
    {
        if (Data == null)
            return;
        if (Data.Data == null)
            return;
        if (CharacterName != null)
        {
            CharacterName.text = "Reset Text";
        }
        if (Description != null)
        {
            Description.text = "Reset Text";
        }
        if (CharacterImage != null)
        {
            CharacterImage.sprite = null;
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
        if (goldPrice != null)
        {
            goldPrice.text = "0";
        }
        if (diaPrice != null)
        {
            diaPrice.text = "0";
        }
    }

    void Update() //아이템 최대레벨 검사 및 재화 검사
    {
        if(isSet)
        {
            if (Data != null)
            {
                PriceType = Data.PriceType;
                if (PriceType == DataType.PriceType.Gold)
                {
                    diaButton.gameObject.SetActive(false);
                    goldButton.gameObject.SetActive(true);
                    goldPrice.text = Data.Data.levelGoldPrice.ToString();
                }
                else
                {
                    diaButton.gameObject.SetActive(true);
                    goldButton.gameObject.SetActive(false);
                    goldPrice.text = Data.Data.levelDiaPrice.ToString();
                }
                if(Data.Data.currentLevel > 6)
                {
                    if (GoldButton != null)
                        GoldButton.gameObject.SetActive(false);
                    if (DiaButton != null)
                        DiaButton.gameObject.SetActive(false);
                }
                isSet = false;
            }
            else
                return;
        }

        if(Data.Data.currentLevel > 6)
            return;

        if(PriceType == DataType.PriceType.Gold)
        {
            if (Data.Data.levelGoldPrice <= Data.Owner.Owner.m_Data.Money)
            {
                Debug.Log("Gold is possible");
                goldButton.interactable = true;
                if (able != null)
                    goldButton.GetComponent<Image>().sprite = able;
                gold.color = Color.white;
            }
            else
            {
                Debug.Log("Gold is impossible");

                goldButton.interactable = false;
                if (dis != null)
                    goldButton.GetComponent<Image>().sprite = dis;
                gold.color = disPriceColor;
            }
        }

        else
        {
            if (Data.Data.levelDiaPrice <= Data.Owner.Owner.m_Data.Cash)
            {
                Debug.Log("Dia is possible");

                diaButton.interactable = true;
                if (able != null)
                    diaButton.GetComponent<Image>().sprite = able;
                dia.color = Color.white;
            }
            else
            {
                Debug.Log("Dia is impossible");

                diaButton.interactable = false;
                if (dis != null)
                    diaButton.GetComponent<Image>().sprite = dis;
                dia.color = disPriceColor;
            }
        }
    }

    public void DiaButtonClick()
    {
        if(Data != null)
            Data.DiaClick();
    }
    public void GoldButtonClick()
    {
        if(Data != null)
            Data.GoldClick();
    }
    public void SetCharacterData(InvenCharacterEntryUI _setData)
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
