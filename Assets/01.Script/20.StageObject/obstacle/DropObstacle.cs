using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropObstacle : BaseObstacle
{
    DataType.MoveObstaclePropertyType type = DataType.MoveObstaclePropertyType.Down;

    [SerializeField] float yPosition;
    [SerializeField] float dropSpeed;
    [SerializeField] ObstacleSensor sensor;
    [SerializeField] float throwPower;
    
    Rigidbody2D rigid;
    float angle = -500.0f;
    float force = 40000.0f;
    //bool isRotate;
    bool isCrashed;
    bool isActive;
    int rot;
    float DestPosition;
    Vector3 SettingPosition;
    BoxCollider2D boxCollider;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        DestPosition = transform.position.y;
        SettingPosition = transform.position;
        transform.position = transform.position + new Vector3(0, yPosition, 0);
        if (dropSpeed <= 1f)
            dropSpeed = 1.5f;
        if(throwPower <= 1) throwPower = 1.5f;
    }
    public override void Init()
    {
        isCrashed = false;
        sensor.isFire = false;
        transform.position = new Vector3(transform.position.x, DestPosition, transform.position.z);
        transform.rotation = Quaternion.identity;
    }
    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            if (sensor.isFire)
            {
                float Temp =  transform.position.y - DestPosition;
                if (Temp >= 0.15f)
                    transform.position -= new Vector3(0, ((yPosition) * Time.deltaTime) * dropSpeed, 0);
                else
                {
                    transform.position = SettingPosition;
                    sensor.isFire = false;
                    isActive = true;
                    //Destroy(sensor);
                }
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!InGameManager.instance.isGameover && collision.gameObject.tag == "Player") //충돌체가 플레이어
        {
            if (!PlayerCharacter.instance.Getinvincibility()) //플레이어 무적상태x
            {
                if (PlayerCharacter.instance.isreinforce && !isCrashed) //파괴상태 o
                {

                    SFXmanager.instance.CrashWithObstacle();
                }
                else //플레이어 파괴상태 x
                {
                    if (!PlayerCharacter.instance.isShield)
                        SFXmanager.instance.PlayOnCrashWithBody();
                    return;
                }
            }
        }
        //if (!InGameManager.instance.isGameover)
        //{
        //    if (collision.gameObject.tag == "Player" && PlayerCharacter.instance.isreinforce && !isCrashed)
        //    {
        //        isCrashed = true;
        //        StopCoroutine(Onrotate());
        //        StartCoroutine(Onrotate());
        //        rigid.constraints = RigidbodyConstraints2D.None;
        //        rigid.AddForce((Vector2.right * throwPower) * force * Time.deltaTime);
        //        SFXmanager.instance.CrashWithObstacle();
        //    }
        //}
    }
    IEnumerator Onrotate()
    {
        gameObject.tag = "Out";
        float time = 0f;
        while (time <= 3f)
        {
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
            //if (rot == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle * Time.deltaTime);
            }
        }
        gameObject.SetActive(false);
    }
}
