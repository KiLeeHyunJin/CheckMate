using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageObstacle : BaseObstacle
{
    [SerializeField] ObstacleSensor sensor;
    [SerializeField] DataType.MoveObstaclePropertyType obstacleType;
    [SerializeField] float yPosition;
    [SerializeField] float dropSpeed;
    DataType.MoveObstaclePropertyType originType;
    //Rigidbody2D rigid;
    float angle = -700.0f;
    float force = 40000.0f;
    float m_FireBallsSpeed;
    //bool isRotate;
    bool isCrashed;
    bool isActive = false;
    int rot;
    Vector3 DestPosition;
    Vector3 startPosistion;
    Vector2 currentPosition;

    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] CircleCollider2D circleCollider;
    SpriteRenderer objectImage;
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        //boxCollider = GetComponent<BoxCollider2D>();
        //circleCollider = GetComponent<CircleCollider2D>();
        objectImage = GetComponent<SpriteRenderer>();
        startPosistion = transform.position;
        originType = obstacleType;
        if (boxCollider != null)
            boxCollider.isTrigger = true;
        else if(circleCollider != null)
            circleCollider.isTrigger = true;
        if (sensor == null)
            sensor = transform.parent.gameObject.GetComponent<ObstacleSensor>();

        isDie = false;
        SettingObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameManager.instance.isTuto)
            return;
        if(!isActive)
        {
            if(sensor.isFire)
            {
                ObstacleMove();
            }
        }
    }

    void ObstacleMove()
    {
        switch (obstacleType)
        {
            case DataType.MoveObstaclePropertyType.FireBall:
                FireBallObstacle();
                break;
            case DataType.MoveObstaclePropertyType.DownStreet:
            case DataType.MoveObstaclePropertyType.Down:
                DropObstacle();
                break;
            case DataType.MoveObstaclePropertyType.GrassPop:
                PopUpObstacle();
                break;
            case DataType.MoveObstaclePropertyType.END:
                Debug.Log("존재하지 않는 장애물입니다. ");
                break;
        }
    }
 
    void DropObstacle()
    {
        float yPos =  DestPosition.y - transform.position.y;
        //if (Vector3.Distance(transform.position, DestPosition) >= 0.2f)
        if(yPos <= 0.14f)  
            transform.position -= new Vector3(0, (Time.deltaTime) * dropSpeed, 0);
        else
        {
            Vector3 Temp = transform.position;
            Temp.y = DestPosition.y;
            transform.position = Temp;
            if (obstacleType == DataType.MoveObstaclePropertyType.DownStreet)
            {
                obstacleType = DataType.MoveObstaclePropertyType.FireBall;
                return;
            }
            sensor.isFire = false;
            isActive = true;
            //Destroy(sensor);
        }
    }
    void FireBallObstacle()
    {
        currentPosition = transform.position;
        currentPosition.x += (m_FireBallsSpeed * -1) * Time.deltaTime;
        transform.position = currentPosition;
        if ((transform.position.x - PlayerCharacter.instance.transform.position.x) < -20f)
            Destroy(gameObject);
    }
    void PopUpObstacle()
    {
        objectImage.enabled = true;
        sensor.isFire = false;
        isActive = true;
       //Destroy(sensor);
    }

    void SettingObstacle()
    {
        switch (obstacleType)
        {
            case DataType.MoveObstaclePropertyType.FireBall:
                {
                    m_FireBallsSpeed = InGameManager.instance.fireBallSpeed;
                }
                break;
            case DataType.MoveObstaclePropertyType.DownStreet:

            case DataType.MoveObstaclePropertyType.Down:
                {
                    if(DataType.MoveObstaclePropertyType.DownStreet == obstacleType)
                    {
                        m_FireBallsSpeed = InGameManager.instance.fireBallSpeed;
                    }
                    DestPosition = transform.position;
                    transform.position = transform.position + new Vector3(0, yPosition, 0);
                    if (dropSpeed <= 1f)
                        dropSpeed = 1.5f;
                }
                break;
            case DataType.MoveObstaclePropertyType.GrassPop:
                objectImage.enabled = false;
                break;
            case DataType.MoveObstaclePropertyType.END:
                break;
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!InGameManager.instance.isGameover && collision.gameObject.tag == "Player") //충돌체가 플레이어
        {
            if(!PlayerCharacter.instance.Getinvincibility()) //플레이어 무적상태x
            {
                Debug.Log("충돌 : " + gameObject.name);

                if (PlayerCharacter.instance.isreinforce && !isCrashed) //파괴상태 o
                {
                    if (obstacleType != DataType.MoveObstaclePropertyType.FireBall || obstacleType != DataType.MoveObstaclePropertyType.DownStreet)
                    {
                        isCrashed = true;
                        StopCoroutine(Onrotate());
                        StartCoroutine(Onrotate());
                        if(rigid != null)
                        {
                            rigid.constraints = RigidbodyConstraints2D.None;
                            rigid.AddForce(Vector2.right * force * Time.deltaTime);
                        }

                        SFXmanager.instance.CrashWithObstacle();
                        //return;
                    }
                    else
                    {
                        SFXmanager.instance.PlayOnCrashWithBody();
                        CrashObj();
                        Invoke("CrashObj", 0.5f);
                        //return;
                    }
                    isDie = true;
                }
                else //플레이어 파괴상태 x
                {
                    if(!PlayerCharacter.instance.isShield)
                       InGameSound.instance.PlayOnCrashWithBody();
                    return;
                }
            }
        }
    }

    void CrashObj()
    {
        if(objectImage != null)
        {
            objectImage.enabled = !objectImage.enabled;
        }
        if(boxCollider != null)
        {
            boxCollider.enabled = !boxCollider.enabled;
        }
        else if(circleCollider != null)
        {
            circleCollider.enabled = !circleCollider.enabled;
        }
    }
    public override void Out()
    {
        gameObject.SetActive(false);

    }
    override public void Init()
    {
        StopCoroutine(Onrotate());                  //스레드 초기화
        gameObject.SetActive(true);
        transform.position = startPosistion;        //위치초기화
        transform.rotation = Quaternion.identity;   //회전 초기화
        isCrashed = isDie = isActive = sensor.isFire = false; //상태 초기화
        switch (originType)
        {
            case DataType.MoveObstaclePropertyType.FireBall:
                {
                }
                break;
            case DataType.MoveObstaclePropertyType.DownStreet:
                {
                    obstacleType = DataType.MoveObstaclePropertyType.DownStreet;
                }
                break;
            case DataType.MoveObstaclePropertyType.Down:
                {
                }
                break;
            case DataType.MoveObstaclePropertyType.GrassPop:
                {
                    objectImage.enabled = false;
                }
                break;
        }
    }

    IEnumerator Onrotate()
    {
        //gameObject.tag = "Out";
        float time = 0f;
        while (time <= 3f)
        {
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
            if (rot == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle * Time.deltaTime);
            }
        }
        Init();
    }

}
