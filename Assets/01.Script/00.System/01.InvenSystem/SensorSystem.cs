using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SensorSystem
{
    public const int ItemCount = 32;

    public SensorEntry[] Entries = new SensorEntry[ItemCount]; //아이템 칸  배열 설정
    public SensorEntry[] GetSlots() { return Entries; }
    public UserDataController m_Data;


    public void LoadToInventory(int _arrayNum, string _sensorName, int _sensorLevel, int _sensorType,int _sensorEffectType, GameData Owner)
    {
        for (int i = 0; i < Owner.gameItemDataBase.GetSensorLenth(); i++)
            if (Owner.gameItemDataBase.GetSensor(i).sensorName == _sensorName)
            {
                Entries[_arrayNum] = new SensorEntry();
                Entries[_arrayNum].sensor = Owner.gameItemDataBase.GetSensor(i);
                Entries[_arrayNum].sensor.type = (DataType.SensorLevelType)_sensorType;
                Entries[_arrayNum].has = true;  //아이템의 개수를 기입한다.
                Entries[_arrayNum].sensor.sensorType = (DataType.SensorType)_sensorEffectType;  //아이템의 개수를 기입한다.
                Entries[_arrayNum].sensor.sensorLevel = _sensorLevel;  //아이템의 개수를 기입한다.
            }
    }
    public bool SensorChecking(SensorItem _ref)
    {
        if (Entries == null)
        {
            Debug.Log("SensorChecking SensorInventory전체 배열에서 null포인터 감지 SensorSystem.csharp");
            return false;
        }
        for (int j = 0; j < Entries.Length; j++)//인벤토리에있는지 순회
        {
            if (Entries[j] == null)//순회했는데 아이템이 없으면 실패
                return false;
            else if (_ref.sensorName == Entries[j].sensor.sensorName)
            {               // 필요아이템과 인벤토리 아이템이 같은지 확인
                return true;//있으면 성공
            }
        }
        return false;
    }
    public void AddSensor(SensorItem item) //아이템 추가
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
            else if (Entries[i].sensor == item) //아이템칸의 아이템 정보가 추가한 아이템과 같다면
            {
                //Entries[i].Count += 1; //아이템 개수를 증가
                Debug.Log("현재 소유중인 Sensor입니다. Index :" + i);
                found = true; //아이템 발견 참으로 변경
            }
        }

        if (!found && firstEmpty != -1) //아이템이 발견되지 않았고, 아이템이 없는 칸이 있다면
        {
            SensorEntry entry = new SensorEntry(); //아이템 변수 생성하고
            entry.sensor = item; //아이템 정보를 기입하고
            entry.has = true;  //아이템의 개수를 기입한다.
            Entries[firstEmpty] = entry; //아이템이 없는 첫번째 칸에 아이템을 채워넣는다.
            Debug.Log(" entry.sensor를 지급하였습니다. entry.sensor :" + item);
        }
        ReSort();
    }

    public bool MinusSensor(int sensorEntry) //아이템을 1개 감소시킨 뒤 개수가 0이하면 데이터를 삭제.
    {
        if (Entries[sensorEntry] != null)
        {
            Entries[sensorEntry].has  = false;

            for (int i = 0; i < 32; ++i) //아이템 칸수만큼 반복적으로 수행
                if (Entries[i] == Entries[sensorEntry]) //사용한 아이템이 있는 칸이면
                {
                    Entries[i] = null; //아이템 정보를 소멸시킨다.
                    break;
                }
            ReSort();
            return true;
        }
        return false;
    }
    public void MinusSensor(List<SensorEntry> temp)
    {
      for (int i = 0; i < temp.Count; i++) //필요 아이템 전체 순회
      {
          for (int j = 0; j < Entries.Length; j++)//i번째 아이템을 찾아 인벤토리 순회
          {
              if (Entries[j] == null)
              { Debug.Log("null포인터 감지 SensorSystem.csharp"); return; }
              else if (temp[i].sensor.sensorName == Entries[j].sensor.sensorName)
              { // 인벤토리[j]번째 아이템이 필요 아이템과 같다면 
                 
                  MinusSensor(j);//인벤토리 j번째 아이템 감소
              }
          }
      }
      
    }
    public void ReSort()
    {
        SensorEntry[] Save = new SensorEntry[32];
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

