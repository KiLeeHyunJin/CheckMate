using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    public static PlayerEffectController instance;
    [SerializeField] GameObject objParent;
    [SerializeField] bool skillOff;
    [Header("Godori")]
    public BirdMagnet bird;
    public float duringtTime;
    public float destroyTime;
    public float magnetMoveXPos = 7;
    public float magnetMoveSpeed = 0.3f;
    public bool isBreakTime = false;

    [Header("HeartQueen")]
    public HeartCard straightFlush;
    public float lenth;
    public float moveSpeedX;
    public float moveSpeedY;
    public bool isStaight = false;
    [SerializeField] int ShootNum;
    [SerializeField] GameObject[] HeartQueenCardDestPos;
    [HideInInspector] public bool isTargetOn = false;
    [Header("HangMan")]
    public float flyTime;
    public bool isFly = false;


    bool isSkillEnd;
    public bool isReset;
    int skillCount;
    BoxCollider2D collider2D;
    //BoxCollider2D collider;
    Vector3 playerPos;
    [SerializeField] InGameManager inGameManager;
    PlayerCharacter player;
    [HideInInspector] public SkillEffectUI skillEffectUI;
    [SerializeField] UserDataController userData;
    [SerializeField] List<GameObject> obstacle = new List<GameObject>();
    HeartCard[] array_staightFlaush;
    [SerializeField] public bool isFever = false;
    public bool isEffect = false;
    [SerializeField] DataType.CharacterType characterType;
    [SerializeField] CharacterBase character;
    [SerializeField] float activeSkillTime;
    [SerializeField] float skillTime;
    public InGameFeverItem feverItem;
    [SerializeField] BaseJelly hearQueenSkillCoinPrefab;
    BaseJelly[] hearQueenSkillCoin;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        if(ShootNum >= HeartQueenCardDestPos.Length)
            ShootNum = HeartQueenCardDestPos.Length - 1;
        array_staightFlaush = new HeartCard[ShootNum];
        for (int i = 0; i < array_staightFlaush.Length; i++)
        {
            array_staightFlaush[i] = Instantiate(straightFlush, transform.position, transform.rotation);
            array_staightFlaush[i].gameObject.SetActive(false);
            array_staightFlaush[i].transform.parent = transform.parent;
        }

        if (hearQueenSkillCoinPrefab != null)
        {
            hearQueenSkillCoin = new BaseJelly[ShootNum];
            for (int i = 0; i < ShootNum; i++)
            {
                hearQueenSkillCoin[i] = Instantiate(hearQueenSkillCoinPrefab, transform.position, transform.rotation);
                hearQueenSkillCoin[i].transform.parent = transform.parent;
                if (objParent != null)
                {
                    hearQueenSkillCoin[i].transform.parent = objParent.transform;
                    //hearQueenSkillCoin[i].transform.parent = transform.parent;
                }

                hearQueenSkillCoin[i].gameObject.SetActive(false);
            }
        }


        if (userData == null)
            userData = UserDataController.Instance;
        playerPos = PlayerCharacter.instance.transform.position;
        if(inGameManager == null)
            inGameManager = InGameManager.instance;
        player = PlayerCharacter.instance;
        isSkillEnd = true;
        skillCount = 0;
        skillTime = 0;
        OffState();
    }
    public void FeverAndFeverReset(bool _isReset) //피버 중 피버 게이지가 모이면 시간을 초기화할지 무시할지 설정한다.
    {
        isReset = _isReset;
        feverItem.isReset = _isReset;
    }
    bool SkillUINullCheck()
    {
        if (skillEffectUI == null)
            return false;
        return true;
    }
    public void SuccessFever()
    {
        isFever = true;
        userData.countData.FeverAdd();

        if (isReset)
        {
            InGameManager.instance.CurrentBoostTimeReset();
        }
    }

    public void Init(DataType.CharacterType _type, CharacterBase _characterBase)
    {
        characterType = _type;
        character = _characterBase;
        if (character == null)
            Debug.Log("character is Null Data _ PlayerFeverEffect.cs 61");
        activeSkillTime = character.SkillAbility;
    }
    private void Update()
    {
        if (inGameManager.isGameover || inGameManager.isCount || inGameManager.isTuto)
            return;

        if (obstacle.Count > 0)
        {
            for (int i = 0; i < obstacle.Count; i++)
            {
                if (obstacle[i].transform.position.x < -15)
                    obstacle.RemoveAt(i);
            }
        }

        if (!isEffect) //이펙트 기술이 실행중이지 않다면
        {
            if (skillTime >= activeSkillTime)//캐릭터 스킬 시간이 되었다면
            {
                if (!(player.isboost))//플레이어가 부스트 중이 아니라면
                {
                    if (isSkillEnd)
                    {
                        if(!skillOff)
                        {
                            //if(isTargetOn) //타겟이 있는지 검사
                            {
                                isEffect = true;
                                SelectCharacterSkill();
                                return;
                            }
                        }
                    }
                }
            }
            if(isFever) //피버 코인이 다 모여있다면
            {
                if(isSkillEnd)//피버가 끝나있다면
                {
                    isEffect = true;
                    StartFever();//부스터
                }
            }
            else
                skillTime += Time.deltaTime; //경과 시간 추가
        }


        if (!player.isboost)
        {
            if(isSkillEnd)
                skillTime += Time.deltaTime;

            if(!skillOff)
            {
                if (skillTime >= activeSkillTime) //스킬 발동 시간
                {
                    //if (isTargetOn)
                    {
                        SelectCharacterSkill();
                    }
                }
            }
        }    

        InGameFeverItem.instance.isFever = false;
    }

    void SelectCharacterSkill()
    {
        skillTime = 0;
        feverItem.feverCount = player.feverCount = skillCount++;
        userData.countData.SkillAdd();

        if (SkillUINullCheck())
        {
            skillEffectUI.SetSkillEffectUI(DataType.EffectUIType.SKill);
        }

        //StartPassiveSkill();
        switch (characterType)
        {
            case DataType.CharacterType.HeartQueen:
                ShootStraight();
                if (skillCount == 3)
                {
                    inGameManager.SetAddFeverScore(character.currentLevel + 2);
                    skillCount = 0;
                }
                break;
            #region
            case DataType.CharacterType.Wolf:
                FlyHangMan();
                break;
            case DataType.CharacterType.Allice:
                ShootBird();
                break;
            case DataType.CharacterType.CapSaller:
                Debug.Log("존재하지 않은 타입입니다. DataType.CharacterType : " + characterType);
                break;
            default:
                Debug.Log("존재하지 않은 타입입니다. DataType.CharacterType : " + characterType);
                break;
                #endregion
        }

        InGameFeverItem.instance.isFever = false;
    }


    void StartFever()
    {
        if(!player.isboost)
        {
            player.isboost = true;
            inGameManager.Onboosteffect();

            //if (skillEffectUI != null)
            //    skillEffectUI.SetSkillEffectUI(DataType.EffectUIType.Fever);
        }
    }

    public void ShootStraight()
    {
        StartCoroutine(HeartQueenStraithFlash());
        StartCoroutine(LateStateOffTime(2));
    }


    public void OffState()
    {
        isEffect = false;
        isFever = false;
        isSkillEnd = true;
        //EndPassiveSkill();
    }

    IEnumerator LateStateOffTime(float _Time)
    {
        float totalTime = 0;
        while(true)
        {
            totalTime += Time.deltaTime;
            if(totalTime >= _Time)
            {
                OffState();
                yield break;
            }
        }
    }


    IEnumerator HeartQueenStraithFlash() //유도 미사일
    {
        int ShootCount = 0;
        int[] PosSave = new int[ShootNum];
        for (int i = 0; i < PosSave.Length; i++)
            PosSave[i] = 99;

        for (int i = 0; i < ShootNum; i++)
        {
            if (i < array_staightFlaush.Length)
            {
                re:
                int arrayNum = Random.Range(0, HeartQueenCardDestPos.Length - 1);
                for (int j = 0; j < ShootCount + 1; j++)
                {
                    if(PosSave[j] == arrayNum)
                    {
                        goto re;
                    }
                }
                PosSave[ShootCount] = arrayNum;
                ShootCount++;
                array_staightFlaush[i].gameObject.SetActive(true);
                array_staightFlaush[i].gameObject.layer = 14;
                array_staightFlaush[i].Setting(player.transform.position, HeartQueenCardDestPos[arrayNum], moveSpeedY, moveSpeedX, i , hearQueenSkillCoin);
            }
            else
                break;
            yield return new WaitForSeconds(0.1f);

        }
        //obstacle = new List<GameObject>();
        //while (obstacle.Count > 0)
        //{
        //    yield return new WaitForSeconds(0.01f);
        //}
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!(InGameManager.instance.isGameover))
        {
            if(collision.gameObject.tag == "Obstacle")
            {
                obstacle.Add(collision.gameObject);
            }
        }
    }



    void StartPassiveSkill() // 패시브 스킬 미구현상태
    {
        switch (characterType)
        {
            case DataType.CharacterType.HeartQueen:
                {
                    if (skillCount == 3)
                    {
                        //skillCount = 0;
                    }
                }
                break;
            #region
            case DataType.CharacterType.Allice:
                {

                }
                break;
            case DataType.CharacterType.Wolf:
                {

                }
                break;
            case DataType.CharacterType.CapSaller:
                Debug.Log("존재하지 않은 타입입니다. DataType.CharacterType : " + characterType);
                break;
            default:
                Debug.Log("존재하지 않은 타입입니다. DataType.CharacterType : " + characterType);
                break;
                #endregion
        }
    }

    void EndPassiveSkill()
    {
        switch (characterType)
        {
            case DataType.CharacterType.HeartQueen:
                break;
            case DataType.CharacterType.CapSaller:
                break;
            case DataType.CharacterType.Wolf:
                break;
            case DataType.CharacterType.Allice:
                Debug.Log("존재하지 않은 타입입니다. DataType.CharacterType : " + characterType);
                break;
            default:
                Debug.Log("존재하지 않은 타입입니다. DataType.CharacterType : " + characterType);
                break;
        }
    }
    public void FlyHangMan()
    {
        StartCoroutine(HangManFly());
    }

    public void ShootBird()
    {
        StartBird();
        bird.isMagnet = true;
        StopCoroutine(Godori_MoveToMagpos());
        StartCoroutine(Godori_MoveToMagpos());
    }

    IEnumerator HangManFly()
    {
        GameObject player = PlayerCharacter.instance.gameObject;
        InGameManager inGameManager = InGameManager.instance;

        float time = 0f;
        float gravity = player.GetComponent<Rigidbody2D>().gravityScale;
        float speed = InGameManager.instance.Getspeed();

        player.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<CircleCollider2D>().enabled = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //player.GetComponent<Rigidbody2D>().gravityScale = 0f;
        inGameManager.OnBoost();
        while (time <= character.SkillAbility) //스킬 능력치 만큼 비행
        {
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }
        inGameManager.OffBoost();
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<BoxCollider2D>().isTrigger = true;
        player.GetComponent<CircleCollider2D>().enabled = true;
        player.GetComponent<Rigidbody2D>().gravityScale = gravity;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        //player.GetComponent<CircleCollider2D>().isTrigger = true;
        //player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;

        OffState();

        yield break;
    }
    void StartBird()
    {
        bird.gameObject.SetActive(true);
        //bird.transform.position = PlayerCharacter.instance.transform.position;
    }
    IEnumerator Godori_MoveToMagpos() //자석 기능
    {
        float time = 0f;
        Vector3 playerCharacter = PlayerCharacter.instance.transform.position;

        while (time <= character.SkillAbility)//InGameManager.instance.magnetTime)
        {
            playerCharacter.y = 0;
            Vector3 distance = playerCharacter - bird.transform.position;
            distance.x += magnetMoveXPos;
            float DestinationXPos = PlayerCharacter.instance.transform.position.x + magnetMoveXPos;
            if (bird.transform.position.x < DestinationXPos)
                bird.transform.position += (distance * Time.deltaTime) * magnetMoveSpeed;
            //transform.position += (new Vector3(magnetMoveSpeed, 0, 0) * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }

        time = 0f;
        //animator.SetBool("Mag", false);
        bird.isMagnet = false;

        while (time < 6f) //(time <= 1f)
        {
            float Dest = PlayerCharacter.instance.transform.position.x - 5.5f;
            Vector3 Direction = (playerCharacter - bird.transform.position).normalized;
            float DestinationXPos = PlayerCharacter.instance.transform.position.x - magnetMoveXPos;
            if (bird.transform.position.x > DestinationXPos)
                bird.transform.position += Direction * magnetMoveSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            Debug.Log("Player : " + PlayerCharacter.instance.transform.position + "Bird : " + bird.transform.position + "Dest : " + Dest + "Direction : " + Direction);
            time += Time.deltaTime;
        }
        InGameManager.instance.ismagatic = false;
        bird.gameObject.SetActive(false);
        gameObject.layer = 14;

        OffState();
        yield break;
    }

    IEnumerator FeverAction()
    {
        yield break;
    }
}
