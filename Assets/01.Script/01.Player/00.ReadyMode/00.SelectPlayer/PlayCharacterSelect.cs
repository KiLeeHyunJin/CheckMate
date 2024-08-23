using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCharacterSelect : MonoBehaviour
{
    [SerializeField] GameObject characterWindow;
    public void ClickCharacter()
    {
        characterWindow.SetActive(true);
    }
    public void ClickOut()
    {
        characterWindow.SetActive(false);
    }
    public class DragData //드레그에 사용할 아이템의 정보를 저장할 클래스
    {
        public InvenSensorEntryUI DraggedEntry; //아이템 드래그, 이미지, 개수, 더블클릭, 장착 베이스 함수 
        public RectTransform OriginalParent; //UI위치
    }
    public RectTransform[] CharacterSlots; //아이템 슬롯 - UI이미지 전부 할당

    public PlayerCharacterEntry CharacterEntryPrefab; //아이템 칸 - 프리펩 하나만 할당
    public ItemTooltip Tooltip; // 아이템 설명창 - 

    //public EquipmentUI EquipementUI; //장비 아이템 - 아이템 UI할당

    public Canvas DragCanvas; // 화면 전체 캔버스
    public CanvasScaler DragCanvasScaler { get; set; }
    public DragData CurrentlyDragged { get; set; }//아이템 칸 이동에 사용할 변수 프로퍼티

    public InvenSensorEntryUI m_HoveredItem;
    //public InventoryPanelHUD InventoryHUD;
    public bool ToolTipActive = false;
    public bool Player = false;
    PlayerCharacterEntry[] m_CharacterEntries;//아이템 슬롯 데이터 복사
    public CharacterSystem m_Data { get; set; }
    public int index = 0;
    public GameObject CharacterInfo;
    bool isMake = true;

    PlayerLoadSelectData selectData;
    private void Awake()
    {
        m_Data = FindObjectOfType<UserDataController>().characterSystem;
        selectData = FindObjectOfType<PlayerLoadSelectData>();


        //m_Data = FindObjectOfType<CharacterData>();
        CurrentlyDragged = null; //현재 드레그 아이템 데이터 초기화

        DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>(); // 드레그에 사용할 캔버스 오브젝트 할당
    }
    private void LateUpdate()
    {
        if(isMake)
        {
            MakeCharacterSlot();
            isMake = false;
        }
    }
    private void Update()
    {

        if (null == m_Data)
        {
            Debug.Log("m_Data = null");
        }
        else
            this.Load(m_Data);

        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //    TestAddItem();
        //}
        if (Input.GetKeyDown(KeyCode.G))
            TestAddCharacter();
    }
    void MakeCharacterSlot()
    {
        m_CharacterEntries = new PlayerCharacterEntry[CharacterSlots.Length]; // 아이템 슬롯 크기 할당

        for (int i = 0; i < m_CharacterEntries.Length; ++i) // 아이템 슬롯 크기만큼 반복
        {
            m_CharacterEntries[i] = Instantiate(CharacterEntryPrefab, CharacterSlots[i]); //프리펩 복사, 부모하위에 복사
            m_CharacterEntries[i].Owner = this; // ui 연결
            m_CharacterEntries[i].CharacterEntry = i; // 몇번째 칸인지 설정
            m_CharacterEntries[i].selectData = selectData;
            if (m_Data.Entries[i] != null)
                m_CharacterEntries[i].characterType = m_Data.Entries[i].Character.CharacterType;
            else
                m_CharacterEntries[i].characterType = DataType.CharacterType.END;
            m_CharacterEntries[i].gameObject.SetActive(false); // 비활성화
        }
    }
    public void TestMinusItem()
    {
        for (int i = 0; i < 8; ++i)
        {
            if ((m_Data.Entries[i]) != null)
            {
                m_Data.MinusCharacter(i);
            }
        }
    }
    public void TestAddItem()
    {
        Owner.AddCharacter(Owner.m_Data.gameData.gameItemDataBase.GetCharacter(0));
        CharacterIndex();
    }
    public void TestAddCharacter()
    {
        Owner.AddCharacter(Owner.m_Data.gameData.gameItemDataBase.GetCharacter(Owner.m_Data.gameData.gameItemDataBase.GetCharacterLenth() - 1));
        CharacterIndex();
    }
    void CharacterIndex()
    {
        index = 0;
        for (int i = 0; i < m_CharacterEntries.Length; i++)
        {
            if (m_Data.Entries[i] != null)
            {
                CharacterSlots[i].GetComponent<Image>().enabled = true;
                index++;
            }
            else
                CharacterSlots[i].GetComponent<Image>().enabled = false;
        }
    }
    public CharacterSystem Owner // 캐릭터 데이터 프로퍼티
    {
        get { return m_Data; }
    }

    public void Load(CharacterSystem data) // 캐릭터 데이터 가져오기
    {
        if (m_CharacterEntries == null)
            return;
        for (int i = 0; i < m_CharacterEntries.Length; ++i) //아이템 슬롯 반복문
        {
            if (m_CharacterEntries[i] == null)
                continue;
            m_CharacterEntries[i].UpdateEntry(); // 장비 아이템 업데이트 
            m_CharacterEntries[i].Player = Player;
            if (m_Data.Entries[i] != null)
            {
                index++;
            }
            else
                CharacterSlots[i].GetComponent<Image>().enabled = false;

        }
    }

    public void ObjectDoubleClicked(CharacterEntry usedItem) //더블클릭한 아이템을 매개변수로 가져온다.
    {
        Load(m_Data);
    }
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
}
