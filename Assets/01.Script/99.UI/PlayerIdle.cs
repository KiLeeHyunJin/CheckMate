
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : MonoBehaviour
{
    //public int characterNum;
    [SerializeField] GameManager gameManager;

    [SerializeField] UserDataController userData;
    public DataType.CharacterType OnCharacterType;

    [SerializeField] Animator HeartQueen;
    [SerializeField] Animator Allice;
    [SerializeField] Animator Wolf;
    [SerializeField] Animator CapSaller;

    [SerializeField] DataType.CharacterType characterType;
    [SerializeField] CharacterSelectLobby SelectLobby;
    Animator mainSkeletonAnimation;
    public bool isChange { get; set; }
    bool isOneCheck;

    void OffAllCharacter()// 전부 종료
    {
        if(HeartQueen != null)
            HeartQueen.gameObject.SetActive(false);
        if(Allice != null)
            Allice.gameObject.SetActive(false);
        if(Wolf != null)
            Wolf.gameObject.SetActive(false);
        if (CapSaller != null)
            CapSaller.gameObject.SetActive(false);
    }
    void OnCharacter(DataType.CharacterType characterType) //캐릭터 하나만 On
    {
        Animator anim = null;

        switch (characterType)
        {
            case DataType.CharacterType.HeartQueen:
                if (HeartQueen != null)
                    anim = HeartQueen;
                break;
            case DataType.CharacterType.Wolf:
                if (Wolf != null)
                    anim = Wolf;
                break;
            case DataType.CharacterType.Allice:
                if (Allice != null)
                    anim = Allice;
                break;
            case DataType.CharacterType.CapSaller:
                if (CapSaller != null)
                    anim = CapSaller;
                break;
            case DataType.CharacterType.END:
                break;
            default:
                break;
        }
        if (anim != null)
        {
            anim.gameObject.SetActive(true);
            anim.speed = 0.3f;
        }
    }
    private void Awake()
    {
        OffAllCharacter(); //우선 전부 종료
        isOneCheck = false;
    }

    public void OnCheck(DataType.CharacterType _characterType)
    {
        isOneCheck = true;
        characterType = OnCharacterType = _characterType;
        Debug.Log("Input Value is " + characterType.ToString());
        //if (UserDataController.Instance.characterSystem.CharacterChecking(characterNum)) //해당 인덱스 캐릭터 확인
        //{
        //    CharacterEntry Temp = UserDataController.Instance.characterSystem.Entries[characterNum]; //캐릭터 정보가져오기
        //    OnCharacterType = Temp.Character.CharacterType; //해당 타입 가져오기
        //}
        //else
        //{

            //OnCharacterType = DataType.CharacterType.END; //타입 없음
        //}
        if(OnCharacterType != DataType.CharacterType.END)
            CharacterOnOff(OnCharacterType); //해당 타입 캐릭터 켜기
    }
    // Update is called once per frame
    void Update()
    {
        if (!isOneCheck)
            return;
        if (isChange)
        {
            //characterType = SelectLobby.GetCurrentCharacterType();
            ChangeCharacter(characterType);
            //CharacterOnOff(characterType);
            //ChangeCharacter(int _num);
            isChange = false;
        }
    }

    public void ChangeCharacter(DataType.CharacterType _characterType = DataType.CharacterType.HeartQueen, int _num = 99)
    {
        //DataType.CharacterType temp = _characterType; //타입 복사

        if(_num != 99) 
        {
            if (userData.characterSystem.Entries != null)
            {
                if (userData.characterSystem.Entries.Length > _num)
                {
                    if (userData.characterSystem.Entries[_num] != null)
                    {
                        _characterType = userData.characterSystem.Entries[_num].Character.CharacterType;
                    }
                }
            }
        }

        if(_characterType != characterType) //타입이 같지 않다면
        {
            characterType = _characterType;
            OnCharacterType = _characterType; //업데이트
            CharacterOnOff(_characterType); //캐릭터 다시 켜기
        }
    }
    void CharacterOnOff(DataType.CharacterType characterType)
    {
        OffAllCharacter(); //전부 종료
        OnCharacter(characterType); //해당 타입만 켜기
        isChange = false;

        switch (characterType) 
        {
            case DataType.CharacterType.HeartQueen:
                mainSkeletonAnimation = HeartQueen;
                break;
            case DataType.CharacterType.Allice:
                mainSkeletonAnimation = Allice;
                break;
            case DataType.CharacterType.CapSaller:
                mainSkeletonAnimation = CapSaller;
                break;
            case DataType.CharacterType.Wolf:
                mainSkeletonAnimation = Wolf;
                break;
            case DataType.CharacterType.END:
                break;
            default:
                Debug.Log("Fata Error! : PlayerIdle.cs _ 55Line");
                break;
        }

        if (mainSkeletonAnimation != null) //애니메이션 재생
        {
            mainSkeletonAnimation.SetBool("run", false);
            mainSkeletonAnimation.SetFloat("IdleSpeed", 0.5f);
        }
    }
}
