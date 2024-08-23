using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLobbyIconEntry : MonoBehaviour
{
    public int IconEntry { get; set; } = -1; // �ش� ���� ��ȣ
    //public EquipmentItem EquipmentItem { get; private set; } //��� ������
    public CharacterSelectLobby Owner { get; set; } //�κ��丮 UI ����
    public int Index { get; set; }
    [HideInInspector] public Image IconImage;
    [HideInInspector] public Button button;
    DataType.CharacterType type;
    public CharacterEntry entry;
    bool isNotDevelop;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        IconImage = GetComponent<Image>();
        entry = Owner.characterSystem.Entries[IconEntry];  //�ش� ���� ĭ�� �������� �����ؿ´�
        type = entry.Character.CharacterType;

        if (type != DataType.CharacterType.HeartQueen)
        {
            button.interactable = isNotDevelop = false;
        }
        else
            isNotDevelop = true;
    }


    public void SelectIcon()
    {
        //Owner.m_Data.m_Data.UserCharacterIcon = IconEntry;

        if (IconEntry != 99)
        {
            if (Owner.characterSystem.Entries.Length >= IconEntry)
            {
                if (IconImage != null)
                {
                    if(entry != null)
                    {
                        Owner.SaveSelectCharacter(type, this);
                    }
                }
            }
        }
    }

    public void UpdateEntry() //�ش� �������� ������ ������Ʈ
    {
        entry = Owner.characterSystem.Entries[IconEntry];  //�ش� ���� ĭ�� �������� �����ؿ´�
        bool isEnabled = entry != null; //�����ؿ� entry������ �����Ͱ� ���°��� �ƴ϶�� isEnabled�� �� 

        gameObject.SetActive(isEnabled);//Ȱ��ȭ

        if (isEnabled) //entry�� null�� �ƴ϶��
        {
            if (entry.Character.LobbySelectIconImage != null)
            {
                if(IconImage != null)
                {
                    if (IconImage.sprite != entry.Character.LobbySelectIconImage)
                        IconImage.sprite = entry.Character.LobbySelectIconImage; //������ �̹����� ����
                }
            }
        }

        if (button != null && isNotDevelop)
        {
            if (Owner.GetselectCharacterType() == type)
                button.interactable = false;
            else
                button.interactable = true;
        }
    }
}
