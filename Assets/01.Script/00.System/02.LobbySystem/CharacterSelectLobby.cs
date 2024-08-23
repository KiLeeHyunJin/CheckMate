using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectLobby : MonoBehaviour
{
    [SerializeField] GameObject Black;
    [SerializeField] UserDataController m_Data;
    [SerializeField] GameObject popUp;
    [SerializeField] PlayerIdle player;

    [SerializeField] GameObject Alice;
    [SerializeField] GameObject HeartQueen;
    [SerializeField] GameObject Wolf;
    [SerializeField] GameObject Capsaler;

    [SerializeField] RectTransform[] rectTransform;


    [SerializeField] Image CurrentCharacterImage;
    [SerializeField] Image ChangeButton;
    [SerializeField] Sprite ChangeCompleteImage;
    [SerializeField] Sprite ChangeImage;

    [SerializeField] CharacterLobbyIconEntry playerIconPrefab;
    [SerializeField] GameObject changeCheckPopUp;
    DataType.CharacterType selectCharacterType;
    [SerializeField] DataType.CharacterType currentType;
    [SerializeField] float flickerSpeed;
    //[Header("�θ�ü")]
    //public GameObject iconPanel;
    CharacterLobbyIconEntry[] m_IconEntries;//������ ���� ������ ����
    CharacterLobbyIconEntry selectCharacterIcon; //Ŭ���� ������
    CharacterLobbyIconEntry currentCharacterIcon; //���� Ȱ��ȭ�Ǿ��ִ� ������
    public CharacterSystem characterSystem { get; set; }
    bool isOn;
    bool isFirst;
    bool isPlus;


    int selectIdx;
    int currentIdx;
    private void Start()
    {
        if(m_Data == null)
            m_Data = FindObjectOfType<UserDataController>();
        //if (currentType == DataType.CharacterType.END)
        //{
        //    AllSetFalse();
        //    //HeartQueen.SetActive(true);
        //}
        //else
        //    OnCharacter(currentType);

        m_IconEntries = new CharacterLobbyIconEntry[rectTransform.Length]; // ������ ���� ũ�� �Ҵ�
        if (rectTransform != null)
        {
            for (int i = 0; i < m_IconEntries.Length; i++)
            {
                m_IconEntries[i] = Instantiate(playerIconPrefab, rectTransform[i]); //������ ����, �θ������� ����
                m_IconEntries[i].gameObject.SetActive(false); // ��Ȱ��ȭ
                m_IconEntries[i].Owner = this; // ui ����
                m_IconEntries[i].IconEntry = i; // ���° ĭ���� ����
            }
        }
        isFirst = true;
        isOn = false;
        MainCharacterImage(currentType);
        popUp.SetActive(isOn);
        if (Black != null)
            Black.SetActive(isOn);
        changeCheckPopUp.SetActive(isOn);
    }

    void OnCheck()
    {
        characterSystem = m_Data.characterSystem;

        if (characterSystem.Entries != null)
        {
            selectCharacterType = m_Data.characterSystem.m_Data.characterType;
            currentType = GetCurrentCharacterType();

            bool findHeartQueen = false;
            for (int i = 0; i < characterSystem.Entries.Length; i++)
            {
                if (characterSystem.Entries[i] != null)
                {
                    if (characterSystem.Entries[i].Character.CharacterType == DataType.CharacterType.HeartQueen)
                    {
                        //HeartQueen.SetActive(true);
                        //MainCharacterImage(currentType);
                        findHeartQueen = true;
                    }
                }
            }
            if (findHeartQueen == false)
            {
                AllSetFalse();
                characterSystem.m_Data.characterType = currentType = selectCharacterType = DataType.CharacterType.END;
            }
            if (selectCharacterType == DataType.CharacterType.END)
                currentIdx = 99;
            else
            {
                for (int i = 0; i < characterSystem.Entries.Length; i++)
                {
                    if (characterSystem.Entries[i].Character.CharacterType == currentType)
                    {
                        currentIdx = i;
                        selectCharacterType = currentType;
                        player.OnCheck(selectCharacterType);
                        if (m_IconEntries[i] != null)
                            SaveSelectCharacter(selectCharacterType, m_IconEntries[i]);
                        YesButton();
                        return;
                    }
                }
            }
            player.OnCheck(selectCharacterType);
        }
    }
    void FlickerIcon()
    {
        //bool isSame = false;
        if (m_IconEntries != null)
        {
            Color fullColor = new Color(1, 1, 1, 1);

            for (int i = 0; i < m_IconEntries.Length; i++)
            {
                if (i == currentIdx)
                    continue;
                if (m_IconEntries[i] != null && m_IconEntries[i].IconImage != null)
                {
                    m_IconEntries[i].IconImage.color = fullColor;
                    if (m_IconEntries[i].entry.Character.CharacterType != DataType.CharacterType.HeartQueen)
                    {
                        m_IconEntries[i].IconImage.color = new Color(0.8f, 0.8f, 0.8f, 1);
                    }
                }
            }

            if (m_IconEntries.Length > currentIdx && m_IconEntries[currentIdx] != null)
            {
                if(m_IconEntries[currentIdx].gameObject.activeSelf)
                {
                    if(m_IconEntries[currentIdx].IconImage != null)
                    {
                        float speed = Time.deltaTime * flickerSpeed;
                        fullColor = m_IconEntries[currentIdx].IconImage.color;
                        fullColor.a = Calculate(fullColor, speed);
                        m_IconEntries[currentIdx].IconImage.color = fullColor;
                    }
                }
            }
        }
    }

    float Calculate(Color color, float speed)
    {
        if (color.a > 0.8f && isPlus)
            isPlus = false;
        else if (color.a < 0.3f && !isPlus)
            isPlus = true;

        switch (isPlus)
        {
            case true:
                color.a += speed;
                break;
            case false:
                color.a -= speed;
                break;
        }
        Debug.Log(color.a);
        return color.a;
    }


    void MainCharacterImage(DataType.CharacterType character)
    {
        if (CurrentCharacterImage == null)
            return;
        for (int i = 0; i < m_Data.characterSystem.Entries.Length; i++)
        {
            if (m_Data.characterSystem.Entries == null)
                return;
            if (m_Data.characterSystem.Entries[i] == null)
                continue;
            if(m_Data.characterSystem.Entries[i].Character.CharacterType == character)
            {
                CurrentCharacterImage.sprite = m_Data.characterSystem.Entries[i].Character.mainCharacter;
            }
        }
    }
    public DataType.CharacterType GetselectCharacterType()
    {
        return selectCharacterType;
    }

    private void Update()
    {
        if(isFirst)
        {
            isFirst = false;

            OnCheck();
            //OnCharacter(selectCharacterType);
        }
        if (isOn)
        {
            this.Load(characterSystem);
            FlickerIcon();
        }
    }

    void ChangeCharacterColor(Color color)//,bool isMain)
    {
        if (m_IconEntries == null)
            return;
        for (int i = 0; i < m_IconEntries.Length; i++)
        {
            if(m_IconEntries[i] != null)
            {
                if(m_IconEntries[i].entry != null)
                {
                    //if(isMain) //���� �̹����� ȸ��
                    {
                        if (m_IconEntries[i].entry.Character.CharacterType == currentType)
                        {
                            m_IconEntries[i].IconImage.color = color;
                        }
                        else
                        {
                            m_IconEntries[i].IconImage.color = Color.white;
                        }
                    }
                }
            }
        }
    }

    public void SaveSelectCharacter(DataType.CharacterType characterType, CharacterLobbyIconEntry _selectIconEntry) //ĳ���͸� Ŭ��������
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        if (characterType < DataType.CharacterType.END)
        {
            if (selectCharacterType != characterType)
            {
                selectCharacterIcon = _selectIconEntry;
                currentIdx = _selectIconEntry.IconEntry;
            }
            selectCharacterType = characterType;
            MainCharacterImage(characterType);
        }
        else
            return;
        if (ChangeButton != null)
        {
            if (selectCharacterType == currentType)
            {
                ChangeButton.sprite = ChangeCompleteImage;
            }
            else
            {
                ChangeButton.sprite = ChangeImage;
            }
        }
        //ChangeCharacterColor(new Color(0.7f, 0.7f, 0.7f, 1));
    }

    public DataType.CharacterType GetCurrentCharacterType() //���� ������� ĳ���͸� ����������
    {
        if(characterSystem != null)
        {
            return characterSystem.m_Data.characterType;
        }
        else
            return DataType.CharacterType.END;
    }
    void AllSetFalse() //���� ����
    {
        if (Alice != null)
            Alice.SetActive(false);
        if (HeartQueen != null)
            HeartQueen.SetActive(false);
        if (Wolf != null)
            Wolf.SetActive(false);
        if (Capsaler != null)
            Capsaler.SetActive(false);
    }

    void OnCharacter(DataType.CharacterType characterType = DataType.CharacterType.END)
    {
        AllSetFalse();
        GameObject selectCharacter = null;

        switch (characterType)
        {
            case DataType.CharacterType.HeartQueen:
                selectCharacter = HeartQueen;
                break;
            case DataType.CharacterType.Wolf:
                selectCharacter = Wolf;
                break;
            case DataType.CharacterType.Allice:
                selectCharacter = Alice;
                break;
            case DataType.CharacterType.CapSaller:
                selectCharacter = Capsaler;
                break;
            default:
                break;
        }

        //if(selectCharacter == null)
        //{
        //    selectCharacter = HeartQueen;
        //    selectCharacter.SetActive(true);
        //}

        if (selectCharacter != null && selectCharacter.GetComponent<Animator>() != null)
        {
            selectCharacter.SetActive(true);
            selectCharacter.GetComponent<Animator>().SetBool("run", false);
            selectCharacter.GetComponent<Animator>().SetFloat("IdleSpeed",0.5f);
        }
    } //ĳ���� Ȱ��ȭ

    public void Load(CharacterSystem data) // ĳ���� ������ ��������
    {
        if(m_IconEntries != null)
        {
            for (int i = 0; i < m_IconEntries.Length; ++i) //������ ���� �ݺ���
            {
                if (m_IconEntries[i] != null)
                {
                    m_IconEntries[i].UpdateEntry(); // ��� ������ ������Ʈ 
                    //SelectCharacterMiddleColor();
                }
            }
        }
    }

    public void OpenCharacterWindow()//�˾�â ����
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        isOn = true;
        popUp.SetActive(isOn);

        if (Black != null)
            Black.SetActive(isOn);
        //ChangeCharacterColor(Color.gray);
    }

    public void SaveCharacter() //�����ư�� ��������
    {
        if(selectCharacterType != DataType.CharacterType.END)
        {
            changeCheckPopUp.SetActive(true);
        }

    }

    public void YesButton() //ĳ���� ����
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        if (characterSystem != null)
        {
            int IconNum = 99;
            for (int i = 0; i < characterSystem.m_Data.characterSystem.Entries.Length; i++)
            {
                if(characterSystem.m_Data.characterSystem.Entries[i] != null)
                {
                   if(characterSystem.m_Data.characterSystem.Entries[i].Character.CharacterType == selectCharacterType)
                    {
                        characterSystem.m_Data.characterType = selectCharacterType;
                        IconNum = i;
                    }
                }
            }
            int EntriesLenth = characterSystem.Entries.Length;

            if (IconNum != 99)
            {
                if (EntriesLenth >= IconNum && characterSystem.Entries[IconNum] != null)
                {
                    DataType.CharacterType characterType = characterSystem.Entries[IconNum].Character.CharacterType;
                    currentType = selectCharacterType;
                    OnCharacter(characterType);
                }
            }
        }
        else
            Debug.Log("characterSystem is NULL Data _CharacterSelectLobby.cs 164Line");
        if(selectCharacterType != currentType)
            characterSystem.m_Data.characterType = currentType;

        player.ChangeCharacter(currentType);

        selectCharacterType = DataType.CharacterType.END;
        //ChangeCharacterColor(new Color(0.7f,0.7f,0.7f,1));
        if(ChangeButton != null)
            ChangeButton.sprite = ChangeCompleteImage;
        Animator anim = changeCheckPopUp.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("IsClose");//.SetActive(false);

        if (Black != null)
            Black.SetActive(false);
    }

    public void NoButton() //���� ���
    {
        selectCharacterType = DataType.CharacterType.END;
        changeCheckPopUp.GetComponent<Animator>().SetTrigger("IsClose");//.SetActive(false);

        if (Black != null)
            Black.SetActive(false);
    }

    public void OffCharacterWindow()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);

        isOn = false;
        if (isOn == false)
            popUp.GetComponent<Animator>().SetTrigger("IsClose");
        else
            popUp.SetActive(isOn);
        if (Black != null)
            Black.SetActive(isOn);
    }

}
