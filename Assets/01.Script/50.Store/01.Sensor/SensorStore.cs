using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorStore : MonoBehaviour
{
    [SerializeField] Sprite BlackJack;
    [SerializeField] Sprite Bluffing;
    [SerializeField] Sprite Bonus;
    [SerializeField] Sprite FighterDog;
    [SerializeField] Sprite Gambler;
    [SerializeField] Sprite UnfairPlay;


    [SerializeField] StoreSensorEntry sensorSlotPrefab;
    [SerializeField] SensorItem[] sellSensorData;
    [SerializeField] RectTransform[] sensorSlot;
    [SerializeField] GameObject SensorBuyPopUpWindow;
    //[SerializeField] Button[] buttonSlot;
    //[SerializeField] int[] priceCharacter;
    StoreSensorEntry[] m_Entries;
    int sensorLenth;
    [HideInInspector] public UserDataController dataController;
    SensorItem[] linckSensor;
    int index = 0;
    void Start()
    {
        dataController = FindObjectOfType<UserDataController>();
        int characterLenth = dataController.gameData.gameItemDataBase.GetCharacterLenth();
        CharacterLinck();
        //CharacterLinck(sellCharacterData); //데이터베이스와 연결할건지 직접 데이터를 입력시켜줄건지 

        SlotActiveCheck();
        m_Entries = new StoreSensorEntry[index]; // 아이템 슬롯 크기 할당
        for (int i = 0; i < index; ++i) // 아이템 슬롯 크기만큼 반복
        {
            m_Entries[i] = Instantiate(sensorSlotPrefab, sensorSlot[i]); //프리펩 복사, 부모하위에 복사
            m_Entries[i].Owner = this;
            m_Entries[i].data = linckSensor[i];//dataController.gameData.gameItemDataBase.GetCharacter(i);
            m_Entries[i].Index = i;
            m_Entries[i].sensorEntry = i;
            m_Entries[i].dataBase = dataController.dataBase;
            //m_Entries[i].buyButton = buttonSlot[i];
            m_Entries[i].gameObject.SetActive(false); // 비활성화
            //m_Entries[i].price = priceCharacter[i];
        }
    }
    void Update()
    {
        //ButtonActiveCheck();
        for (int i = 0; i < m_Entries.Length; ++i) //아이템 슬롯 반복문
        {
            m_Entries[i].UpdateEntry(); // 장비 아이템 업데이트 
        }
    }
    public void CloseSensorPopUpWindow()
    {
        if (SensorBuyPopUpWindow != null)
            SensorBuyPopUpWindow.SetActive(false);
    }
    public void OpenSensorPopUpWindow(StoreSensorEntry _Data)
    {
        if (SensorBuyPopUpWindow != null)
            SensorBuyPopUpWindow.SetActive(true);
        Sprite temp = SensorTypeReturn(_Data.data.sensorType);
        if (temp != null)
            SensorBuyPopUpWindow.GetComponent<StoreSensorPopUp>().SetSensorData(_Data, temp);
    }

    Sprite SensorTypeReturn(DataType.SensorType sensorType)
    {
        Sprite sprite = null;
        switch (sensorType)
        {
            case DataType.SensorType.Bonus:
                sprite = Bonus;
                break;
            case DataType.SensorType.FightDog:
                sprite = FighterDog;
                break;
            case DataType.SensorType.Bluffing:
                sprite = Bluffing;
                break;
            case DataType.SensorType.BlackJack:
                sprite = BlackJack;
                break;
            case DataType.SensorType.Gambler:
                sprite = Gambler;
                break;
            case DataType.SensorType.UnfairPlay:
                sprite = UnfairPlay;
                break;
            case DataType.SensorType.END:
                break;
        }
        return sprite;
    }
    void CharacterLinck(SensorItem[] bases = null)
    {
        if (bases != null)
            linckSensor = bases;
        else
        {
            int idx = dataController.gameData.gameItemDataBase.GetSensorLenth();
            SensorItem[] Temp = new SensorItem[idx];
            for (int i = 0; i < idx; i++)
            {
                Temp[i] = dataController.gameData.gameItemDataBase.GetSensor(i);
            }
            linckSensor = Temp;
        }
    }
    int CharacterLenth()
    {
        return linckSensor.Length;
    }
    public void BuySensor(int _num)
    {
        //if (dataController.Money >= m_Entries[_num].data.GoldPrice)
        //{
            SensorItem Temp = m_Entries[_num].data;//dataController.gameData.gameItemDataBase.GetCharacter(_num);
            if (Temp != null)
            {
                Temp.sensorLevel = 1;
                Temp.levelUpDiaPrice = 20;
                Temp.levelUpGoldPrice = 200;

                switch (Temp.sensorType)
                {
                    case DataType.SensorType.Bonus:
                        Temp.percentScore = 10;
                        break;
                    case DataType.SensorType.FightDog:
                        Temp.skillTime = 20f;
                        break;
                    case DataType.SensorType.Bluffing:
                        Temp.skillTime = 60;
                        break;
                    case DataType.SensorType.BlackJack:
                        Temp.percentScore = 30;
                        break;
                    case DataType.SensorType.Gambler:
                        Temp.skillTime = 8;
                        Temp.percentScore = 50;
                    break;
                    case DataType.SensorType.UnfairPlay:
                        Temp.skillTime = 30;
                        break;
                    default:
                        break;
                }

                dataController.sensorSystem.AddSensor(Temp);

            
                //dataController.Money -= m_Entries[_num].data.GoldPrice;
                Debug.Log("아이템을 구매하였습니다. m_Entries.name : " + m_Entries[_num].data.sensorName);
            }
            else
            {
                Debug.Log("직감 지급이 실패하였습니다. 접근 IndexNum : " + _num);
            }
        //}
    }
    void ButtonActiveCheck()
    {
        for (int i = 0; i < index; i++)
        {
            m_Entries[i].gameObject.SetActive(true);

            if ((dataController.Money < m_Entries[i].data.GoldPrice))
            {
                //buttonSlot[i].interactable = false;
            }
            //else
                //buttonSlot[i].interactable = true;
        }
    }
    void SlotActiveCheck()
    {
        index = 0;
        sensorLenth = sensorSlot.Length;
        for (int i = 0; i < sensorLenth; i++)
        {
            if (i < CharacterLenth())
            {
                sensorSlot[i].gameObject.SetActive(true);
                //buttonSlot[i].gameObject.SetActive(true);

                ++index;
                continue;
            }
            else
            {
                sensorSlot[i].gameObject.SetActive(false);
                //buttonSlot[i].gameObject.SetActive(false);
            }
        }
    }
}
