using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowObstacle : BaseObstacle
{
    DataType.MoveObstaclePropertyType type = DataType.MoveObstaclePropertyType.Arrow;
    public Transform[] p = new Transform[4];
    [SerializeField] float speed = 0.5f;
    [SerializeField] float magicNum = 3;
    [SerializeField] ObstacleSensor sensor;
    Rigidbody2D rigid;
    [SerializeField] float throwPower;
    float angle = -600.0f;
    float force = 40000.0f;
    float t = 0f;
    Vector3 bezierPosition;
    Vector2 gizmosPosition;
    Vector3 startPosition;
    BoxCollider2D boxCollider;

    bool isCrashed;
    int rot;

    override public void Init()
    {
        sensor.isFire = false;
        isCrashed = false;
        transform.position = startPosition;
        t = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        rot = Random.Range(0, 2);
        rigid = this.GetComponent<Rigidbody2D>();
        boxCollider = this.GetComponent<BoxCollider2D>();   
        //isRotate = false;
        isCrashed = false;
        if (throwPower <= 1) throwPower = 1.5f;
        if (!boxCollider.isTrigger)
            boxCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(sensor.isFire)
        {
            if (Vector3.Distance(transform.position, p[3].transform.position) > 0.2f)
                StartCoroutine(BezierCurveStart());
            else
            {
                transform.position = p[3].transform.position;
                sensor.isFire = false;
            }
        }
    }
    IEnumerator BezierCurveStart()
    {
        t += Time.deltaTime * speed;
        while(t < 1)
        {
            //for (int i = 0; i < p.Length; i++)
            //{
            //}
            bezierPosition = Mathf.Pow(1 - t, magicNum) * p[0].position
                + magicNum * t * Mathf.Pow(1 - t, magicNum) * p[1].position
                + magicNum * t * (1 - t) * p[2].position
                + Mathf.Pow(t, magicNum) * p[3].position;
            transform.position = bezierPosition;
            yield return new WaitForEndOfFrame();
        }
        t = 0f;
    }
    void OnDrawGizmos()
    {
        for (float t = 0; t< 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, magicNum) * p[0].position
                + magicNum * t * Mathf.Pow( 1 - t , magicNum) * p[1].position
                + magicNum * t * (1 - t) * p[2].position
                + Mathf.Pow(t, magicNum) * p[3].position;
            Gizmos.DrawSphere(gizmosPosition, 0.05f);
        }
        Gizmos.DrawLine(new Vector2(p[0].position.x, p[0].position.y),
                        new Vector2(p[1].position.x, p[1].position.y));
        Gizmos.DrawLine(new Vector2(p[1].position.x, p[1].position.y),
                        new Vector2(p[2].position.x, p[2].position.y));
        Gizmos.DrawLine(new Vector2(p[2].position.x, p[2].position.y),
                        new Vector2(p[3].position.x, p[3].position.y));
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
        //if (!isCrashed)
        //{
        //    if (!InGameManager.instance.isGameover && collision.gameObject.tag == "Player")
        //    {
        //        if (!PlayerCharacter.instance.Getinvincibility())
        //        {
        //            if (collision.gameObject.tag == "Player" && PlayerCharacter.instance.isreinforce)
        //            {
        //                isCrashed = true;
        //                StopCoroutine(Onrotate());
        //                StartCoroutine(Onrotate());
        //                rigid.constraints = RigidbodyConstraints2D.None;
        //                rigid.AddForce((Vector2.right * throwPower) * force * Time.deltaTime);
        //                SFXmanager.instance.CrashWithObstacle();
        //            }
        //            else
        //            {
        //                SFXmanager.instance.PlayOnCrashWithBody();
        //            }
        //        }
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
