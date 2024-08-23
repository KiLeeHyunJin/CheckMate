using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreSensorEntry : MonoBehaviour
{
    public Image priceTypeIcon;
    public Image image;
    public Image[] allImage;
    public TextMeshProUGUI price; //현재 해당 아이템 개수
    public TextMeshProUGUI name; //현재 해당 아이템 개수
    [HideInInspector] public SensorItem data;
    public int sensorEntry { get; set; } = -1; // 해당 슬롯 번호
    //public EquipmentItem EquipmentItem { get; private set; } //장비 아이템
    public SensorStore Owner { get; set; } //인벤토리 UI 연결
    public Button buyButton;
    public int Index { get; set; }
    public PlayerLoadSelectData selectData { get; set; }
    public GameItemDataBase dataBase;
    void Start()
    {
        if (Owner != null)
        {
            if (Index <= Owner.dataController.gameData.gameItemDataBase.GetSensorLenth())
            {
                data = Owner.dataController.gameData.gameItemDataBase.GetSensor(sensorEntry);
            }
        }
    }

    public void Update()
    {
    }
    public void BuyButton()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        Owner.OpenSensorPopUpWindow(this);
    }

    public void GoldClick()
    {
        Owner.dataController.Money -= data.GoldPrice;
        AddItem();
    }
    public void DiaClick()
    {
        Owner.dataController.Cash -= data.DiaPrice;
        AddItem();
    }
    void AddItem()
    {
        Owner.BuySensor(sensorEntry);
        Owner.CloseSensorPopUpWindow();
        BuyCount();
    }
    void BuyCount()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Buy);

        if (Owner.dataController != null)
        {
            if (Owner.dataController.countData != null)
            {
                Owner.dataController.countData.BuyAdd();

            }
        }
    }
    public void UpdateEntry() //해당 아이템의 정보를 업데이트
    {
        if (Owner == null)
            Debug.Log("ItemEntryUI _ Owner :" + Owner);
        SensorItem entry = data; //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 

        gameObject.SetActive(isEnabled);//활성화

        if (isEnabled) //entry가 null이 아니라면
        {
            image.sprite = entry.sensorSprite; //아이템 이미지로 변경

            DataType.PriceType priceType = entry.priceType;
            if(priceTypeIcon != null)
            {
                if (dataBase != null)
                {
                    Sprite sprite = null;
                    switch (priceType)
                    {
                        case DataType.PriceType.Gold:
                            sprite = dataBase.GoldePriceImage;
                            break;
                        case DataType.PriceType.Diamond:
                            sprite = dataBase.DiamondPriceImage;
                            break;
                        default:
                            break;
                    }
                    if (sprite != null)
                        priceTypeIcon.sprite = sprite;
                }
                else
                {
                    if (priceTypeIcon.gameObject.activeSelf)
                        priceTypeIcon.gameObject.SetActive(false);
                }
            }
            price.gameObject.SetActive(true); //텍스트 활성화
            //name.gameObject.SetActive(true);
            //name.text = entry.sensorName;
            price.text = string.Format("{0:#,###}", entry.GoldPrice);
            if (!Owner.dataController.sensorSystem.SensorChecking(data) ) //.has == true) //아이템이 1개 이상이라면
            {
                //if((Owner.dataController.Money >= data.GoldPrice || Owner.dataController.Cash >= data.DiaPrice))
                {

                    image.color = new Color(1f, 1f, 1f, 1f);
                    //priceType.sprite = 
                    buyButton.interactable = true;
                    //buyButton.enabled = true;
                    return;
                }
            }
            for (int i = 0; i < allImage.Length; i++)
            {
                allImage[i].color = new Color(0.7f, 0.7f, 0.7f, 1f);
            }
            buyButton.interactable = false;
            //buyButton.enabled = false;
            //price.gameObject.SetActive(false); //텍스트 비활성화
        }
    }
}
