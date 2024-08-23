using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InvenSensorEntryUI : MonoBehaviour, IPointerClickHandler
{

    public Image IconeImage;
    public TextMeshProUGUI ItemLevel; //���� �ش� ������ ����
    //public TextMeshProUGUI TypeLevel;
    public TextMeshProUGUI ItemName;
    public Button LevelUpButton;
    public Image[] LevelButtonImage;
    public int InventoryEntry { get; set; } = -1; // �ش� ���� ��ȣ
    //public EquipmentItem EquipmentItem { get; private set; } //��� ������
    public InvenSensorUI Owner { get; set; } //�κ��丮 UI ����
    public int Index { get; set; }
    //public FeildStorage feild { get; set; }
    public bool Player;
    public SensorItem Data;
    bool isPossible = false;

    public void OnPointerClick(PointerEventData eventData) //Ŭ��
    {
        if (InventoryEntry != -1)
        {
            Debug.Log("CharacterIndex Num : " + InventoryEntry);
        }
    }
    public void UpdateEntry() //�ش� �������� ������ ������Ʈ
    {
        Image img = LevelUpButton.GetComponent<Image>();
        if(img != null)
            img.color = new Color(0, 0, 0, 0);

        if (Owner == null)
            Debug.Log("ItemEntryUI _ Owner :" + Owner);
        if (Owner.m_Data == null)
            Debug.Log("ItemEntryUI _ Owner.m_Data : " + Owner.m_Data);
        SensorEntry entry = Owner.m_Data.Entries[InventoryEntry]; //�ش� ���� ĭ�� �������� �����ؿ´�
        bool isEnabled = entry != null; //�����ؿ� entry������ �����Ͱ� ���°��� �ƴ϶�� isEnabled�� �� 

        gameObject.SetActive(isEnabled);//Ȱ��ȭ

        if (isEnabled) //entry�� null�� �ƴ϶��
        {
            if (entry.sensor != null)
            {
                IconeImage.sprite = entry.sensor.sensorSprite; //������ �̹����� ����
                Data = entry.sensor;
                if (entry.has == true) //�������� 1�� �̻��̶��
                {
                    ItemLevel.gameObject.SetActive(true); //�ؽ�Ʈ Ȱ��ȭ
                    ItemLevel.text = entry.sensor.sensorLevel.ToString(); //������ ���� ����
                    //TypeLevel.text = entry.sensor.type.ToString(); //������ ���� ����
                    ItemName.text = entry.sensor.sensorName.ToString();
                    LevelUpCheck();
                }
                else
                {
                    ItemLevel.gameObject.SetActive(false); //�ؽ�Ʈ ��Ȱ��ȭ
                }
            }
            else
                Debug.Log("entry.sensor�� �����Ͱ� null�Դϴ�.");
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
        if (LevelUpPriceCheck())//�ִ� ������ �ƴ϶��
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
            Debug.Log("�ִ� �����Դϴ�.");
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
            Debug.Log("�ִ� �����Դϴ�.");
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
                    Debug.Log("�߸��� �����Դϴ�. CharacterLevel : " + Data.sensorLevel);
                    break;
            }
        }
    }
    Vector3 UnscaleEventDelta(Vector3 vec)//�巡�� �̵� ��� �Լ�
    {
        Vector2 referenceResolution = Owner.DragCanvasScaler.referenceResolution;
        Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

        float widthRatio = currentResolution.x / referenceResolution.x;
        float heightRatio = currentResolution.y / referenceResolution.y;
        float ratio = Mathf.Lerp(widthRatio, heightRatio, Owner.DragCanvasScaler.matchWidthOrHeight);

        return vec / ratio;
    }
}
