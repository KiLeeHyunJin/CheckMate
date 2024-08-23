using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loulette : MonoBehaviour
{
    [SerializeField] float rotSpeed;
    [SerializeField] float stopSpeed;
    float saveSpeed;
    [SerializeField] bool isStart;
    // Start is called before the first frame update
    void Start()
    {
        isStart = false;
        if (rotSpeed <= 0)
            rotSpeed = 100f;
        saveSpeed = rotSpeed;
        if (stopSpeed <= 0)
            stopSpeed = 0.1f;
        rotSpeed += Random.Range(0, 20);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            isStart = !isStart;
        if(isStart)
        {
            transform.Rotate(0, 0, this.rotSpeed);
            rotSpeed *= Random.Range(0.98f, 0.999f);

            if (stopSpeed > rotSpeed)
            {
                isStart = false;
                rotSpeed = saveSpeed;
                rotSpeed += Random.Range(0, 20);
            }
        }
    }

    public void StartLoulette()
    {
        isStart = !isStart;
    }
}
