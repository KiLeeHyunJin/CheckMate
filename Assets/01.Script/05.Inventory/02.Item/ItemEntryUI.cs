using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemEntryUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler //아이템 슬롯 이미지 및 아이템 개수 표기
{    
    public Image IconeImage;
    public TextMeshProUGUI ItemCount; //현재 해당 아이템 개수

    public int InventoryEntry { get; set; } = -1; // 해당 슬롯 번호
    //public EquipmentItem EquipmentItem { get; private set; } //장비 아이템
    public InventoryUI Owner { get; set; } //인벤토리 UI 연결
    public int Index { get; set; }
    //public FeildStorage feild { get; set; }
    public bool Player;
    public void Update()
    {
       
    }
    public void OnPointerClick(PointerEventData eventData) //클릭
    {
        Debug.Log("OnPointerClick : " + InventoryEntry);
         //if (eventData.button == PointerEventData.InputButton.Left &&eventData.clickCount % 2 == 0) //더블클릭이라면  <- 작동할 일이 없다.
         //{
         //    if(Owner.Player) //플레이어 인벤토리
         //    {
         //        if (InventoryEntry != -1) // 제대로 설정되어있는 슬롯인지 확인 (아이템 슬롯 번호로 할당안되어있는지 확인)
         //        {
         //            if (Owner.m_Data.Entries[InventoryEntry] != null) //해당 배열에 아이템이 있는지 확인
         //            {                                                 //Owner.ObjectHoveredEnter(this);
         //                Owner.ObjectDoubleClicked(Owner.m_Data.Entries[InventoryEntry]); //아이템 더블클릭 함수 실행
         //                return;
         //            }
         //        }
         //        else //장비
         //        {
         //            Owner.EquipmentDoubleClicked(EquipmentItem); //장비 탈착
         //            return;
         //        }
         //    }
         //    else //하단 인벤토리
         //    {
         //        ItemSwap(); //아이템 교환
         //        return;
         //    }
         //}
         //else if (eventData.button == PointerEventData.InputButton.Right )
         //{
         //    if (InventoryEntry == -1)//장비창
         //    {
         //        if (!Player && Owner.m_Data == Owner.InventoryHUD.gameData.characterData.PlayerInventory)
         //        {//플레이어가 아님 && 인벤 시스템 == 플레이어 인벤토리 시스템 ????????????????
         //            Owner.EquipmentDoubleClicked(EquipmentItem);
         //            return;
         //        }
         //    }
         //    else if (Owner.m_Data.Entries[InventoryEntry] != null)//해당 슬롯의 아이템이 NULL값이 아니라면
         //    {
         //        ItemSwap();
         //    }
         //}
         //Item Temp = Owner.Owner.Entries[InventoryEntry].Item;//인벤토리의 아이템을 복사한다.
         //bool check =  Owner.Owner.MinusItem(InventoryEntry); //인벤토리의 아이템을 한개 축소한다.
         //Owner.InventoryHUD.BottomInventorySystem.AddItem(Temp); //스토리지에 아이템 붙여넣는다.
    }

    private void ItemSwap()
    {
        //if (Owner.InventoryHUD.InventoryUI[1] == null || Owner.InventoryHUD.InventoryUI[0])
        //return;
        //if (Owner == Owner.InventoryHUD.InventoryUI[1])//InventoryUI 1번 배열에서 클릭했다면
        //{
        //    Item Temp = Owner.m_Data.Entries[InventoryEntry].Item;//아이템을 복사
        //    Owner.InventoryHUD.InventorySystem[1].MinusItem(InventoryEntry); // 1번 배열에 아이템 삭제
        //    Owner.InventoryHUD.InventorySystem[0].AddItem(Temp); // 0번 배열에 아이템 추가
        //}
        //else if (Owner == Owner.InventoryHUD.InventoryUI[0])//InventoryUI 0번 배열에서 클릭했다면
        //{
        //    Item Temp = Owner.m_Data.Entries[InventoryEntry].Item;//아이템을 복사
        //    Owner.InventoryHUD.InventorySystem[0].MinusItem(InventoryEntry); // 1번 배열에 아이템 삭제
        //    Owner.InventoryHUD.InventorySystem[1].AddItem(Temp);// 1번 배열에 아이템 추가
        //}
    }
    

    public void OnPointerEnter(PointerEventData eventData)//아이템슬롯에 커서 진입 함수
    {
    }

    public void OnPointerExit(PointerEventData eventData)//아이템슬롯에 커서 탈출 함수
    {
        //Owner.ObjectHoverExited(this); // 해당 아이템의 설명창 비활성화
    }

    public void UpdateEntry() //해당 아이템의 정보를 업데이트
    {
        if (Owner == null)
            Debug.Log("ItemEntryUI _ Owner :" + Owner);
        if (Owner.m_Data == null)
            Debug.Log("ItemEntryUI _ Owner.m_Data : " + Owner.m_Data);
        InventoryEntry entry = Owner.m_Data.Entries[InventoryEntry]; //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 
    
        gameObject.SetActive(isEnabled);//활성화
    
        if (isEnabled) //entry가 null이 아니라면
        {
            IconeImage.sprite = entry.Item.ItemSprite; //아이템 이미지로 변경

            if (entry.Count > 1) //아이템이 1개 이상이라면
            {
                ItemCount.gameObject.SetActive(true); //텍스트 활성화
                ItemCount.text = entry.Count.ToString(); //아이템 개수 기입
            }
            else
            {
                ItemCount.gameObject.SetActive(false); //텍스트 비활성화
            }
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

    public void OnBeginDrag(PointerEventData eventData) //아이템 드래그 시작함수   - 사용안하는 함수
    {
        //if (eventData.button == PointerEventData.InputButton.Left)
            //if (EquipmentItem != null) //장비아이템의 데이터가 있다면 함수 탈출
            //return;
    
        Owner.CurrentlyDragged = new InventoryUI.DragData();//현재 드레그 변수에 드래그 데이터 재할당
        Owner.CurrentlyDragged.DraggedEntry = this;//현재 드래그 변수에 해당 아이템 복사
        Owner.CurrentlyDragged.OriginalParent = (RectTransform)transform.parent;//현재 드래그 위치의 부모를 현재 위치로 할당
        transform.SetParent(Owner.DragCanvas.transform, true); 
    }
    
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
        float ratio = Mathf.Lerp(widthRatio, heightRatio,  Owner.DragCanvasScaler.matchWidthOrHeight);
 
        return vec / ratio;
    }

    public void OnEndDrag(PointerEventData eventData)//드래그 이동 종료 함수
    {
        //if(EquipmentItem != null)
        //    return;
    
        //Owner.HandledDroppedEntry(eventData.position);
    
        //RectTransform t = transform as RectTransform;
    
        //transform.SetParent(Owner.CurrentlyDragged.OriginalParent, true);

        //t.offsetMax = -Vector2.one * 4;
        //t.offsetMin = Vector2.one * 4;
    }
}
