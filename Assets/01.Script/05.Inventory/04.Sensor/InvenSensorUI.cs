using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSensorUI : MonoBehaviour
{
    public GameObject PopUpWindow;
    public GameObject SensorLevelUpPopUpWindow;
    public class DragData //드레그에 사용할 아이템의 정보를 저장할 클래스
    {
        public InvenSensorEntryUI DraggedEntry; //아이템 드래그, 이미지, 개수, 더블클릭, 장착 베이스 함수 
        public RectTransform OriginalParent; //UI위치
    }

    public RectTransform[] SensorSlots; //아이템 슬롯 - UI이미지 전부 할당

    public InvenSensorEntryUI CharacterEntryPrefab; //아이템 칸 - 프리펩 하나만 할당
    public ItemTooltip Tooltip; // 아이템 설명창 - 

    //public EquipmentUI EquipementUI; //장비 아이템 - 아이템 UI할당

    public Canvas DragCanvas; // 화면 전체 캔버스
    public CanvasScaler DragCanvasScaler { get; set; }
    public DragData CurrentlyDragged { get; set; }//아이템 칸 이동에 사용할 변수 프로퍼티

    public GameObject characterPanel;
    public InvenSensorEntryUI m_HoveredItem;
    //public InventoryPanelHUD InventoryHUD;
    public bool ToolTipActive = false;
    public bool Player = false;
    InvenSensorEntryUI[] m_SensorEntries;//아이템 슬롯 데이터 복사
    public SensorSystem m_Data { get; set; }
    public int index = 0;
    public GameObject CharacterInfo;
    public void Start()
    {

        m_Data = FindObjectOfType<UserDataController>().sensorSystem;
        //m_Data = FindObjectOfType<CharacterData>();
        CurrentlyDragged = null; //현재 드레그 아이템 데이터 초기화

        DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>(); // 드레그에 사용할 캔버스 오브젝트 할당

        m_SensorEntries = new InvenSensorEntryUI[SensorSlots.Length]; // 아이템 슬롯 크기 할당

        for (int i = 0; i < m_SensorEntries.Length; ++i) // 아이템 슬롯 크기만큼 반복
        {
            m_SensorEntries[i] = Instantiate(CharacterEntryPrefab, SensorSlots[i]); //프리펩 복사, 부모하위에 복사
            m_SensorEntries[i].Owner = this; // ui 연결
            m_SensorEntries[i].InventoryEntry = i; // 몇번째 칸인지 설정
            m_SensorEntries[i].gameObject.SetActive(false); // 비활성화
        }
        SensorIndex();
        for (int i = 0; i < m_SensorEntries.Length; i++)
        {
            if (m_Data.Entries[i] != null)
            {
                index++;
            }
            else
                SensorSlots[i].gameObject.SetActive(false);
        }
        if (PopUpWindow != null)
            PopUpWindow.SetActive(false);

        Debug.Log("Sensor index :" + index);

    }
    private void Update()
    {
        //if ()
        //{
        //    activeInventory = !activeInventory;
        //    inventoryPanel.SetActive(activeInventory);
        //}
        this.Load(m_Data);

        if (null == m_Data)
        {
            for (int j = 0; j <= Owner.Entries.Length; j++)
            {
                if (Owner.Entries[j] != null)
                    m_SensorEntries[j].gameObject.SetActive(true);
            }
            Debug.Log("m_Data = null");
        }
    }
    public void LevelUpAbilitySet(SensorItem statUpSensor) 
    {
        int skillValue = ReturnSensorLevelUpValue(statUpSensor);
        Debug.Log("skillValue is " + skillValue);
        switch (statUpSensor.sensorType)
        { 
            case DataType.SensorType.Bonus:
                BonusLevelUp(statUpSensor, skillValue);
                break;
            case DataType.SensorType.FightDog:
                FightDogLevelUp(statUpSensor, skillValue);
                break;
            case DataType.SensorType.Bluffing:
                BluffingLevelUp(statUpSensor, skillValue);
                break;
            case DataType.SensorType.BlackJack:
                BlackJackLevelUp(statUpSensor, skillValue);
                break;
            case DataType.SensorType.Gambler:
                GamblerLevelUp(statUpSensor, skillValue);
                break;
            case DataType.SensorType.UnfairPlay:
                UnfairPlayLevelUp(statUpSensor, skillValue);
                break;
            case DataType.SensorType.END:
                break;
        }
        statUpSensor.sensorLevel++;
    }
    int ReturnSensorLevelUpValue(SensorItem statUpSensor)
    {
        int temp = 1;
        switch (statUpSensor.sensorLevel)
        {
            case 2:
                temp = statUpSensor.Level2_SkillTime;
                break;
            case 3:
                temp = statUpSensor.Level3_SkillTime;
                break;
            case 4:
                temp = statUpSensor.Level4_SkillTime;
                break;
            case 5:
                temp = statUpSensor.Level5_SkillTime;
                break;
            case 6:
                temp = statUpSensor.Level6_SkillTime;
                break;
            case 7:
                temp = statUpSensor.Level7_SkillTime;
                break;
            default:
                break;
        }
        return temp;
    }

    void BonusLevelUp(SensorItem statUpSensor, int skillValue)
    {
        float saveValue = statUpSensor.percentScore;
        if(skillValue > 0)
        {
            statUpSensor.percentScore = 10 + (2 * skillValue);
        }
        ReturnSkillvalue(statUpSensor, saveValue);
    }

    void FightDogLevelUp(SensorItem statUpSensor, int skillValue)
    {
        float saveTime = statUpSensor.skillTime;
        if (skillValue > 0)
        {
            statUpSensor.skillTime = 20 - (0.5f * skillValue);
        }
        ReturnSkillTime(statUpSensor, saveTime);
    }

    void BluffingLevelUp(SensorItem statUpSensor, int skillValue)
    {
        float saveTime = statUpSensor.skillTime;
        if (skillValue > 0)
        {
            statUpSensor.skillTime = 60 - 3 * skillValue;
        }
        ReturnSkillTime(statUpSensor, saveTime);
    }

    void BlackJackLevelUp(SensorItem statUpSensor, int skillValue)
    {
        float saveValue = statUpSensor.percentScore;
        if (skillValue > 0)
        {
            statUpSensor.percentScore = 30 + (3 * skillValue);
        }
        ReturnSkillvalue(statUpSensor, saveValue);
    }

    void GamblerLevelUp(SensorItem statUpSensor, int skillValue)
    {
        statUpSensor.skillTime = 5;
        float saveValue = statUpSensor.skillTime;
        if (skillValue > 0)
        {
            statUpSensor.skillTime = 8 - skillValue;
        }
        ReturnSkillTime(statUpSensor, saveValue);
    }

    void UnfairPlayLevelUp(SensorItem statUpSensor, int skillValue)
    {
        float saveTime = statUpSensor.skillTime;
        if (skillValue > 0)
        {
            statUpSensor.skillTime = 30 - skillValue;
        }
        ReturnSkillTime(statUpSensor, saveTime);
    }

    void ReturnSkillvalue(SensorItem statUpSensor, float saveValue)
    {
        if (statUpSensor.percentScore < 1)
        {
            statUpSensor.percentScore = saveValue;
        }
    }
    void ReturnSkillTime(SensorItem statUpSensor, float saveTime)
    {
        if (statUpSensor.skillTime < 1)
        {
            statUpSensor.skillTime = saveTime;
        }
    }

    public void CloseSensorLevelUpWindow()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);

        if (PopUpWindow != null)
            PopUpWindow.SetActive(false);
    }
    public void OpenSensorLevelUpWindow(InvenSensorEntryUI _Data)
    {
        if (SensorLevelUpPopUpWindow != null)
        {
            InvenSensorPopUp popUp = SensorLevelUpPopUpWindow.GetComponent<InvenSensorPopUp>();
            if (popUp != null)
            {
                popUp.SetSensorData(_Data);
            }
            PopUpWindow.SetActive(true);
        }

    }
    public void TestMinusItem()
    {
        for (int i = 0; i < 8; ++i)
        {
            if ((m_Data.Entries[i]) != null)
            {
                m_Data.MinusSensor(i);
            }
        }
    }
    public void TestAddItem()
    {
        Owner.AddSensor(Owner.m_Data.gameData.gameItemDataBase.GetSensor(0));
        SensorIndex();

    }
    public void TestAddCharacter()
    {
        Owner.AddSensor(Owner.m_Data.gameData.gameItemDataBase.GetSensor(Owner.m_Data.gameData.gameItemDataBase.GetSensorLenth()));
        SensorIndex();
    }
    void SensorIndex()
    {
        index = 0;
        if (m_Data != null)
        {
            if (m_Data.Entries != null)
            {
                for (int j = 0; j < SensorSlots.Length; j++)
                {
                    if (m_Data.Entries[j] != null)
                    {
                        m_SensorEntries[j].gameObject.SetActive(true);
                        SensorSlots[j].gameObject.SetActive(true);
                        index++;
                    }
                    else
                        m_SensorEntries[j].gameObject.SetActive(false);
                }
                Debug.Log("CharacterUI_Index : " + index);
                return;
            }
            else
                Debug.Log("CharacterUI_Index : m_Data.Entries != null");
            return;
        }
        //Debug.Log("CharacterUI_m_Data : m_Data.Entries != null");
    }
    public SensorSystem Owner // 캐릭터 데이터 프로퍼티
    {
        get { return m_Data; }
    }
    void OnEnable()
    {
        m_HoveredItem = null; //복사해서 저장할 데이터 공간 초기화
        if(Tooltip !=null)
            Tooltip.gameObject.SetActive(false); //아이템 설명창 비활성화
    }

    public void Load(SensorSystem data) // 캐릭터 데이터 가져오기
    {
        //m_Data = data; // 매개변수 복사

        for (int i = 0; i < m_SensorEntries.Length; ++i) //아이템 슬롯 반복문
        {
            m_SensorEntries[i].UpdateEntry(); // 장비 아이템 업데이트 
            m_SensorEntries[i].Player = Player;
        }
        SensorIndex();
        //EquipementUI.UpdateEquipment(m_Data.m_Data.gameData.characterData.PlayerEquipment, m_Data.m_Data.gameData.UserData.Stats); // 장비아이템 업데이트
    }
    //public void ObjectDoubleClicked(InventorySystem.InventoryEntry usedItem)

    public void ObjectDoubleClicked(CharacterEntry usedItem) //더블클릭한 아이템을 매개변수로 가져온다.
    {
        //if(m_Data.UseItem(usedItem)) //아이템 사용 함수 돌입
        //{ }
        //ObjectHoverExited(m_HoveredItem);// 아이템 설명창 비활성화 함수
        Load(m_Data);
    }

    //public void EquipmentDoubleClicked(EquipmentItem equItem)
    //{
    //    // InventoryHUD.gameData.characterData.PlayerEquipment.Unequip(equItem.Slot);
    //    //ObjectHoverExited(m_HoveredItem);
    //    Load(m_Data);
    //}

    public void ObjectHoveredEnter(InvenSensorEntryUI hovered)//아이템 설명창 활성화
    {
        if (ToolTipActive != true)
        {
            Tooltip.Getter = ItemTooltip.Own.Player;
            //m_HoveredItem = hovered; //아이템 데이터 복사
            //Tooltip.Owner = this;
            Tooltip.gameObject.SetActive(true); //아이템 설명창 활성화
                                                //Item itemUsed = m_HoveredItem.InventoryEntry != -1 ? m_Data.Inventory.Entries[m_HoveredItem.InventoryEntry].Item : m_HoveredItem.EquipmentItem;
            //Item itemUsed = m_HoveredItem.CharacterEntry != -1 ? m_Data.Entries[m_HoveredItem.CharacterEntry].Character : m_HoveredItem.EquipmentItem;
            //CharacterBase character = m_HoveredItem.CharacterEntry != -1 ? m_Data.Entries[m_HoveredItem.CharacterEntry].Character ;
            //Tooltip.Name.text = itemUsed.ItemName; //아이템 이름 복사
            //Tooltip.DescriptionText.text = itemUsed.GetDescription(); //아이템 설명 복사

            ToolTipActive = true;
        }
    }

    public void ObjectHoverExited() //아이템 설명창 비활성화
    {
        if (ToolTipActive == true) // 매개변수의 데이터와 같다면
        {
            m_HoveredItem = null;//데이터 지우기

            ToolTipActive = false;
            Tooltip.gameObject.SetActive(false);//설명창 비활성화
        }
    }

    //public void HandledDroppedEntry(Vector3 position)
    //{
    //    for (int i = 0; i < CharacterSlots.Length; ++i) //아이템 슬롯 길이만큼 반복
    //    {
    //        RectTransform t = CharacterSlots[i]; //아이템 위치 복사

    //        if (RectTransformUtility.RectangleContainsScreenPoint(t, position)) //마우스 포인터가 특정 범위안에 있는지 확인
    //        {
    //            if (m_CharacterEntries[i] != CurrentlyDragged.DraggedEntry) //현재 드레그하고 있는 아이템이 해당 슬롯의 아이템과 같지 않다면
    //            {
    //                m_Data.ReSort();

    //                //아이템 스왑 코드
    //                var prevItem = m_Data.Entries[CurrentlyDragged.DraggedEntry.CharacterEntry];
    //                m_Data.Entries[CurrentlyDragged.DraggedEntry.CharacterEntry] = m_Data.Entries[i];
    //                m_Data.Entries[i] = prevItem;

    //                CurrentlyDragged.DraggedEntry.UpdateEntry();//아이템 슬롯  - 클래스 함수
    //                m_CharacterEntries[i].UpdateEntry(); //아이템 슬롯  - 클래스 함수

    //            }
    //        }
    //    }
    //}
}
