using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleeffect : MonoBehaviour
{

    Rigidbody2D rigid;
    [SerializeField] float throwPower;
    float angle = -600.0f;
    float force = 40000.0f;
    //bool isRotate;
    bool isCrashed;
    int rot;

    void Start()
    {
        rot = Random.Range(0, 2);
        rigid = this.GetComponent<Rigidbody2D>();
        //isRotate = false;
        isCrashed = false;
        if (throwPower <= 1) throwPower = 1.5f;
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
                        rigid.AddForce((Vector2.right * throwPower) * force * Time.deltaTime);
                        SFXmanager.instance.CrashWithObstacle();
                    }
                    else
                    {
                        SFXmanager.instance.PlayOnCrashWithBody();
                    }
                }
            }
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
            //if (rot == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle * Time.deltaTime);
            }
        }
        gameObject.SetActive(false);
    }
}
