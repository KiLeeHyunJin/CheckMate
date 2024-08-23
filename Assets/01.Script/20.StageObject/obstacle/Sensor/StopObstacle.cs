using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopObstacle : BaseObstacle
{
    public DataType.StopObstacleType type;
    private const bool V = false;
    public bool isFire = V;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;


    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        
        if (boxCollider != null)
            boxCollider.isTrigger = true;
        else if (!circleCollider.isTrigger)
            circleCollider.isTrigger = true;
    }
    public override void Init()
    {
        this.gameObject.SetActive(true);
        isFire = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFire)
        {
            if (collision.gameObject.tag == "Player")
            {
                isFire = true;
            }
        }
    }
}
