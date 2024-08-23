using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCharacterEntry : MonoBehaviour
{
    [SerializeField] Color disColor;
    public Image IconeImage;
    public Button button;
    public Sprite selectImage;
    public Sprite completeImage;
    //public TextMeshProUGUI Level; //현재 해당 아이템 개수
    //public TextMeshProUGUI Name;
    //[HideInInspector] public Image Position; //현재 해당 아이템 개수
    //[HideInInspector] public Image Grade; //현재 해당 아이템 개수
    public DataType.CharacterType characterType;
    public int CharacterEntry = -1; // 해당 슬롯 번호
    public PlayCharacterSelect Owner { get; set; } //인벤토리 UI 연결
    public int Index { get; set; }
    public bool Player;
    public PlayerLoadSelectData selectData { get; set; }

    private void OnEnable()
    {
        if(characterType != DataType.CharacterType.HeartQueen)
        {
            if(button == null)
                button = gameObject.AddComponent<Button>();
            if(button != null)
            {
                button.interactable = false;
                IconeImage.color = disColor;
            }
        }
    }

    public void ButtonClick() //클릭
    {
        if(characterType == DataType.CharacterType.HeartQueen)
        {
            if (CharacterEntry != -1)
            {
                Debug.Log("[OnPointerClick]CharacterIndex Num : " + CharacterEntry);
                selectData.SelectCharacter(CharacterEntry);
            }
        }
    }
    //public void PointerClick()
    //{
    //    if (CharacterEntry != -1)
    //    {
    //        Debug.Log("[PointerClick]CharacterIndex Num : " + CharacterEntry);
    //    }
    //}
    public void UpdateEntry() //해당 아이템의 정보를 업데이트
    {
        CharacterEntry entry = Owner.m_Data.Entries[CharacterEntry]; //해당 슬롯 칸의 아이템을 복사해온다
        bool isEnabled = entry != null; //복사해온 entry변수가 데이터가 없는것이 아니라면 isEnabled는 참 

        gameObject.SetActive(isEnabled);//활성화

        if (isEnabled) //entry가 null이 아니라면
        {
            if(selectData.GetSelectCharacter() == CharacterEntry)
            {
                button.GetComponent<Image>().sprite = completeImage;
                IconeImage.color = disColor;
            }
            else
            {
                button.GetComponent<Image>().sprite = selectImage;
                IconeImage.color = Color.white;
            }

            IconeImage.sprite = entry.Character.readyCharacterImage; //아이템 이미지로 변경

            if (entry.Character != null) //아이템이 1개 이상이라면
            {
                //if (Level != null)
                //    Level.text = entry.Character.currentLevel.ToString(); //아이템 개수 기입
                //if (Name != null)
                //    Name.text = entry.Character.name.ToString();
            }
        }
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
