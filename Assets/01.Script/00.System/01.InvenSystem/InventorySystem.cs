using System.Collections.Generic;
using UnityEngine;

//캐릭터의아이템의 움직임 담당 스크립트 (저장 관련 스크립트가 아니다.)
public class InventorySystem //아이템 합치기 줄이기
{
    public const int ItemCount = 32;
    
    public InventoryEntry[] Entries = new InventoryEntry[ItemCount]; //아이템 칸  배열 설정
    public InventoryEntry[] GetSlots() { return Entries; }
    public UserDataController m_Data;

    
    public void LoadToInventory(int _arrayNum, string _iTemName, int _itemNum, GameData Owner) 
    {
      for (int i = 0; i < Owner.gameItemDataBase.GetItemLenth(); i++)
        if(Owner.gameItemDataBase.GetItem(i).ItemName == _iTemName)
        {
            Entries[_arrayNum] = new InventoryEntry();
            Entries[_arrayNum].Item = Owner.gameItemDataBase.GetItem(i);
            Entries[_arrayNum].Count = _itemNum;
        }
    }
    public void AddItem(Item item) //아이템 추가
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
            else if (Entries[i].Item == item) //아이템칸의 아이템 정보가 추가한 아이템과 같다면
            {
                Entries[i].Count += 1; //아이템 개수를 증가
                found = true; //아이템 발견 참으로 변경
            }
        }

        if (!found && firstEmpty != -1) //아이템이 발견되지 않았고, 아이템이 없는 칸이 있다면
        {
            InventoryEntry entry = new InventoryEntry(); //아이템 변수 생성하고
            entry.Item = item; //아이템 정보를 기입하고
            entry.Count = 1;  //아이템의 개수를 기입한다.
            Entries[firstEmpty] = entry; //아이템이 없는 첫번째 칸에 아이템을 채워넣는다.
        }
        ReSort();
    }
    public bool ItemChecking(List<InventoryEntry> ItemCheckArray)
    {
        if (Entries == null)
        {
            Debug.Log("ItemChecking PlayerInventory전체 배열에서 null포인터 감지 InventorySystem.csharp");
            return false;
        }
        for (int i = 0; i < ItemCheckArray.Count; i++) //필요 아이템 전체 순회
        {
            for (int j = 0; j < Entries.Length; j++)//인벤토리에있는지 순회
            {
                if (Entries[j] == null)//순회했는데 아이템이 없으면 실패
                    return false;
                else if (ItemCheckArray[i].Item.ItemName == Entries[j].Item.ItemName)
                {               // 필요아이템과 인벤토리 아이템이 같은지 확인
                    if (Entries[j].Count >= ItemCheckArray[i].Count)
                        break;//인벤에 필요량이랑 같거나 많으면 다음 반복 실행
                    else
                        return false;//보다 작으면 실패
                }
            }
        }
        return true;
    }
    public void MinusItem(List<InventoryEntry> ItemCheckArray)
    {
        for (int i = 0; i < ItemCheckArray.Count; i++) //필요 아이템 전체 순회
        {
            for (int j = 0; j < Entries. Length; j++)//i번째 아이템을 찾아 인벤토리 순회
            {
                if (Entries[j] == null)
                { 
                    Debug.Log("MinusItem  PlayerInventory 일부에서 null포인터 감지 InventorySystem.csharp"); 
                    return; 
                }
                else if (ItemCheckArray[i].Item.ItemName == Entries[j].Item.ItemName)
                { // 인벤토리[j]번째 아이템이 필요 아이템과 같다면 
                    for (int k = 0; k < ItemCheckArray[i].Count; k++)
                    {   //[i]번째 아이템의 소모량만큼 반복
                        MinusItem(j);//인벤토리 j번째 아이템 감소
                    }
                    break;
                }
            }
        }
    }

    public bool MinusItem(int InventoryEntry) //아이템을 1개 감소시킨 뒤 개수가 0이하면 데이터를 삭제.
    {
        if (Entries[InventoryEntry] != null)
        {
            Entries[InventoryEntry].Count -= 1;

            if (Entries[InventoryEntry].Count <= 0) //차감한 아이템 개수가 0이하면
            {
                for (int i = 0; i < 32; ++i) //아이템 칸수만큼 반복적으로 수행
                    if (Entries[i] == Entries[InventoryEntry]) //사용한 아이템이 있는 칸이면
                    {
                        Entries[i] = null; //아이템 정보를 소멸시킨다.
                        break;
                    }
            }
        ReSort();
        return true;
        }
        return false;
    }

    public void ReSort()
    {
         InventoryEntry[] Save = new InventoryEntry[32];
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
