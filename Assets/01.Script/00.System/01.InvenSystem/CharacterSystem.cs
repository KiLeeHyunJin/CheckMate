using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem
{
    public const int CharacterCount = 32;

    public CharacterEntry[] Entries = new CharacterEntry[CharacterCount]; //아이템 칸  배열 설정
    public UserDataController m_Data;
    public CharacterEntry[] GetSlots() { return Entries; }
    //(CharacterEntry[] characters,List<int> position, List<string> name, List<int> grade, List<int> level, List<int> strenth, List<int> defense, List<int> agility, List<int> health)
    public void LoadToInventory(int _arrayNum,/* int _position,*/ string _iTemName, /*int _Grade,*/ int _Level,float m_ItemPercent, float m_hpMinusPercent , int _CurrentEXP, int _MaxEXP, GameData Owner)
    {
        for (int i = 0; i < Owner.gameItemDataBase.GetCharacterLenth(); i++)
            if (Owner.gameItemDataBase.GetCharacter(i).CharacterName == _iTemName)
            {
                Entries[_arrayNum] = new CharacterEntry();
                Entries[_arrayNum].Character = Owner.gameItemDataBase.GetCharacter(i);
                //등급
                Entries[_arrayNum].Character.currentLevel = _Level;
                Entries[_arrayNum].Character.hpMinusPercent = m_hpMinusPercent;
                Entries[_arrayNum].Character.itemPercent = m_ItemPercent;
                Entries[_arrayNum].Character.CurrentEXP = _CurrentEXP;
                Entries[_arrayNum].Character.MaxEXP = _MaxEXP;
            }
    }
    public bool CharacterChecking(int Num)
    {
        if (Entries == null)
        {
            Debug.Log("CharacterChecking PlayerInventory전체 배열에서 null포인터 감지 CharacterSystem.csharp");
            return false;
        }
        if (Num >= Entries.Length)
        {
            Debug.Log("CharacterChecking PlayerInventory전체 배열의 길이를 넘었습니다. CharacterSystem.csharp");
            return false;
        }
        for (int j = 0; j < Entries.Length; j++)//인벤토리에있는지 순회
        {
            if (Entries[j] == null)//순회했는데 아이템이 없으면 실패
                return false;
            else if (Entries[Num] != null)
            {               // 필요아이템과 인벤토리 아이템이 같은지 확인
                return true;//있으면 성공
            }
        }
        return false;
    }
    public bool CharacterChecking(CharacterBase character)//캐릭터가 존재하는지 체크
    {
        if (Entries == null)
        {
            Debug.Log("CharacterChecking PlayerInventory전체 배열에서 null포인터 감지 CharacterSystem.csharp");
            return false;
        }
        for (int j = 0; j < Entries.Length; j++)//인벤토리에있는지 순회
        {
            if (Entries[j] == null)//순회했는데 아이템이 없으면 실패
                return false;
            else if (character.CharacterName == Entries[j].Character.CharacterName)
            {               // 필요아이템과 인벤토리 아이템이 같은지 확인
                return true;//있으면 성공
            }
        }
        return false;
    }
    public void AddCharacter(CharacterBase character) //아이템 추가
    {
        bool found = false;  //아이템 발견 초기화
        int firstEmpty = -1; //칸 위치 번호 초기화
        for (int i = 0; i < 32; ++i) // 아이템 칸 전부 탐색
        {
            if (Entries[i] == null) // 아이템칸의 정보가 없다면
            {
                if (firstEmpty == -1) //아이템 칸의 번호가 초기화 번호라면
                    firstEmpty = i; //아이템이 없는 칸 중 처음 번호를 기입
            }

            else if (Entries[i].Character.name == character.name) //아이템칸의 아이템 정보가 추가한 아이템과 같다면
            {
                found = true; //아이템 발견 참으로 변경
                bool giveItem = false;
                int Num = m_Data.gameData.gameItemDataBase.GetGradetItemLenth(); //승급재화 총 개수를 가져온다.
                for (int j = 0; j <= Num; j++)//개수만큼 반복하면서
                {
                    if (m_Data.gameData.gameItemDataBase.GetGradeItem(j) != null)
                        if (m_Data.gameData.gameItemDataBase.GetGradeItem(j).CharacterName == Entries[i].Character.name) //해당 캐릭터의 승급재화가발견되면
                        {
                            giveItem = true;
                            break;
                        }
                }
                if (giveItem == false)
                    Debug.Log(Entries[i].Character.name + "의 승급 아이템이 지급되지 못하였습니다. 승급 아이템을 등록하였는지 확인바랍니다.");
                else
                    Debug.Log(Entries[i].Character.name + "의 승급 아이템을 지급받았습니다.");
            }
        }

        if (!found && firstEmpty != -1) //아이템이 발견되지 않았고, 아이템이 없는 칸이 있다면
        {
            CharacterEntry entry = new CharacterEntry(); //아이템 변수 생성하고
            for (int i = 0; i < m_Data.gameData.gameItemDataBase.GetCharacterLenth(); i++)
            {
                if (m_Data.gameData.gameItemDataBase.GetCharacter(i).CharacterName == character.CharacterName)
                {
                    entry.Character = m_Data.gameData.gameItemDataBase.GetCharacter(i);
                    if (entry != null)
                    {
                        entry.Character.currentLevel = 1;
                        entry.Character.levelDiaPrice = 50;
                        entry.Character.levelGoldPrice = 200;
                        SetSkillAbility(entry);
                    }
                    Entries[firstEmpty] = entry; //아이템이 없는 첫번째 칸에 아이템을 채워넣는다.
                    break;
                }
            }
        }
        ReSort();
    }
    void SetSkillAbility(CharacterEntry entry)//스킬 쿨타임 초기화
    {
        switch (entry.Character.CharacterType)
        {
            case DataType.CharacterType.HeartQueen:
                entry.Character.SkillAbility = 20;
                break;
            case DataType.CharacterType.Allice:
                entry.Character.SkillAbility = 5;
                break;
            case DataType.CharacterType.Wolf:
                entry.Character.SkillAbility = 5;
                break;
            case DataType.CharacterType.CapSaller:
                entry.Character.SkillAbility = 5;
                break;
            default:
                break;
        }
    }

    public bool MinusCharacter(int InventoryEntry) //아이템을 1개 감소시킨 뒤 개수가 0이하면 데이터를 삭제.
    {
        return false;
    }

    public void ReSort()
    {
        CharacterEntry[] Save = new CharacterEntry[32];
        int Num = 0;
        for (int i = 0; i < 32; ++i)
        {
            if (Entries[i] != null)
            {
                Save[Num] = Entries[i];
                Num++;
            }
        }
        for (int i = 0; i < 32; ++i)
        {
            Entries[i] = Save[i];
        }
    }
}
