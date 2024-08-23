using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

#region Data 게임 플레이 데이터
#endregion Data
#region Data 게임 세이브 파일 안의 클래스
[Serializable]
public class StageData
{
    public bool m_1ChapterIsClear;
    public List<int> m_1stageArrayNum = new List<int>();
    public List<bool> m_1stageIsClear = new List<bool>();
    public List<bool> m_1stageIsFirst = new List<bool>();
    public List<int> m_1stageClearLevel = new List<int>();
    public List<int> m_1stageHighScore = new List<int>();

    public bool m_2ChapterIsClear;

    public List<int> m_2stageArrayNum = new List<int>();
    public List<bool> m_2stageIsClear = new List<bool>();
    public List<bool> m_2stageIsFirst = new List<bool>();
    public List<int> m_2stageClearLevel = new List<int>();
    public List<int> m_2stageHighScore = new List<int>();
}

[Serializable]
public class ItemArrayData //게임 세이브 파일 안의 플레이어 데이터
{
    //아이템 인벤토리
    public List<int> m_playerInvenItemNumber = new List<int>();
    public List<int> m_playerInvenItemCount = new List<int>();
    public List<string> m_playerInvenItemName = new List<string>();

    //기타 데이터
    public int m_money;
    public int m_stamina;
    public int m_cash;
    public int m_currentEXP;
    public int m_maxEXP;
    public int m_level;
    public string m_name;

    public int m_UserCharacter;
    public int m_LobbyCharacter;
    public bool m_isGive;
}
[Serializable]
public class SensorArrayData //게임 세이브 파일 안의 플레이어 데이터
{
    //아이템 인벤토리
    public List<int> m_playerInvenSensorNumber = new List<int>();
    public List<int> m_playerInvenSensorLevel = new List<int>();
    public List<int> m_PlayerInvenSensorType = new List<int>();
    public List<int> m_PlayerInvenSensorEffectType = new List<int>();
    public List<string> m_playerInvenSensorName = new List<string>();
}
[Serializable]

public class GradeItemArrayData
{
    //승급재화 인벤토리
    public List<int> m_characterGradeValue = new List<int>();
    public List<int> m_characterGradeNumber = new List<int>();
    public List<int> m_characterGradeCount = new List<int>();
    public List<string> m_characterGradeName = new List<string>();
}
[Serializable]

public class CharacteArrayData
{
    //캐릭터 인벤토리
    public List<int> m_characterNumber = new List<int>();
    public List<string> m_characterName = new List<string>();
    public List<int> m_characterPosition = new List<int>();
    public List<int> m_characterGrade = new List<int>();
    public List<int> m_characterLevel = new List<int>();
    public List<int> m_characterStrength = new List<int>();
    public List<float> m_characterHpMinusPercent = new List<float>();
    public List<float> m_characterItemPercent = new List<float>();
    public List<float> m_characterHealth = new List<float>();
    public List<int> m_characterCurrentEXP = new List<int>();
    public List<int> m_characterMaxEXP = new List<int>();

    public List<int> m_QuestValue = new List<int>();
    public List<int> m_QuestType = new List<int>();
    public List<bool> m_QuestClear = new List<bool>();
    public bool m_Reward;
    public int m_QuestIndex;
}
[Serializable]
public class PlayerCountData
{
    public int m_SlideCount;
    public int m_JumpCount;
    public int m_DoubleCount;
    public int m_FeverCount;
    public int m_SkillCount;
    public int m_Sensor;
    public int m_LevelUpCount;
    public int m_ItemLevelUpCount;
    public int m_BuyCount;
}

[Serializable]

public class SaveData
{
    public ItemArrayData m_itemData = new ItemArrayData();
    public GradeItemArrayData m_gradeItemData = new GradeItemArrayData();
    public CharacteArrayData m_characterData = new CharacteArrayData();
    public StageData m_stageData = new StageData();
    public SensorArrayData m_sensorData = new SensorArrayData();
    public PlayerCountData m_count = new PlayerCountData();
}
#endregion Data

public class GameData : MonoBehaviour
{
    public GameItemDataBase gameItemDataBase { get; set; } //아이템 데이터 베이스
    static GameData instance;
    public UserDataController UserData { get; set; } //플레이어 데이터 
    [SerializeField] TextMeshProUGUI txt;

    #region SaveLoadData 게임 세이브 파일 관련 자료형
    private string SAVE_DATA_DIRECTORY;                 //저장 경로
    private string SAVE_DATA_FILENAME = "/SaveFile.txt";//저장 파일 이름
    [HideInInspector]
    public SaveData savingData = new SaveData(); //저장하고 불러올때 담아 놓을 데이터
    #endregion SaveLoadData 

    bool b_itemData = false;
    bool b_gradeItemData = false;
    bool b_characterData = false;
    bool b_stageData = false;
    bool b_sensorData = false;
    bool b_count = false;
    bool total = false;
    bool isRemove;
    private void Awake()
    {
        //DontDestroyOnLoad(this);
        SingleTon();
        gameItemDataBase = GetComponent<GameItemDataBase>();
        //SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/"; //현재 경로 + /Saves/ 
        //if (!Directory.Exists(SAVE_DATA_DIRECTORY))              //경로가 존재하지 않는다면
        //{
        //    Directory.CreateDirectory(SAVE_DATA_DIRECTORY);     //경로 생성
        //    if(txt != null)
        //        txt.gameObject.SetActive(true);
        //}
    }

    void Start()
    {
        GameLoad();
        isRemove = false;
    }

    private void Update()
    {
        //if(!total)
        //{
        //    if (txt != null)
        //        txt.text = "False";

        //    total = true;
        //    if (!b_itemData)
        //    {
        //        GetPrefUserData();
        //    }
        //    //if(!b_gradeItemData)
        //    //{
        //    //    GetGradeItem();
        //    //}
        //    if (!b_characterData)
        //    {
        //        GetPrefCharacter();
        //    }
        //    if(!b_stageData)
        //    {
        //        GetPrefStage();
        //    }
        //    if(!b_sensorData)
        //    {
        //        GetPrefSensor();
        //    }
        //    if(!b_count)
        //    {
        //        GetPrefCountData();
        //    }
        //    return;
        //}
        if (txt != null)
            txt.text = "Finish";
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    ResetData();
        //    Debug.Log("Data Reset Complete");
        //}
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        UserData.isGiveMoney = false;
        UserData.Money = 0;
        UserData.Cash = 0;
        isRemove = true;
        PlayerPrefs.SetInt("money", 1000);
        PlayerPrefs.SetInt("cash", 1000);
    }

    #region SetData 세이브 관련 함수
    public void SetCountData()
    {
        savingData.m_count.m_SlideCount = UserData.countData.SlideCount;
        savingData.m_count.m_JumpCount = UserData.countData.JumpCount;
        savingData.m_count.m_DoubleCount = UserData.countData.DoubleCount;
        savingData.m_count.m_FeverCount = UserData.countData.FeverCount;
        savingData.m_count.m_SkillCount = UserData.countData.SkillCount;
        savingData.m_count.m_Sensor = UserData.countData.SensorCount;
        savingData.m_count.m_LevelUpCount = UserData.countData.LevelUpCount;
        savingData.m_count.m_ItemLevelUpCount = UserData.countData.ItemLevelUpCount;
        savingData.m_count.m_BuyCount = UserData.countData.BuyCount;
    }
    public void SetPrefsCountData()
    {
        PlayerPrefs.SetInt("m_SlideCount", UserData.countData.SlideCount);
        PlayerPrefs.SetInt("m_JumpCount", UserData.countData.JumpCount);
        PlayerPrefs.SetInt("m_DoubleCount", UserData.countData.DoubleCount);
        PlayerPrefs.SetInt("m_FeverCount", UserData.countData.FeverCount);
        PlayerPrefs.SetInt("m_SkillCount", UserData.countData.SkillCount);
        PlayerPrefs.SetInt("m_Sensor", UserData.countData.SensorCount);
        PlayerPrefs.SetInt("m_LevelUpCount", UserData.countData.LevelUpCount);
        PlayerPrefs.SetInt("m_ItemLevelUpCount", UserData.countData.ItemLevelUpCount);
        PlayerPrefs.SetInt("m_BuyCount", UserData.countData.BuyCount);

        PlayerPrefs.Save();
    }
    public void SetGradeItemInventory()
    {
        SetGradeItemMethod(UserData.gradeItemSystem.Entries, savingData.m_gradeItemData.m_characterGradeNumber, savingData.m_gradeItemData.m_characterGradeCount, savingData.m_gradeItemData.m_characterGradeName);
    }
    void SetGradeItemMethod(GradeItemEntry[] items, List<int> ArrayNum, List<int> ItemNum, List<string> ItemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                ArrayNum.Add(i);
                ItemName.Add(items[i].Item.name);
                ItemNum.Add(items[i].Count);
            }
        }
    }
    public void SetUserData()
    {
        savingData.m_itemData.m_cash = UserData.Cash;
        savingData.m_itemData.m_money = UserData.Money;
        savingData.m_itemData.m_stamina = UserData.Stamina;
        savingData.m_itemData.m_maxEXP = UserData.MaxEXP;
        savingData.m_itemData.m_currentEXP = UserData.CurrentEXP;
        savingData.m_itemData.m_level = UserData.UserLevel;
        savingData.m_itemData.m_name = UserData.UserName;
        savingData.m_itemData.m_UserCharacter = UserData.UserCharacterIcon;
        savingData.m_itemData.m_LobbyCharacter = (int)UserData.characterType;
        savingData.m_itemData.m_isGive = UserData.isGiveMoney;
    }
    public void SetPrefsUserData()
    {
        PlayerPrefs.SetInt("cash", UserData.Cash);
        PlayerPrefs.SetInt("money", UserData.Money);
        PlayerPrefs.SetInt("stamina", UserData.Stamina);
        PlayerPrefs.SetInt("maxEXP", UserData.MaxEXP);
        PlayerPrefs.SetInt("currentEXP", UserData.CurrentEXP);
        PlayerPrefs.SetInt("level", UserData.UserLevel);
        PlayerPrefs.SetString("name", UserData.UserName);
        PlayerPrefs.SetInt("UserCharacter", UserData.UserCharacterIcon);
        PlayerPrefs.SetInt("LobbyCharacter", (int)UserData.characterType);
        int GiveMoney = 0;
        if (UserData.isGiveMoney)
            GiveMoney = 10;
        PlayerPrefs.SetInt("First", GiveMoney);
        PlayerPrefs.Save();
    }
    public void SetSensorData()
    {
        for (int i = 0; i < UserData.sensorSystem.Entries.Length; i++)
        {
            if (UserData.sensorSystem.Entries[i] != null)
            {
                savingData.m_sensorData.m_playerInvenSensorNumber.Add(i);
                savingData.m_sensorData.m_playerInvenSensorLevel.Add(UserData.sensorSystem.Entries[i].sensor.sensorLevel);
                savingData.m_sensorData.m_playerInvenSensorName.Add(UserData.sensorSystem.Entries[i].sensor.sensorName);
                savingData.m_sensorData.m_PlayerInvenSensorType.Add((int)UserData.sensorSystem.Entries[i].sensor.type);
                savingData.m_sensorData.m_PlayerInvenSensorEffectType.Add((int)UserData.sensorSystem.Entries[i].sensor.sensorType);
            }
        }
    }
    public void SetPrefsSensor()
    {
        if(UserData != null && UserData.sensorSystem != null && UserData.sensorSystem.Entries!= null)
        {
            string SensorNumber = "SensorNumber";
            string SensorLevel = "SensorLevel";
            string SensorName = "SensorName";
            string SensorType = "SensorType";
            string SensorEffectType = "SensorEffectType";

            for (int i = 0; i < UserData.sensorSystem.Entries.Length; i++)
            {
                if (UserData.sensorSystem.Entries[i] != null)
                {
                    PlayerPrefs.SetInt(SensorNumber + i.ToString(), i);
                    PlayerPrefs.SetInt(SensorLevel + i.ToString(), UserData.sensorSystem.Entries[i].sensor.sensorLevel);
                    PlayerPrefs.SetString(SensorName + i.ToString(), UserData.sensorSystem.Entries[i].sensor.sensorName);
                    PlayerPrefs.SetInt(SensorType + i.ToString(), (int)UserData.sensorSystem.Entries[i].sensor.type);
                    PlayerPrefs.SetInt(SensorEffectType + i.ToString(), (int)UserData.sensorSystem.Entries[i].sensor.sensorType);
                }
            }
            PlayerPrefs.Save();
        }
    }
    public void SetStageData()
    {
        int m_1Chapter = 1;
        int m_2Chapter = 2;

        for (int i = 0; i < UserData.GetStageData().GetStageLenth(m_1Chapter); i++)
        {
            savingData.m_stageData.m_1stageArrayNum.Add(i);
            savingData.m_stageData.m_1stageIsClear.Add(UserData.GetStageData().GetStageData(i, m_1Chapter).isClear);
            savingData.m_stageData.m_1stageClearLevel.Add(UserData.GetStageData().GetStageData(i, m_1Chapter).clearLevel);
            savingData.m_stageData.m_1stageHighScore.Add(UserData.GetStageData().GetStageData(i, m_1Chapter).highScore);
            savingData.m_stageData.m_1stageIsFirst.Add(UserData.GetStageData().GetStageData(i, m_1Chapter).isFirst);
        }
        for (int i = 0; i < UserData.GetStageData().GetStageLenth(m_2Chapter); i++)
        {
            savingData.m_stageData.m_2stageArrayNum.Add(i);
            savingData.m_stageData.m_2stageIsClear.Add(UserData.GetStageData().GetStageData(i, m_2Chapter).isClear);
            savingData.m_stageData.m_2stageClearLevel.Add(UserData.GetStageData().GetStageData(i, m_2Chapter).clearLevel);
            savingData.m_stageData.m_2stageHighScore.Add(UserData.GetStageData().GetStageData(i, m_2Chapter).highScore);
            savingData.m_stageData.m_2stageIsFirst.Add(UserData.GetStageData().GetStageData(i, m_2Chapter).isFirst);
        }
        //UserData.GetStageData().LoadToStageData(savingData.m_stageData.m_arrayNum[i], savingData.m_stageData.m_isClear[i], savingData.m_stageData.m_clearLevel[i], savingData.m_stageData.m_highScore[i]);
    }
    public void SetPrefsStage()
    {//1ChapterArrayNum
        int m_1Chapter = 1;
        int m_2Chapter = 2;
        int isTrue = 10;
        int isFalse = 0;
        string idx = null;
        int HighScore = 0;

        for (int i = 0; i < UserData.GetStageData().GetStageLenth(m_1Chapter); i++)
        {
            idx = i.ToString();
            PlayerPrefs.SetInt("1ChapterArrayNum" + idx, i);
            if (UserData.GetStageData().GetStageData(i, m_1Chapter).isClear)
                PlayerPrefs.SetInt("1ChapterIsClear" + idx, isTrue);
            else
                PlayerPrefs.SetInt("1ChapterIsClear" + idx, isFalse);
            HighScore = UserData.GetStageData().GetStageData(i, m_1Chapter).highScore;
            PlayerPrefs.SetInt("1ChapterClearLevel"  + idx, UserData.GetStageData().GetStageData(i, m_1Chapter).clearLevel);
            PlayerPrefs.SetInt("1ChapterHighScore" + idx, HighScore);
            if (UserData.GetStageData().GetStageData(i, m_1Chapter).isFirst)
                PlayerPrefs.SetInt("1ChapterArrayNum" + idx, isTrue);
            else
                PlayerPrefs.SetInt("1ChapterArrayNum" + idx, isFalse);
            Debug.Log(idx + "Stage Save HighScore :" + HighScore);
            Debug.Log(idx + "Stage Save HighScore Check : " + PlayerPrefs.GetInt("1ChapterHighScore" + idx));

        }
        PlayerPrefs.Save();

        for (int i = 0; i < UserData.GetStageData().GetStageLenth(m_2Chapter); i++)
        {
            idx = i.ToString();
            PlayerPrefs.SetInt("2ChapterArrayNum" + idx, i);
            if (UserData.GetStageData().GetStageData(i, m_2Chapter).isClear)
                PlayerPrefs.SetInt("2ChapterIsClear" + idx, isTrue);
            else
                PlayerPrefs.SetInt("2ChapterIsClear" + idx, isFalse);

            PlayerPrefs.SetInt("2ChapterClearLevel" + idx, UserData.GetStageData().GetStageData(i, m_2Chapter).clearLevel);
            PlayerPrefs.SetInt("2ChapterHighScore" + idx, UserData.GetStageData().GetStageData(i, m_2Chapter).highScore);
            if (UserData.GetStageData().GetStageData(i, m_2Chapter).isFirst)
                PlayerPrefs.SetInt("2ChapterArrayNum" + idx, isTrue);
            else
                PlayerPrefs.SetInt("2ChapterArrayNum" + idx, isFalse);
        }
        PlayerPrefs.Save();

    }
    public void SetInventory()
    {
        SetInventoryMethod(UserData.invenSystem.Entries, savingData.m_itemData.m_playerInvenItemNumber, savingData.m_itemData.m_playerInvenItemCount, savingData.m_itemData.m_playerInvenItemName);
    }
    public void SetCharacterInventory()
    {
        SetCharacterMethod(UserData.characterSystem.Entries,
            savingData.m_characterData.m_characterNumber,
            savingData.m_characterData.m_characterPosition,
            savingData.m_characterData.m_characterName,
            savingData.m_characterData.m_characterGrade,
            savingData.m_characterData.m_characterLevel,
            savingData.m_characterData.m_characterItemPercent,
            savingData.m_characterData.m_characterHpMinusPercent,
            savingData.m_characterData.m_characterCurrentEXP,
            savingData.m_characterData.m_characterMaxEXP
            );
    }
    public void SetPrefsCharacter()
    {
        SetCharacterMethod(UserData.characterSystem.Entries,
           savingData.m_characterData.m_characterNumber,
           savingData.m_characterData.m_characterPosition,
           savingData.m_characterData.m_characterName,
           savingData.m_characterData.m_characterGrade,
           savingData.m_characterData.m_characterLevel,
           savingData.m_characterData.m_characterItemPercent,
           savingData.m_characterData.m_characterHpMinusPercent,
           savingData.m_characterData.m_characterCurrentEXP,
           savingData.m_characterData.m_characterMaxEXP
           );
        for (int i = 0; i < UserData.characterSystem.Entries.Length; i++)
        {
            if(UserData.characterSystem.Entries[i] != null)
            {
                PlayerPrefs.SetInt("characterNumber" + i.ToString(), i);
                PlayerPrefs.SetInt("characterPosition" + i.ToString(), savingData.m_characterData.m_characterPosition[i]);
                PlayerPrefs.SetString("characterName" + i.ToString(), savingData.m_characterData.m_characterName[i]);
                PlayerPrefs.SetInt("characterGrade" + i.ToString(), savingData.m_characterData.m_characterGrade[i]);
                PlayerPrefs.SetInt("characterLevel" + i.ToString(), savingData.m_characterData.m_characterLevel[i]);
                PlayerPrefs.SetFloat("characterItemPercent" + i.ToString(), savingData.m_characterData.m_characterItemPercent[i]);
                PlayerPrefs.SetFloat("characterHpMinusPercent" + i.ToString(), savingData.m_characterData.m_characterHpMinusPercent[i]);
                PlayerPrefs.SetInt("characterCurrentEXP" + i.ToString(), savingData.m_characterData.m_characterMaxEXP[i]);
                PlayerPrefs.SetInt("characterMaxEXP" + i.ToString(), savingData.m_characterData.m_characterMaxEXP[i]);
            }
        }
        PlayerPrefs.Save();
    }
    public void SetInventoryMethod(InventoryEntry[] items, List<int> ArrayNum, List<int> ItemNum, List<string> ItemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                ArrayNum.Add(i);
                ItemName.Add(items[i].Item.name);
                ItemNum.Add(items[i].Count);
            }
        }
    }
    public void SetCharacterMethod(CharacterEntry[] characters,List<int> num,List<int> position, List<string> name, List<int> grade, List<int> level,List<float> _itemPercent, List<float> _hpMinusPercent, List<int> currentExp, List<int> maxExp)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] !=null)
            {
                num.Add(i);
                position.Add(i);
                grade.Add(i);
                name.Add(characters[i].Character.CharacterName);
                level.Add(characters[i].Character.currentLevel);
                _itemPercent.Add(characters[i].Character.itemPercent);
                _hpMinusPercent.Add(characters[i].Character.hpMinusPercent);
                currentExp.Add(characters[i].Character.CurrentEXP);
                maxExp.Add(characters[i].Character.MaxEXP);
            }
        }
    }

    public void SetQuestData()
    {
        savingData.m_characterData.m_QuestIndex = UserData.questData.GetQuestIndex();

        for (int i = 0; i < UserData.questData.GetCurrenQuest().questEntries.Length; i++)
        {
            savingData.m_characterData.m_QuestClear.Add(UserData.questData.GetCurrenQuest().questEntries[i].isClear);
            savingData.m_characterData.m_QuestValue.Add(UserData.questData.GetCurrenQuest().questEntries[i].iCurrentCount);
            savingData.m_characterData.m_QuestType.Add((int)UserData.questData.GetCurrenQuest().questEntries[i].type);
        }
        savingData.m_characterData.m_Reward = UserData.questData.GetCurrenQuest().isReward;
    }


    #endregion SetData

    #region GetData 로드 관련 함수
    public void GetQuestData()
    {
        //UserData.SetQuestData(savingData.m_characterData.m_QuestType, savingData.m_characterData.m_QuestValue, savingData.m_characterData.m_QuestClear, savingData.m_characterData.m_QuestIndex, savingData.m_characterData.m_Reward);
    }

    public void GetCountData()
    {
        int SlideCount = savingData.m_count.m_SlideCount;
        int JumpCount = savingData.m_count.m_JumpCount;
        int DoubleCount = savingData.m_count.m_DoubleCount;
        int FeverCount = savingData.m_count.m_FeverCount;
        int SkillCount = savingData.m_count.m_SkillCount;
        int SensorCount = savingData.m_count.m_Sensor;
        int LevelUpCount = savingData.m_count.m_LevelUpCount;
        int ItemLevelUpCount = savingData.m_count.m_ItemLevelUpCount;
        int BuyCount = savingData.m_count.m_BuyCount;

        UserData.SetCountData(SlideCount, JumpCount, DoubleCount, FeverCount, SkillCount, SensorCount, LevelUpCount, ItemLevelUpCount, BuyCount);
    }

    public void GetPrefCountData()
    {
        int SlideCount = 0;
        int JumpCount = 0;
        int DoubleCount = 0;
        int FeverCount = 0;
        int SkillCount = 0;
        int SensorCount = 0;
        int LevelUpCount = 0;
        int ItemLevelUpCount = 0;
        int BuyCount = 0;
        if (PlayerPrefs.HasKey("SlideCount"))
            SlideCount = PlayerPrefs.GetInt("SlideCount");
        if (PlayerPrefs.HasKey("JumpCount"))
            SlideCount = PlayerPrefs.GetInt("JumpCount");
        if (PlayerPrefs.HasKey("DoubleCount"))
            SlideCount = PlayerPrefs.GetInt("DoubleCount");
        if (PlayerPrefs.HasKey("FeverCount"))
            SlideCount = PlayerPrefs.GetInt("FeverCount");
        if (PlayerPrefs.HasKey("SkillCount"))
            SlideCount = PlayerPrefs.GetInt("SkillCount");
        if (PlayerPrefs.HasKey("SensorCount"))
            SlideCount = PlayerPrefs.GetInt("SensorCount");
        if (PlayerPrefs.HasKey("LevelUpCount"))
            SlideCount = PlayerPrefs.GetInt("LevelUpCount");
        if (PlayerPrefs.HasKey("ItemLevelUpCount"))
            SlideCount = PlayerPrefs.GetInt("ItemLevelUpCount");
        if (PlayerPrefs.HasKey("BuyCount"))
            SlideCount = PlayerPrefs.GetInt("BuyCount");
        UserData.SetCountData(SlideCount, JumpCount, DoubleCount, FeverCount, SkillCount, SensorCount, LevelUpCount, ItemLevelUpCount, BuyCount);

    }
    public void GetSensorData()
    {
        if (UserData == null && UserData.sensorSystem == null)
        {
            b_sensorData = false;
            total = false;
            return;
        }
        for (int i = 0; i < savingData.m_sensorData.m_playerInvenSensorNumber.Count; i++)
        {
            UserData.sensorSystem.LoadToInventory(savingData.m_sensorData.m_playerInvenSensorNumber[i], savingData.m_sensorData.m_playerInvenSensorName[i], savingData.m_sensorData.m_playerInvenSensorLevel[i], savingData.m_sensorData.m_PlayerInvenSensorType[i], savingData.m_sensorData.m_PlayerInvenSensorEffectType[i], this);
        }
    }
    public void GetPrefSensor()
    {
        int Length = 0;
        for (int i = 0; i < 10; i++)
        {
            if(PlayerPrefs.HasKey("SensorNumber"+i.ToString()))
            {
                Length++;
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < Length; i++)
        {
            if (PlayerPrefs.HasKey("SensorNumber" + i.ToString()))
            {
                UserData.sensorSystem.LoadToInventory(PlayerPrefs.GetInt("SensorNumber" + i.ToString()),
                PlayerPrefs.GetString("SensorName" + i.ToString()),
                PlayerPrefs.GetInt("SensorLevel" + i.ToString()),
                PlayerPrefs.GetInt("SensorType" + i.ToString()),
                PlayerPrefs.GetInt("SensorEffectType" + i.ToString()),
                this);
            }
        }
    }
    public void GetUserData()
    {
        bool isGive = false;
        if (UserData == null)
        {
            b_itemData = false;
            total = false;
            return;
        }
        UserData.UserName = savingData.m_itemData.m_name;

        if(PlayerPrefs.HasKey("First"))
        {
            if (PlayerPrefs.GetInt("First") == 0)
                isGive = false;
            else
                isGive = true;
        }
        else
        {
            PlayerPrefs.SetInt("First", 0);
            isGive = false;
        }

        UserData.isGiveMoney = isGive;//savingData.m_itemData.m_isGive;

        UserData.characterType = (DataType.CharacterType)savingData.m_itemData.m_LobbyCharacter;
        UserData.UserCharacterIcon = savingData.m_itemData.m_UserCharacter;
        UserData.SetUserData(savingData.m_itemData.m_money, savingData.m_itemData.m_cash, savingData.m_itemData.m_stamina, savingData.m_itemData.m_level, savingData.m_itemData.m_currentEXP, savingData.m_itemData.m_maxEXP, savingData.m_itemData.m_UserCharacter, savingData.m_itemData.m_LobbyCharacter);
    }
    public void GetPrefUserData()
    {
        bool isGive = false;
        if (PlayerPrefs.HasKey("First"))
        {
            if (PlayerPrefs.GetInt("First") == 0)
                isGive = false;
            else
                isGive = true;
        }
        UserData.isGiveMoney = isGive;//savingData.m_itemData.m_isGive;

        if (PlayerPrefs.HasKey("LobbyCharacter"))
            UserData.characterType = (DataType.CharacterType)PlayerPrefs.GetInt("m_LobbyCharacter");//savingData.m_itemData.m_LobbyCharacter;
        if (PlayerPrefs.HasKey("UserCharacter"))
            UserData.UserCharacterIcon = PlayerPrefs.GetInt("UserCharacter");//savingData.m_itemData.m_UserCharacter;
        int m_money = 0;
        int m_cash = 0;
        int m_stamina = 0;
        int m_level = 0;
        int m_currentEXP = 0;
        int m_maxEXP = 0;
        int m_UserCharacter = 0;
        int m_LobbyCharacter = 0;
        if (PlayerPrefs.HasKey("money"))
            m_money = PlayerPrefs.GetInt("money");
        if (PlayerPrefs.HasKey("cash"))
            m_cash = PlayerPrefs.GetInt("cash");
        if (PlayerPrefs.HasKey("stamina"))
            m_stamina = PlayerPrefs.GetInt("stamina");
        if (PlayerPrefs.HasKey("level"))
            m_level = PlayerPrefs.GetInt("level");
        if (PlayerPrefs.HasKey("currentEXP"))
            m_currentEXP = PlayerPrefs.GetInt("currentEXP");
        if (PlayerPrefs.HasKey("maxEXP"))
            m_maxEXP = PlayerPrefs.GetInt("maxEXP");
        if (PlayerPrefs.HasKey("UserCharacter"))
            m_UserCharacter = PlayerPrefs.GetInt("UserCharacter");
        if (PlayerPrefs.HasKey("LobbyCharacter"))
            m_LobbyCharacter = PlayerPrefs.GetInt("LobbyCharacter");
        UserData.SetUserData(
            m_money,
            m_cash,
            m_stamina,
            m_level,
            m_currentEXP,
            m_maxEXP,
            m_UserCharacter,
            m_LobbyCharacter
            );
    }

    public void GetStageData()
    {
        int m_1Chapter = 1;
        int m_2Chapter = 2;

        if (UserData == null && UserData.GetStageData() == null)
        {
            b_stageData = false;
            total = false;
            return;
        }

        for (int i = 0; i < UserData.GetStageData().GetStageLenth(m_1Chapter); i++)
        {
            if(UserData.GetStageData().GetStageData(i , m_1Chapter) != null)
                UserData.GetStageData().LoadToStageData(savingData.m_stageData.m_1stageArrayNum[i], savingData.m_stageData.m_1stageIsClear[i], savingData.m_stageData.m_1stageClearLevel[i], savingData.m_stageData.m_1stageHighScore[i], m_1Chapter, savingData.m_stageData.m_1stageIsFirst[i]);
        }
        for (int i = 0; i < UserData.GetStageData().GetStageLenth(m_2Chapter); i++)
        {
            if (UserData.GetStageData().GetStageData(i, m_2Chapter) != null)
                UserData.GetStageData().LoadToStageData(savingData.m_stageData.m_2stageArrayNum[i], savingData.m_stageData.m_2stageIsClear[i], savingData.m_stageData.m_2stageClearLevel[i], savingData.m_stageData.m_2stageHighScore[i], m_2Chapter, savingData.m_stageData.m_2stageIsFirst[i]);
        }
        //for (int i = 0; i < UserData.GetStageData().GetStageLenth(); i++)
        //{
        //    if (UserData.GetStageData().GetStageData(i) == null)
        //        continue;
        //    savingData.m_stageData.m_arrayNum.Add(i);
        //    savingData.m_stageData.m_clearLevel.Add(UserData.GetStageData().GetStageData(i).clearLevel);
        //    savingData.m_stageData.m_highScore.Add(UserData.GetStageData().GetStageData(i).highScore);
        //    savingData.m_stageData.m_isClear.Add(UserData.GetStageData().GetStageData(i).isClear);
        //}
        //UserData.GetStageData().SaveToStageData(savingData.m_stageData.m_arrayNum, savingData.m_stageData.m_isClear, savingData.m_stageData.m_clearLevel, savingData.m_stageData.m_highScore);
    }
    public void GetPrefStage()
    {
        int m_1Chapter = 1;
        int m_2Chapter = 2;

        int ArrayNum= 0; 
        int ClearLevel= 0;
        int HighScore= 0;
        string idx = null;

        if (UserData == null && UserData.GetStageData() == null)
        {
            b_stageData = false;
            total = false;
            return;
        }

        if(UserData.GetStageData() != null)
        {
            for (int i = 0; i < UserData.GetStageData().GetStageLenth(m_1Chapter); i++)
            {
                if (UserData.GetStageData().GetStageData(i, m_1Chapter) == null)
                    continue;
                idx = i.ToString();
                bool isClear = false;
                bool isFirst = false;

                if (PlayerPrefs.HasKey("1ChapterArrayNum" + idx))
                    ArrayNum = PlayerPrefs.GetInt("1ChapterArrayNum" + idx);

                if (PlayerPrefs.HasKey("1ChapterIsClear" + idx))
                {
                    if (PlayerPrefs.GetInt("1ChapterIsClear" + idx) == 10)
                        isClear = true;
                }

                if (PlayerPrefs.HasKey("1ChapterClearLevel" + idx))
                    ClearLevel = PlayerPrefs.GetInt("1ChapterClearLevel" + idx);
                else
                    ClearLevel = 0;

                if (PlayerPrefs.HasKey("1ChapterHighScore" + idx))
                    HighScore = PlayerPrefs.GetInt("1ChapterHighScore" + idx);
                else
                    HighScore = 0;

                if (PlayerPrefs.HasKey("1ChapterIsFirst" + idx))
                {
                    if (PlayerPrefs.GetInt("1ChapterIsFirst" + idx) == 10)
                        isFirst = true;
                }

                UserData.GetStageData().LoadToStageData(
                    i,
                    isClear,
                    ClearLevel,
                    HighScore,
                    m_1Chapter,
                    isFirst);

                Debug.Log(idx + "Stage Load HightScore :" + HighScore);
            }
            for (int i = 0; i < UserData.GetStageData().GetStageLenth(m_2Chapter); i++)
            {
                if (UserData.GetStageData().GetStageData(i, m_2Chapter) == null)
                    continue;

                idx = i.ToString();
                bool isClear = false;
                bool isFirst = false;

                if (PlayerPrefs.HasKey("2ChapterArrayNum" + idx))
                    ArrayNum = PlayerPrefs.GetInt("2ChapterArrayNum" + idx);
                if (PlayerPrefs.HasKey("2ChapterIsClear" + idx))
                {
                    if (PlayerPrefs.GetInt("2ChapterIsClear" + idx) == 20)
                        isClear = true;
                }
                if (PlayerPrefs.HasKey("2ChapterClearLevel" + idx))
                    ClearLevel = PlayerPrefs.GetInt("2ChapterClearLevel" + idx);
                if (PlayerPrefs.HasKey("2ChapterHighScore" + idx))
                    HighScore = PlayerPrefs.GetInt("2ChapterHighScore" + idx);
                if (PlayerPrefs.HasKey("2ChapterIsFirst" + idx))
                {
                    if (PlayerPrefs.GetInt("2ChapterIsFirst" + idx) == 20)
                        isFirst = true;
                }

                UserData.GetStageData().LoadToStageData(
                    i,
                    isClear,
                    ClearLevel,
                    HighScore,
                    m_2Chapter,
                    isFirst);
            }
        }

    }
    public void GetGradeItem()
    {
        if (UserData == null && UserData.gradeItemSystem == null)
        {
            b_gradeItemData = false;
            total = false;
            return;
        }

        for (int i = 0; i < savingData.m_gradeItemData.m_characterGradeName.Count; i++)
        {
                UserData.gradeItemSystem.LoadToInventory(
                savingData.m_gradeItemData.m_characterGradeNumber[i],
                savingData.m_gradeItemData.m_characterGradeName[i],
                savingData.m_gradeItemData.m_characterGradeCount[i], this);
        }
    }
    public void GetCharacter()
    {
        if (UserData == null && UserData.characterSystem == null)
        {
            b_characterData = false;
            total = false;
            return;
        }

        for (int i = 0; i < savingData.m_characterData.m_characterName.Count; i++)
        {
            UserData.characterSystem.LoadToInventory(
                savingData.m_characterData.m_characterNumber[i],
                //savingData.m_characterData.m_characterPosition[i],
                savingData.m_characterData.m_characterName[i],
                //savingData.m_characterData.m_characterGrade[i],
                savingData.m_characterData.m_characterLevel[i],
                savingData.m_characterData.m_characterItemPercent[i],
                savingData.m_characterData.m_characterHpMinusPercent[i],
                //savingData.m_characterData.m_characterHealth[i],
                savingData.m_characterData.m_characterCurrentEXP[i],
                savingData.m_characterData.m_characterMaxEXP[i],
                this
                );
        }
    }
    public void GetPrefCharacter()
    {
        int length = 0;
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("characterNumber" + i.ToString()))
                length++;
            else
                break;
        }
        for (int i = 0; i < length; i++)
        {
            int characterNumber = 0;
            int characterPosition = 0;
            string characterName = "";
            int characterGrade = 0;
            int characterLevel = 0;
            int characterItemPercent = 0;
            int characterHpMinusPercent = 0;
            int characterHealth = 0;
            int characterCurrentEXP = 0;
            int characterMaxEXP = 0;
            if (PlayerPrefs.HasKey("characterNumber" + i.ToString()))
                characterNumber = PlayerPrefs.GetInt("characterNumber" + i.ToString());
            if (PlayerPrefs.HasKey("characterPosition" + i.ToString()))
                characterPosition = PlayerPrefs.GetInt("characterPosition" + i.ToString());
            if (PlayerPrefs.HasKey("characterName" + i.ToString()))
                characterName = PlayerPrefs.GetString("characterName" + i.ToString());
            if (PlayerPrefs.HasKey("characterGrade" + i.ToString()))
                characterGrade = PlayerPrefs.GetInt("characterGrade" + i.ToString());
            if (PlayerPrefs.HasKey("characterLevel" + i.ToString()))
                characterLevel = PlayerPrefs.GetInt("characterLevel" + i.ToString());
            if (PlayerPrefs.HasKey("characterItemPercent" + i.ToString()))
                characterItemPercent = PlayerPrefs.GetInt("characterItemPercent" + i.ToString());
            if (PlayerPrefs.HasKey("characterHpMinusPercent" + i.ToString()))
                characterHpMinusPercent = PlayerPrefs.GetInt("characterHpMinusPercent" + i.ToString());
            if (PlayerPrefs.HasKey("characterHealth" + i.ToString()))
                characterHealth = PlayerPrefs.GetInt("characterHealth" + i.ToString());
            if (PlayerPrefs.HasKey("characterCurrentEXP" + i.ToString()))
                characterCurrentEXP = PlayerPrefs.GetInt("characterCurrentEXP" + i.ToString());
            if (PlayerPrefs.HasKey("characterMaxEXP" + i.ToString()))
                characterMaxEXP = PlayerPrefs.GetInt("characterMaxEXP" + i.ToString());

            UserData.characterSystem.LoadToInventory(
                 characterNumber,
               //characterPosition[i],
                 characterName,
               //characterGrade[i],
                 characterLevel,
                 characterItemPercent,
                 characterHpMinusPercent,
               //characterHealth[i],
                 characterCurrentEXP,
                 characterMaxEXP,
               this
               );
        }
    }

    public void GetInventory()
    {
        if (UserData == null && UserData.invenSystem == null)
        {
            b_itemData = false;
            total = false;
            return;
        }

        GetInventoryMethod(UserData.invenSystem, 
            savingData.m_itemData.m_playerInvenItemNumber, 
            savingData.m_itemData.m_playerInvenItemCount, 
            savingData.m_itemData.m_playerInvenItemName);
    }

    public void GetInventoryMethod(InventorySystem items, List<int> ArrayNum, List<int> ItemCount, List<string> ItemName)
    {
        for (int i = 0; i < ArrayNum.Count; i++)
        {
            items.LoadToInventory(ArrayNum[i], ItemName[i], ItemCount[i], this);
        }
    }
    #endregion GetData

    #region SaveLoadMethod
    public void SaveData()//게임 자료 저장 함수 처리
    {
        string json = JsonUtility.ToJson(savingData);             //데이터 형식을 JSon으로 변경

        File.WriteAllText(/*SAVE_DATA_DIRECTORY + */SAVE_DATA_FILENAME, json);  //경로에 텍스트 형식으로 JSon 파일 저장

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()//게임 자료 불러오기 함수 처리
    {
        string LoadJson = File.ReadAllText(/*SAVE_DATA_DIRECTORY +*/ SAVE_DATA_FILENAME);   //텍스트 파일 읽어오기
        savingData = JsonUtility.FromJson<SaveData>(LoadJson); 
        Debug.Log("로드 완료");
    }
    #endregion SaveLoadMethod

    #region SaveLoadCall
    public void GameSave()//게임 세이브 작동 함수
    {
        //savingData = new SaveData();

        //SetInventory();
        //SetCharacterInventory();
        //SetStageData();
        //SetUserData();
        //SetSensorData();
        //SetQuestData();
        //SetCountData();
        //SaveData();

        if (!isRemove)
        {
            SetPrefsUserData();
            SetPrefsCharacter();
            SetPrefsSensor();
            SetPrefsCountData();
            SetPrefsStage();
        }
        else
            isRemove = false;
    }
    public void GameLoad()//게임 로드 작동 함수
    {
        //if (File.Exists(/*SAVE_DATA_DIRECTORY + */SAVE_DATA_FILENAME)) //세이브 파일이 있다면
        //{
        //    LoadData();
        //    GetUserData();
        //    GetCharacter();
        //    GetStageData();
        //    GetSensorData();
        //    //GetQuestData();
        //    //GetCountData();
        //    GetInventory();
        //}
        GetPrefUserData();
        GetPrefCharacter();
        GetPrefSensor();
        GetPrefCountData();
        GetPrefStage();
        //else
        //{
        //    Debug.Log("세이브 파일이 없습니다.");
        //}
    }
    #endregion SaveLoadCall

    #region SingleTon
    void SingleTon()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    #endregion SingleTon
    public void OnDestroy()
    {
        GameSave();
    }
}
