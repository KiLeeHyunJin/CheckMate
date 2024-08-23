using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLobbyIconEntry : MonoBehaviour
{
    public int IconEntry { get; set; } = -1; // 해당 슬롯 번호
    //public EquipmentItem EquipmentItem { get; private set; } //장비 아이템
    public CharacterSelectLobby Owner { get; set; } //인벤토리 UI 연결
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
        entry = Owner.characterSystem.Entries[IconEntry];  //해당 슬롯 칸의 아이템을 복사해온다
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

    public void UpdateEntry() //해당 아이템의 정보를 업데이트
    {
        entry = Owner.characterSystem.Entries[IconEntry];  //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 

        gameObject.SetActive(isEnabled);//활성화

        if (isEnabled) //entry가 null이 아니라면
        {
            if (entry.Character.LobbySelectIconImage != null)
            {
                if(IconImage != null)
                {
                    if (IconImage.sprite != entry.Character.LobbySelectIconImage)
                        IconImage.sprite = entry.Character.LobbySelectIconImage; //아이템 이미지로 변경
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
