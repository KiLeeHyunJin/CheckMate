using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopObstacle : BaseObstacle
{
    DataType.MoveObstaclePropertyType type = DataType.MoveObstaclePropertyType.GrassPop;

    [SerializeField] ObstacleSensor sensor;

    //Rigidbody2D rigid;
    float angle = -700.0f;
    float force = 40000.0f;
    //bool isRotate;
    bool isCrashed;
    int rot;
    Vector3 DestPosition;
    Vector3 SettingPosition;
    BoxCollider2D boxCollider;
    SpriteRenderer objectImage;
    bool isActive = false;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        objectImage = GetComponent<SpriteRenderer>();
        objectImage.enabled = false;
        if(!boxCollider.isTrigger)
            boxCollider.isTrigger = true;
    }
    public override void Init()
    {
        transform.rotation = Quaternion.identity;
        objectImage.enabled = false;
        sensor.isFire = false;
        isCrashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            if (sensor.isFire)
            {
                objectImage.enabled = true;
                sensor.isFire = false;
                isActive = true;
                Destroy(sensor);
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
        //        //rigid.constraints = RigidbodyConstraints2D.None;
        //        //rigid.AddForce(Vector2.right * force * Time.deltaTime);
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
            if (rot == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle * Time.deltaTime);
            }
        }
        gameObject.SetActive(false);
    }
}
