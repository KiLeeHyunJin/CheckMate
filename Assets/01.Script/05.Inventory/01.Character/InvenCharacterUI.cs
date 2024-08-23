using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InvenCharacterUI : MonoBehaviour
{
    public class DragData //드레그에 사용할 아이템의 정보를 저장할 클래스
    {
        public InvenCharacterEntryUI DraggedEntry; //아이템 드래그, 이미지, 개수, 더블클릭, 장착 베이스 함수 
        public RectTransform OriginalParent; //UI위치
    }
    public GameObject PopUpWindow;
    public GameObject CharacterLevelUpPopUpWindow;
    public RectTransform[] CharacterSlots; //아이템 슬롯 - UI이미지 전부 할당
    public InvenCharacterEntryUI CharacterEntryPrefab; //아이템 칸 - 프리펩 하나만 할당
    public ItemTooltip Tooltip; // 아이템 설명창
    public Canvas DragCanvas; // 화면 전체 캔버스
    public CanvasScaler DragCanvasScaler { get; set; }
    public DragData CurrentlyDragged { get; set; }//아이템 칸 이동에 사용할 변수 프로퍼티
    //[Header("")]
    public InvenCharacterEntryUI m_HoveredItem;

    public bool ToolTipActive = false;
    public bool Player = false;

    [Header("부모객체")]
    public GameObject inventoryPanel;

    InvenCharacterEntryUI[] m_CharacterEntries;//아이템 슬롯 데이터 복사
    public CharacterSystem m_Data { get; set; }
    int index = 0;
    void OnEnable()
    {
        m_HoveredItem = null; //복사해서 저장할 데이터 공간 초기화
        if (Tooltip != null)
            Tooltip.gameObject.SetActive(false); //아이템 설명창 비활성화
    }

    public void Load(CharacterSystem data) // 캐릭터 데이터 가져오기
    {
        CharacterIndex();
        Debug.Log("Character index :" + index);
        for (int i = 0; i < m_CharacterEntries.Length; ++i) //아이템 슬롯 반복문
        {
            m_CharacterEntries[i].UpdateEntry(); // 장비 아이템 업데이트 
            m_CharacterEntries[i].Player = Player;
        }
    }
    public void Start()
    {
        m_Data = FindObjectOfType<UserDataController>().characterSystem;
        DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>(); // 드레그에 사용할 캔버스 오브젝트 할당
        m_CharacterEntries = new InvenCharacterEntryUI[CharacterSlots.Length]; // 아이템 슬롯 크기 할당
        CurrentlyDragged = null; //현재 드레그 아이템 데이터 초기화

        for (int i = 0; i < m_CharacterEntries.Length; ++i) // 아이템 슬롯 크기만큼 반복
        {
            m_CharacterEntries[i] = Instantiate(CharacterEntryPrefab, CharacterSlots[i]); //프리펩 복사, 부모하위에 복사
            m_CharacterEntries[i].gameObject.SetActive(false); // 비활성화
            m_CharacterEntries[i].Owner = this; // ui 연결
            m_CharacterEntries[i].CharacterEntry = i; // 몇번째 칸인지 설정
        }
        CloseCharacterLevelUpWindow();
    }
    private void Update()
    {
        this.Load(m_Data);
        if (null == m_Data)
            Debug.Log("m_Data = null");
    }

    public void OpenCharacterLevelUpWindow(InvenCharacterEntryUI _Data)
    {
        if(CharacterLevelUpPopUpWindow != null)
        {
            CharacterLevelUpPopUpWindow.GetComponent<InvenCharacterPopUp>().SetCharacterData(_Data);
            PopUpWindow.SetActive(true);
        }
    }
    public void CloseCharacterLevelUpWindow()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);
        if (CharacterLevelUpPopUpWindow != null)
            PopUpWindow.SetActive(false);
    }

    public CharacterSystem Owner // 캐릭터 데이터 프로퍼티
    {
        get { return m_Data; }
    }


    void CharacterIndex() //배열개수 재확인
    {
        index = 0;
        for (int i = 0; i < CharacterSlots.Length; i++)
        {
            if (m_Data != null)
            {
                if (m_Data.Entries != null)
                {
                    if (m_Data.Entries[i] != null)
                    {
                        CharacterSlots[i].gameObject.SetActive(true);
                        index++;
                    }
                    else
                        CharacterSlots[i].gameObject.SetActive(false);
                }
                else
                    Debug.Log("CharacterUI_Index : m_Data.Entries != null");
            }
            Debug.Log("CharacterUI_m_Data : m_Data.Entries != null");
        }
    }

    public void LevelUpAbilitySet(CharacterBase statUpCharacter)
    {
        switch (statUpCharacter.CharacterType)
        {
            case DataType.CharacterType.HeartQueen:
                statUpCharacter.SkillAbility -= 1;//로얄 스트레이트 플러쉬 패를 내보내 n초마다 장애물을 코인으로 변환
                break;
            case DataType.CharacterType.Wolf:
                TwoOneUpSkillAbility(statUpCharacter);//n초 동안 비행으로 모든 장애물 회피
                break;
            case DataType.CharacterType.Allice:
                TwoOneUpSkillAbility(statUpCharacter);//화투 패에 그려진 ‘새’를 내보내 n초간 코인을 흡수
                break;
            case DataType.CharacterType.CapSaller:
                Debug.Log("존재하지 않는 캐릭터 타입니다! CharacterType : " + statUpCharacter.CharacterType + "CharacterName : " + statUpCharacter.CharacterName);
                break;
            default:
                break;
        }
        statUpCharacter.currentLevel++;
    }
    void TwoOneUpSkillAbility(CharacterBase statUpCharacter)
    {
        {
            int Temp = statUpCharacter.currentLevel;
            int Dest = Temp / 2;
            if (Dest > 1)
            {
                Temp -= Dest * 2;
            }
            if (Temp == 0)
            {
                statUpCharacter.SkillAbility += 3;
            }
            else
            {
                statUpCharacter.SkillAbility += 2;
            }
        }
    }

    public void ObjectDoubleClicked(InventoryEntry usedItem) //더블클릭한 아이템을 매개변수로 가져온다.
    {
        //if(m_Data.UseItem(usedItem)) //아이템 사용 함수 돌입
        //{ }
        //ObjectHoverExited(m_HoveredItem);// 아이템 설명창 비활성화 함수
        Load(m_Data);
    }
    private void TestMinusItem()
    {
        List<SensorEntry> temp = new List<SensorEntry>();
        SensorEntry sour = new SensorEntry();
        sour.has = true;
        sour.sensor = m_Data.m_Data.gameData.gameItemDataBase.GetSensor(m_Data.m_Data.gameData.gameItemDataBase.GetSensorLenth() - 1);
        temp.Add(sour);
    }
    private void TestAddItem()
    {
        int Temp = m_Data.m_Data.gameData.gameItemDataBase.GetSensorLenth() - 1;
        Debug.Log(Temp + "의 index의 아이템에 접근하였습니다.");
        m_Data.AddCharacter(m_Data.m_Data.gameData.gameItemDataBase.GetCharacter(Temp));
    }
    private void TestAddOtehrItem()
    {
        m_Data.AddCharacter(m_Data.m_Data.gameData.gameItemDataBase.GetCharacter(0));
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

}