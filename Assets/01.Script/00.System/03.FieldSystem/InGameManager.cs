using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    [Header("FeverState")]
    float CharacterMass = 1;
    float CharacterJump = 1300;
    float CharacterDrag = 0;
    float CharacterGravity = 11;
    float CharacterDoubleJump = 1500;

    [Header("DefaultState")]
    float CharacterDfMass = 1;
    float CharacterDfJump = 1200;
    float CharacterDfDrag = 0;
    float CharacterDfGravity = 9;
    float CharacterDfDoubleJump = 1300;

    [Header("플레이어")]

    float DefaultCookieSpeed = 12;
    float boosterSpeed = 18;
    float currentCookieSpeed;
    public int jump = 2;


    [SerializeField] ComboCountSystem comboCountSystem;
    [SerializeField] GameObject pauseManu;
    [SerializeField] GameManager gameManager;
    [SerializeField] UserDataController userData;
    [SerializeField] PlayerEffectController feverEffect;
    [SerializeField] GameObject objectParent;
    [SerializeField] GameObject Canvas;
    [SerializeField] SkillEffectUI skillEffectUI;
    [Header("종료 후 씬 전환")]
    [SerializeField] SceneChanger sceneChanger;
    [SerializeField] DataType.SceneType EndchangScene;
    [Header("스테이지")]
    [SerializeField] int stageNum; // 인게임 스테이지 번호
    [SerializeField] int chapterNum;
    [SerializeField] int bestScore; // 스테이지 최고 점수
    [SerializeField] int gamescore = 0;

    [Header("설정 값")]
    [SerializeField] float currenBoostTime;
    [Range(0.1f, 1)]
    [SerializeField] float feverSlowValue;
    [SerializeField] float feverSlowTime;
    [SerializeField] float objectHurt = 0.03f;
    [SerializeField] float timeHurt = 0.02f;
    [SerializeField] float healValue = 0.1f;



    [Header("UI")]
    [SerializeField] AddScoreText addScoreText;
    public Slider hpbar;
    public TextMeshProUGUI totalScore;
    public TextMeshProUGUI endScore;
    [SerializeField] GameObject resultOffObject;
    public GameObject result;
    public Image newicon;
    public GameObject boosteffectPrefab;
    [SerializeField] int coinNum = 0;
    public TextMeshProUGUI coinCount;
    public TextMeshProUGUI resultStageNum;
    public TextMeshProUGUI rewardCoinCount;
    [SerializeField] Animator readyCount;
    TextMeshProUGUI stageName;
    [Header("Reward")]
    [SerializeField] RewardIconEntry rewardEntryPrefab;
    [SerializeField] RectTransform[] rewardRectTransform;
    RewardIconEntry[] UIPrefabs;

    [Header("장애물")]
    public float biggerTime = 3.5f;
    public float boosterTime = 3f;
    public float magnetTime = 3f;
    public float petAbilityTime = 2f;
    public float fireBallSpeed = 1.5f;
    public float characterFevetTime = 1.5f;

    [Header("ScoreTxt")]
    [SerializeField] float ScoreTxtAlphaValue;
    [SerializeField] float ScoreTxtAlphaSpeed;
    [SerializeField] float ScoreTxtScale;
    [SerializeField] float ScoreTxtSpeed;

    public bool isGameover;
    [HideInInspector] public bool isBoost;
    [HideInInspector] public float feverBonusCoinValue;
    public bool ismagatic { get; set; }
    public float fireBallHurt { get; set; }
    PlayerCharacter playerCharacter;

    GameObject[] boosteffect;
    GameObject[] basicjellies;

    float multipleScore = 0;
    int idx;
    int targetidx;
    int obstacleTargetidx;
    public int comboNum {  get;  set; }

    string strScore = "";
    bool isAddScoreSensor = false;
    public bool isCount { get; private set; }
    public bool isFeverTime { get; set; }
    public bool isFeverState { get; private set; }
    float isStartTimeCheck;
    float isFeverTimeCheck;
    float AddScorePercent = 0f;
    float isFeverBonusScore = 0;
    float comboTxtDefaultValue = 0;
    bool isBonusSwitch = false;
    bool isBonus = false;
    bool isBlackJack = false;
    bool isEffectScore = false;
    public bool isTuto { get; set; }
    int blackJackScore = 0;
    public SkillEffectUI GetSkillEffectUI()
    {
        return skillEffectUI;
    }
    public void StageInit()
    {
        stageNum = GameManager.instance.GetStageNum();
        chapterNum = GameManager.instance.GetChapterNum();
        isBoost = false;
        if(result.activeSelf)
            result.SetActive(false);
        multipleScore = 1;
        if (stageNum == 99)
        {
            stageNum = 0;
            Debug.Log("스테이지 넘버가 할당되지 못하였습니다. 강제로 튜토리얼 스테이지에 진입합니다. stageNum : " + stageNum);
        }
        if(chapterNum == 99)
        {
            chapterNum = 1; 
            Debug.Log("챕터 넘버가 할당되지 못하였습니다. 강제로 튜토리얼 스테이지에 진입합니다. chapterNum : " + chapterNum);
        }
        if(stageNum != 99 && chapterNum != 99)
            bestScore = UserDataController.Instance.GetStageData().GetStageData(stageNum, chapterNum).highScore;
        if (totalScore != null)
            totalScore.text = "0"; 

        comboNum = 0;
    }

    public void PlayerCharacterInit(float _damagePercent, float _timeDamamgePercent, float _feverTime, float _itemTime)
    {
        objectHurt -= (_damagePercent / 100) * characterFevetTime;
        fireBallHurt -= (_damagePercent / 100) * fireBallHurt;

        timeHurt -= (_damagePercent / 100) * timeHurt;

        characterFevetTime = _feverTime;

        boosterTime += ((_itemTime / 100) * boosterTime) * 10f;
        magnetTime += (_itemTime / 100) * magnetTime;
        biggerTime += (_itemTime / 100) * biggerTime;
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        Canvas.SetActive(true);
        boosteffect = new GameObject[20];
        for (int i = 0; i < boosteffect.Length; i++)
        {
            boosteffect[i] = Instantiate(boosteffectPrefab);
            boosteffect[i].transform.parent = objectParent.transform;
            boosteffect[i].SetActive(false);
        }
        if(readyCount != null)
        {
            readyCount.gameObject.SetActive(true);
            feverEffect.skillEffectUI = skillEffectUI;
        }
        isStartTimeCheck = 5;
        playerCharacter = PlayerCharacter.instance;
        isCount = true;


    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("BestScore"))
            PlayerPrefs.SetInt("BestScore", 0);
        isGameover = false;
        ismagatic = false;
        isTuto = false;
        idx = 0;
        coinNum = 0;
        if (coinCount != null)
            coinCount.text = coinNum.ToString();
        if (objectHurt <= 0) objectHurt = 0.03f;
        if (timeHurt <= 0) timeHurt = 0.02f;
        if (fireBallHurt <= 0) fireBallHurt = 0.03f;
        if (healValue <= 0) healValue = 0.1f;
        StageInit();

        float animSpeed;
        if (feverSlowTime != 0)
            animSpeed = 1 / feverSlowTime;
        else
        {
            animSpeed = 5;
            feverSlowTime = 0.2f;
        }

        if(skillEffectUI != null)
        {
            skillEffectUI.AnimSpeed(animSpeed);
        }
        OffRigidState();
    }
    private void OnEnable()
    {
        if (DefaultCookieSpeed <= 0f)
            DefaultCookieSpeed = 10f;
        if (boosterSpeed <= DefaultCookieSpeed)
            boosterSpeed = (DefaultCookieSpeed * 2) - (DefaultCookieSpeed / 2);
        currentCookieSpeed = DefaultCookieSpeed;
    }

    public void AddScore(bool isCheck, int percent)
    {
        if (percent > 0)
        {
            AddScorePercent = percent / 100;
            isAddScoreSensor = isCheck;
        }
        else
            isAddScoreSensor = false;
    }

    void Update()
    {
        if (isGameover || isTuto) return;

        if (isCount)
        {
            ReadyCount();
            return;
        }

        if(isFeverTime)
        {
            //FeverTimeEffect(); //슬로우
            return;
        }
           
        hpbar.value -= timeHurt * Time.deltaTime; //체력 감소
        if(isEffectScore)
            ScoreEffect();

        if (hpbar.value == 0)
            GameOver();
    }
   
    void ReadyCount()
    {
        isStartTimeCheck -= Time.deltaTime;
        Image image = readyCount.GetComponent<Image>();
        if (readyCount != null && image != null)
        {
            if(isStartTimeCheck > 5)
            {
                if(!readyCount.GetCurrentAnimatorStateInfo(0).IsName("Five"))
                {
                    readyCount.SetTrigger("5");
                    image.SetNativeSize();
                }

            }
            else if (isStartTimeCheck > 4)
            {
                if (!readyCount.GetCurrentAnimatorStateInfo(0).IsName("Four"))
                {
                    readyCount.SetTrigger("4");
                    image.SetNativeSize();
                }
            }
            else if (isStartTimeCheck > 3)
            {
                if (!readyCount.GetCurrentAnimatorStateInfo(0).IsName("Three"))
                {
                    readyCount.SetTrigger("3");
                    image.SetNativeSize();
                }
            }
            else if (isStartTimeCheck > 2)
            {
                if (!readyCount.GetCurrentAnimatorStateInfo(0).IsName("Two"))
                {
                    readyCount.SetTrigger("2");
                    image.SetNativeSize();
                }
            }
            else if(isStartTimeCheck > 1)
            {
                if (!readyCount.GetCurrentAnimatorStateInfo(0).IsName("One"))
                {
                    readyCount.SetTrigger("1");
                    image.SetNativeSize();
                }
            }
            else
            {
                if (!readyCount.GetCurrentAnimatorStateInfo(0).IsName("Go!"))
                {
                    readyCount.SetTrigger("0");
                    image.SetNativeSize();
                }
            }
        }
        if (isStartTimeCheck <= 0.6f)
        {
            isCount = false;
            if (readyCount != null)
                readyCount.gameObject.SetActive(false);
        }
    }
    void FeverTimeEffect()
    {
        if (isFeverTimeCheck > feverSlowTime)
        {
            isFeverTimeCheck = 0;
            Time.timeScale = 1;
            isFeverTime = false;
        }
        else if (isFeverTimeCheck < 0.1)
        {
            Time.timeScale = feverSlowValue;

        }
        isFeverTimeCheck += Time.unscaledDeltaTime;
    }
    public void GameOver()
    {
        if (pauseManu != null && pauseManu.activeSelf)
            pauseManu.SetActive(false);
        hpbar.value = 0;
        isGameover = true;
        BGMmanager.instance.PlayOnMusic(BGMmanager.MusicName.bgm_Result);
        
        DefaultCookieSpeed = 0f;
        objectHurt = 0f;
        timeHurt = 0f;
        playerCharacter.Die();
        Debug.Log("종료");
        //SFXmanager.instance.PlayOnGameEnd();
        GameStop();
        //StartCoroutine(GameStopCour(2f));
        //resultStageNum.text = stageNum +".챕터 이름";
    }
    IEnumerator GameStopCour(float wantTime)
    {
        float testTime = 0;
        while(true)
        {
            testTime += Time.deltaTime;
            if (wantTime <= testTime)
            {
                GameStop();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public float GetPetAbilityTime()
    {
        return petAbilityTime;
    }
    public float GetBiggerTime()
    {
        return biggerTime;
    }
    public void SetAddFeverTime(float _addFeverTime)
    {
        characterFevetTime += _addFeverTime;
    }
    public void SetAddFeverScore(float _addFeverScore)
    {
        if (isBonus)
        {
            isFeverBonusScore += _addFeverScore;
        }
        else
        {
            isFeverBonusScore = _addFeverScore;
            isBonus = true;
        }
    }
    public void SetHealValuePercent(float _healPercent = 0)
    {
        healValue = (_healPercent / 100) * healValue;
    }
    public void Healhp(float percent = 0)
    {
        if (percent < 0 && percent > 1)
            hpbar.value += percent;
        else if(percent < 10 && percent > 100)
            hpbar.value += percent * 0.1f;

        hpbar.value += healValue;

        if (hpbar.value >= 1)
            hpbar.value = 1;
    }

    public void Damagedhp(float percent = 0)
    {
        if (percent <= 0)
            hpbar.value -= objectHurt;
        hpbar.value -= percent; //체력 감소
        comboNum = 0;
        comboCountSystem.OnResetCombo();
    }
    public void ResetCoin()
    {
        coinNum = 0;
        if(coinCount != null)
            coinCount.text = coinNum.ToString();
    }
    public void AddCoin(int coin = 1, DataType.JellyType _type = DataType.JellyType.Coin)
    {
        if (feverEffect.isFever)
        {
            if(feverBonusCoinValue > 0)
                coinNum += (int)( (coin / (float)100) * feverBonusCoinValue);
        }
        coinNum += coin;
        if (coinCount != null)
            coinCount.text = coinNum.ToString();
    }
    public void SettingBlackJack(int _value)
    {
        isBlackJack = true;
        blackJackScore = _value;
    }

    public void UpdateScore(int score, DataType.JellyType jellyType)
    {
        int inputScore = score;
        isEffectScore = true;
        comboCountSystem.isEffectCombo = true;
        if (isAddScoreSensor)
        {
            float Temp = (AddScorePercent * score);
            inputScore += (int)Temp;
        }
        if(isBonusSwitch)
        {
            inputScore += (int) (( (float) score) * (isFeverBonusScore * 0.01));
        }

        if(isBlackJack)
        {
            inputScore += blackJackScore;
        }
        gamescore += inputScore;

        //if (addScoreText != null)
        //    addScoreText.SetAddScoreText(inputScore, jellyType);  //획득점수 표기
        //strScore = string.Format("{0:#,##0}", gamescore);
        strScore = string.Format("{0:#,###}", gamescore);
        totalScore.text = strScore;
        Color color = totalScore.color;
        color.a = ScoreTxtAlphaValue;
        totalScore.color = color;
        totalScore.fontSize = ScoreTxtScale;
        comboCountSystem.AddCombo();
       
        comboNum++;
    }


    public void GameStop()
    {
        isGameover = true;
        currentCookieSpeed = 0f;
        Debug.Log("종료!");
        //SFXmanager.instance.PlayOnResult();
        StopAllCoroutines();
        if (skillEffectUI != null)
            skillEffectUI.gameObject.SetActive(false);
        if (gamescore > PlayerPrefs.GetInt("BestScore"))
            PlayerPrefs.SetInt("BestScore", (int)gamescore);

        if(resultOffObject != null)
            resultOffObject.SetActive(false);
        if(endScore != null && totalScore != null)
            endScore.text = totalScore.text;
        int clearLevel = StageClearCheck();
        if (result != null)
            result.SetActive(true);
        GameObject findObj = GameObject.Find("ChapterName");

        if (findObj != null)
        {
            stageName = findObj.GetComponent<TextMeshProUGUI>();
            if (stageName != null)
            {
                stageName.wordSpacing = 0.3f;
                stageName.text = (chapterNum).ToString() + "  -  " + (stageNum + 1).ToString();
            }
        }
        AddReward(clearLevel);
    }

    public void CurrentBoostTimeReset()
    {
        currenBoostTime = 0;
    }

    public void ChangeRigidMass()
    {
        if (playerCharacter.rigidbody != null)
        {
            playerCharacter.rigidbody.mass = CharacterMass;
            playerCharacter.rigidbody.drag = CharacterDrag;
            playerCharacter.rigidbody.gravityScale = CharacterGravity;
            playerCharacter.jumpforce = CharacterJump;
            playerCharacter.doubleJumpForce = CharacterDoubleJump;
        }
    }

    public void OffRigidState()
    {
        playerCharacter.rigidbody.mass = CharacterDfMass;
        playerCharacter.rigidbody.drag = CharacterDfDrag;
        playerCharacter.rigidbody.gravityScale = CharacterDfGravity;
        playerCharacter.jumpforce = CharacterDfJump;
        playerCharacter.doubleJumpForce = CharacterDfDoubleJump;
    }

    public void Onboosteffect()
    {
        if (playerCharacter != null || playerCharacter.mainAnimator != null)
        {
            if(playerCharacter.mainAnimator.GetBool("Boost") != true)
            {
                playerCharacter.mainAnimator.SetBool("Boost", true);
            }
        }
        isBoost = true;
        //isFeverTime = true; //슬로우
        playerCharacter.isreinforce = true;
        if(isBonus)
        {
            isBonusSwitch = true;
        }
        OnBoost();
        StopAllCoroutines();
        StartCoroutine(BoostTime());
    }

    void Offboosteffect()
    {
        if (playerCharacter != null || playerCharacter.mainAnimator != null)
        {
            if (playerCharacter.mainAnimator.GetBool("Boost") != false)
            {
                playerCharacter.mainAnimator.SetBool("Boost", false);
            }
        }
        isBoost = false;
        playerCharacter.isreinforce = false;
        feverEffect.isEffect = false;
        if(isBonus)
        {
            isBonusSwitch = false;
        }
        OffBoost();
        playerCharacter.Offboost();
        StopCoroutine(BoostTime());
        feverEffect.OffState();
    }

    public Vector3 GetTargetjellyPos()
    {
        basicjellies = GameObject.FindGameObjectsWithTag("Basicjelly");
        if (basicjellies.Length == 0) 
            return Vector3.zero;

        targetidx = basicjellies.Length+1;

        for(int i = 0; i < basicjellies.Length; i++)
        {
            if (playerCharacter.transform.position.x + 10f < basicjellies[i].transform.position.x && playerCharacter.transform.position.x + 15f > basicjellies[i].transform.position.x)
                targetidx = i;
        }

        if (basicjellies.Length <= targetidx)
            return Vector3.zero;

        Vector3 targetpos = basicjellies[targetidx].transform.position;
        return targetpos;
    }

    public void OnBoost()
    {
        isFeverState = true;
        currentCookieSpeed = boosterSpeed;
        ChangeRigidMass();
    }
    public void OffBoost()
    {
        isFeverState = false;
        currentCookieSpeed = DefaultCookieSpeed;
        OffRigidState();
    }
    public void OffTargetjelly()
    {
        basicjellies[targetidx].SetActive(false);
    }
    public void PlayerShield()
    {
        playerCharacter.isShield = true;
    }

    IEnumerator BoostTime()
    {
        Vector3 OneVector = new Vector3(1, 1, 1);
        Vector3 CookiePos = new Vector3(0, 1, 0);
        #region
        //for (int i = 0; i < boosterTime; i++)
        //{
        //    if (idx > boosteffect.Length - 1) idx = 0;
        //    boosteffect[idx].SetActive(true);
        //    boosteffect[idx].transform.localScale = OneVector;
        //    //Vector2 Temp = boosteffect[idx].transform.position;
        //    Vector3 Temp = playerCharacter.transform.position + CookiePos;
        //    boosteffect[idx++].transform.position = Temp;
        //    currenBoostTime += 0.1f;
        //    yield return new WaitForSeconds(0.1f);
        //}
        #endregion
        while (true)
        {
            if (currenBoostTime >= boosterTime)
            {
                Offboosteffect();
                CurrentBoostTimeReset();
                yield break;
            }
            currenBoostTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }


    public float Getspeed()
    {
        return currentCookieSpeed;
    }

    public string Getscore()
    {
        return strScore;
    }
    public int StageClearCheck()
    {
        StageDataSetting stageData = UserDataController.Instance.GetStageData().GetStageData(stageNum, chapterNum);
        int clearLevel = 0;
        int currentLevel = stageData.clearLevel;

        if (gamescore > stageData.level.lowLevel)
        {
            if (gamescore > stageData.level.middleLevel)
            {
                if (gamescore > stageData.level.highLevel)
                    clearLevel = 3;
                else
                    clearLevel = 2;
            }
            else
                clearLevel = 1;

            stageData.isClear = true;
        }
        else
        {
            if (!stageData.isClear) //점수도 낮고 클리어 상태가 아니라면
                stageData.isClear = false;
        }

        if (currentLevel < clearLevel)
            stageData.clearLevel = clearLevel;

        return clearLevel;
    }

    public void EndSceneChangerValue()
    {
        if(EndchangScene == DataType.SceneType.login || EndchangScene == DataType.SceneType.END)
            EndchangScene = DataType.SceneType.mode_stage;

        sceneChanger.MoveScene((int)EndchangScene);
    }
    void ScoreEffect()
    {
        if (totalScore != null)
        {
            bool isCheck = true;
            RectTransform txtTransform = totalScore.GetComponent<RectTransform>();
            float time = Time.deltaTime;
            if (txtTransform != null && txtTransform.sizeDelta.x > 1)
            {
                totalScore.fontSize -= ScoreTxtSpeed;
                
                if(totalScore.fontSize <= 40)
                {
                    totalScore.fontSize = 40;
                }
                isCheck = false;
            }
            Color txtColor = totalScore.color;
            if(txtColor.a < 1)
            {
                txtColor.a += ScoreTxtAlphaSpeed * time;
                totalScore.color = txtColor;
                isCheck = false;
            }
            if (isCheck)
            {
                isEffectScore = false;
                totalScore.fontSize = 40;
                totalScore.color = Color.white;
            }
        }
    }
    void AddReward(int _clearLevel)
    {
        StageDataSetting stageInfo = UserDataController.Instance.GetStageData().GetStageData(stageNum, chapterNum); //스테이지 정보를 가져온다.
        bool isClearState = false;
        bool firstClear = false; //최초 클리어인지 저장
        bool newScoreIcon = false; //점수 갱신 상태 저장
        if (stageInfo != null)
            isClearState = stageInfo.isClear; //전 클리어 상태 저장
        int clearLevel = _clearLevel;//클리어 상태 업데이트
        int firstRewardCount = 0;
        RewardEntry firstReward = null;
        RewardEntry[] rewards = new RewardEntry[clearLevel];

        //bestScore = (ulong)PlayerPrefs.GetInt("BestScore");

        if (!isClearState) //클리어 상태가 아니었다면
        {
            isClearState = UserDataController.Instance.GetStageData().GetStageData(stageNum, chapterNum).isClear;//클리어 여부 최신것으로 가져오기

            if (isClearState) //클리어했다면
            {
                firstClear = true; //최초 클리어
                Debug.Log("최초 클리어입니다.");
            }
        }
        if (bestScore <= gamescore) //새로운 스코어 갱신
        {
            newScoreIcon = true;//새로운 점수 갱신
            UserDataController.Instance.GetStageData().SetStageHightScore(stageNum, gamescore, chapterNum);  //최고 점수 갱신
        }

        if (newicon != null)//갱신 축하 이미지가 있다면
            newicon.gameObject.SetActive(newScoreIcon);//뉴스코어 ui 활성화여부 판단

        if (firstClear)
            firstReward = UserDataController.Instance.GetStageData().FirstClearReward(stageNum, chapterNum); //최초 보상 아이템 획득

        if (clearLevel > 0)
        {
            rewards[0] = UserDataController.Instance.GetStageData().CheckClearReward(1, stageNum, chapterNum); //보상 아이템 획득
            if (clearLevel > 1)
            {
                rewards[1] = UserDataController.Instance.GetStageData().CheckClearReward(2, stageNum, chapterNum); //보상 아이템 획득
                if (clearLevel > 2)
                {
                    rewards[2] = UserDataController.Instance.GetStageData().CheckClearReward(3, stageNum, chapterNum); //보상 아이템 획득
                }
            }
        }

        int rewardExp = UserDataController.Instance.GetStageData().GetStageEXP(stageNum, chapterNum, clearLevel); //클리어 여부에 따라 경험치 획득

        RewardEntry coinEntry = new RewardEntry();
        coinEntry.SetData(DataType.RewardType.Gold, coinNum);

        GameManager.instance.AddEXP(rewardExp); //경험치 추가
        GameManager.instance.LevelCheck();//레벨업 체크
        for (int i = 0; i < rewardRectTransform.Length; i++)
        {
            rewardRectTransform[i].gameObject.SetActive(false);
        }

        if (rewards != null)
        {
            if (rewards != null)
                AddReward(rewards);
        }

        if (firstReward != null)
            firstRewardCount = AddFirstReward(firstRewardCount , firstReward);

        StartCoroutine(RewardUI(firstReward, rewards, coinEntry));
    }

    public string GetBestscore()
    {
        StageDataSetting stageInfo = UserDataController.Instance.GetStageData().GetStageData(stageNum, chapterNum); //스테이지 정보를 가져온다.

        if (stageInfo == null)
            //return string.Format("{0:#,##0}", bestScore);
            return string.Format("{0:#,###}", bestScore);

        //return string.Format("{0:#,##0}", bestScore);
        return string.Format("{0:#,###}", bestScore);
    }
    IEnumerator RewardUI(RewardEntry reward, RewardEntry[] rewards, RewardEntry rewardGold)
    {
        yield return new WaitForSeconds(0.05f);

        List<RewardEntry> rewardEntries = new List<RewardEntry>();
        bool isFirstItem = false;
        bool isFirstItemIcon = false;
        if(reward != null)
        {
            if (reward!= null)
            {
                rewardEntries.Add(reward);
                isFirstItem = true;
            }

        }
        if (rewards != null)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                if (rewards != null)
                {
                    rewardEntries.Add(rewards[i]);
                }
            }

        }
        if(rewardGold != null && rewardGold.rewardCount > 0)
        {
            rewardEntries.Add(rewardGold);
        }

        if (rewardRectTransform != null)
        {
            UIPrefabs = new RewardIconEntry[rewardEntries.Count];

            for (int i = 0; i < rewardEntries.Count; i++)
            {
                if (rewardRectTransform[i] == null)
                    yield break;
                rewardRectTransform[i].gameObject.SetActive(true);
                if (isFirstItem)
                {
                    isFirstItem = false;
                    isFirstItemIcon = true;
                }
                else
                    isFirstItemIcon = false;

                UIPrefabs[i] = Instantiate(rewardEntryPrefab, rewardRectTransform[i]);
                UIPrefabs[i].SetData(rewardEntries[i].rewardCount, isFirstItemIcon, rewardEntries[i].rewardType);
                yield return new WaitForSeconds(0.05f);
                //Invoke("AddRewardImage(i, isFirstItem, isFirstItemIcon, rewardEntries[i])", 0.2f);
            }
        }
        yield break;
    }
    int  AddRewardImage(int count ,int i, bool isFirstItem, bool isFirstItemIcon, RewardEntry rewardEntries)
    {
        if (rewardRectTransform[i] == null)
            return count;

        if (i == 0 && isFirstItem)
            isFirstItemIcon = true;
        else
            isFirstItemIcon = false;

        UIPrefabs[i] = Instantiate(rewardEntryPrefab, rewardRectTransform[i]);
        UIPrefabs[i].SetData(rewardEntries.rewardCount, isFirstItemIcon, rewardEntries.rewardType);
        return count;
    }
    public bool GetClearCheck()
    {
        GameStageData stageData = userData.GetStageData();
        if(stageData != null)
        {
            StageDataSetting dataSetting = stageData.GetStageData(stageNum, chapterNum);
            if(dataSetting != null)
            {
                return dataSetting.isClear;
            }
        }
        return false;
    }
    int AddFirstReward(int count ,RewardEntry rewardEntry)
    {
        if (rewardEntry != null)
        {
            if (rewardEntry != null)
            {
                DataType.RewardType rewardType = rewardEntry.rewardType;
                int Count = rewardEntry.rewardCount;
                count++;
                switch (rewardType)
                {
                    case DataType.RewardType.Gold:
                        UserDataController.Instance.Money += Count;
                        break;
                    case DataType.RewardType.Diamond:
                        UserDataController.Instance.Cash += Count;
                        break;
                    default:
                        break;
                }
            }
        }
        return count;
    }

    void AddReward(RewardEntry[] rewardEntry)
    {
        for (int i = 0; i < rewardEntry.Length; i++)
        {
            if (rewardEntry[i] != null)
            {
                DataType.RewardType rewardType = rewardEntry[i].rewardType;
                int Count = rewardEntry[i].rewardCount;
                switch (rewardType)
                {
                    case DataType.RewardType.Gold:
                        UserDataController.Instance.Money += Count;
                        break;
                    case DataType.RewardType.Diamond:
                        UserDataController.Instance.Cash += Count;
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
