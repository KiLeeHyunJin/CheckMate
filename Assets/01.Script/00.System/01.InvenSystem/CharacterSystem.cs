using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem
{
    public const int CharacterCount = 32;

    public CharacterEntry[] Entries = new CharacterEntry[CharacterCount]; //������ ĭ  �迭 ����
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
                //���
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
            Debug.Log("CharacterChecking PlayerInventory��ü �迭���� null������ ���� CharacterSystem.csharp");
            return false;
        }
        if (Num >= Entries.Length)
        {
            Debug.Log("CharacterChecking PlayerInventory��ü �迭�� ���̸� �Ѿ����ϴ�. CharacterSystem.csharp");
            return false;
        }
        for (int j = 0; j < Entries.Length; j++)//�κ��丮���ִ��� ��ȸ
        {
            if (Entries[j] == null)//��ȸ�ߴµ� �������� ������ ����
                return false;
            else if (Entries[Num] != null)
            {               // �ʿ�����۰� �κ��丮 �������� ������ Ȯ��
                return true;//������ ����
            }
        }
        return false;
    }
    public bool CharacterChecking(CharacterBase character)//ĳ���Ͱ� �����ϴ��� üũ
    {
        if (Entries == null)
        {
            Debug.Log("CharacterChecking PlayerInventory��ü �迭���� null������ ���� CharacterSystem.csharp");
            return false;
        }
        for (int j = 0; j < Entries.Length; j++)//�κ��丮���ִ��� ��ȸ
        {
            if (Entries[j] == null)//��ȸ�ߴµ� �������� ������ ����
                return false;
            else if (character.CharacterName == Entries[j].Character.CharacterName)
            {               // �ʿ�����۰� �κ��丮 �������� ������ Ȯ��
                return true;//������ ����
            }
        }
        return false;
    }
    public void AddCharacter(CharacterBase character) //������ �߰�
    {
        bool found = false;  //������ �߰� �ʱ�ȭ
        int firstEmpty = -1; //ĭ ��ġ ��ȣ �ʱ�ȭ
        for (int i = 0; i < 32; ++i) // ������ ĭ ���� Ž��
        {
            if (Entries[i] == null) // ������ĭ�� ������ ���ٸ�
            {
                if (firstEmpty == -1) //������ ĭ�� ��ȣ�� �ʱ�ȭ ��ȣ���
                    firstEmpty = i; //�������� ���� ĭ �� ó�� ��ȣ�� ����
            }

            else if (Entries[i].Character.name == character.name) //������ĭ�� ������ ������ �߰��� �����۰� ���ٸ�
            {
                found = true; //������ �߰� ������ ����
                bool giveItem = false;
                int Num = m_Data.gameData.gameItemDataBase.GetGradetItemLenth(); //�±���ȭ �� ������ �����´�.
                for (int j = 0; j <= Num; j++)//������ŭ �ݺ��ϸ鼭
                {
                    if (m_Data.gameData.gameItemDataBase.GetGradeItem(j) != null)
                        if (m_Data.gameData.gameItemDataBase.GetGradeItem(j).CharacterName == Entries[i].Character.name) //�ش� ĳ������ �±���ȭ���߰ߵǸ�
                        {
                            giveItem = true;
                            break;
                        }
                }
                if (giveItem == false)
                    Debug.Log(Entries[i].Character.name + "�� �±� �������� ���޵��� ���Ͽ����ϴ�. �±� �������� ����Ͽ����� Ȯ�ιٶ��ϴ�.");
                else
                    Debug.Log(Entries[i].Character.name + "�� �±� �������� ���޹޾ҽ��ϴ�.");
            }
        }

        if (!found && firstEmpty != -1) //�������� �߰ߵ��� �ʾҰ�, �������� ���� ĭ�� �ִٸ�
        {
            CharacterEntry entry = new CharacterEntry(); //������ ���� �����ϰ�
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
                    Entries[firstEmpty] = entry; //�������� ���� ù��° ĭ�� �������� ä���ִ´�.
                    break;
                }
            }
        }
        ReSort();
    }
    void SetSkillAbility(CharacterEntry entry)//��ų ��Ÿ�� �ʱ�ȭ
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

    public bool MinusCharacter(int InventoryEntry) //�������� 1�� ���ҽ�Ų �� ������ 0���ϸ� �����͸� ����.
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
