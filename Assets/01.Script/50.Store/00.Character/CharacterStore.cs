using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStore : MonoBehaviour
{
    [SerializeField] Sprite capSallerImage;
    [SerializeField] Sprite wolfImage;
    [SerializeField] Sprite alliceImage;
    [SerializeField] Sprite heartQueenImage;
    [SerializeField] StoreCharacterEntry characterSlotPrefab;
    [SerializeField] CharacterBase[] sellCharacterData;
    [SerializeField] RectTransform[] characterSlot;
    [SerializeField] Button[] buttonSlot;
    [SerializeField] GameObject CharacterBuyPopUpWindow;
    //[SerializeField] int[] priceCharacter;
    StoreCharacterEntry[] m_Entries;
    int characterLenth;
    [HideInInspector] public UserDataController dataController;
    CharacterBase[] linckCharacter;
    int index = 0;
    void Start()
    {
        dataController = FindObjectOfType<UserDataController>();
        int characterLenth =  dataController.gameData.gameItemDataBase.GetCharacterLenth();
        CharacterLinck();
        //CharacterLinck(sellCharacterData); //데이터베이스와 연결할건지 직접 데이터를 입력시켜줄건지 

        SlotActiveCheck();
        m_Entries = new StoreCharacterEntry[index]; // 아이템 슬롯 크기 할당

        for (int i = 0; i < index; ++i) // 아이템 슬롯 크기만큼 반복
        {
            m_Entries[i] = Instantiate(characterSlotPrefab, characterSlot[i]); //프리펩 복사, 부모하위에 복사
            m_Entries[i].Owner = this;
            m_Entries[i].data = linckCharacter[i];//dataController.gameData.gameItemDataBase.GetCharacter(i);
            m_Entries[i].Index = i;
            m_Entries[i].characterEntry = i;
            buttonSlot[i].gameObject.SetActive(false);
            //m_Entries[i].buyButton = buttonSlot[i];
            m_Entries[i].gameObject.SetActive(false); // 비활성화
            //m_Entries[i].price = priceCharacter[i];
        }
    }
    void Update()
    {
        //ButtonActiveCheck();
        for (int i = 0; i < m_Entries.Length; ++i) //아이템 슬롯 반복문
        {
            m_Entries[i].UpdateEntry(); // 장비 아이템 업데이트 
        }
    }
    public void CloseCharacterPopUpWindow()
    {
        if (CharacterBuyPopUpWindow != null)
            CharacterBuyPopUpWindow.SetActive(false);
    }
    public void OpenCharacterPopUpWindow(StoreCharacterEntry _Data)
    {
        if (CharacterBuyPopUpWindow != null)
            CharacterBuyPopUpWindow.SetActive(true);
        Sprite temp = characterTypeReturn(_Data.data.CharacterType);
        if(temp != null)
            CharacterBuyPopUpWindow.GetComponent<StoreCharacterPopUp>().SetCharacterData(_Data, temp);
    }

    Sprite characterTypeReturn(DataType.CharacterType characterType)
    {
        Sprite sprite = null;
        switch (characterType)
        {
            case DataType.CharacterType.HeartQueen:
                sprite = heartQueenImage;
                break;
            case DataType.CharacterType.Wolf:
                sprite = wolfImage;
                break;
            case DataType.CharacterType.Allice:
                sprite = alliceImage;
                break;
            case DataType.CharacterType.CapSaller:
                sprite = capSallerImage;
                break;
            case DataType.CharacterType.END:
                sprite = null;
                break;
        }
        return sprite;
    }

        void CharacterLinck(CharacterBase[] bases = null)
    {
        if(bases != null)
        linckCharacter = bases;
        else
        {
            int idx = dataController.gameData.gameItemDataBase.GetCharacterLenth();
            CharacterBase[] Temp = new CharacterBase[idx]; 
            for (int i = 0; i < idx; i++)
            {
                Temp[i] = dataController.gameData.gameItemDataBase.GetCharacter(i);
            }
            linckCharacter = Temp;
        }    
    }
    int CharacterLenth()
    {
        return linckCharacter.Length;
    }
    public void BuyCharacter(int _num)
    {
        CharacterBase Temp = m_Entries[_num].data;

        if (Temp != null)
        {
            dataController.characterSystem.AddCharacter(Temp);
            Debug.Log("아이템을 구매하였습니다. m_Entries.name : " + m_Entries[_num].name);
        }
        else
        {
            Debug.Log("캐릭터 지급이 실패하였습니다. 접근 IndexNum : " + _num);
        }

        //bool isCash = false;
        //bool isMoney = false;
        //if(dataController.Money >= m_Entries[_num].data.characterGoldPrice)
        //{
        //    isMoney = true;
        //}
        //else if (dataController.Cash >= m_Entries[_num].data.characterDiaPrice)
        //{
        //    isCash = true;
        //}
        //if (isMoney || isCash)
        //{
        //    CharacterBase Temp = m_Entries[_num].data;//dataController.gameData.gameItemDataBase.GetCharacter(_num);

        //    //if (UserDataController.Instance.characterSystem.CharacterChecking(Temp))
        //    //{
        //    //}
        //    if (Temp != null)
        //    {
        //        dataController.characterSystem.AddCharacter(Temp);
        //        if(isMoney)
        //            dataController.Money -= m_Entries[_num].data.characterGoldPrice;
        //        else
        //            dataController.Cash -= m_Entries[_num].data.characterDiaPrice;

        //        Debug.Log("아이템을 구매하였습니다. m_Entries.name : " + m_Entries[_num].name);
        //    }
        //    else
        //    {
        //        Debug.Log("캐릭터 지급이 실패하였습니다. 접근 IndexNum : " + _num);
        //    }
        //}
    }
    public void BuyCharacterGold()
    {

    }
    public void BuyCharacterDia()
    {

    }
    void ButtonActiveCheck()
    {
        for (int i = 0; i < index; i++)
        {
            m_Entries[i].gameObject.SetActive(true);

            if ((dataController.Money <= m_Entries[i].data.characterGoldPrice || dataController.Money <= m_Entries[i].data.characterDiaPrice))
            {
                buttonSlot[i].interactable = false;
            }
            else
                buttonSlot[i].interactable = true;
    }
    }
    void SlotActiveCheck()
    {
        index = 0;
        characterLenth = characterSlot.Length;
        for (int i = 0; i < characterLenth; i++)
        {
            if (i <CharacterLenth())
            {
                characterSlot[i].gameObject.SetActive(true);
                buttonSlot[i].gameObject.SetActive(true);

                ++index;
                continue;
            }
            else
            {
                characterSlot[i].gameObject.SetActive(false);
                buttonSlot[i].gameObject.SetActive(false);
            }
        }
    }
}