using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScrolling : MonoBehaviour
{
    float speed;
    bool currentBoost;
    // Start is called before the first frame update
    void Start()
    {
        speed = InGameManager.instance.Getspeed();
        speed *= -1;
        currentBoost = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameManager.instance.isGameover || InGameManager.instance.isCount || InGameManager.instance.isTuto) return;
        if(currentBoost != InGameManager.instance.isBoost)
        {
            currentBoost = InGameManager.instance.isBoost;
            speed = InGameManager.instance.Getspeed() * -1;
        }
        float Temp = (speed) * Time.deltaTime;

        transform.Translate(Vector3.right * Temp);
    }
}
