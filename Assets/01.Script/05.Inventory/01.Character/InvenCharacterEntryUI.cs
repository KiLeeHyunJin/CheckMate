using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class InvenCharacterEntryUI : MonoBehaviour, IPointerClickHandler, IDragHandler //아이템 슬롯 이미지 및 아이템 개수 표기
{

    public Image IconeImage;
    public Image PriceTypeImage;
    public TextMeshProUGUI Level; //현재 해당 아이템 개수
    public TextMeshProUGUI Name;
    public TextMeshProUGUI LevelUpPrice;
    [HideInInspector] public Image Position; //현재 해당 아이템 개수
    [HideInInspector] public Image Grade; //현재 해당 아이템 개수
    public Button LevelUpButton;
    public Image[] LevelButtonImage;
    public int CharacterEntry { get; set; } = -1; // 해당 슬롯 번호
    public InvenCharacterUI Owner { get; set; } //인벤토리 UI 연결
    public int Index;
    public bool Player;
    public CharacterBase Data;
    public int LevelUpGoldPrice;
    public int LevelUpDiaPrice;
    public DataType.PriceType PriceType;
    bool isPossible = false;
    int currentLevel;
    int price;

    void Start() //레벨 초기화
    {
        currentLevel = Data.currentLevel;
    }
    public void Update()
    {
       if (currentLevel != Data.currentLevel)
           LevelUpPriceCheck();
    }

    public void UpdateEntry() //해당 아이템의 정보를 업데이트
    {
        CharacterEntry entry = Owner.m_Data.Entries[CharacterEntry]; //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 

        gameObject.SetActive(isEnabled);//활성화

        if (isEnabled) //entry가 null이 아니라면
        {
            IconeImage.sprite = entry.Character.CharacterSprite; //아이템 이미지로 변경
            Data = entry.Character;
            LevelUpCheck();

            if (entry.Character != null) //아이템이 1개 이상이라면
            {
                if (Level != null)
                    Level.text = Data.currentLevel.ToString(); //아이템 개수 기입
                if (Name != null)
                    Name.text = Data.CharacterName;
                if(LevelUpPrice != null)
                {
                    if(entry.Character.LevelUpPriceType == DataType.PriceType.Gold)
                        LevelUpPrice.text = entry.Character.levelGoldPrice.ToString();
                    else
                        LevelUpPrice.text = entry.Character.levelDiaPrice.ToString();
                }
                if(PriceTypeImage != null)
                {
                    PriceType = entry.Character.LevelUpPriceType;
                    if (PriceType == DataType.PriceType.Gold)
                        PriceTypeImage.sprite = Owner.Owner.m_Data.dataBase.GoldePriceImage;
                    else
                        PriceTypeImage.sprite = Owner.Owner.m_Data.dataBase.DiamondPriceImage;
                }
            }
        }
    }
    void LevelUpCheck()
    {
        if ((Owner.Owner.m_Data.Money >= Data.levelGoldPrice || Owner.Owner.m_Data.Cash >= Data.levelDiaPrice) && isPossible)
        {
            LevelUpButton.interactable = true;
            return;
        }
    }
    public void LevelUpClick()
    { 
            SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
            Owner.OpenCharacterLevelUpWindow(this);
    }
    public void GoldClick()
    {
            Owner.Owner.m_Data.Money -= Data.levelGoldPrice;
            Owner.LevelUpAbilitySet(Data);
            Owner.CloseCharacterLevelUpWindow(); 
            LevelUpCount();
    }
    public void DiaClick()
    {
            Owner.Owner.m_Data.Cash -= Data.levelDiaPrice;
            Owner.LevelUpAbilitySet(Data);
            Owner.CloseCharacterLevelUpWindow();
            LevelUpCount();
    }
    void LevelUpCount()
    {
        if(Owner.Owner != null)
        {
            if(Owner.Owner.m_Data != null)
            {
                if(Owner.Owner.m_Data.countData != null)
                {
                    Owner.Owner.m_Data.countData.ItemLevelUpAdd();
                }
            }
        }
    }
    bool LevelUpPriceCheck()
    {
        if (Data.currentLevel >= 7)
        {
            isPossible = false;
            Debug.Log("최대 레벨입니다.");
            return false;
        }
        else
        {
            SetCharacterPrice();
        }
        return true;
    }

    public void OnPointerClick(PointerEventData eventData) //클릭
    {
        Debug.Log("OnPointerClick : " + CharacterEntry);
    }

    public void SetCharacterPrice()
    {
        currentLevel = Data.currentLevel;
        isPossible = true;
        switch (Data.currentLevel)
        {
            case 1:
                Data.levelDiaPrice = Data.DiaLevel2;
                Data.levelGoldPrice = Data.GoldLevel2;
                break;
            case 2:
                Data.levelDiaPrice = Data.DiaLevel3;
                Data.levelGoldPrice = Data.GoldLevel3;
                break;
            case 3:
                Data.levelDiaPrice = Data.DiaLevel4;
                Data.levelGoldPrice = Data.GoldLevel4;
                break;
            case 4:
                Data.levelDiaPrice = Data.DiaLevel5;
                Data.levelGoldPrice = Data.GoldLevel5;
                break;
            case 5:
                Data.levelDiaPrice = Data.DiaLevel6;
                Data.levelGoldPrice = Data.GoldLevel6;
                break;
            case 6:
                Data.levelDiaPrice = Data.DiaLevel7;
                Data.levelGoldPrice = Data.GoldLevel7;
                break;
            default:
                Debug.Log("Character의 최대 레벨입니다.");
                break;
        }
    }

    //public void SetupEquipment(EquipmentItem itm) //장비 아이템 장착 업데이트
    //{
    //    EquipmentItem = itm; //장비 아이템 복사

    //    enabled = itm != null; //itm의 데이터가 없는것이 아니라면 enabled는 참
    //    IconeImage.enabled = enabled; //itm의 데이터가 없는것이 아니라면 활성화
    //    if (enabled)
    //        IconeImage.sprite = itm.ItemSprite;//아이콘을 아이템 이미지로 변경
    //}

    public void OnDrag(PointerEventData eventData) //아이템 드래그 이동
    {
        //if(EquipmentItem != null) //장비아이템이 아니라면
        //    return;
        //transform.localPosition = transform.localPosition + UnscaleEventDelta(eventData.delta);//현재 위치 + 이동 위치
    }


    Vector3 UnscaleEventDelta(Vector3 vec)//드래그 이동 계산 함수
    {
        Vector2 referenceResolution = Owner.DragCanvasScaler.referenceResolution;
        Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

        float widthRatio = currentResolution.x / referenceResolution.x;
        float heightRatio = currentResolution.y / referenceResolution.y;
        float ratio = Mathf.Lerp(widthRatio, heightRatio, Owner.DragCanvasScaler.matchWidthOrHeight);

        return vec / ratio;
    }

}
