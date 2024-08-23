using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAttack : MonoBehaviour
{

    //Rigidbody2D rigid;
    float angle = -700.0f;
    float force = 40000.0f;
    //bool isRotate;
    bool isCrashed;
    int rot;
    Vector3 DestPosition;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] CircleCollider2D circleCollider;
    bool isActive = false;
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (boxCollider != null)
            boxCollider.isTrigger = true;
        else if (circleCollider != null)
            circleCollider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!InGameManager.instance.isGameover && collision.gameObject.tag == "Player") //충돌체가 플레이어
        {
            Debug.Log("공격 충돌!");
                if (PlayerCharacter.instance.isreinforce && !isCrashed) //파괴상태 o
                {
                    isCrashed = true;
                    StopCoroutine(Onrotate());
                    StartCoroutine(Onrotate());
                    if (rigid != null)
                    {
                        rigid.constraints = RigidbodyConstraints2D.None;
                        rigid.AddForce(Vector2.right * force * Time.deltaTime);
                    }

                    SFXmanager.instance.CrashWithObstacle();
                    return;
                }
                else //플레이어 파괴상태 x
                {
                    if (!PlayerCharacter.instance.isShield)
                        SFXmanager.instance.PlayOnCrashWithBody();
                    return;
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
