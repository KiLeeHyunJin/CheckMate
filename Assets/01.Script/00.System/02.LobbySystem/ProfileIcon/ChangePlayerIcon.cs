using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerIcon : MonoBehaviour
{
    [SerializeField] GameObject iconWindow;
    [SerializeField] GameObject Black;
    [SerializeField] RectTransform[] rectTransform;
    public Image[] profileIcon;

    [SerializeField] PlayerIconEntry playerIconPrefab;
    [SerializeField] float flickerSpeed;
    [SerializeField] float AlphaValue;
    //[Header("부모객체")]
    //public GameObject iconPanel;
    PlayerIconEntry[] m_IconEntries;//아이템 슬롯 데이터 복사
    public UserDataController userData;
    public CharacterSystem m_Data;
    int index;
    public int selectIndex;
    int beforeIndex;
    Sprite selectIcon;
    bool isPlus;
    bool isFirst;
    // Start is called before the first frame update
    private void Awake()
    {
        //int IconNum = m_Data.m_Data.UserCharacterIcon;
        //int EntriesLenth = m_Data.Entries.Length;

        //if (IconNum != 99)
        //{
        //    if (EntriesLenth >= IconNum && m_Data.Entries[IconNum] != null)
        //    {
        //        if (icon != null && m_Data.m_Data.dataBase.GetCharacter(IconNum).IconImage != null)
        //        {
        //            icon.sprite = m_Data.m_Data.dataBase.GetCharacter(IconNum).IconImage;
        //            return;
        //        }
        //    }
        //}
        //icon.sprite = null;
        if (flickerSpeed <= 0)
            flickerSpeed = 1.2f;
        if(userData == null)
            userData = FindObjectOfType<UserDataController>();
        m_Data = userData.characterSystem;
        m_IconEntries = new PlayerIconEntry[rectTransform.Length]; // 아이템 슬롯 크기 할당
        isFirst = true;
        for (int i = 0; i < m_IconEntries.Length; ++i) // 아이템 슬롯 크기만큼 반복
        {
            m_IconEntries[i] = Instantiate(playerIconPrefab, rectTransform[i]); //프리펩 복사, 부모하위에 복사
            m_IconEntries[i].gameObject.SetActive(false); // 비활성화
            m_IconEntries[i].Owner = this; // ui 연결
            m_IconEntries[i].IconEntry = i; // 몇번째 칸인지 설정
            m_IconEntries[i].Index = i; // 몇번째 칸인지 설정
        }
    }

    void InitFrameColor()
    {
        if (m_IconEntries == null)
            return;
        for (int i = 0; i < m_IconEntries.Length; i++)
        {
            if (m_IconEntries[i] == null || m_IconEntries[i].frameImage == null)
                continue;
            m_IconEntries[i].frameImage.color = Color.white;
        }
    }
    public void SetSelectIndex(int _num)
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        InitFrameColor();
        selectIndex = _num;
    }
    public void SetSelectIcon(Sprite sprite)
    {
        selectIcon = sprite;
    }
    void IconSetCheck()
    {
        selectIndex = m_Data.m_Data.UserCharacterIcon;

        if (profileIcon != null && m_IconEntries != null)
        {
            if (m_IconEntries.Length > selectIndex && m_Data.Entries[selectIndex] != null)
            {
                for (int i = 0; i < profileIcon.Length; i++)
                {
                    if (profileIcon[i].sprite != m_IconEntries[selectIndex].IconImage.sprite)
                    {
                        profileIcon[i].sprite = m_IconEntries[selectIndex].IconImage.sprite;
                    }
                }
            }
            else
            {
                profileIcon[0].gameObject.SetActive(false);
                profileIcon[1].gameObject.SetActive(false);
                return;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isFirst)
        {
            IconSetCheck();
            if (iconWindow.activeSelf)
            {
                if(Black != null)
                Black.SetActive(false);
                iconWindow.GetComponent<Animator>().SetTrigger("IsClose");//SetActive(false);
                if (Black != null)
                    Black.SetActive(false);
            }
            isFirst = false;
        }
        if(m_Data != null)
            this.Load(m_Data);
        UpdateIcon();
        FlickerIcon();
    }
    void FlickerIcon()
    {
        //bool isSame = false;
        if (profileIcon != null && m_IconEntries != null)
        {
            Color fullColor = new Color(1, 1, 1, 1);

            for (int i = 0; i < m_IconEntries.Length; i++)
            {
                if (m_IconEntries[i] != null)
                    m_IconEntries[i].IconImage.color = fullColor;
            }

            if (m_IconEntries.Length > selectIndex && m_IconEntries[selectIndex] != null)
            {

                 Color color = new Color();
                 float speed = Time.deltaTime * flickerSpeed;
                 color = m_IconEntries[selectIndex].frameImage.color;
                 color.a = Calculate(color, speed);
                 m_IconEntries[selectIndex].frameImage.color = color;
            }
        }
    }


    float Calculate(Color color, float speed)
    {
        if (color.a >= 0.9f && isPlus)
            isPlus = false;
        else if (color.a < 0.1f && !isPlus)
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
        return AlphaValue = color.a;
    }

    public void Load(CharacterSystem data) // 캐릭터 데이터 가져오기
    {
        //CharacterIndex();
        //Debug.Log("Character index :" + index);
        //m_Data = data; // 매개변수 복사
        for (int i = 0; i < m_IconEntries.Length; ++i) //아이템 슬롯 반복문
        {
            if(m_IconEntries[i] != null)
                m_IconEntries[i].UpdateEntry(); //  업데이트 
        }
        //CharacterIndex();
        //EquipementUI.UpdateEquipment(m_Data.m_Data.gameData.characterData.PlayerEquipment, m_Data.m_Data.gameData.UserData.Stats); // 장비아이템 업데이트
    }

    void UpdateIcon()
    {
        int IconNum = m_Data.m_Data.UserCharacterIcon;
        int EntriesLenth = m_Data.Entries.Length;
        if (IconNum != 99)
        {
            if (EntriesLenth >= IconNum && m_Data.Entries[IconNum] != null)
            {
                if (profileIcon != null && m_Data.Entries[IconNum].Character.ProfileIconImage != null)
                {
                    for (int i = 0; i < profileIcon.Length; i++)
                    {
                        if (profileIcon[i] != null)
                        {
                            profileIcon[i].sprite = m_Data.Entries[IconNum].Character.ProfileIconImage;
                        }
                    }
                    return;
                }
            }
        }
        if (profileIcon != null)
        {
            for (int i = 0; i < profileIcon.Length; i++)
            {
                if (profileIcon[i] != null)
                    profileIcon[i].sprite = null;
            }
        }

    }

    void CharacterIndex()
    {
        index = 0;
        for (int i = 0; i < rectTransform.Length; i++)
        {
            if (m_Data != null)
            {
                if (m_Data.Entries != null)
                {
                    if (m_Data.Entries[i] != null)
                    {
                        rectTransform[i].gameObject.SetActive(true);
                        index++;
                    }
                    else
                        rectTransform[i].gameObject.SetActive(false);
                }
                else
                    Debug.Log("CharacterUI_Index : m_Data.Entries != null");
            }
            Debug.Log("CharacterUI_m_Data : m_Data.Entries != null");
        }
    }

    public void ChangeButton()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        if (m_Data != null && m_Data.m_Data != null)
            m_Data.m_Data.UserCharacterIcon = selectIndex;
        if (selectIcon != null && profileIcon != null)
        {
            for (int i = 0; i < profileIcon.Length; i++)
            {
                if (profileIcon[i] != null)
                {
                    profileIcon[i].sprite = selectIcon;
                    if (selectIcon != null && !profileIcon[i].gameObject.activeSelf)
                        profileIcon[i].gameObject.SetActive(true);
                }
            }
        }
        OffWindow();
    }

    public void OffWindow()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);

        if (iconWindow != null)
        {
            iconWindow.GetComponent<Animator>().SetTrigger("IsClose");//.SetActive(false);
            if (Black != null)
                Black.SetActive(false);
        }
    }
    public void OnOffWindow()
    {
        if(iconWindow != null)
        {
            if (iconWindow.activeSelf)
            {
                SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);
                iconWindow.GetComponent<Animator>().SetTrigger("IsClose");//.SetActive(false);
                if (Black != null)
                    Black.SetActive(false);
            }
            else
            {
                SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

                iconWindow.SetActive(true);
                if (Black != null)
                    Black.SetActive(true);
                InitFrameColor();
                selectIndex = m_Data.m_Data.UserCharacterIcon;
            }
        }
    }
}
