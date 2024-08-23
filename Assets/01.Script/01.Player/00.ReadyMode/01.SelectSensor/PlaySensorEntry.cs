using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaySensorEntry : MonoBehaviour
{
    public Image IconeImage;
    public Button button;
    public Sprite selectImage;
    public Sprite completeImage;
    //public TextMeshProUGUI ItemLevel; //현재 해당 아이템 레벨
    //public TextMeshProUGUI ItemName; //현재 해당 아이템 이름

    public int InventoryEntry { get; set; } = -1; // 해당 슬롯 번호
    //public EquipmentItem EquipmentItem { get; private set; } //장비 아이템
    public PlaySensorSelect Owner { get; set; } //인벤토리 UI 연결
    public int Index { get; set; }
    //public FeildStorage feild { get; set; }
    public bool Player;
    public bool iswear;
    [SerializeField] TextMeshProUGUI levelTxt;

    public PlayerLoadSelectData selectData { get; set; }
    Image buttonImage;
    public void Update()
    {

    }
    public void ButtonClick()//클릭
    {
        Debug.Log("OnPointerClick : " + InventoryEntry);
        if (selectData.isWearSensorCheck(InventoryEntry))
            selectData.UnWearingSensor(InventoryEntry);
        else
            selectData.WearingSensor(InventoryEntry);
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
        SensorEntry entry = Owner.m_Data.Entries[InventoryEntry]; //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 

        gameObject.SetActive(isEnabled);//활성화
        if (isEnabled) //entry가 null이 아니라면
        {
            ButtonImageState();

            if (IconeImage != null)
                IconeImage.sprite = entry.sensor.ReadySensorImage; //아이템 이미지로 변경
            if(levelTxt != null)
                levelTxt.text = entry.sensor.sensorLevel.ToString();
            if (entry.has == true) //아이템이 1개 이상이라면
            {

                //if(ItemLevel != null)
                //{
                //    if(ItemLevel.gameObject.activeSelf)
                //        ItemLevel.gameObject.SetActive(true); //텍스트 활성화
                //    ItemLevel.text = entry.sensor.sensorLevel.ToString();
                //}
                //if(ItemName != null)
                //{
                //    if(ItemName.gameObject.activeSelf)
                //        ItemLevel.gameObject.SetActive(true); //텍스트 활성화
                //    ItemLevel.text = entry.sensor.sensorName.ToString();
                //}
                //ItemCount.text = entry.Count.ToString(); //아이템 개수 기입
            }
            else
            {
                //ItemLevel.gameObject.SetActive(false); //텍스트 비활성화
                //ItemLevel.gameObject.SetActive(false); //텍스트 활성화
            }
        }
    }
    void ButtonImageState()
    {
        if (buttonImage == null)
        {
            buttonImage = button.GetComponent<Image>();
        }
        if (selectData.isWearSensorCheck(InventoryEntry))
        {
            buttonImage.sprite = completeImage;
        }
        else
        {
            buttonImage.sprite = selectImage;
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
    }

    public void OnDrag(PointerEventData eventData) //아이템 드래그 이동
    {
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
