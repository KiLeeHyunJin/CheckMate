using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreCharacterPopUp : MonoBehaviour
{
    public Button diaButton;
    public Button goldButton;
    [SerializeField] TextMeshProUGUI diaPrice;
    [SerializeField] TextMeshProUGUI goldPrice;
    [SerializeField] Image scoutCharagerImage;
    [SerializeField] TextMeshProUGUI characterName;
    CharacterStore Owner;
    StoreCharacterEntry Data;
    
    void Start()
    {
        Owner = FindObjectOfType<CharacterStore>();
    }
    void Update()
    {
        if (Data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (Data.data.characterDiaPrice <= Data.Owner.dataController.Cash)
            diaButton.interactable = true;
        else
            diaButton.interactable = false;

        if (Data.data.characterGoldPrice <= Data.Owner.dataController.Money)
            goldButton.interactable = true;
        else
            goldButton.interactable = false;

        if (diaPrice != null)
        {
            diaPrice.text = string.Format("{0:#,###}", Data.data.characterDiaPrice);
        }
        if (goldPrice != null)
        {
            goldPrice.text = string.Format("{0:#,###}", Data.data.characterGoldPrice);
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
    public void SetCharacterData(StoreCharacterEntry _setData, Sprite sprite)
    {
        if (_setData != null)
            Data = _setData;
        else
            Debug.Log("캐릭터의 데이터가 정상적으로 전달되지 못하였습니다. _InvenCharacterPopUp_ Data : " + _setData);
        if(sprite != null && scoutCharagerImage != null)
        {
            scoutCharagerImage.sprite = sprite;
            characterName.text = "캐릭터-" + Data.data.CharacterName;
        }
    }
    public void RemoveCharacterData()
    {
        Data = null;
    }
}
