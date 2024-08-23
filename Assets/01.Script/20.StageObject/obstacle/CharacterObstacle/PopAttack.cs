using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAttack : MonoBehaviour
{
    [SerializeField] GameObject characterArm;
    [SerializeField] GameObject attackObject;
    [SerializeField] ObstacleSensor sensor;
    [SerializeField] float moveDistance;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackDistance;
    [SerializeField] float attackSpeed;
    [SerializeField] float alphaSpeed;

    float addMoveDistance;
    float armPositionY;
    float attackPositionY;
 
    //Rigidbody2D rigid;
    float angle = -700.0f;
    float force = 40000.0f;
    float alphaValue;
    //bool isRotate;
    bool isCrashed;
    bool isAttack;
    bool isFinish;
    int rot;
    BoxCollider2D boxCollider;
    CircleCollider2D circleCollider;
    bool isActive = false;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    SpriteRenderer armRenderer;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        armRenderer = characterArm.GetComponent<SpriteRenderer>();
        attackPositionY = attackObject.transform.position.y + attackDistance;
        isAttack = false;
        isFinish = false;
        alphaValue = 1;
        if (boxCollider != null)
            boxCollider.isTrigger = true;
        else if (circleCollider != null)
            circleCollider.isTrigger = true;
        if (sensor == null)
            sensor = transform.parent.gameObject.GetComponent<ObstacleSensor>();
        armPositionY = characterArm.transform.position.y;
        attackObject.SetActive(false);
        //SettingObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            if (sensor.isFire)
            {
                if(!isFinish)
                {
                    if (!isAttack)
                    {
                        if (moveDistance >= addMoveDistance)
                        {
                            float Temp = moveSpeed * Time.deltaTime;
                            addMoveDistance += Temp;
                            characterArm.transform.Translate(0, Temp, 0);
                        }
                        else
                        {
                            if (armPositionY > characterArm.transform.position.y)
                            {
                                isAttack = true;
                            }
                            characterArm.transform.Translate(0, moveSpeed * Time.deltaTime * -1, 0);
                        }
                    }
                    else
                    {
                        //AttackOn();
                        AttackPop();
                    }
                }
                else
                {
                    alphaValue -= Time.deltaTime * alphaSpeed;
                    if (alphaValue <= 0)
                    {
                        this.gameObject.SetActive(false);
                        Destroy(this);
                    }
                    else
                    {
                        spriteRenderer.material.color = new Color(1, 1, 1, alphaValue);
                        armRenderer.material.color = new Color(1, 1, 1, alphaValue);
                    }

                }
            }
        }
        void AttackOn()
        {
            if (attackObject.transform.position.y <= attackPositionY)
            {
                attackObject.transform.Translate(0, attackSpeed * Time.deltaTime, 0);
            }
            else
            {
                attackObject.transform.position = new Vector3(attackObject.transform.position.x, attackPositionY, attackObject.transform.position.z);
                isFinish = true;
            }
        }
        void AttackPop()
        {
            attackObject.SetActive(true);
            isFinish = true;
        }

        void SettingObstacle()
        {

        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!InGameManager.instance.isGameover && collision.gameObject.tag == "Player") //충돌체가 플레이어
            {
                if (!PlayerCharacter.instance.Getinvincibility()) //플레이어 무적상태x
                {
                    if (PlayerCharacter.instance.isreinforce && !isCrashed) //파괴상태 o
                    {
                        //SFXmanager.instance.CrashWithObstacle();
                    }
                    else //플레이어 파괴상태 x
                    {
                        if (!PlayerCharacter.instance.isShield)
                            //SFXmanager.instance.PlayOnCrashWithBody();
                        return;
                    }
                }
            }
            if (!isCrashed)
            {
                if (!InGameManager.instance.isGameover && collision.gameObject.tag == "Player")
                {
                    if (!PlayerCharacter.instance.Getinvincibility())
                    {
                        if (collision.gameObject.tag == "Player" && PlayerCharacter.instance.isreinforce)
                        {
                            isCrashed = true;
                            StopCoroutine(Onrotate());
                            StartCoroutine(Onrotate());
                            rigid.constraints = RigidbodyConstraints2D.None;
                            rigid.AddForce((Vector2.right ) * force * Time.deltaTime);
                            //SFXmanager.instance.CrashWithObstacle();
                        }
                        else
                        {
                            //SFXmanager.instance.PlayOnCrashWithBody();
                        }
                    }
                }
            }
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
}

