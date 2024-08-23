
using UnityEngine;

public class InventoryEntry 
{
    public int Count; //아이템 개수
    public Item Item; //아이템 정보
}

public class CharacterEntry
{
    public CharacterBase Character; //아이템 정보
}
public class GradeItemEntry
{
    public int Count; //아이템 개수
    public GradeItem Item; //아이템 정보
}
public class SensorEntry
{
    public bool has;
    public SensorItem sensor; //아이템 정보
}
public class SensorSelectEntry
{
    public bool equipState;
    public int invenNum;
    public SensorItem sensosr;

}

public class StoreCharacter
{
    public string name;
    public Sprite sprite;
    public int price;
}

