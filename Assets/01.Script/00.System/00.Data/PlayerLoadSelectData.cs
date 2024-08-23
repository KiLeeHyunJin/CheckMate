
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoadSelectData : MonoBehaviour
{
    [SerializeField] bool isStopCheck;
    [SerializeField] int StopStageNum;
    [Header("StageInfo")]
    [SerializeField] int chapterNum;
    [SerializeField] int stageNum;
    //[Header("Background")]
    //[SerializeField] GameObject chapter1Background;
    //[SerializeField] GameObject chapter2Background;
    [Header("UI")]
    [SerializeField] Image[] sensorImage;
    [SerializeField] Image[] mainSensorImage;
    [SerializeField] Image[] characterImage;
    [SerializeField] Sprite defaultImage;

    [Header("NULL Data PopUP Window")]
    [SerializeField] GameObject nullCharacterPopUp;
    [SerializeField] GameObject nullSensorPopUp;

    [Header("PlayerCharacter")]
    [SerializeField] ModeReadyPlayerButton readyState;
    [SerializeField] GameObject playerParents;
    [SerializeField] GameObject HeartQueen;
    [SerializeField] GameObject Allice;
    [SerializeField] GameObject Capsaler;
    [SerializeField] GameObject Wolf;

    [Header("Chapter")]
    [SerializeField] StageClick[] chapters;
    [SerializeField] Button[] Chapter1Buttons;
    [SerializeField] Button[] Chapter2Buttons;

    GameObject userCharacter;

    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI highScoreText;

    public class CharacterSelectEntry
    {
        public bool equipState;
        public int invenNum;
        public CharacterBase character;
    }

    UserDataController dataController;
    GameManager gameManager;

    SensorSelectEntry[] sensorEntry = null;
    CharacterSelectEntry[] characterEntry = null;

    [SerializeField] int[] useSensorNum = new int[3] { 99,99,99};
    int useCharacterNum = 99;
    SensorItem[] userSensor = new SensorItem[3];
    CharacterBase useCharacter = null;
    bool isCheck = true;
    void Start()
    {
        dataController = FindObjectOfType<UserDataController>();
        gameManager = FindObjectOfType<GameManager>();

        if(nullCharacterPopUp != null)
            nullCharacterPopUp.SetActive(false);
        if(nullSensorPopUp != null)
            nullSensorPopUp.SetActive(false);
    
    }
    void OnCheck()
    {
        LoadSetting();

        int characterNum = gameManager.GetPlayCharacterNum();
        int entryLenth = UserDataController.Instance.characterSystem.Entries.Length;

        if (entryLenth > characterNum)
        {
            CharacterEntry entry = UserDataController.Instance.characterSystem.Entries[characterNum];
            if (entry != null)
                OnPlayerCharacter(entry.Character.CharacterType);
        }
        else
            OffAllPlayerCharacter();


        SensorEntry[] Temp = dataController.sensorSystem.Entries;
        CharacterEntry[] Dest = dataController.characterSystem.Entries;
        TypeTransSensor(Temp);
        TypeTransCharacter(Dest);
        for (int i = 0; i < userSensor.Length; i++)
        {
            userSensor[i] = new SensorItem();
            sensorImage[i].GetComponent<Button>().enabled = false;
        }
        List<int> Sour = GameManager.instance.GetSensorNum();
        for (int i = 0; i < Sour.Count; i++)
        {
            if (dataController.sensorSystem.Entries.Length <= Sour[i])
                continue;
            useSensorNum[i] = Sour[i];
            userSensor[i] = dataController.sensorSystem.Entries[Sour[i]].sensor;
        }
        stageNum = gameManager.GetStageNum();
        chapterNum = gameManager.GetChapterNum();

        if (stageNum == 99)
            gameManager.SetStageNum((stageNum = 0));
        if (chapterNum == 99)
            gameManager.SetChapterNum((chapterNum = 1));

        if (chapterNum == 1)
        {
            //chapter1Background.SetActive(true);
            //chapter2Background.SetActive(false);
        }
        else
        {
            //chapter1Background.SetActive(false);
            //chapter2Background.SetActive(true);
        }

        if (stageNum == 99)
        {
            Debug.Log(" stageNum is 99 delivered. PlayerLoadSelectData.cs _ 58Line");
            stageNum = 1;
        }
        if (stageText != null)
        {
            stageText.text = "Stage " + stageNum.ToString();
        }
        if (highScoreText != null)
            highScoreText.text = UserDataController.Instance.GetStageData().GetStageData(stageNum, chapterNum).highScore.ToString();
        isCheck = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(isCheck)
        {
            OnCheck();
        }
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;
        bool isOnStage = true;

        if (readyState.onCharacter || (readyState.onSensor))
        {
            if (!playerParents.activeSelf)
                playerParents.SetActive(true); 
            playerParents.transform.position = new Vector3(-0.37f , -0.3f , 0);
        }
        else if (readyState.onReady)
        {
            if (!playerParents.activeSelf)
                playerParents.SetActive(true);
            playerParents.transform.position = new Vector3(-0.2f, -0.46f, 0);
        }
        else if(readyState.onStage)
        {
            if (playerParents.activeSelf)
                playerParents.SetActive(false);
            isOnStage = false;
        }
        if (isOnStage)
        playerParents.transform.position *= new Vector2(width, height);
    }
    private void LateUpdate()
    {
        stageNum = gameManager.GetStageNum();
        chapterNum = gameManager.GetChapterNum();

        if (stageText != null)
        {
            stageText.text = "Stage " + (stageNum + 1).ToString();
        }
        if (highScoreText != null)
            highScoreText.text = UserDataController.Instance.GetStageData().GetStageData(stageNum, chapterNum).highScore.ToString();
    }
    public void SelectCharacter(int _invenCharacterArrayNum)
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        useCharacterNum = _invenCharacterArrayNum;
        useCharacter = dataController.gameData.gameItemDataBase.GetCharacter(_invenCharacterArrayNum);
        for (int i = 0; i < characterImage.Length; i++)
        {
            if(characterImage[i] != null)
                characterImage[i].sprite = useCharacter.CharacterSprite;
            OnPlayerCharacter(useCharacter.CharacterType);
        }
    }

    public int GetSelectCharacter()
    {
        return useCharacterNum;
    }

    public void WearingSensor(int _invenSensorArrayNum)
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        for (int j = 0; j < useSensorNum.Length; j++)
        {
            if (_invenSensorArrayNum == useSensorNum[j])
                return;
        }
        for (int i = 0; i < useSensorNum.Length; i++)
        {
            if (useSensorNum[i] == 99)
            {
                Debug.Log("wearingSensor Num : " + _invenSensorArrayNum);
                useSensorNum[i] = _invenSensorArrayNum;
                userSensor[i] = dataController.sensorSystem.Entries[_invenSensorArrayNum].sensor;//dataController.gameData.gameItemDataBase.GetSensor(_invenSensorArrayNum);
                sensorImage[i].sprite = userSensor[i].InGameSensorSprite;
                mainSensorImage[i].sprite = userSensor[i].InGameSensorSprite;
                sensorImage[i].GetComponent<Button>().enabled = true;
                //sensorImage[i].GetComponent<Button>().
                break;
            }
        }
    }

    public void UnWearingSensor(int _selectSensorArrayNum)
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        for (int i = 0; i < userSensor.Length; i++)
        {
            if (useSensorNum[i] == _selectSensorArrayNum)
            {
                useSensorNum[i] = 99;
                userSensor[i] = null;
                SensorDefaultImage(i);
                sensorImage[i].GetComponent<Button>().enabled = false;
                break;
            }
        }
    }
    public void RemoveSensor(int _idx)
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        if (useSensorNum[_idx] != 99 )
         {
             useSensorNum[_idx] = 99;
             userSensor[_idx] = null;
            SensorDefaultImage(_idx);
             sensorImage[_idx].GetComponent<Button>().enabled = false;
         }
    }
    void SensorDefaultImage(int _idx)
    {
        sensorImage[_idx].sprite = defaultImage;
        mainSensorImage[_idx].sprite = defaultImage;
    }
    public bool isWearSensorCheck(int _sensorArrayNum)
    {
        for (int i = 0; i < useSensorNum.Length; i++)
        {
            if (useSensorNum[i] == _sensorArrayNum)
                return true;
        }
        return false;
    }

    public void StartButton()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        if (useCharacter == null || useCharacterNum == 99)
        {
            Debug.Log("useCharacter 데이터가 null입니다.");

            if(nullCharacterPopUp != null)
                nullCharacterPopUp.SetActive(true);
            return;
        }

        List<int> Temp = new List<int>();
        bool isSensorWear = false;



        for (int i = 0; i < useSensorNum.Length; i++)
        {
            Temp.Add(useSensorNum[i]);

            if(useSensorNum[i] != 99)//직감이 하나라도 있으면 장착으로 표기
                isSensorWear = true;
        }

        GameManager.instance.SetSensorNum(Temp);
        GameManager.instance.SetPlayCharacterNum(useCharacterNum);

        GameStartScene();
        return;

        //직감 장착 없을 시 팝업창 등장
        //if (isSensorWear)
        //{
        //    GameStartScene();
        //    return;
        //}
        //if(nullSensorPopUp != null)
        //    nullSensorPopUp.SetActive(true);
    }
    public void NextStage()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        GameStageData stageData = dataController.GetStageData();
        bool isClearStage = false;
        int stageLenth = 0;

        if (stageData != null)
        {
            StageDataSetting stage = stageData.GetStageData(stageNum, chapterNum);
            if(stage != null)
            {
                isClearStage = stage.isClear;
                stageLenth = stageData.GetStageLenth(chapterNum) - 1;
            }
        }
        if(isClearStage)
        {
            if (stageNum < stageLenth)
            {
                int nextStageNum = stageNum + 1;
                gameManager.SetStageNum(nextStageNum);

                if(chapters != null)
                { 
                    if(chapters[chapterNum - 1] != null)
                    {
                        OnBackButtonClick(chapters, nextStageNum);
                        switch (chapterNum)
                        {
                            case 1:
                                OnButtonClick(Chapter1Buttons);
                                break;
                            case 2:
                                OnButtonClick(Chapter2Buttons);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
                return;
        }
    }
    void OnBackButtonClick(StageClick[] stageClick, int caseNum)
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);

        if (stageClick != null && stageClick.Length > caseNum  && stageClick[caseNum] != null)
        {
            stageClick[caseNum].OnOffStageInfo();
            Debug.Log("Operation BackButton");
        }
    }
    void OnButtonClick(Button[] buttons)
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        if (buttons != null)
        {
            if (buttons.Length >= stageNum + 1 && buttons[stageNum + 1] != null)
            {
                Button button = buttons[stageNum + 1].GetComponent<Button>();
                if (button != null)
                    button.onClick.Invoke();
            }
        }
    }

    void GameStartScene()
    {
        string sceneName = "OvenBreak";
        if (stageNum >= 99)  //스테이지 값이 할당 안되어있으면
            return;

        if(isStopCheck) //멈출지 정함
        {
            if (stageNum > StopStageNum) //스테이지가 준비되어있지 않다면
                return;
        }

        if (stageNum > 0 ) //0번째가 아니라면 문자열 결합
            sceneName += " " + stageNum.ToString();

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    

    public void CloseNullCharacterPopUP()//캐릭터 팝업 종료
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        if (nullCharacterPopUp != null)
            nullCharacterPopUp.SetActive(false);
    }
    public void YesSensorPopUpButton()//직감없어도 그냥 시작
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        GameStartScene();
    }

    public void CloseNUllSensorPopUp()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        if (nullSensorPopUp != null)
            nullSensorPopUp.SetActive(false);
    }

    public void OffSensorIcon()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        for (int i = 0; i < useSensorNum.Length; i++)
        {
            sensorImage[i].GetComponent<Button>().enabled = false;
        }
    }
    public void OnSensorIcon()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        for (int i = 0; i < useSensorNum.Length; i++)
        {
            sensorImage[i].GetComponent<Button>().enabled = true;
        }
    }
    void LoadSetting()
    {
        if (gameManager.GetPlayCharacterNum() != 99)
        {
            SelectCharacter(gameManager.GetPlayCharacterNum());
        }
        if (gameManager.GetSensorNum().Count != 0)
        {
            List<int> list = new List<int>();
            list = gameManager.GetSensorNum();
            for (int i = 0; i < gameManager.GetSensorNum().Count; i++)
            {
                WearingSensor(list[i]);
            }
        }
    }
    void TypeTransSensor(SensorEntry[] _temp)
    {
        int index = 0;
        for (int i = 0; i < _temp.Length; i++)
        {
            if (_temp[i] != null && _temp[i].sensor != null)
            {
                ++index;
                continue;
            }
            else
                break;
        }
        sensorEntry = new SensorSelectEntry[index];
        for (int i = 0; i < index; i++)
        {
            sensorEntry[i] = new SensorSelectEntry();
            sensorEntry[i].sensosr = _temp[i].sensor;
        }
            //SensorCopy(sensorEntry, _temp);
    }
    void TypeTransCharacter(CharacterEntry[] _temp)
    {
        int index = 0;
        for (int i = 0; i < _temp.Length; i++)
        {
            if (_temp[i] != null && _temp[i].Character != null)
            {
                ++index;
                continue;
            }
            else
                break;
        }
        characterEntry = new CharacterSelectEntry[index];

        for (int i = 0; i < index; i++)
        {
            characterEntry[i] = new CharacterSelectEntry();
            characterEntry[i].character = _temp[i].Character;
            //CharacterCopy(characterEntry, _temp);
        }
    }
    void SensorCopy(SensorSelectEntry[]  _ref, SensorEntry[] _origin )
    {
        for (int i = 0; i < _ref.Length; i++)
        {
            _ref[i].sensosr.sensorInfo = _origin[i].sensor.sensorInfo;
            _ref[i].sensosr.sensorLevel = _origin[i].sensor.sensorLevel;
            _ref[i].sensosr.sensorName = _origin[i].sensor.sensorName;
            _ref[i].sensosr.sensorSprite = _origin[i].sensor.sensorSprite;
        }
    }
    int ArrayLenth<T>(T[] _temp)
    {
        int index = 0;
        for (int i = 0; i < _temp.Length; i++)
        {
            if (_temp[i] != null)
            {
                ++index;
                continue;
            }
            else
                break;
        }
        return index;
    }

    void BonusEffectCheck()
    {

    }
    void OnBonusEffect()
    {

    }
    void OnPlayerCharacter(DataType.CharacterType characterType)
    {
        OffAllPlayerCharacter();
        Animator anim = null;
        switch (characterType)
        {
            case DataType.CharacterType.HeartQueen:
                if(HeartQueen != null)
                {
                    HeartQueen.SetActive(true);
                    userCharacter = HeartQueen;
                }
                break;
            case DataType.CharacterType.Wolf:
                if(Wolf != null)
                {
                    Wolf.SetActive(true);
                    userCharacter = Wolf;
                }
                break;
            case DataType.CharacterType.CapSaller:
                if(Capsaler != null)
                {
                    Capsaler.SetActive(true);
                    userCharacter = Capsaler;
                }
                break;
            case DataType.CharacterType.Allice:
                if (Allice != null)
                {
                    Allice.SetActive(true);
                    userCharacter = Allice;
                }
                break;
            default:
                break;
        }
        anim = userCharacter.GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("Init", true);
    }
    void OffAllPlayerCharacter()
    {
        OffCharacter(HeartQueen);
        OffCharacter(Wolf);
        OffCharacter(Capsaler);
        OffCharacter(Allice);
    }

    void OffCharacter(GameObject userCharacter)
    {
        if(userCharacter != null)
        {
            userCharacter.SetActive(false);
        }
    }
}
