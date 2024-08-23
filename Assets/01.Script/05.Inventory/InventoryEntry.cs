
using UnityEngine;

public class InventoryEntry 
{
    public int Count; //������ ����
    public Item Item; //������ ����
}

public class CharacterEntry
{
    public CharacterBase Character; //������ ����
}
public class GradeItemEntry
{
    public int Count; //������ ����
    public GradeItem Item; //������ ����
}
public class SensorEntry
{
    public bool has;
    public SensorItem sensor; //������ ����
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

