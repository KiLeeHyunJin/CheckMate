using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InvenSensorEntryUI : MonoBehaviour, IPointerClickHandler
{

    public Image IconeImage;
    public TextMeshProUGUI ItemLevel; //현재 해당 아이템 개수
    //public TextMeshProUGUI TypeLevel;
    public TextMeshProUGUI ItemName;
    public Button LevelUpButton;
    public Image[] LevelButtonImage;
    public int InventoryEntry { get; set; } = -1; // 해당 슬롯 번호
    //public EquipmentItem EquipmentItem { get; private set; } //장비 아이템
    public InvenSensorUI Owner { get; set; } //인벤토리 UI 연결
    public int Index { get; set; }
    //public FeildStorage feild { get; set; }
    public bool Player;
    public SensorItem Data;
    bool isPossible = false;

    public void OnPointerClick(PointerEventData eventData) //클릭
    {
        if (InventoryEntry != -1)
        {
            Debug.Log("CharacterIndex Num : " + InventoryEntry);
        }
    }
    public void UpdateEntry() //해당 아이템의 정보를 업데이트
    {
        Image img = LevelUpButton.GetComponent<Image>();
        if(img != null)
            img.color = new Color(0, 0, 0, 0);

        if (Owner == null)
            Debug.Log("ItemEntryUI _ Owner :" + Owner);
        if (Owner.m_Data == null)
            Debug.Log("ItemEntryUI _ Owner.m_Data : " + Owner.m_Data);
        SensorEntry entry = Owner.m_Data.Entries[InventoryEntry]; //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 

        gameObject.SetActive(isEnabled);//활성화

        if (isEnabled) //entry가 null이 아니라면
        {
            if (entry.sensor != null)
            {
                IconeImage.sprite = entry.sensor.sensorSprite; //아이템 이미지로 변경
                Data = entry.sensor;
                if (entry.has == true) //아이템이 1개 이상이라면
                {
                    ItemLevel.gameObject.SetActive(true); //텍스트 활성화
                    ItemLevel.text = entry.sensor.sensorLevel.ToString(); //아이템 개수 기입
                    //TypeLevel.text = entry.sensor.type.ToString(); //아이템 개수 기입
                    ItemName.text = entry.sensor.sensorName.ToString();
                    LevelUpCheck();
                }
                else
                {
                    ItemLevel.gameObject.SetActive(false); //텍스트 비활성화
                }
            }
            else
                Debug.Log("entry.sensor의 데이터가 null입니다.");
        }
    }
    public void LevelUpClick()
    {
        //if (isPossible)
        {
            SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

            //Owner.Owner.m_Data.Money -= LevelUpPrice;
            //Owner.LevelUpAbilitySet(Data);
            Owner.OpenSensorLevelUpWindow(this);
        }
    }
    public void GoldClick()
    {
        if (isPossible)
        {
            Owner.Owner.m_Data.Money -= Data.levelUpGoldPrice;
            Owner.LevelUpAbilitySet(Data);
            Owner.CloseSensorLevelUpWindow();
            LevelUpCount();
        }
    }
    public void DiaClick()
    {
        if (isPossible)
        {
            Owner.Owner.m_Data.Cash -= Data.levelUpDiaPrice;
            Owner.LevelUpAbilitySet(Data);
            Owner.CloseSensorLevelUpWindow();
            LevelUpCount();
        }
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
    void LevelUpCheck()
    {
        if (LevelUpPriceCheck())//최대 레벨이 아니라면
        {
            if ((Owner.Owner.m_Data.Money >= Data.levelUpGoldPrice || Owner.Owner.m_Data.Cash >= Data.levelUpDiaPrice) && isPossible)
            {
                LevelUpButton.interactable = true;
            }
            else
            {
                LevelUpButton.interactable = false;
                if (LevelButtonImage.Length >= 1)
                {
                    for (int i = 0; i < LevelButtonImage.Length; i++)
                    {
                        LevelButtonImage[i].color = new Color(0.7f, 0.7f, 0.7f, 1f);
                    }
                }
            }
        }
    }
    bool LevelUpPriceCheck()
    {
        isPossible = false;
        if (Data.sensorLevel >= 7)
        {
            isPossible = false;
            Debug.Log("최대 레벨입니다.");
            return false;
        }
        else
        {
            SetSensorPrice();
        }
        return true;
    }

    void SetSensorPrice()
    {
        isPossible = true;
        if (Data.sensorLevel >= 7)
        {
            isPossible = false;
            Debug.Log("최대 레벨입니다.");
            return;
        }
        else
        {
            switch (Data.sensorLevel)
            {
                case 0:
                    Data.levelUpGoldPrice = Data.GoldLevel2;
                    break;
                case 1:
                    Data.levelUpGoldPrice = Data.GoldLevel2;
                    Data.levelUpDiaPrice = Data.DiaLevel2;
                    break;
                case 2:
                    Data.levelUpGoldPrice = Data.GoldLevel3;
                    Data.levelUpDiaPrice = Data.DiaLevel3;
                    break;
                case 3:
                    Data.levelUpGoldPrice = Data.GoldLevel4;
                    Data.levelUpDiaPrice = Data.DiaLevel4;
                    break;
                case 4:
                    Data.levelUpGoldPrice = Data.GoldLevel5;
                    Data.levelUpDiaPrice = Data.DiaLevel5;
                    break;
                case 5:
                    Data.levelUpGoldPrice = Data.GoldLevel6;
                    Data.levelUpDiaPrice = Data.DiaLevel6;
                    break;
                case 6:
                    Data.levelUpGoldPrice = Data.GoldLevel7;
                    Data.levelUpDiaPrice = Data.DiaLevel7;
                    break;
                default:
                    isPossible = false;
                    Debug.Log("잘못된 접근입니다. CharacterLevel : " + Data.sensorLevel);
                    break;
            }
        }
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
