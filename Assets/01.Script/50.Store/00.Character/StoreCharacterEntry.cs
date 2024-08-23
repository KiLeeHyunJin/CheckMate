using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreCharacterEntry : MonoBehaviour
{
    public Image image;
    //public Image[] allImage;
    //public TextMeshProUGUI price; //현재 해당 아이템 개수
    //public TextMeshProUGUI name; //현재 해당 아이템 개수
    [HideInInspector]public CharacterBase data;
    [SerializeField] TextMeshProUGUI characterName;
    public int characterEntry { get; set; } = -1; // 해당 슬롯 번호
    //public EquipmentItem EquipmentItem { get; private set; } //장비 아이템
    public CharacterStore Owner { get; set; } //인벤토리 UI 연결
    public Button buyButton;
    public int Index { get; set; }

    public PlayerLoadSelectData selectData { get; set; }
    void Start()
    {
        if(Owner != null)
        {
            if (Index <= Owner.dataController.gameData.gameItemDataBase.GetCharacterLenth())
            {
                data = Owner.dataController.gameData.gameItemDataBase.GetCharacter(characterEntry);
            }
        }
    }
    public void BuyButton()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        Owner.OpenCharacterPopUpWindow(this);
    }
    public void DiaClick()
    {
        Owner.dataController.Cash -= data.characterDiaPrice;
        AddItem();
    }
    public void GoldClick()
    {
        Owner.dataController.Money -= data.characterGoldPrice;
        AddItem();
    }

    void AddItem()
    {
        Owner.BuyCharacter(characterEntry);
        Owner.CloseCharacterPopUpWindow();
        BuyCount();
    }
    void BuyCount()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Buy);
        Owner.dataController.countData.BuyAdd();
    }

    public void UpdateEntry() //해당 아이템의 정보를 업데이트
    {
        if (Owner == null)
            Debug.Log("ItemEntryUI _ Owner :" + Owner);
        CharacterBase entry = data; //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 

        gameObject.SetActive(isEnabled);//활성화

        if (isEnabled) //entry가 null이 아니라면
        {
            image.sprite = entry.MarketCharacterSprite; //아이템 이미지로 변경
            if(characterName != null)
                characterName.text = entry.CharacterName;

            if (!Owner.dataController.characterSystem.CharacterChecking(data)) //.has == true) //아이템이 1개 이상이라면
            {
               // if (Owner.dataController.Money >= data.characterGoldPrice || Owner.dataController.Cash >= data.characterDiaPrice)
                {
                    //price.gameObject.SetActive(true); //텍스트 활성화
                    //name.gameObject.SetActive(true);
                    //name.text = entry.CharacterName;
                    //price.text = entry.characterGoldPrice.ToString();
                    image.color = new Color(1f, 1f, 1f, 1f);
                    buyButton.interactable = true;
                    //buyButton.enabled = true;
                    return;
                }
            }
                //for (int i = 0; i < allImage.Length; i++)
                //{
                //    allImage[i].color = new Color(0.7f, 0.7f, 0.7f, 1f);
                //}
                buyButton.interactable = false;
                //buyButton.enabled = false;
                //price.gameObject.SetActive(false); //텍스트 비활성화
        }
    }
}
