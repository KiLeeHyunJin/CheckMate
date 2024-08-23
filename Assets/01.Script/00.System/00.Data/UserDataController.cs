using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct DefaultCharacterAbility
{
    public float ItemPercent;
    public float HPMinusPercent;
    public float FeverTime;
}
[Serializable]
public class CountData
{
    UserDataController Owner;
    public int SlideCount { get;private set; }

    public int JumpCount { get; private set; }
    public int DoubleCount { get; private set; }
    public int FeverCount { get; private set; }
    public int SkillCount { get; private set; }
    public int SensorCount { get; private set; }
    public int LevelUpCount { get; private set; }
    public int ItemLevelUpCount { get; private set; }

    public int BuyCount { get; private set; }
    public int StageScoreCount { get; private set; }
    public void Reset()
    {
        SlideCount = 0;
        JumpCount = 0;
        DoubleCount = 0;
        FeverCount = 0;
        SkillCount = 0;
        SensorCount = 0;
        LevelUpCount = 0;
        ItemLevelUpCount = 0;

        BuyCount = 0;
    }
    public void SetCountData(int _slide, int _jump, int _double, int _fever, int _skill, int _sensor, int _levelUp, int _itemLevelUp, int _buy, UserDataController userData)
    {
        Owner = userData;
        SlideCount = _slide;
        JumpCount = _jump;
        DoubleCount = _double;
        FeverCount = _fever;
        SkillCount = _skill;
        SensorCount = _sensor;

        LevelUpCount = _levelUp;
        ItemLevelUpCount = _itemLevelUp;

        BuyCount = _buy;
    }
    public void SendCount(QuestEntry[] _questEntry)
    {
         QuestEntry[] questEntry = _questEntry;
         if(questEntry != null)
         {
             for (int i = 0; i < questEntry.Length; i++)
             {
                 QuestEntry quest = questEntry[i];
                 quest.iCurrentCount = Divid(quest.type, quest);
             }
         }
    }
    int Divid(DataType.QuestType _type, QuestEntry questEntry)
    {
        int count = 0;
        switch (_type)
        {
            case DataType.QuestType.Slide:
                count = SlideCount;
                break;
            case DataType.QuestType.Jump:
                count = JumpCount;
                break;
            case DataType.QuestType.Double:
                count = DoubleCount;
                break;
            case DataType.QuestType.Fever:
                count = FeverCount;
                break;
            case DataType.QuestType.Skill:
                count = SkillCount;
                break;
            case DataType.QuestType.Sensor:
                count = SensorCount;
                break;
            case DataType.QuestType.Buy:
                count = BuyCount;
                break;
            case DataType.QuestType.LevelUp:
                count = LevelUpCount;
                break;
            case DataType.QuestType.ItemLevelUp:
                count = ItemLevelUpCount;
                break;
            case DataType.QuestType.StageScore:
                if(Owner != null)
                {
                    StageDataSetting stageData = Owner.GetStageData().GetStageData(questEntry.iStageNum, questEntry.iChapterNum);
                    if (stageData != null)
                        count = (int)stageData.highScore;
                }
                break;
            default:
                break;
        }
        return count;
    }
    public void SlideAdd() { SlideCount++; }
    public void JumpAdd() { JumpCount++; }
    public void DoubleJumpAdd() { DoubleCount++; }
    public void FeverAdd() { FeverCount++; }
    public void SkillAdd() { SkillCount++; }
    public void SensorAdd() { SensorCount++; }
    public void LevelUpAdd() { LevelUpCount++; }
    public void ItemLevelUpAdd() { ItemLevelUpCount++; }
    public void BuyAdd() { BuyCount++; }


}
public class UserDataController : MonoBehaviour
{
    public static UserDataController Instance { get; protected set; }
    public GameData gameData { get; set; }
    public GameItemDataBase dataBase { get; set; }
    public CountData countData = new CountData();
    public QuestData questData = new QuestData();
    public InventorySystem invenSystem = new InventorySystem();//플레이어의 아이템을 저장할 시스템
    public CharacterSystem characterSystem = new CharacterSystem();
    public GradeItemSystem gradeItemSystem = new GradeItemSystem();//플레이어의 아이템을 저장할 시스템
    public SensorSystem sensorSystem = new SensorSystem();

    [SerializeField] GameStageData gameStageData = new GameStageData();

    public DefaultCharacterAbility levelUpCharacterAbility { get; set; }

    public string UserName;

    public int[] playerLevelMaxEXP;

    public int UserCharacterIcon = 99; 

    public DataType.CharacterType characterType;

    public int Money;
    public int Stamina { get; set; }
    public int Cash;
    public int UserLevel { get; set; }
    public int CurrentEXP;
    public int MaxEXP;
    public bool isGiveMoney { get; set; }
    public int QuestIndex;
    [SerializeField] TextMeshProUGUI moneyTxt;
    [SerializeField] TextMeshProUGUI diamondTxt;
    [SerializeField] TextMeshProUGUI userlevel;
    [SerializeField] Image userlevelFillImage;
    [SerializeField] int setUserLevel;
    //[SerializeField] TextMeshProUGUI ;
  
    public void SetCountData(int _slide, int _jump, int _doubleJump, int _fever, int _skill, int _sensor, int _levelUP, int _itemLevelup, int _buy)
    {
        if(countData == null)
            countData = new CountData();
        countData.SetCountData(_slide, _jump, _doubleJump, _fever, _skill, _sensor, _levelUP, _itemLevelup, _buy, this);
    }

    public void SetUserData(int _money = 0, int _cash = 0, int _stamia = 100, int _level = 1, int _exp = 0, int _maxExt = 100, int _userCharacter = 99, int _lobbyCharacterNum = 99)
    {
        Money = _money;
        Cash =  _cash;
        Stamina = _stamia;
        CurrentEXP = _exp;
        MaxEXP = _maxExt;
        UserLevel = _level;
        CurrentEXP = _exp;
        MaxEXP = _maxExt;
        UserCharacterIcon = _userCharacter;
        characterType = (DataType.CharacterType)_lobbyCharacterNum;
    }

    public void SetQuestData(List<int> _type, List<int> _currentCount, List<bool> _isClear, int _questCount, bool _isReward)//로드
    {
        if(questData == null)
            questData = new QuestData();
        questData.Owner = this;

        QuestIndex = _questCount;
        questData.SetCurrentQuest(QuestIndex);
    
        StageQuestEntry stageQuestEntry = questData.GetCurrenQuest();
        if (stageQuestEntry != null)
        {
            stageQuestEntry.isReward = _isReward;
            QuestEntry[] entry = stageQuestEntry.questEntries;

            int lenth = entry.Length;
            for (int i = 0; i < lenth; i++)
            {
                if(_type.Count <= i || _currentCount.Count <= i || _isClear.Count <= i)
                    return;
                //Debug.Log("_type.Count : " + _type.Count + "_currentCount.Count : " + _currentCount.Count + "_isClear.Count : " + _isClear.Count + " i : " + i + "entry[i] : " + lenth);
                entry[i].type = (DataType.QuestType)_type[i];
                entry[i].iCurrentCount = _currentCount[i];
                entry[i].isClear = _isClear[i];
            }
        }
    }

    public void GetQuestData(List<int> _type, List<int> _currentCount, List<bool> _isClear)//세이브
    {
        StageQuestEntry stageQuestEntry = questData.GetCurrenQuest();

        if (stageQuestEntry != null)
        {
            QuestEntry[] entry = stageQuestEntry.questEntries;

            for (int i = 0; i < entry.Length; i++)
            {
                _type.Add((int)entry[i].type);
                _currentCount.Add(entry[i].iCurrentCount);
                _isClear.Add(entry[i].isClear);
            }
        }
    }
    void SingleTone()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Awake() //싱글톤
    {
        SingleTone();
        dataBase = GetComponent<GameItemDataBase>();
        gameData = GetComponent<GameData>();
        gameData.UserData = this;
        DataLink();
    }

    void Start()
    {
        TextSetting();
        if(gameStageData != null)
            Debug.Log("스테이지 데이터가 정상적으로 할당되었습니다.");
        else
        Debug.Log("스테이지 데이터의 할당이 실패되었습니다.");
        gameStageData.Start();
        if (UserLevel <= 0)
            UserLevel = 1;
    }

    void Update()
    {
        if (isGiveMoney == false)
        {
            Cash += 1000;
            Money += 1000;
            isGiveMoney = true;
            PlayerPrefs.SetInt("First", 10);
            PlayerPrefs.Save();
        }
        TestKeyCode();
        UpdateMaxExp();
        //UpdateMaxExp();
    }
    private void LateUpdate()
    {
        if(questData != null || questData.QuestNullCheck())
            LinkCountData();
    }


    void LinkCountData()
    {
        StageQuestEntry quest = questData.GetCurrenQuest();
        if (quest != null)
        {
            QuestEntry[] questEntries = quest.questEntries;

            countData.SendCount(questEntries);

            bool isClear = questData.QuestClearCheck();
            if (isClear)
            {
                quest.isReward = true;
                GameManager.isQuestClear = true;
                //bool isSusses = questData.QuestNext();

                //if (isSusses)
                //    Debug.Log("다음 퀘스트를 정상적으로 할당하였습니다.");
                //else
                //    Debug.Log("마지막 퀘스트에 도달하였기에 다음 퀘스트를 할당하지 못하였습니다.");
            }
        }
        else
            Debug.Log("quest Data is NullReference");
    }

    void CheckCountData(QuestEntry questEntry)
    {
        DataType.QuestType type = questEntry.type;
        switch (type)
        {
            case DataType.QuestType.Slide:
                questEntry.iCurrentCount = countData.SlideCount;
                break;
            case DataType.QuestType.Jump:
                questEntry.iCurrentCount = countData.JumpCount;
                break;
            case DataType.QuestType.Double:
                questEntry.iCurrentCount = countData.DoubleCount;
                break;
            case DataType.QuestType.Fever:
                questEntry.iCurrentCount = countData.FeverCount;
                break;
            case DataType.QuestType.Skill:
                questEntry.iCurrentCount = countData.SkillCount;
                break;
            case DataType.QuestType.Buy:
                questEntry.iCurrentCount = countData.BuyCount;
                break;
            case DataType.QuestType.LevelUp:
                questEntry.iCurrentCount = countData.LevelUpCount;
                break;
            case DataType.QuestType.ItemLevelUp:
                questEntry.iCurrentCount = countData.ItemLevelUpCount;
                break;
            case DataType.QuestType.Sensor:
                questEntry.iCurrentCount= countData.SensorCount;
                break;
            case DataType.QuestType.StageScore:
                {
                    StageDataSetting stageData = GetStageData().GetStageData(questEntry.iStageNum, questEntry.iChapterNum);
                    if (stageData != null)
                        questEntry.iCurrentCount = (int)stageData.highScore;
                }
                break;
            default:
                break;
        }
    }

    void UpdateMaxExp()
    {
        if (dataBase != null)
        {
            switch (UserLevel)
            {
                case 1:
                    MaxExpCheck(dataBase.ExpData.Level1);
                    break;
                case 2:
                    MaxExpCheck(dataBase.ExpData.Level2);
                    break;
                case 3:
                    MaxExpCheck(dataBase.ExpData.Level3);
                    break;
                case 4:
                    MaxExpCheck(dataBase.ExpData.Level4);
                    break;
                case 5:
                    MaxExpCheck(dataBase.ExpData.Level5);
                    break;
                case 6:
                    MaxExpCheck(dataBase.ExpData.Level6);
                    break;
                case 7:
                    MaxExpCheck(dataBase.ExpData.Level7);
                    break;
                case 8:
                    MaxExpCheck(dataBase.ExpData.Level8);
                    break;
                default:
                    break;
            } //레벨에 따른 최대 경험치량 재설정

            if(MaxEXP <= 0)
            {
                Debug.Log("MaxEXP is Zero. This is Fatal Error!");
                return;
            }

            if (CurrentEXP >= MaxEXP)
            {
                if(UserLevel < 8)
                {
                    while (CurrentEXP >= MaxEXP)
                    {
                        CurrentEXP -= MaxEXP;
                        ++UserLevel;
                        countData.LevelUpAdd();
                    }
                }
            }

        }
    }
    void MaxExpCheck(int _LevelMaxExp)
    {
        if(MaxEXP != _LevelMaxExp)
            MaxEXP = _LevelMaxExp;
    }
    public GameStageData GetStageData()
    {
        if(gameStageData != null)
        return gameStageData;
        return null;
    }
    public void LevelUpAndReSettingEXP()
    {
        if (playerLevelMaxEXP.Length < UserLevel) //배열을 벗어날 경우
        {
            CurrentEXP = MaxEXP;
            return;
        }
        if(CurrentEXP > MaxEXP) //최대 경험치를 달성한 경우
            CurrentEXP -= MaxEXP;
        if (UserLevel <= 0)
            UserLevel = 1;
        MaxEXP = playerLevelMaxEXP[UserLevel];
        UserLevel++;
        GameManager.isLevelUp = true;
    }

    void DataLink()
    {
        gameData.UserData = this;
        gradeItemSystem.m_Data = this;
        invenSystem.m_Data = this;
        characterSystem.m_Data = this;
        sensorSystem.m_Data = this;
    }
    void TextSetting()
    {
        if(moneyTxt != null) moneyTxt.text = Money.ToString();
        if(diamondTxt != null) diamondTxt.text = Cash.ToString();
        if(userlevel != null) userlevel.text = UserLevel.ToString();
    }
    void TestKeyCode()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isGiveMoney = false;
            PlayerPrefs.SetInt("First", 0);
            PlayerPrefs.Save();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Money += 100;
            Cash += 100;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (Money >= 100 && Cash >= 100)
            {
                Money -= 100;
                Cash -= 100;
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            CurrentEXP += 5;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UserLevel = setUserLevel;
        }
    }
}
