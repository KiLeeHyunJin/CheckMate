using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    /// <summary>
    /// Handle all the UI code related to the inventory (drag'n'drop of object, using objects, equipping object etc.)
    /// </summary>
public class GradeItemUI : MonoBehaviour  //인벤토리 칸 UI
{
    public class DragData //드레그에 사용할 아이템의 정보를 저장할 클래스
    {
        public GradeItemEntryUI DraggedEntry; //아이템 드래그, 이미지, 개수, 더블클릭, 장착 베이스 함수 
        public RectTransform OriginalParent; //UI위치
    }
    
    public RectTransform[] GradeItemSlots; //아이템 슬롯 - UI이미지 전부 할당
    
    public GradeItemEntryUI GradeItemEntryPrefab; //아이템 칸 - 프리펩 하나만 할당
    public ItemTooltip Tooltip; // 아이템 설명창 - 

    public Canvas DragCanvas; // 화면 전체 캔버스
    public CanvasScaler DragCanvasScaler { get; set; }
    
    public DragData CurrentlyDragged { get; set; }//아이템 칸 이동에 사용할 변수 프로퍼티

    //[Header("")]
    public GradeItemEntryUI m_HoveredItem;

    public bool ToolTipActive = false;
    public bool Player = false;

    [Header("부모객체")]
    public GameObject inventoryPanel;
    //public InventoryPanelHUD InventoryHUD;

    GradeItemEntryUI[] m_ItemEntries;//아이템 슬롯 데이터 복사
    public GradeItemSystem m_Data { get; set; }

    public void Start()
    {
        m_Data = FindObjectOfType<UserDataController>().gradeItemSystem;
        DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>(); // 드레그에 사용할 캔버스 오브젝트 할당
        m_ItemEntries = new GradeItemEntryUI[GradeItemSlots.Length]; // 아이템 슬롯 크기 할당
        CurrentlyDragged = null; //현재 드레그 아이템 데이터 초기화

        for (int i = 0; i < m_ItemEntries.Length; ++i) // 아이템 슬롯 크기만큼 반복
        {
            m_ItemEntries[i] = Instantiate(GradeItemEntryPrefab, GradeItemSlots[i]); //프리펩 복사, 부모하위에 복사
            m_ItemEntries[i].gameObject.SetActive(false); // 비활성화
            m_ItemEntries[i].Owner = this; // ui 연결
            m_ItemEntries[i].InventoryEntry = i; // 몇번째 칸인지 설정
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
        if(null == m_Data)
        {
            Debug.Log("m_Data = null");
        }
        if (Input.GetKeyDown(KeyCode.D))
            TestAddOtehrItem();
        if (Input.GetKeyDown(KeyCode.G))
            TestAddItem();
        if (Input.GetKeyDown(KeyCode.F))
            TestMinusItem();
    }

    private void TestMinusItem()
    {
        List<GradeItemEntry> temp = new List<GradeItemEntry>();
        GradeItemEntry sour = new GradeItemEntry();
        sour.Count = 1;
        sour.Item = m_Data.m_Data.gameData.gameItemDataBase.GetGradeItem(0);
        temp.Add(sour);

        GradeItemEntry Dest = new GradeItemEntry();
        Dest.Count = 1;
        Dest.Item = m_Data.m_Data.gameData.gameItemDataBase.GetGradeItem(1);
        temp.Add(Dest);
        m_Data.MinusGradeItem(temp);
    }

    private void TestAddItem()
    {
        m_Data.AddGradeItem(m_Data.m_Data.gameData.gameItemDataBase.GetGradeItem(1));
    }
    private void TestAddOtehrItem()
    {
        m_Data.AddGradeItem(m_Data.m_Data.gameData.gameItemDataBase.GetGradeItem(0));
    }

    public GradeItemSystem Owner // 캐릭터 데이터 프로퍼티
    {
        get { return m_Data; }
    }
    void OnEnable()
    {
        m_HoveredItem = null; //복사해서 저장할 데이터 공간 초기화
        Tooltip.gameObject.SetActive(false); //아이템 설명창 비활성화
    }

    public void Load(GradeItemSystem data) // 캐릭터 데이터 가져오기
    {
        //m_Data = data; // 매개변수 복사
        for (int i = 0; i < m_ItemEntries.Length; ++i) //아이템 슬롯 반복문
        {
            m_ItemEntries[i].UpdateEntry(); // 장비 아이템 업데이트 
            m_ItemEntries[i].Player = Player;

            if (m_Data.Entries[i] != null)
            {
                m_ItemEntries[i].gameObject.SetActive(true);
            }
            else
                m_ItemEntries[i].gameObject.SetActive(false);
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
    
    
    public void ObjectHoveredEnter(GradeItemEntryUI hovered)//아이템 설명창 활성화
    {
        if(ToolTipActive != true)
        {
           //Tooltip.Getter = ItemTooltip.Own.Player;
           //m_HoveredItem = hovered; //아이템 데이터 복사
           //Tooltip.Owner = this;
           //Tooltip.gameObject.SetActive(true); //아이템 설명창 활성화
           //Item itemUsed = null;                                //Item itemUsed = m_HoveredItem.InventoryEntry != -1 ? m_Data.Entries[m_HoveredItem.InventoryEntry].Item : m_HoveredItem.EquipmentItem;
           //if (m_HoveredItem.InventoryEntry == -1)
           //    return;
           //else
           //     itemUsed = m_Data.Entries[m_HoveredItem.InventoryEntry].Item;
           //Tooltip.Name.text = itemUsed.ItemName; //아이템 이름 복사
           //Tooltip.DescriptionText.text = itemUsed.GetDescription(); //아이템 설명 복사
           ////Item itemUsed = m_HoveredItem.InventoryEntry != -1 ? m_Data.Inventory.Entries[m_HoveredItem.InventoryEntry].Item : m_HoveredItem.EquipmentItem;
           
           //ToolTipActive = true;
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
    //    for (int i = 0; i < ItemSlots.Length; ++i) //아이템 슬롯 길이만큼 반복
    //    {
    //        RectTransform t = ItemSlots[i]; //아이템 위치 복사
    //        if (RectTransformUtility.RectangleContainsScreenPoint(t, position)) //마우스 포인터가 특정 범위안에 있는지 확인
    //        {
    //            if (m_ItemEntries[i] != CurrentlyDragged.DraggedEntry) //현재 드레그하고 있는 아이템이 해당 슬롯의 아이템과 같지 않다면
    //            {
    //                m_Data.ReSort();
    
    //                //아이템 스왑 코드
    //                var prevItem = m_Data.Entries[CurrentlyDragged.DraggedEntry.GradeItemEntry]; 
    //                m_Data.Entries[CurrentlyDragged.DraggedEntry.GradeItemEntry] = m_Data.Entries[i];
    //                m_Data.Entries[i] = prevItem;
    
    //                CurrentlyDragged.DraggedEntry.UpdateEntry();//아이템 슬롯  - 클래스 함수
    //                m_ItemEntries[i].UpdateEntry(); //아이템 슬롯  - 클래스 함수
                    
    //            }
    //        }
    //    }
    //}
}
