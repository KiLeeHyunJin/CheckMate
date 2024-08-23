using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetScript : MonoBehaviour
{
    [SerializeField] float magnetMoveXPos = 7f;
    [SerializeField] float magnetMoveSpeed = 0.3f;
    [SerializeField] float magneticRange { get; set; }
    [SerializeField] GameObject jellyParent;
    public static PetScript instance { get; set; }
    public DataType.SensorType SensorType;
    Animator animator;

    GameObject[] gomjellies;
    Vector3 targetpos;
    public GameObject gomjellytPrefab;
    public float abilityMoveSpeed = 0.3f;
    float posx;
    float posy;
    float abilityTime;
    float abilityTimer;
    bool isAbility;

    int gomidx;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        gomjellies = new GameObject[6];
        gomidx = 0;
        isAbility = false;
        for (int i = 0; i < gomjellies.Length; i++)
        {
            gomjellies[i] = Instantiate(gomjellytPrefab);
            gomjellies[i].transform.parent = jellyParent.transform;
            gomjellies[i].SetActive(false);
        }
    }

    void Start()
    {
        if (abilityTime <= 0f)
            abilityTime = 2f;
        if(posx >= 0)
        posx = -1.03f;
        if(posy == 0)
        posy = 2.39f;
        if(magneticRange <= 0)
        magneticRange = 1f;
        animator = GetComponent<Animator>();
        abilityTime = InGameManager.instance.GetPetAbilityTime();
    }

    private void LateUpdate()
    {
        if (!InGameManager.instance.ismagatic && PlayerCharacter.instance != null && !isAbility)//자석 기능 활성화X && 플레이어 존재 && 능력 사용X
            gameObject.transform.position = new Vector3(PlayerCharacter.instance.transform.position.x + posx, PlayerCharacter.instance.transform.position.y + posy, transform.position.z); //위치 = 플레이어 위치 + (posX, PosY)
        if (abilityTimer > abilityTime && !isAbility && !InGameManager.instance.ismagatic)
            OnAbility(); //능력 사용
        else
            abilityTimer += Time.deltaTime;
    }

    public void OnMagneticeffect()
    {
        animator.SetBool("Mag", true);
        gameObject.layer = 8;
        StopCoroutine(MoveToMagpos());
        StartCoroutine(MoveToMagpos());
    }

    void OnAbility()
    {
        isAbility = true;
        if (SensorType == DataType.SensorType.FightDog || SensorType == DataType.SensorType.Gambler)
            StartCoroutine(MoveToAbilitypos()); //능력 코루틴
        else
        {
            abilityTimer = 0f;
            isAbility = false;
        }
    }

    public float getMagneticRange()
    {
        return magneticRange;
    }

    IEnumerator MoveToMagpos() //자석 기능
    {
        float time = 0f;
        while (time <= InGameManager.instance.magnetTime)
        {
            Vector3 playerCharacter = PlayerCharacter.instance.transform.position;
            playerCharacter.y = 0;
            Vector3 distance = playerCharacter - transform.position;
            distance.x += magnetMoveXPos;
            float DestinationXPos = PlayerCharacter.instance.transform.position.x + magnetMoveXPos;
            //transform.position =  Vector3.MoveTowards(transform.position, new Vector3(CooKie.instance.transform.position.x + 6.99f, 0, 0), magnetMoveSpeed);
            if (transform.position.x < DestinationXPos)
                transform.position += (distance * Time.deltaTime) * magnetMoveSpeed;
                //transform.position += (new Vector3(magnetMoveSpeed, 0, 0) * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }
            
        time = 0f;
        animator.SetBool("Mag", false);

        while (time <= 1f)
        {
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(CooKie.instance.transform.position.x + posx, CooKie.instance.transform.position.y + posy, transform.position.z), magnetMoveSpeed);
            if (transform.position.x > PlayerCharacter.instance.transform.position.x + posx)
                transform.position -= (new Vector3(magnetMoveSpeed, 0, 0) * Time.deltaTime);
            else
                transform.position = new Vector3(PlayerCharacter.instance.transform.position.x + posx, PlayerCharacter.instance.transform.position.y + posy, 0f);
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }
        InGameManager.instance.ismagatic = false;
        gameObject.layer = 14;

        yield break;
    }

    IEnumerator MoveToAbilitypos()
    {
        //float time = 0f;
        targetpos = TargetPos();
        if (targetpos == Vector3.zero)
        {
            isAbility = false;
            abilityTimer = 0;
            yield break;
        }
        //while (time <= abilityMoveSpeed)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, targetpos, 1.0f); //변경시킬 젤리 위치로 이동
        //    yield return new WaitForSeconds(0.01f);
        //    time += Time.deltaTime;
        //}
        TargetOff();
        //gomjellies[gomidx].SetActive(true); //곰젤리 활성화
        //gomjellies[gomidx].gameObject.GetComponent<Animator>().SetTrigger("Born");
        //gomjellies[gomidx++].transform.position = targetpos; //곰젤리를 젤리 위치로 이동
        //if (gomidx >= gomjellies.Length) gomidx = 0;
        //time = 0f;
        //while (time <= abilityMoveSpeed)
        //{
        //    transform.position = //위치 제자리로 이동
        //        Vector3.MoveTowards(transform.position, new Vector3(PlayerCharacter.instance.transform.position.x + posx, PlayerCharacter.instance.transform.position.y + posy, transform.position.z), 1.0f);
        //    yield return new WaitForSeconds(0.01f);
        //    time += Time.deltaTime;
        //}
        isAbility = false;
        abilityTimer = 0;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!InGameManager.instance.isGameover)
        {
            #region OffItem
            //if (collision.gameObject.tag == "Boost")
            //{
            //    PlayerCharacter.instance.isreinforce = true;
            //    animator.SetBool("Boost", true);
            //    PlayerCharacter.instance.isboost = true;
            //    InGameManager.instance.Onboosteffect();
            //    SFXmanager.instance.PlayOnGetItemJelly();
            //}
            //if (collision.gameObject.tag == "Bigger")
            //{
            //    PlayerCharacter.instance.isreinforce = true;
            //    PlayerCharacter.instance.OnBigger();
            //    PlayerCharacter.instance.isbig = true;
            //}
            //if (collision.gameObject.tag == "Magnetic")
            //{
            //    InGameManager.instance.ismagatic = true;
            //    OnMagneticeffect();
            //}
            #endregion OffItem
            if (collision.gameObject.tag == "Potion")
            {
                InGameManager.instance.Healhp();
                SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Jelly);
            }
        }
    }

    Vector3 TargetPos()
    {
        if (SensorType == DataType.SensorType.FightDog || SensorType == DataType.SensorType.UnfairPlay)
            return InGameManager.instance.GetTargetjellyPos();
        else
            return Vector3.zero;

        //switch (SensorType)
        //{
        //    case DataType.SensorType.FightDog:
        //        return InGameManager.instance.GetTargetjellyPos();
        //        //return InGameManager.instance.GetTargetObstacle();
        //    case DataType.SensorType.Bluffing:
        //        return InGameManager.instance.GetTargetjellyPos();
        //    default:
        //        break;
        //}
    }

    void TargetOff()
    {
        if(SensorType == DataType.SensorType.FightDog || SensorType == DataType.SensorType.UnfairPlay)
            InGameManager.instance.OffTargetjelly();
    }
}