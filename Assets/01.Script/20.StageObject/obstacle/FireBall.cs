using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : BaseObstacle
{
    DataType.MoveObstaclePropertyType type = DataType.MoveObstaclePropertyType.FireBall;

    CircleCollider2D circleCollider;
    [SerializeField] ObstacleSensor sensor;
    float m_FireBallsSpeed;
    public float m_Damage;
    Vector2 currentPosition;
    Vector2 readyPos;
    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        readyPos = transform.position;
    }
    public override void Init()
    {
        sensor.isFire = false;
        transform.position = readyPos;
    }
    void Start()
    {
        m_FireBallsSpeed = InGameManager.instance.fireBallSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(sensor.isFire)
        {
            currentPosition = transform.position;
            currentPosition.x += (m_FireBallsSpeed * -1) * Time.deltaTime;
            transform.position = currentPosition;
            if((transform.position.x - PlayerCharacter.instance.transform.position.x) < -20f)
                Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && PlayerCharacter.instance.isreinforce)
        {
            Destroy(gameObject);
        }
    }
}
