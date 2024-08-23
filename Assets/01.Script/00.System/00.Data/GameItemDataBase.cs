using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class NumberSprite
{ 
    public Sprite One;
    public Sprite Two;
    public Sprite Three;
    public Sprite Four;
    public Sprite Five;
    public Sprite Six;
    public Sprite Seven;
}


[Serializable]
public class InterectObstacle
{
    public GameObject Arrow;
    public GameObject BrownGrass;
    public GameObject Character;
    public GameObject DownStreetFireBall;
    public GameObject DropTile;
    public GameObject FireBall;
    public GameObject GreenGrass;
    public GameObject OliveGrass;
    public GameObject PinkDown;
    public GameObject YellowDown;
}
[Serializable]
public class NoInterectObstacle
{
    public GameObject BlueBottle;
    public GameObject BrownGrass;
    public GameObject GreenGrass;
    public GameObject OliveGrass;
    public GameObject RedBottle;
}
[Serializable]
public class ScoreItemData
{
    public GameObject Clover;
    public GameObject Spade;
    public GameObject Heart;
    public GameObject Diamond;

    public GameObject Coin;
}

[Serializable]
public class FeverItemData
{
    public GameObject White;
    public GameObject Black;
    public GameObject Yellow;
    public GameObject Blue;
    public GameObject Red;
    public GameObject Green;
}
[Serializable]
public class TileData
{
    public GameObject nomal;
    public GameObject down;
    public GameObject up;
}
[Serializable]
public class AbilityItemData
{
    public GameObject SmallTime;
    public GameObject BigTime;
}


public class GameItemDataBase : MonoBehaviour
{
    public static GameItemDataBase Instance;
    [Header("PriceType")]
    public Sprite GoldePriceImage;
    public Sprite DiamondPriceImage;
    [Header("RewardIconImage")]
    public Sprite GoldRewardIcon;
    public Sprite DiamondRewardIcon;
    [Header("게임 아이템 데이터 베이스")]
    [SerializeField] CharacterBase[] characters;
    [SerializeField] GradeItem[] gradeItems;
    [SerializeField] Item[] items;
    //[SerializeField] Item[] Sensor;
    [SerializeField] SensorItem[] Sensor;
    [Header("캐릭터 포지션 이미지")]
    public Sprite[] Grade;
    public Sprite[] Position;
    public UserExpLevel ExpData;

    public InterectObstacle MoveObstacle;
    public NoInterectObstacle StopObstacle;
    public ScoreItemData Jelly;
    public FeverItemData FeverCoin;
    public AbilityItemData AbilityItem;
    public TileData Tile;
    public NumberSprite Num;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    public int GetItemLenth()
    {
        return items.Length;
    }
    public Item GetItem(int _index)
    {
        if(_index < items.Length)
            return items[_index];
        else
            Debug.Log("Item이 없습니다. _index Num : " + _index);
        return null;
    }
    public int GetSensorLenth()
    {
        return Sensor.Length;
    }
    public SensorItem GetSensor(int __index)
    {
        if (__index < Sensor.Length && __index >= 0)
            return Sensor[__index];
        else
            Debug.Log("Sensor이 없습니다. _index Num : " + __index);
        return null;
    }
    //public Item GetSensor(int _index)
    //{
    //    if (_index < items.Length)
    //        return Sensor[_index];
    //    else
    //        Debug.Log("Item이 없습니다. _index Num : " + _index);
    //    return null;
    //}
    public int GetCharacterLenth()
    {
        return characters.Length;
    }

    public CharacterBase GetCharacter(int _index)
    {
        if (_index < characters.Length)
            return characters[_index];
        else
            Debug.Log("Character가 없습니다. _index Num : " + _index);
        return null;
    }
    public int GetGradetItemLenth()
    {
        return gradeItems.Length;
    }
    public GradeItem GetGradeItem(int _index)
    {
        if (_index < gradeItems.Length)
            return gradeItems[_index];
        else
            Debug.Log("GradeItem이 없습니다. _index Num : " + _index);
        return null;
    }
    public GameObject GetTile(DataType.TileType _enum)
    {
        GameObject gameObject = null;
        switch (_enum)
        {
            case DataType.TileType.NormalTile:
                gameObject = Tile.nomal;
                break;
            case DataType.TileType.UpTile:
                gameObject = Tile.up;
                break;
            case DataType.TileType.DownTile:
                gameObject = Tile.down;
                break;
            case DataType.TileType.End:
                break;
            default:
                break;
        }
        return gameObject;
    }

    public GameObject GetFieldItem(DataType.JellyType _enum)
    {
        GameObject gameObject = null;
        switch (_enum)
        {
            case DataType.JellyType.Black:
                gameObject = FeverCoin.Black;
                break;
            case DataType.JellyType.Blue:
                gameObject = FeverCoin.Blue;
                break;
            case DataType.JellyType.Green:
                gameObject = FeverCoin.Green;
                break;
            case DataType.JellyType.Red:
                gameObject = FeverCoin.Red;
                break;
            case DataType.JellyType.White:
                gameObject = FeverCoin.White;
                break;
            case DataType.JellyType.Yellow:
                gameObject = FeverCoin.Yellow;
                break;
            case DataType.JellyType.Spade:
                gameObject = Jelly.Spade;
                break;
            case DataType.JellyType.Clover:
                gameObject = Jelly.Clover;
                break;
            case DataType.JellyType.Heart:
                gameObject = Jelly.Heart;
                break;
            case DataType.JellyType.Diamond:
                gameObject = Jelly.Diamond;
                break;
            case DataType.JellyType.SmallTime:
                gameObject = AbilityItem.SmallTime;
                break;
            case DataType.JellyType.BigTime:
                gameObject = AbilityItem.BigTime;
                break;
            case DataType.JellyType.Coin:
                gameObject = Jelly.Coin;
                break;
            default:
                break;
        }
        return gameObject;
    }
    public GameObject GetMoveObstacle(DataType.MoveObstacleType _enum)
    {
        GameObject stopObstacle = null;
        switch (_enum)
        {
            case DataType.MoveObstacleType.Arrow:
                stopObstacle = MoveObstacle.Arrow;
                break;
            case DataType.MoveObstacleType.BrownGrass:
                stopObstacle = MoveObstacle.BrownGrass;
                break;
            case DataType.MoveObstacleType.Character:
                stopObstacle = MoveObstacle.Character;
                break;
            case DataType.MoveObstacleType.DownStreetFireBall:
                stopObstacle = MoveObstacle.DownStreetFireBall;
                break;
            case DataType.MoveObstacleType.DropTile:
                stopObstacle = MoveObstacle.DropTile;
                break;
            case DataType.MoveObstacleType.FireBall:
                stopObstacle = MoveObstacle.FireBall;
                break;
            case DataType.MoveObstacleType.GreenGrass:
                stopObstacle = MoveObstacle.GreenGrass;
                break;
            case DataType.MoveObstacleType.OliveGrass:
                stopObstacle = MoveObstacle.OliveGrass;
                break;
            case DataType.MoveObstacleType.PinkDown:
                stopObstacle = MoveObstacle.PinkDown;
                break;
            case DataType.MoveObstacleType.YellowDown:
                stopObstacle = MoveObstacle.YellowDown;
                break;
            case DataType.MoveObstacleType.END:
                break;
            default:
                break;
        }
        return stopObstacle;
    }
    public GameObject GetStopObstacle(DataType.StopObstacleType _enum)
    {
        GameObject stopObstacle = null;
        switch (_enum)
        {
            case DataType.StopObstacleType.BlueBottle:
                stopObstacle = StopObstacle.BlueBottle;
                break;
            case DataType.StopObstacleType.BrownGrass:
                stopObstacle = StopObstacle.BrownGrass;
                break;
            case DataType.StopObstacleType.GreenGrass:
                stopObstacle = StopObstacle.GreenGrass;
                break;
            case DataType.StopObstacleType.OliveGrass:
                stopObstacle = StopObstacle.OliveGrass;
                break;
            case DataType.StopObstacleType.RedBottle:
                stopObstacle = StopObstacle.RedBottle;
                break;
            case DataType.StopObstacleType.END:
                break;
            default:
                break;
        }
        return stopObstacle;
    }
}
[Serializable]
public class UserExpLevel
{
    public int Level1;
    public int Level2;
    public int Level3;
    public int Level4;
    public int Level5;
    public int Level6;
    public int Level7;
    public int Level8;
}
