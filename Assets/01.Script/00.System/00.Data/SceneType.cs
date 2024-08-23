public class DataType
{
    public enum SceneType
    {
        login,          //0
        lobby,          //1
        Not,            //2
        mode_select,    //3
        ovenBreak,      //4
        characters,     //55
        store,          //6
        mode_stage,     //7

        END
    }
    public enum TileType
    {
        NormalTile,
        UpTile,
        DownTile,
        End
    }

    public enum GraphicType
    {
        GraphicSize,        //0
        GraphicQuility,     //1
        END                 
    }

    public enum SoundType
    {
        BGMSound,       //0
        SFXSound,       //1
        MasterSound,    //2
        END
    }

    public enum SensorType
    { 
        Bonus,
        FightDog,
        Bluffing,
        BlackJack,
        Gambler,
        UnfairPlay,

        END
    }
    public enum SensorActiveType
    { 
        Count,
        Ever,
        END
    }

    public enum ItemType
    { 
        Basic,
        Rare,
        Special,

        END
    }
    public enum PriceType
    {
        Gold,
        Diamond,
        END
    }

    public enum SensorLevelType
    { 
        Spade,
        Clover,
        Heart,
        Diamond,

        END
    }

    public enum JellyType
    {
        Black, //0
        Blue, // 1
        Green, //2
        Red, //3
        White, //4
        Yellow, //5

        Heart, //6
        Spade,//7
        Clover,//8
        Diamond,//9


        SmallTime,//10
        BigTime,//11

        Coin//12
    }

    public enum StopObstacleType
    {
        BlueBottle,
        BrownGrass,
        GreenGrass,
        OliveGrass,
        RedBottle,
        END
    }
    public enum MoveObstaclePropertyType
    {
        Arrow,
        GrassPop,
        Character,
        DownStreet,
        DropTile,
        FireBall,
        Down,

        END
    }
    public enum MoveObstacleType
    {
        Arrow,
        BrownGrass,
        Character,
        DownStreetFireBall,
        DropTile,
        FireBall,
        GreenGrass,
        OliveGrass,
        PinkDown,
        YellowDown,
        END

    }

    public enum CharacterType
    {
        HeartQueen,
        Wolf,
        Allice,
        CapSaller,

        END
    }

    public enum FeverCoinType
    {
        Heart, //6
        Spade,//7
        Clover,//8
        Diamond,//9

        END
    }
    public enum RewardType
    { 
        Gold,
        Diamond,
        END
    }

    public enum QuestType
    {
        Slide,
        Jump,
        Double,
        Fever,
        Skill,
        Sensor,
        Buy,
        LevelUp,
        ItemLevelUp,
        StageScore,
        END
    }

    public enum EffectUIType
    { 
        Fever,
        SKill,
        Sensor,
        END
    }

}



