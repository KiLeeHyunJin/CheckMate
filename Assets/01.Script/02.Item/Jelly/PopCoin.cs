using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCoin : BaseJelly
{
    // Start is called before the first frame update
    CircleCollider2D circleCollider;
    [SerializeField] float PopSpeed;
    float angle;
    [SerializeField] float dest = 1.5f;
    Vector3[] direc; //방향
    Vector3[] DestPos; //목적지
    GameObject[] PopCoins;//이동할 코인
    public override void Init()
    {
        isDie = isCrashed = false;
        if (animator != null)
        {
            animator.SetTrigger("Init");
        }

        float Total = 0;
        for (int i = 0; i < PopCoins.Length; i++)
        {
            Total += angle;
            PopCoins[i].GetComponent<BaseJelly>().Init();
            PopCoins[i].transform.position = transform.position;
            PopCoins[i].gameObject.SetActive(true);
            if (Total >= 360)
            {
                Total -= 360;
            }
            var quaternion = Quaternion.Euler(0, 0, Total);
            //if (Total == 0)
            //    direc[i] = Vector3.up;
            //else
            //    direc[i] = new Vector3(Mathf.Cos(Total), Mathf.Sin(Total), 0);
            direc[i] = quaternion * Vector3.up ;//direc[i] * dest;


            //Vector3 direction = transform.forward;
            //Vector3 newDirection = Vector3.up;
            //Total += angle;

            //if (angle < 359)
            //{
            //    var quaternion = Quaternion.Euler(0, Total, 0);
            //    newDirection = quaternion * direction * PopPower;
            //}
            //PopCoins[i].transform.Translate(newDirection);
        }
    }
    
    protected override void Go()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    protected override void Tick()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * 900, 0));

        if (PopCoins == null)
            return;
        if(PopCoins.Length > 0)
        {
            if(Vector2.Distance(PopCoins[0].transform.position,transform.position) < dest)
            {
                float time = Time.deltaTime;
                for (int i = 0; i < PopCoins.Length; i++)
                {
                    if (PopCoins[i] == null)
                        continue;
                    PopCoins[i].transform.Translate(direc[i] * PopSpeed * time);
                }
            }
        }
    }
    public void PopSetting(GameObject[] gameObjects)
    {
        PopCoins = gameObjects;
        direc = new Vector3[PopCoins.Length];
        DestPos = new Vector3[PopCoins.Length];
        if (gameObjects != null)
        {
            int count = gameObjects.Length;
            if (count != 0)
            {
                angle = 360 / count;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCrashed)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Pet")
            {
                isCrashed = true;

                AddData();
                Effect();
            }
        }
    }
}
