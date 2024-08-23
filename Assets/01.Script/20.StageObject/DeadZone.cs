using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    BoxCollider2D collider2D;
    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!InGameManager.instance.isGameover)
        {
            if (collision.gameObject.tag == "Player")
            {
                float Temp = InGameManager.instance.hpbar.value;
                InGameManager.instance.Damagedhp(Temp);
            }
        }
    }
}
