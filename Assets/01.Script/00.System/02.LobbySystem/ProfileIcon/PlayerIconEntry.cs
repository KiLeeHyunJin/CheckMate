using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconEntry : MonoBehaviour
{
    public int IconEntry { get; set; } = -1; // 해당 슬롯 번호
    //public EquipmentItem EquipmentItem { get; private set; } //장비 아이템
    public ChangePlayerIcon Owner { get; set; } //인벤토리 UI 연결
    public int Index { get; set; }
    [SerializeField] public Image IconImage;
    [SerializeField] public Image frameImage;
    [HideInInspector] public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }
    public void SelectIcon()
    {
        if (IconEntry != 99)
        {
            if (Owner.m_Data.Entries.Length >= IconEntry)
            {
                if (IconImage != null)
                {
                    if (Owner.profileIcon != null)
                    {
                        for (int i = 0; i < Owner.profileIcon.Length; i++)
                        {
                            if (Owner.profileIcon[i] != null)
                            {
                                Owner.SetSelectIndex(IconEntry);
                                Owner.SetSelectIcon(IconImage.sprite);
                            }
                        }
                    }
                }
            }
        }
    }

    //public void ChangeIcon()
    //{
    //    Owner.m_Data.m_Data.UserCharacterIcon = IconEntry;
    //    if(IconEntry != 99)
    //    {
    //        if(Owner.m_Data.Entries.Length >= IconEntry)
    //        {
    //            if (IconImage != null)
    //            {
    //                if(Owner.profileIcon != null)
    //                {
    //                    for (int i = 0; i < Owner.profileIcon.Length; i++)
    //                    {
    //                        if (Owner.profileIcon[i] != null)
    //                        {
    //                            Owner.profileIcon[i].sprite = IconImage.sprite;
    //                            Owner.selectIndex = IconEntry;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public void UpdateEntry() //해당 아이템의 정보를 업데이트
    {
        if (Owner == null && Owner.m_Data.Entries == null && Owner.m_Data.Entries.Length >= IconEntry)
            return;
        CharacterEntry entry = Owner.m_Data.Entries[IconEntry];  //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 

        if (isEnabled)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(isEnabled);//활성화
                if (entry.Character.ProfileIconImage != null)
                    if (IconImage.sprite != entry.Character.ProfileIconImage)
                        IconImage.sprite = entry.Character.ProfileIconImage; //아이템 이미지로 변경
            }
        }
        else
            gameObject.SetActive(false);



        if (button != null) // 현재 적용중인 아이콘 클릭 시 선택 불가
        {
            //if (Owner.m_Data.m_Data.UserCharacterIcon == IconEntry)
            //    button.interactable = false;
            //else
            //    button.interactable = true;
        }
    }
}
