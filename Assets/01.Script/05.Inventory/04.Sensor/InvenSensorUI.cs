using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSensorUI : MonoBehaviour
{
    public GameObject PopUpWindow;
    public GameObject SensorLevelUpPopUpWindow;
    public class DragData //�巹�׿� ����� �������� ������ ������ Ŭ����
    {
        public InvenSensorEntryUI DraggedEntry; //������ �巡��, �̹���, ����, ����Ŭ��, ���� ���̽� �Լ� 
        public RectTransform OriginalParent; //UI��ġ
    }

    public RectTransform[] SensorSlots; //������ ���� - UI�̹��� ���� �Ҵ�

    public InvenSensorEntryUI CharacterEntryPrefab; //������ ĭ - ������ �ϳ��� �Ҵ�
    public ItemTooltip Tooltip; // ������ ����â - 

    //public EquipmentUI EquipementUI; //��� ������ - ������ UI�Ҵ�

    public Canvas DragCanvas; // ȭ�� ��ü ĵ����
    public CanvasScaler DragCanvasScaler { get; set; }
    public DragData CurrentlyDragged { get; set; }//������ ĭ �̵��� ����� ���� ������Ƽ

    public GameObject characterPanel;
    public InvenSensorEntryUI m_HoveredItem;
    //public InventoryPanelHUD InventoryHUD;
    public bool ToolTipActive = false;
    public bool Player = false;
    InvenSensorEntryUI[] m_SensorEntries;//������ ���� ������ ����
    public SensorSystem m_Data { get; set; }
    public int index = 0;
    public GameObject CharacterInfo;
    public void Start()
    {

        m_Data = FindObjectOfType<UserDataController>().sensorSystem;
        //m_Data = FindObjectOfType<CharacterData>();
        CurrentlyDragged = null; //���� �巹�� ������ ������ �ʱ�ȭ

        DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>(); // �巹�׿� ����� ĵ���� ������Ʈ �Ҵ�

        m_SensorEntries = new InvenSensorEntryUI[SensorSlots.Length]; // ������ ���� ũ�� �Ҵ�

        for (int i = 0; i < m_SensorEntries.Length; ++i) // ������ ���� ũ�⸸ŭ �ݺ�
        {
            m_SensorEntries[i] = Instantiate(CharacterEntryPrefab, SensorSlots[i]); //������ ����, �θ������� ����
            m_SensorEntries[i].Owner = this; // ui ����
            m_SensorEntries[i].InventoryEntry = i; // ���° ĭ���� ����
            m_SensorEntries[i].gameObject.SetActive(false); // ��Ȱ��ȭ
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
    public SensorSystem Owner // ĳ���� ������ ������Ƽ
    {
        get { return m_Data; }
    }
    void OnEnable()
    {
        m_HoveredItem = null; //�����ؼ� ������ ������ ���� �ʱ�ȭ
        if(Tooltip !=null)
            Tooltip.gameObject.SetActive(false); //������ ����â ��Ȱ��ȭ
    }

    public void Load(SensorSystem data) // ĳ���� ������ ��������
    {
        //m_Data = data; // �Ű����� ����

        for (int i = 0; i < m_SensorEntries.Length; ++i) //������ ���� �ݺ���
        {
            m_SensorEntries[i].UpdateEntry(); // ��� ������ ������Ʈ 
            m_SensorEntries[i].Player = Player;
        }
        SensorIndex();
        //EquipementUI.UpdateEquipment(m_Data.m_Data.gameData.characterData.PlayerEquipment, m_Data.m_Data.gameData.UserData.Stats); // �������� ������Ʈ
    }
    //public void ObjectDoubleClicked(InventorySystem.InventoryEntry usedItem)

    public void ObjectDoubleClicked(CharacterEntry usedItem) //����Ŭ���� �������� �Ű������� �����´�.
    {
        //if(m_Data.UseItem(usedItem)) //������ ��� �Լ� ����
        //{ }
        //ObjectHoverExited(m_HoveredItem);// ������ ����â ��Ȱ��ȭ �Լ�
        Load(m_Data);
    }

    //public void EquipmentDoubleClicked(EquipmentItem equItem)
    //{
    //    // InventoryHUD.gameData.characterData.PlayerEquipment.Unequip(equItem.Slot);
    //    //ObjectHoverExited(m_HoveredItem);
    //    Load(m_Data);
    //}

    public void ObjectHoveredEnter(InvenSensorEntryUI hovered)//������ ����â Ȱ��ȭ
    {
        if (ToolTipActive != true)
        {
            Tooltip.Getter = ItemTooltip.Own.Player;
            //m_HoveredItem = hovered; //������ ������ ����
            //Tooltip.Owner = this;
            Tooltip.gameObject.SetActive(true); //������ ����â Ȱ��ȭ
                                                //Item itemUsed = m_HoveredItem.InventoryEntry != -1 ? m_Data.Inventory.Entries[m_HoveredItem.InventoryEntry].Item : m_HoveredItem.EquipmentItem;
            //Item itemUsed = m_HoveredItem.CharacterEntry != -1 ? m_Data.Entries[m_HoveredItem.CharacterEntry].Character : m_HoveredItem.EquipmentItem;
            //CharacterBase character = m_HoveredItem.CharacterEntry != -1 ? m_Data.Entries[m_HoveredItem.CharacterEntry].Character ;
            //Tooltip.Name.text = itemUsed.ItemName; //������ �̸� ����
            //Tooltip.DescriptionText.text = itemUsed.GetDescription(); //������ ���� ����

            ToolTipActive = true;
        }
    }

    public void ObjectHoverExited() //������ ����â ��Ȱ��ȭ
    {
        if (ToolTipActive == true) // �Ű������� �����Ϳ� ���ٸ�
        {
            m_HoveredItem = null;//������ �����

            ToolTipActive = false;
            Tooltip.gameObject.SetActive(false);//����â ��Ȱ��ȭ
        }
    }

    //public void HandledDroppedEntry(Vector3 position)
    //{
    //    for (int i = 0; i < CharacterSlots.Length; ++i) //������ ���� ���̸�ŭ �ݺ�
    //    {
    //        RectTransform t = CharacterSlots[i]; //������ ��ġ ����

    //        if (RectTransformUtility.RectangleContainsScreenPoint(t, position)) //���콺 �����Ͱ� Ư�� �����ȿ� �ִ��� Ȯ��
    //        {
    //            if (m_CharacterEntries[i] != CurrentlyDragged.DraggedEntry) //���� �巹���ϰ� �ִ� �������� �ش� ������ �����۰� ���� �ʴٸ�
    //            {
    //                m_Data.ReSort();

    //                //������ ���� �ڵ�
    //                var prevItem = m_Data.Entries[CurrentlyDragged.DraggedEntry.CharacterEntry];
    //                m_Data.Entries[CurrentlyDragged.DraggedEntry.CharacterEntry] = m_Data.Entries[i];
    //                m_Data.Entries[i] = prevItem;

    //                CurrentlyDragged.DraggedEntry.UpdateEntry();//������ ����  - Ŭ���� �Լ�
    //                m_CharacterEntries[i].UpdateEntry(); //������ ����  - Ŭ���� �Լ�

    //            }
    //        }
    //    }
    //}
}
