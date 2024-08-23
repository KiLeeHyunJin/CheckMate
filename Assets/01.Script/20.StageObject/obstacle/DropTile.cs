using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTile : BaseObstacle
{
    [SerializeField] GameObject Owner;
    [SerializeField] ObstacleSensor Sensor;
    [SerializeField] float downSpeed;
    [SerializeField] float currentTime;
    [SerializeField] float fireTime;
    [SerializeField] bool isFire;
    [SerializeField] float zRot;
    [SerializeField] bool isPlus;
    float yPos;
    // Start is called before the first frame update
    void Start()
    {
        yPos = Owner.transform.position.y;
        isFire = false;
        currentTime = 0f;
    }
    public override void Init()
    {
        transform.rotation = Quaternion.identity;
        Owner.transform.position = new Vector2(Owner.transform.position.x, yPos);

        isFire = false;
        Sensor.isFire = false;

        currentTime = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            Dropping();
            return;
        }
        if (Sensor.isFire)
        {
            ShakeTile();
        }
    }

    void ShakeTile()
    {
        Quaternion Temp = transform.rotation;
        int plus = 1;
        if (zRot > 20 && !isPlus)
        {
            isPlus = true;
            plus = 1;
        }
        else if(zRot < -20 && isPlus)
        {
            isPlus = false;
            plus = -1;
        }

        zRot += Time.deltaTime * 50 * plus;
        Temp.z = zRot;
        transform.rotation = Temp;

        currentTime += Time.deltaTime;
        if (currentTime >= fireTime)
        {
            currentTime = 0;
            isFire = true;
            Sensor.isFire = false;
        }
    }

    void Dropping()
    {
        currentTime += Time.deltaTime;
        Owner.transform.Translate(Vector2.down * Time.deltaTime * downSpeed);
        if (currentTime >= fireTime)
        {
            Init();
        }

    }

}
