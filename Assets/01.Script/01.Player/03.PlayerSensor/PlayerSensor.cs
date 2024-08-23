using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSensor : MonoBehaviour
{
    public static PlayerSensor Instance;
    [SerializeField] float AlphaValue;
    [SerializeField] float flickrSpeed;

    public GameObject sensorFrame;
    public Image[] sensorImage;
    public Image[] sensorPercent;
    public GameObject creatCoin;
    public GameObject PopCoin;
    [HideInInspector] public SkillEffectUI skillEffectUI;

    [SerializeField] GameObject parentObject;
    [SerializeField] SensorEffect[] monitor;

    GameObject[] changeItem_Copy;
    GameObject[] spreadItem;
    GameObject[] spreadItem2;
    GameObject[][] spreadItem_Copy = new GameObject[2][];
    UserDataController userData;
    Vector3 targetpos;

    List<GameObject> targetObject = new List<GameObject>();
    List<GameObject> fighterDog = new List<GameObject>();
    List<GameObject> unfairPlay = new List<GameObject>();
    List<int> sensorNum = new List<int>();
    List<SensorEffect> sensors = new List<SensorEffect>();
    List<bool> sensorsEffect = new List<bool>();
    public InGameManager inGameManager;
    CircleCollider2D collider2D;
    Rigidbody2D rigidbody2;
    [SerializeField] GameObject startObj;
    [SerializeField] GameObject endObj;
    [SerializeField] Color flickerColor;
    [SerializeField] GameObject OffSensorPopUp;
    [SerializeField] float PopPower;
    float sensorFlickerSpeed;
    float startXPos;
    float endXPos;
    int targetIdx;
    int changeIdx;
    int creatCoinValue;
    int gamblerNum;
    bool isSpreadNum;
    bool isPlus;
    bool isHaveEver;
    bool isNotSensorMake = true;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(this);

        changeItem_Copy = new GameObject[10];
        spreadItem = new GameObject[6];
        spreadItem2 = new GameObject[6];
        changeIdx = 0;

        if(changeItem_Copy != null)
        {
            for (int i = 0; i < changeItem_Copy.Length; i++)
            {
                //if (changeItem_Copy[i] == null)
                //    continue;
                changeItem_Copy[i] = Instantiate(PopCoin);
                changeItem_Copy[i].transform.parent = parentObject.transform;
                changeItem_Copy[i].SetActive(false);
                PopCoin temp = changeItem_Copy[i].GetComponent<PopCoin>();
                if (temp != null)
                if(spreadItem.Length > i)
                {
                    spreadItem[i] = Instantiate(creatCoin);
                    spreadItem[i].transform.parent = parentObject.transform;
                    spreadItem[i].SetActive(false); 

                    spreadItem2[i] = Instantiate(creatCoin);
                    spreadItem2[i].transform.parent = parentObject.transform;
                    spreadItem2[i].SetActive(false);
                }
            }
        }
        spreadItem_Copy[0] = spreadItem;
        spreadItem_Copy[1] = spreadItem2;

        if (OffSensorPopUp != null)
            OffSensorPopUp.SetActive(false);
    }

    void Start()
    {
        userData = FindObjectOfType<UserDataController>();
        inGameManager = FindObjectOfType<InGameManager>();

        monitor = new SensorEffect[sensorNum.Count];

        collider2D = GetComponent<CircleCollider2D>();
        rigidbody2 = GetComponent<Rigidbody2D>();

        startXPos = startObj.transform.position.x;
        endXPos = endObj.transform.position.x;
        startObj.SetActive(false);
        endObj.SetActive(false);

        //List<int> Temp = GameManager.instance.GetSensorNum();
        gamblerNum = 0;
        int[] sensorArray = GameManager.instance.GetSensorArray();

        for (int i = 0; i < sensorArray.Length; i++)
        {
            if (sensorArray[i] == 99)
                continue;
            else
                sensorNum.Add(sensorArray[i]);
        }
        skillEffectUI = inGameManager.GetSkillEffectUI();

        if(sensorNum.Count == 0)
        {
            isNotSensorMake = false;
            sensorFrame.SetActive(false);
        }
        else
        {
            isNotSensorMake = true;
            SensorMakeFx();
        }
        //flickerColor = Color.white;

        if (flickrSpeed <= 0)
            flickrSpeed = 1.5f;
    }
    void SensorMakeFx()
    {
        for (int i = 0; i < sensorNum.Count; i++)
        {
            int sensorNum_Temp = sensorNum[i];
            if (userData.sensorSystem.Entries.Length >= sensorNum_Temp)
            {
                SensorEntry sensorEntry_Temp = userData.sensorSystem.Entries[sensorNum_Temp];
                if (sensorEntry_Temp != null)
                {
                    if (!sensorFrame.gameObject.activeSelf)
                        sensorFrame.SetActive(true);
                    if (isNotSensorMake == true)
                        isNotSensorMake = false;

                    SensorEffect makeSensor = new SensorEffect();
                    makeSensor.Init(sensorEntry_Temp.sensor, this, i);
                    sensors.Add(makeSensor);
                    sensorsEffect.Add(false);

                    if (monitor.Length > i)
                        monitor[i] = sensors[i];

                    SensorEntrySlogActive(i);

                    sensorImage[i].sprite = sensorEntry_Temp.sensor.InGameSensorSprite;
                    sensorPercent[i].sprite = sensorEntry_Temp.sensor.InGameSensorSprite;
                    //ChargeSensorTime(i);
                }
                else
                {
                    Debug.Log(i + "번째 PlayerSensor의 값이 null입니다.");
                    if(isNotSensorMake == false)
                        isNotSensorMake = true;

                    sensorImage[i].gameObject.SetActive(false);
                    sensorPercent[i].gameObject.SetActive(false);
                }
            }
            else
                Debug.Log("sensorNum_Temp의 번호가 sensorSystem.Entries의 범위를 넘었습니다. sensorSystem.Entries : " + userData.sensorSystem.Entries.Length + ",  sensorNum_Temp : " + sensorNum_Temp);
        }
        if (isNotSensorMake == false) //제작이되었다면 
        {
            EverSensorSkill();
        }
        if(sensorNum.Count == 0)
        {
            sensorFrame.SetActive(false);
        }
    }

    void SensorEntrySlogActive(int _idx)
    {
        if (!sensorImage[_idx].gameObject.activeSelf)
        {
            sensorImage[_idx].gameObject.SetActive(true);
        }
        if (!sensorPercent[_idx].gameObject.activeSelf)
        {
            sensorPercent[_idx].gameObject.SetActive(true);
        }
    }

    void SensorReCheck()
    {
        for (int i = 0; i < sensorNum.Count; i++)
        {
            int sensorNum_Temp = sensorNum[i];
            if (userData.sensorSystem.Entries.Length >= sensorNum_Temp)
            {
                SensorEntry sensorEntry_Temp = userData.sensorSystem.Entries[sensorNum_Temp];
                if (sensorEntry_Temp != null)
                {
                    if(!sensorFrame.gameObject.activeSelf)
                        sensorFrame.SetActive(true);    
                    if (isNotSensorMake == true)
                        isNotSensorMake = false;

                    SensorEffect makeSensor = new SensorEffect();
                    makeSensor.Init(sensorEntry_Temp.sensor, this, i);
                    sensors.Add(makeSensor);
                    sensorsEffect.Add(false);
                    if (monitor.Length > i)
                        monitor[i] = sensors[i];
                    sensorImage[i].gameObject.SetActive(true);
                    sensorPercent[i].gameObject.SetActive(true);
                    sensorImage[i].sprite = sensorEntry_Temp.sensor.InGameSensorSprite;
                    sensorPercent[i].sprite = sensorEntry_Temp.sensor.InGameSensorSprite;
                    //ChargeSensorTime(i);
                }
                else
                {
                    isNotSensorMake = true;
                }
            }
        }
        if (isNotSensorMake == false)
            EverSensorSkill();
    }
    public void CompleteSensor(int _index)
    {
        if(sensorsEffect.Count > _index)
            sensorsEffect[_index] = true;
    }
    public bool CompleteCheckSensor(int _index)
    {
        if (sensorsEffect.Count > _index)
            return sensorsEffect[_index];
        else
            return false;
    }
    void EverSensorSkill()
    {
        for (int i = 0; i < sensors.Count; i++)
        {
            if (sensors[i] == null || sensors[i].sensor == null)
                continue;
            switch (sensors[i].sensor.sensorType)
            {
                case DataType.SensorType.Bonus:
                    AddFeverScore(i);
                    isHaveEver = true;
                    break;
                case DataType.SensorType.BlackJack:
                    UpScore(i);
                    isHaveEver = true;
                    break;
                case DataType.SensorType.Gambler:
                    creatCoinValue = (int)sensors[i].sensor.percentScore;
                    break;
                default:
                    sensorPercent[i].color = Color.white;
                    sensorImage[i].color = Color.gray;
                    break;
            }
        }
    }
    void AddFeverScore(int _idx)
    {
        float addFever = sensors[_idx].sensor.percentScore;
        inGameManager.SetAddFeverScore(addFever);
    }
    void UpScore(int _idx) //블랙잭
    {
        int value = (int)sensors[_idx].sensor.percentScore;
        if (value < 0)
            inGameManager.SettingBlackJack(value);
    }

    // Update is called once per frame
    void Update()
    {
        if (inGameManager.isGameover || inGameManager.isTuto)
            return;
        if (isNotSensorMake)
        {
            SensorMakeFx();
        }
        else
        {
            if (inGameManager.isCount)
                return;

            float _Time = Time.deltaTime;
            bool isReady = false;

            for (int i = 0; i < sensorsEffect.Count; i++)
            {
                if (sensorsEffect[i])
                    isReady = true;
            }
            if (isHaveEver || isReady)
            {
                sensorFlickerSpeed = _Time * flickrSpeed;
                Calculate();
            }

            for (int i = 0; i < sensors.Count; i++)
            {
                if (sensors[i].sensor.activeType == DataType.SensorActiveType.Ever) //영구 직감은 통과
                    EverSensorFlicker(i);
                else
                {
                    CountSensor(_Time, i);
                }
            }
        }
        
    }
    void CountSensor(float _Time, int i)
    {
         if (sensors[i].sensor.sensorType == DataType.SensorType.Bluffing && PlayerCharacter.instance.isShield == true)
         {
             EverSensorFlicker(i);
         }
         else
         {
             if (CompleteCheckSensor(i))//완료상태면 대기
             {
                EverSensorFlicker(i);
                SensorSkill(sensors[i].sensor.sensorType, i);
                if(sensorsEffect[i])
                    StartSkillEffetUI();
             }
             else //아니라면 시간 누적
             {
                sensors[i].Update(_Time);
                float percent = sensors[i].GetPercent();
                sensorPercent[i].fillAmount = percent;
             }
        }
    }
    void ChargeSensorTime(int _idx)
    {
        //if (sensorPercent[_idx] != null)
        //{
        //    sensorPercent[_idx].color = Color.gray;
        //}
    }

    void EverSensorFlicker(int _idx)
    {
        if (sensorPercent[_idx] != null)
        {
             sensorPercent[_idx].color = flickerColor;
        }
    }
    float Calculate()
    {
        if (flickerColor.a >= 0.9f && isPlus)
        {
            isPlus = false;
        }
        else if (flickerColor.a < 0.1f && !isPlus)
        {
            isPlus = true;
        }
        switch (isPlus)
        {
            case true:
                flickerColor.a += sensorFlickerSpeed;
                break;
            case false:
                flickerColor.a -= sensorFlickerSpeed;
                break;
        }
        return AlphaValue = flickerColor.a;
    }

    public void StartSkillEffetUI()
    {
        if (SkillUINUllCheck())
        {
            if (skillEffectUI != null)
                skillEffectUI.SetSkillEffectUI(DataType.EffectUIType.Sensor);
        }
    }

    public void SensorSkill(DataType.SensorType sensorType, int _num)
    {
        bool isAcive = false;
        switch (sensorType)
        {
            case DataType.SensorType.FightDog: //투견
                 isAcive = BreakObj(fighterDog);
                break;
            case DataType.SensorType.UnfairPlay: //언페어 플레이
                isAcive = BreakObj(unfairPlay);
                break;
            case DataType.SensorType.Bluffing:  // 블러핑
                inGameManager.PlayerShield();
                isAcive = !PlayerCharacter.instance.isShield;
                break;
            case DataType.SensorType.Gambler: //도박사
                isAcive = !CreateCoin();
                break;

            case DataType.SensorType.END:
                Debug.Log("잘못된 접근입니다. : SensorSkill _ sensorType : " + sensorType);
                break;
            default:
                break;
        }
        sensorsEffect[_num] = isAcive;
        if(sensorsEffect[_num] == false)
        {
            sensors[_num].ResetTime();
            sensorImage[_num].color = Color.gray;
            sensorPercent[_num].color = Color.white;
            //ChargeSensorTime(_num);
            sensorPercent[_num].fillAmount = 0;
        }

    }

    public bool CreateCoin() //겜블러
    {
        if(creatCoin != null)
        {
            if (changeItem_Copy.Length <= gamblerNum)
                gamblerNum = 0;
            if(changeItem_Copy[gamblerNum] != null)
                changeItem_Copy[gamblerNum].transform.position = PlayerCharacter.instance.transform.position + (new Vector3(8, 2.5f, 0));
            isSpreadNum = !isSpreadNum;
            PopCoin jelly = changeItem_Copy[gamblerNum].GetComponent<PopCoin>();
            gamblerNum++;
            if (jelly == null)
            {
                return false;
            }
            else
            {
                GameObject[] games;
                if(isSpreadNum)
                    games = spreadItem_Copy[0];
                else
                    games = spreadItem_Copy[1];

                jelly.gameObject.SetActive(true);
                jelly.SetScore(creatCoinValue);
                jelly.PopSetting(games);
                jelly.Init();
                return true;
            }
        }
        return false;
    }

    bool BreakObj(List<GameObject> games)
    {
        //targetpos = GetTargetPos(_targetTag);
        //if (targetpos == Vector3.zero)
        //    return false;
        return !OffTargetObject(games);
        #region
        //if(changeItem_Copy != null)
        //{
        //    if(changeItem_Copy[changeIdx] != null)
        //    {
        //        changeItem_Copy[changeIdx].SetActive(true); //곰젤리 활성화
        //        changeItem_Copy[changeIdx].gameObject.GetComponent<Animator>().SetTrigger("Born");
        //        changeItem_Copy[changeIdx++].transform.position = targetpos; //곰젤리를 젤리 위치로 이동
        //    }
        //}
        //if (changeIdx >= changeItem_Copy.Length) changeIdx = 0;
        #endregion
    }

    bool OffTargetObject(List<GameObject> games)
    {
        GameObject temp = null;
        float gamseXPos;
        for (int i = 0; i < games.Count; i++)
        {
            if(games[i] != null)
            {
                gamseXPos = games[i].transform.position.x;

                if (startXPos < gamseXPos) //앞에 있는지 확인
                {
                    if(endXPos > gamseXPos)
                    {
                        if (temp == null)//기록이 없으면 기록
                            temp = games[i];
                        else
                        {
                            if (temp.transform.position.x > gamseXPos) //기록된거와 비교
                            {
                                temp = games[i]; //더 가까운걸로 기록
                            }
                        }
                    }
                }
                else
                {
                    games.RemoveAt(i);
                }
            }
        }
        if(temp != null)
        {
            fighterDog.Remove(temp);
            unfairPlay.Remove(temp);

            temp.SetActive(false);   //해당 오브젝트 비활성화
            games.Clear();
            games = new List<GameObject>(); //리스트 초기화
            return true;
        }
        return false;
    }

    Vector3 GetTargetPos(string _targetTag)
    {
        GameObject[] Temp = GameObject.FindGameObjectsWithTag(_targetTag);

        ArrayToList(Temp);

        if (targetObject.Count == 0)  //발견된것이 없으면 탈출
            return Vector3.zero;

        targetIdx = targetObject.Count - 1;

        for (int i = 0; i < targetObject.Count; i++)
        {
            if (PlayerCharacter.instance.transform.position.x + 1f < targetObject[i].transform.position.x) //플레이어보다 앞에 있고
            {
                if (PlayerCharacter.instance.transform.position.x + 15f > targetObject[i].transform.position.x) //시야범위 안에 있으면
                {
                    targetIdx = i;
                }
            }
        }

        if (targetObject.Count <= targetIdx) //잘못된 인덱스면 탈출
            return Vector3.zero;

        Vector3 targetpos = targetObject[targetIdx].transform.position;

        return targetpos;
    }

    void ArrayToList(GameObject[] objectArray)
    {
        targetObject = new List<GameObject>(); //리스트 초기화
        for (int i = 0; i < objectArray.Length; i++)
        {
            if (objectArray[i] != null)
                targetObject.Add(objectArray[i]); //리스트에 오브젝트 추가
        }
    }


    bool SkillUINUllCheck()
    {
        if (skillEffectUI == null)
            return false;
        return true;
    }

    void OnTriggerEnter2D(Collider2D other) 
    { 
        if(!inGameManager.isGameover)
        {
            //for (int i = 0; i < targetObject.Count; i++)
            //{
            //    if (targetObject[i] == other.gameObject)
            //        return;
            //}
            if(other.tag == "Obstacle")
            {
                targetObject.Add(other.gameObject);
                fighterDog.Add(other.gameObject);
                unfairPlay.Add(other.gameObject);
            }
        }
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (!inGameManager.isGameover)
    //    {
    //        for (int i = 0; i < targetObject.Count; i++)
    //        {
    //            if (targetObject[i] == other.gameObject)
    //            {
    //                targetObject.Remove(other.gameObject);
    //                fighterDog.Remove(other.gameObject);
    //                unfairPlay.Remove(other.gameObject);
    //                return;
    //            }
    //        }
    //    }
    //}


}
