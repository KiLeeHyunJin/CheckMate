
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//점프중 collision trigger 켜기
//타일에 끼일때 밀어내기
//

public class PlayerCharacter : MonoBehaviour
{
    [Header("FeverState")]
    [SerializeField] float CharacterMass;
    [SerializeField] float CharacterJump;
    [SerializeField] float CharacterDrag;
    [SerializeField] float CharacterGravity;
    [SerializeField] float CharacterDoubleJump;

    [Header("DefaultState")]
    [SerializeField] float CharacterDfMass;
    [SerializeField] float CharacterDfJump;
    [SerializeField] float CharacterDfDrag;
    [SerializeField] float CharacterDfGravity;
    [SerializeField] float CharacterDfDoubleJump;

    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] int numDigit;
    float jumpTimer = 0.17f;
    [SerializeField] float MonitorjumpTrigger;
    [Header("Character")]
    [SerializeField] Animator HeartQueen;
    [SerializeField] Animator Godori;
    [SerializeField] Animator HangMan;
    [SerializeField] Animator Sample;

    [SerializeField] PlayerEffectController characterSkill;
    public static PlayerCharacter instance;
    [SerializeField] UserDataController userData;
    [SerializeField] InGameFeverItem feverItem;
    [SerializeField] float hurtMotionTime = 0.4f;
    [SerializeField] readonly float invincibilityTime = 1f;
    [SerializeField] float groundCheckLenth;
    [SerializeField] public int feverCount;
    [SerializeField] bool isFeverAndFever;
    //[SpineAnimation]
    //public string RunAnimation;
    ////[SpineAnimation]
    //public string JumpAnimation;
    ////[SpineAnimation]
    //public string SlideAnimation;
    ////[SpineAnimation]
    //public string LandAnimation;
    ////[SpineAnimation]
    //public string DoubleJumpAnimation;
    ////[SpineAnimation]
    //public string HurtAnimation;
    //public string IdleAnimation;

    public bool isboost;
    public bool isbig;
    public bool isreinforce = false;
    public bool isSlide = false;
    public float objectHurt { get; set; } // 장애물 퍼센트
    public float timeHurt { get; set; } // 시간 퍼센트
    public float potionHealValue { get; set; } // 회복량 퍼센트
    public float addBoostValue { get; set; } 
    public float itemValue { get; set; } //아이템 퍼센트
    public bool isShield { get; set ;}
    public int jump { get; set; }
    public bool isHangManFever { get; set; }

    private bool isGround = true;
    private bool isHurt = false;
    private bool isinvincibility = false;
    private bool isLand = false;
    private bool scaleNormal;
    private bool isJump = false;
    private float hangManAddFeverTime = 0f;
    private float landTime = 0f;
    [HideInInspector] public float jumpforce = 460.0f;
    [HideInInspector] public float doubleJumpForce;
    private float biggerTime;
    private float biggerduringTime;
    private float hurtTime = 0f;
    CapsuleCollider2D capsuleCollider;
    //private BoxCollider2D boxCollider2D;
    //CircleCollider2D circleCollider;
    CharacterBase character;
    [SerializeField] DataType.CharacterType characterType;
    [SerializeField] InGameSound gameSound;
    [HideInInspector] public Animator mainAnimator;
    MeshRenderer mainSkeletonMat;
    InGameManager inGameManager;
    [HideInInspector] public Rigidbody2D rigidbody;
    int beforeYpos;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        rigidbody = GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        if (numDigit < 10)
            numDigit = 1000;
        beforeYpos = (int)transform.position.y * numDigit;
        CharacterEntry Temp = null;
        isbig = false;
        isboost = false;
        feverCount = 0;
        inGameManager = InGameManager.instance;
        if(feverItem == null)
        {
            feverItem = InGameFeverItem.instance;
        }
        if(characterSkill == null)
        {
            characterSkill = PlayerEffectController.instance;
        }
        if (gameSound == null)
            gameSound = InGameSound.instance;
        characterSkill.FeverAndFeverReset(isFeverAndFever);

        //boxCollider2D = GetComponent<BoxCollider2D>();
        //circleCollider = GetComponent<CircleCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //mainAnimator = GetComponent<Animator>();
        int Dest = GameManager.instance.GetPlayCharacterNum(); //달릴 캐릭터 번호(인벤토리 내 번호)

        if (Dest == 99) //번호가 존재하지않으면
        {
            Temp = userData.characterSystem.Entries[0];//맨 첫번째 캐릭터를 가져온다.
            if (Temp == null) //첫번째 캐릭터도 없다면
            {
                userData.characterSystem.AddCharacter(userData.gameData.gameItemDataBase.GetCharacter(0));//캐릭터 지급
                Temp = userData.characterSystem.Entries[0];//다시 첫번째 캐릭터를 가져온다.
                Debug.Log("캐릭터의 index를 가져오는것에 실패하였습니다. 캐릭터를 지급하였습니다. indext : " + Dest);
                //isSample = true;
            }
            else
                Debug.Log("캐릭터의 index를 가져오는것에 실패하였습니다. indext : " + Dest);
        }
        else//번호가 존재한다면
        {
            Temp = userData.characterSystem.Entries[Dest];//해당번호 캐릭터를 불러온다.
            Debug.Log("캐릭터의 index : " + Dest);
            if (Temp == null)//캐릭터가 없다면
            {
                userData.characterSystem.AddCharacter(userData.gameData.gameItemDataBase.GetCharacter(0));//캐릭터 지급
                Temp = userData.characterSystem.Entries[0];//다시 첫번째 캐릭터를 가져온다.
                Debug.Log("캐릭터의 index를 가져오는것에 실패하였습니다. 캐릭터를 지급하였습니다. indext : " + Dest);
                //isSample = true;
            }
        }
        if(Temp != null)
            character = Temp.Character;//캐릭터 베이스를 가져온다.

        //if(character != null)
        //    character.CharacterType = DataType.CharacterType.HeartQueen;
        characterType = character.CharacterType;//캐릭터타입을 따로 담는다.
        characterSkill.Init(character.CharacterType, character);//캐릭터의 정보에 맞춰서 초기화한다.
        characterOnOff(characterType);

        //skeletonAnimation.OnRebuild += Apply;


        #region
        //if (mainSkeletonAnimation !=null)
        //{
        //   mainSkeletonAnimation.timeScale = 1.4f;
        //    HeartQueen.gameObject.SetActive(true);
        //    mainSkeletonAnimation = HeartQueen;
        //    MeshRenderer renderer = HeartQueen.gameObject.GetComponent<MeshRenderer>();
        //    if(renderer != null)
        //        mainSkeletonMat.material = renderer.material;
        //    //HeartQueen.gameObject.SetActive(false);
        //}
        //if (character == null)
        //{
        //    mainSkeletonAnimation.timeScale = 1.4f;
        //    Sample.gameObject.SetActive(true);
        //    mainSkeletonAnimation = Sample;
        //    MeshRenderer renderer = Sample.gameObject.GetComponent<MeshRenderer>();
        //}
        //else
        //{
        //    characterOnOff(characterType);
        //}
        #endregion

        if (timeHurt <= 0) timeHurt = 0.02f;
        {
            if (potionHealValue <= 0) potionHealValue = 0.1f;
            {
                inGameManager.PlayerCharacterInit(objectHurt, timeHurt, character.fevetTime, itemValue);
            }
        }
        if (characterType == DataType.CharacterType.HeartQueen)
            inGameManager.feverBonusCoinValue = 5;  //패시브
        inGameManager.SetHealValuePercent(potionHealValue);
        jump = inGameManager.jump;
        biggerTime = inGameManager.GetBiggerTime();
        if (CharacterDfJump != 0)
            jumpforce = CharacterDfJump;
    }

    void Update()
    {
        if (inGameManager.isCount)
        {
            if (mainAnimator.GetBool("Init"))
            {
                return;
            }
            mainAnimator.SetBool("Init", true);
            //scaleNormal = false;
        }
        else if(mainAnimator.GetBool("Init"))
        {
            mainAnimator.SetBool("Init", false);
        }

        if (inGameManager.isGameover)
        {
            //DieAnimation();
            return;
        }

        //if(!scaleNormal)
        //{
        //    rigidbody.gravityScale = 3;
        //    scaleNormal = true;
        //}

        //characterOnOff(characterType);
        //gameObject.transform.Translate(Vector3.right * inGameManager.Getspeed() * Time.deltaTime);


        if(!isJump)
        {
            Vector3 currentPos = transform.position;
            currentPos.y += 0.8f;
            bool isCheck = Physics.Raycast(currentPos, Vector3.down, 0.19f, LayerMask.GetMask("Ground"));
            if (isCheck)
            {
                currentPos = transform.position;
                currentPos.y += 0.8f;
                transform.position = currentPos;
            }
        }

        if (!isGround)
            isGround = Physics.Raycast(transform.position, Vector3.down, groundCheckLenth, LayerMask.GetMask("Ground"));


        if (isbig)
        {
            biggerduringTime += Time.deltaTime;
            if (biggerduringTime >= biggerTime)
                Offbigger();
        }

        if (isHangManFever)
            hangManAddFeverTime += Time.deltaTime;
        else
            hangManAddFeverTime = .0f;

        if(isHurt)
            OnHurt();

        InComputerControll();
    }
    void InComputerControll()
    {
        if (!isSlide)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                OnClickJumpButton();
        }
        bool isTrue = false;
        if(tutorialManager != null)
        {
            isTrue = tutorialManager.isSlide;
        }
        if (isTrue || (!isJump && isGround))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                DownSlideButton();

            else if ((Input.GetKeyUp(KeyCode.LeftShift)))
                UpSlideButton();
        }
    }
    private void LateUpdate()
    {
        if (inGameManager.isGameover)
        {
            return;
        }
        //UpdateBeforeYPos();
    }

    void DieAnimation()
    {
        if (mainAnimator != null)
        {
            if (mainAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                //mainAnimator.SetFloat("DieSpeed", 0);
            }
        }
    }

    public void Die()
    {
        if(mainAnimator != null)
            mainAnimator.SetTrigger("Die");
        rigidbody.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {

            InGameSound.instance.JumpCountReset();
            jump = 2;
            isGround = true;
            isJump = false;
            if (!isSlide)
            {
                if (mainAnimator != null)
                {
                    mainAnimator.SetBool("Jump", false);
                    mainAnimator.SetBool("Jump2", false);
                    mainAnimator.SetBool("Grounded", true);
                }
            }
        }
    }
    public void OnBoostAnim()
    {
        mainAnimator.SetBool("Boost", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!inGameManager.isGameover)
        {
            CollisionInteractItem(collision);
            if(!isinvincibility)
            {
                CollisionDamageObject(collision);
            }
        }
    }
    public void Offboost()
    {
        isboost = false;
        if (!isbig && !isboost)
            isreinforce = false;

        if (mainAnimator != null)
            mainAnimator.SetBool("Boost", false);
        StopCoroutine(Invincibilityeffect());
        StartCoroutine(Invincibilityeffect());


    }


    void OnBigger()
    {
        StopCoroutine(Biggerscale());
        StartCoroutine(Biggerscale());
        isreinforce = true;
    }

    void Offbigger()
    {
        isbig = false;
        StopCoroutine(Smallerscale());
        StartCoroutine(Smallerscale());
        StopCoroutine(Invincibilityeffect());
        StartCoroutine(Invincibilityeffect());
    }

    IEnumerator Biggerscale()
    {
        float time = 0f;
        biggerduringTime = 0f;
        //SFXmanager.instance.PlayOnGiganticStart();
        while (time <= 1f)
        {
            yield return new WaitForSeconds(0.001f);
            time += Time.deltaTime * 2f;
            gameObject.transform.localScale = new Vector3(2f + time, 2f + time, 1f);
        }
        gameObject.transform.localScale = new Vector3(3f, 3f, 1f);
        isbig = true;
    }

    IEnumerator Smallerscale()
    {
        float time = 0f;
        //SFXmanager.instance.PlayOnGiganticEnd();
        while (time <= 1f)
        {
            yield return new WaitForSeconds(0.001f);
            time += Time.deltaTime * 2f;
            gameObject.transform.localScale = new Vector3(3f - time, 3f - time, 1f);
        }
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }


    IEnumerator Invincibilityeffect()
    {
        if (isbig || isboost) yield break;
        gameObject.layer = 11;
        if (!isbig && !isboost)
            isreinforce = false;
        if (true)//isShield)
        {
            isHurt = true;
        }
        for (int i = 0; i < 5; i++)
        {
            if(GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                yield return new WaitForSeconds(0.3f);
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                yield return new WaitForSeconds(0.3f);
            }
            //else if(GetComponent<MeshRenderer>().material.mainTexture.)
        }
        gameObject.layer = 8;
        yield break;
    }
    void CollisionInteractItem(Collider2D collision)
    {
        #region OffItem
        //if (collision.gameObject.tag == "Boost")
        //{
        //    isreinforce = true;
        //    if (animator != null)
        //        animator.SetBool("Boost", true);
        //    else if (skeletonAnimation != null)
        //        skeletonAnimation.AnimationName = RunAnimation;
        //    isboost = true;
        //    inGameManager.Onboosteffect();
        //    SFXmanager.instance.PlayOnGetItemJelly();
        //}
        //if (collision.gameObject.tag == "Bigger")
        //{
        //    OnBigger();
        //}
        //if (collision.gameObject.tag == "Magnetic")
        //{
        //    inGameManager.ismagatic = true;
        //    if(PetScript.instance != null)
        //        PetScript.instance.OnMagneticeffect();
        //}
        #endregion OffItem

        if (collision.gameObject.tag == "Potion")
        {
            inGameManager.Healhp(15);
            SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Swallow);
        }
        if(collision.gameObject.tag == "SmallPotion")
        {
            inGameManager.Healhp(2);
            SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Swallow);
        }
        if (collision.gameObject.tag == "FeverItem")
        {
            DataType.FeverCoinType feverCoinType = collision.GetComponent<FeverCoinData>().FeverCoinType;
            feverItem.AddFeverItem(feverCoinType);
        }
    }
    void CollisionDamageObject(Collider2D collision)
    {

        bool coll = false;
        if (collision.gameObject.tag == "FireBall" && !isreinforce)
        {
            coll = true;
        }
        if (collision.gameObject.tag == "Obstacle" && !isreinforce && !coll)
        {
            Debug.Log("충돌체 : " + collision.gameObject.name);
            coll = true;
        }
        if(coll)
        {
            if (!isShield)
            {
                hurtTime = 0f;
                isHurt = true;
                DamagedUI.instance.Ondamaged();
                if (mainAnimator != null)
                    mainAnimator.SetTrigger("Hurt");
                //inGameManager.Damagedhp(collision.GetComponent<FireBall>().m_Damage);
                inGameManager.Damagedhp(objectHurt);
            }
            else
                isShield = false;

            StopCoroutine(Invincibilityeffect());
            StartCoroutine(Invincibilityeffect());
        }
    }

    void UpdateBeforeYPos()
    {
        int currentYPos = (int)(transform.position.y * numDigit);
        if (beforeYpos < currentYPos)
        {
            //if(!boxCollider2D.isTrigger)
            //    boxCollider2D.isTrigger = true;
            //if(!circleCollider.isTrigger)
            //    circleCollider.isTrigger = true;
        }
        else
        {
            //if (boxCollider2D.isTrigger)
            //    boxCollider2D.isTrigger = false;
            //if (circleCollider.isTrigger)
            //    circleCollider.isTrigger = false;
        }
        beforeYpos = currentYPos;
    }


    public void DownSlideButton()
    {
        if (tutorialManager != null && tutorialManager.isSlide)
        {
            tutorialManager.DisSlideInfo(); 
            if (mainAnimator != null)
            {
                mainAnimator.SetBool("Jump", false);
                mainAnimator.SetBool("Jump2", false);
                mainAnimator.SetBool("Grounded", true);
            }
            isJump = false;
        }

        if (!isJump)
        {
            if (mainAnimator != null)
                if (mainAnimator.GetBool("Slide") != true)
                {
                    //Vector3 Temp = mainAnimator.gameObject.transform.position;
                    //Temp.y -= 0.65f;
                    //mainAnimator.gameObject.transform.position = Temp;
                    if (gameSound != null)
                        gameSound.PlayOnSlideclip();
                    mainAnimator.SetBool("Slide", true);
                }

            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Vector2 collSize = capsuleCollider.size;
            Vector2 collPos = capsuleCollider.offset;

            collPos.y = 0.75f;
            collSize.y = 1.5f;

            capsuleCollider.offset = collPos;
            capsuleCollider.size = collSize;

            isSlide = true;
            userData.countData.SlideAdd();
        }
    }

    public void UpSlideButton()
    {
        if (mainAnimator != null)
            if (mainAnimator.GetBool("Slide") != false)
            {
                //Vector3 Temp = mainAnimator.gameObject.transform.position;
                //Temp.y += 0.65f;
                //mainAnimator.gameObject.transform.position = Temp;

                mainAnimator.SetBool("Slide", false);
            }

        //gameObject.GetComponent<BoxCollider2D>().enabled = true;
        Vector2 collSize = capsuleCollider.size;
        Vector2 collPos = capsuleCollider.offset;

        collPos.y = 1.5f;
        collSize.y = 3;

        capsuleCollider.offset = collPos;
        capsuleCollider.size = collSize;

        isSlide = false;
    }

    public void OnClickJumpButton()
    {
        if(tutorialManager != null)
        {
            if (tutorialManager.isJump)
                tutorialManager.DisJumpInfo();
            else if (tutorialManager.isDoubleJump)
                tutorialManager.DisDoubleJumpInfo();
        }


        if (isGround && !inGameManager.isGameover && !isSlide)
        {
            isGround = false;

            if (mainAnimator != null)
            {
                if (gameSound != null)
                    gameSound.PlayOnJumpclip();
                mainAnimator.SetBool("Grounded", false);
                mainAnimator.SetBool("Jump", true);
                isJump = true;
                StartCoroutine(JumpTrigger());
            }
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(new Vector2(0, jumpforce));
            jump--;
            userData.countData.JumpAdd();
        }
        else if (!isGround && jump == 1 && !inGameManager.isGameover)
        {
            if (mainAnimator != null)
            {
                gameSound.PlayOnJumpclip();
                mainAnimator.SetBool("Jump", false);
                mainAnimator.SetBool("Jump2", true);
                isJump = true;
                if (MonitorjumpTrigger <= 0)
                {
                    StartCoroutine(JumpTrigger());
                }
                else
                {
                    MonitorjumpTrigger += jumpTimer;
                }
                    
            }
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(new Vector2(0, doubleJumpForce));
            jump--;
            userData.countData.DoubleJumpAdd();
        }
    }

    IEnumerator JumpTrigger()
    {
        //boxCollider2D.isTrigger = true;
        //circleCollider.isTrigger = true;
        capsuleCollider.isTrigger = true;
        MonitorjumpTrigger += jumpTimer;

        while (true)
        {
            MonitorjumpTrigger -= Time.deltaTime;
            if (MonitorjumpTrigger <= 0)
            {
                //boxCollider2D.isTrigger = false;
                //circleCollider.isTrigger = false;
                capsuleCollider.isTrigger = false;

                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    void OnHurt()
    {
        hurtTime += Time.deltaTime;

        if (hurtTime < invincibilityTime)
        {
            isinvincibility = true;
        }
        else if (hurtTime >= invincibilityTime)
        {
            hurtTime = 0f;
            isinvincibility = false;
            isHurt = false;
        }

        if (hurtTime > hurtMotionTime)
        {
            if(mainAnimator != null)
            {
                mainAnimator.SetBool("Init",false);
            }
        }
    }

    void characterOnOff(DataType.CharacterType characterType)
    {
        //Animator TempAnimation = null;
        switch (characterType)
        {
            case DataType.CharacterType.HeartQueen:
                mainAnimator = HeartQueen;
                break;
            case DataType.CharacterType.CapSaller:
                mainAnimator = HangMan;
                break;
            case DataType.CharacterType.Allice:
                mainAnimator = Godori;
                break;
            case DataType.CharacterType.Wolf:
                mainAnimator = Sample;
                break;
            default:
                Debug.Log("Fata Error! : PlayerIdle.cs _ 55Line");
                break;
        }
        if (mainAnimator != null)
        {
            mainAnimator.gameObject.SetActive(true);
            //MeshRenderer renderer = skeletonAnimation.gameObject.GetComponent<MeshRenderer>();
        }
        else
        {
            Sample.gameObject.SetActive(true);
            mainAnimator = Sample;
            //MeshRenderer renderer = Sample.gameObject.GetComponent<MeshRenderer>();
        }
    }

    void SetAnimation()
    {
        //RunAnimation = "run";
        //JumpAnimation = "jump";
        //SlideAnimation = "slide";
        ////LandAnimation = ;
        //DoubleJumpAnimation = "dubble jump";
        //HurtAnimation = ;
    }

    public bool Getinvincibility()
    {
        return isinvincibility;
    }

    void OnDrawGizmos() //레이캐스트의 거리를 시각적으로 확인하기 위한 함수.
    {
        Gizmos.color = Color.blue; //빨간색으로 셋팅
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckLenth); //현재 위치 , 아랫방향으로 1f만큼 직선
        
        Gizmos.color = Color.red; //빨간색으로 셋팅
        Vector3 currentPos = transform.position;
        currentPos.y += 0.8f;
        Gizmos.DrawLine(currentPos, currentPos + Vector3.down * 0.19f); //현재 위치 // 0.8f 올려야됨
    }
}
