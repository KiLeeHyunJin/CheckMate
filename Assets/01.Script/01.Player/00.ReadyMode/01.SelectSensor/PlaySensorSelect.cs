using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySensorSelect : MonoBehaviour
{
    [SerializeField] GameObject itemSelectWindow;
    public void ClickItem()
    {
        if(itemSelectWindow != null)
        itemSelectWindow.SetActive(true);
    }
    public void ClickOut()
    {
        if (itemSelectWindow != null)
            itemSelectWindow.SetActive(false);
    }

    public RectTransform[] ItemSlots; //아이템 슬롯 - UI이미지 전부 할당

    public PlaySensorEntry ItemEntryPrefab; //아이템 칸 - 프리펩 하나만 할당
    public ItemTooltip Tooltip; // 아이템 설명창 - 

    public Canvas DragCanvas; // 화면 전체 캔버스
    public CanvasScaler DragCanvasScaler { get; set; }


    //[Header("")]
    public PlaySensorEntry m_HoveredItem;

    public bool ToolTipActive = false;
    public bool Player = false;
    [Header("부모객체")]
    public GameObject inventoryPanel;
    //public InventoryPanelHUD InventoryHUD;

    PlaySensorEntry[] m_ItemEntries;//아이템 슬롯 데이터 복사
    public SensorSystem m_Data { get; set; }
    PlayerLoadSelectData selectData;


    public void Start()
    {
        m_Data = FindObjectOfType<UserDataController>().sensorSystem;
        selectData = FindObjectOfType<PlayerLoadSelectData>();

        DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>(); // 드레그에 사용할 캔버스 오브젝트 할당
        if(ItemSlots!= null)
        {
            m_ItemEntries = new PlaySensorEntry[ItemSlots.Length]; // 아이템 슬롯 크기 할당

            for (int i = 0; i < m_ItemEntries.Length; ++i) // 아이템 슬롯 크기만큼 반복
            {
                m_ItemEntries[i] = Instantiate(ItemEntryPrefab, ItemSlots[i]); //프리펩 복사, 부모하위에 복사
                m_ItemEntries[i].gameObject.SetActive(false); // 비활성화
                m_ItemEntries[i].Owner = this; // ui 연결
                m_ItemEntries[i].InventoryEntry = i; // 몇번째 칸인지 설정
                m_ItemEntries[i].Index = i;
                m_ItemEntries[i].selectData = selectData;
            }
        }
    }
    private void Update()
    {
        //if()
        //{
        //    activeInventory = !activeInventory;
        //    inventoryPanel.SetActive(activeInventory);
        //}

        this.Load(m_Data);
        if (null == m_Data)
        {
            Debug.Log("m_Data = null");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {

        }
        //if (Input.GetKeyDown(KeyCode.F))
        //    TestMinusItem();
    }


    public SensorSystem Owner // 캐릭터 데이터 프로퍼티
    {
        get { return m_Data; }
    }
    void OnEnable()
    {
        m_HoveredItem = null; //복사해서 저장할 데이터 공간 초기화
        if (Tooltip != null)
            Tooltip.gameObject.SetActive(false); //아이템 설명창 비활성화
    }

    public void Load(SensorSystem data) // 캐릭터 데이터 가져오기
    {
        //m_Data = data; // 매개변수 복사
        if(m_ItemEntries != null)
        {
            for (int i = 0; i < m_ItemEntries.Length; ++i) //아이템 슬롯 반복문
            {
                m_ItemEntries[i].UpdateEntry(); // 장비 아이템 업데이트 
                m_ItemEntries[i].Player = Player;
            }
        }
        //EquipementUI.UpdateEquipment(m_Data.m_Data.gameData.characterData.PlayerEquipment, m_Data.m_Data.gameData.UserData.Stats); // 장비아이템 업데이트
    }
    //public void ObjectDoubleClicked(InventorySystem.InventoryEntry usedItem)

    public void ObjectDoubleClicked(InventoryEntry usedItem) //더블클릭한 아이템을 매개변수로 가져온다.
    {
        //if(m_Data.UseItem(usedItem)) //아이템 사용 함수 돌입
        //{ }
        //ObjectHoverExited(m_HoveredItem);// 아이템 설명창 비활성화 함수
        Load(m_Data);
    }


    public void ObjectHoveredEnter(ItemEntryUI hovered)//아이템 설명창 활성화
    {
        if (ToolTipActive != true)
        {
            Tooltip.Getter = ItemTooltip.Own.Player;
            //m_HoveredItem = hovered; //아이템 데이터 복사
            //Tooltip.Owner = this;
            Tooltip.gameObject.SetActive(true); //아이템 설명창 활성화
            SensorItem itemUsed = null;                                //Item itemUsed = m_HoveredItem.InventoryEntry != -1 ? m_Data.Entries[m_HoveredItem.InventoryEntry].Item : m_HoveredItem.EquipmentItem;
            if (m_HoveredItem.InventoryEntry == -1)
                return;
            else
                itemUsed = m_Data.Entries[m_HoveredItem.InventoryEntry].sensor;
            Tooltip.Name.text = itemUsed.name; //아이템 이름 복사
            Tooltip.DescriptionText.text = itemUsed.sensorInfo; //아이템 설명 복사
                                                                      //Item itemUsed = m_HoveredItem.InventoryEntry != -1 ? m_Data.Inventory.Entries[m_HoveredItem.InventoryEntry].Item : m_HoveredItem.EquipmentItem;

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
