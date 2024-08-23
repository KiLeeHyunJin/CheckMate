using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSensor : MonoBehaviour
{
    public DataType.MoveObstaclePropertyType type;
    public DataType.MoveObstacleType obstacleType;
    private const bool V = false;
    public bool isFire = V;
    private BoxCollider2D boxCollider;
    
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if(!boxCollider.isTrigger)
            boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isFire)
        {
            if (collision.gameObject.tag == "Player")
                    isFire = true;
        }
    }
}