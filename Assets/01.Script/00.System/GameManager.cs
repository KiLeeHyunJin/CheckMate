using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //스테이지 챕터 위치
    static int currentStageNum = 99; // 인게임에 들어갈 스테이지 번호
    static int currentChapterNum = 99;
    static int characterNum = 99; //인게임에 달릴 캐릭터 번호
    static bool isNextStage = false;
    //직감 리스트
    static List<int> sensorNum = new List<int>();//인게임에 사용할 아이템 번호
    static int sensor1 = 99;
    static int sensor2 = 99;
    static int sensor3 = 99;


    //씬 위치
    static DataType.SceneType sceneType;
    static DataType.SceneType beforeSceneType;
    public static bool isLevelUp;
    public static bool isQuestClear;

    public static DataType.CharacterType characterType;

    UserDataController userData;
    QuestData questData;
    [SerializeField] int stage = 99;
    [SerializeField] int character = 99;
    [SerializeField] int chapter = 99;
    [SerializeField] bool isQuestActive;
    [SerializeField] DataType.SceneType monitorBeforeSceneType;
    [SerializeField] DataType.SceneType monitorSceneType;

    [SerializeField] TextMeshProUGUI highScoreTxt;
    [SerializeField] TextMeshProUGUI stageTxt;

    [SerializeField] List<int> sensor = new List<int>();
    [SerializeField] int[] sensorArray = new int[3];

    [SerializeField] GameObject levelUpPopUp;
    [SerializeField] GameObject questClearPopUp;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        monitorBeforeSceneType = beforeSceneType;
        monitorSceneType = sceneType;
    }
    void Start()
    {
        userData = FindObjectOfType<UserDataController>();
        if(userData != null)
            questData = userData.questData;
        sensor = sensorNum;
        character = characterNum;
        stage = currentStageNum;
        chapter = currentChapterNum;

        for (int i = 0; i < sensorArray.Length; i++)
        {
            if(i == 0)
            {
                sensorArray[0] = sensor1;
            }
            else if(i == 1)
            {
                sensorArray[1] = sensor2;
            }
            else
            {
                sensorArray[2] = sensor3;
            }
        }

        ClosePopUp();

        LevelUpPopUp();
        if(isQuestActive)
            QuestClearPopUp();
    }
    void ClosePopUp()
    {
        if(levelUpPopUp != null)
            levelUpPopUp.SetActive(false);
        if(questClearPopUp != null  )
            questClearPopUp.SetActive(false);
    }
    public void UpdateStageTxt(int _stageNum)
    {
        highScoreTxt.text = UserDataController.Instance.GetStageData().GetStageData(stage, chapter).highScore.ToString();
        stageTxt.text = (stage + 1).ToString();
    }



    public void SetSceneType(DataType.SceneType type, DataType.SceneType currentSence = DataType.SceneType.END)
    {
        if (currentSence == DataType.SceneType.END) // 디폴트 매개변수
            monitorBeforeSceneType = beforeSceneType = sceneType; //전 씬을 현재씬으로 복사
        else  //2번째 매개변수에 데이터가 들어왔다면
            monitorBeforeSceneType = beforeSceneType = currentSence;//전 씬을 매개변수로 받아 대입

        monitorSceneType = sceneType = type;  //바뀐 씬 타입을 현재씬에 저장
    } 

    public DataType.SceneType GetCurrentSceneType()
    { return sceneType; }

    public DataType.SceneType GetBeforeSceneType()
    {   return beforeSceneType; }

    public void AddEXP(int value)
    {
        if (userData != null)
            userData.CurrentEXP += value;
    }

    float GetEXPPercent()
    {
        if (userData != null)
        {
            float maxExp = userData.MaxEXP;
            float currentExp = userData.CurrentEXP;

            if (maxExp  != 0)
            {
                float returnValue = currentExp / maxExp;
                return returnValue;
            }
        }
        return 0;
    }


    public void LevelCheck()
    {
        float check = GetEXPPercent();

        if (userData == null || check < 1)
        {
            if(isLevelUp)
                isLevelUp = false;
            return;
        }

        userData.LevelUpAndReSettingEXP();
    }
    void QuestClearPopUp()
    {
        if(isQuestClear)
        {
            if(questClearPopUp != null)
            {
                isQuestClear = false;
                questClearPopUp.SetActive(true);
                questData.QuestNext();
            }
        }
    }

    void LevelUpPopUp()
    {
        LevelCheck();
        if (isLevelUp)
        {
            if(levelUpPopUp != null)
            {
                isLevelUp = false;
                levelUpPopUp.SetActive(true);
                //레벨업 팝업 활성화
            }
        }
    }

    public int GetPlayCharacterNum()
    {   return characterNum;    }

    public void SetPlayCharacterNum(int _characterNum)
    {
        characterNum = character = _characterNum;
    }

    public List<int> GetSensorNum()
    {   return sensorNum;   }

    public void SetSensorNum(List<int> _sensorNum)
    {   sensorNum = sensor = _sensorNum;
        for (int i = 0; i < _sensorNum.Count; i++)
        {
            switch (i)
            {
                case 0:
                    sensorArray[0] = sensor1 = _sensorNum[0];
                    break;
                case 1:
                    sensorArray[1] = sensor2 = _sensorNum[1];
                    break;
                case 2:
                    sensorArray[2] = sensor3 = _sensorNum[2];
                    break;
                default:
                    sensorArray[2] = sensor3 = _sensorNum[2];
                    break;
            }
        }
    }
    public int[] GetSensorArray()
    {
        for (int i = 0; i < sensorArray.Length; i++)
        {
            switch (i)
            {
                case 0:
                    sensorArray[0] = sensor1;
                    break;                 
                case 1:                    
                    sensorArray[1] = sensor2;
                    break;                  
                case 2:                     
                    sensorArray[2] = sensor3;
                    break;                  
                default:                    
                    sensorArray[2] = sensor3;
                    break;
            }
        }
        return sensorArray;
    }

    public int GetStageNum()
    {   return currentStageNum; }

    public void SetChapterNum(int _chapterNum)
    {   chapter = currentChapterNum = _chapterNum;  }

    public int GetChapterNum()
    {   return currentChapterNum;   }

    public void SetStageNum(int _stageNum)
    {   currentStageNum = stage =_stageNum; }

    void OnEnable()
    {   SceneManager.sceneLoaded += OnSceneLoaded;  }

    void OnDisable()
    { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "login")
        {
            Time.timeScale = 1;
            BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_Intro);

        }
        else if (scene.name == "mode_select")
        {
            Time.timeScale = 1;
            BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_Chapter);

        }
        else if (scene.name == "characters")
        {
            Time.timeScale = 1;
            BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_Inven);

        }
        else if (scene.name == "Inventory")
        {
            Time.timeScale = 1;
            BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_Inven);

        }
        else if (scene.name == "store")
        {
            Time.timeScale = 1;
            BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_Market);

        }
        else if (scene.name == "lobby")
        {
            Time.timeScale = 1;
            BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_Lobby);

        }
        else if (scene.name == "mode_stage")
        {
            Time.timeScale = 1;
            BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_Stage);
            return;
        }
        else// if (scene.name.Contains("OvenBreak"))
        {
            Time.timeScale = 1;
            BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_InGame);
        }
    }


}
