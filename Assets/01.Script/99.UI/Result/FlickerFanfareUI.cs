using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickerFanfareUI : MonoBehaviour
{
    [SerializeField] Image fanfareUI;

    [SerializeField] float flickerSpeed;
    [SerializeField] float AlphaValue;

    bool isObj;
    bool isPlus;

    private void OnEnable()
    {
        if (flickerSpeed <= 0)
            flickerSpeed = 1.2f;
        if (fanfareUI != null)
            isObj = true;
        else
            isObj = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isObj)
        {
            Color color = new Color();
            float speed = Time.deltaTime * flickerSpeed;
            color = fanfareUI.color;
            color.a = Calculate(color, speed);
            fanfareUI.color = color;
        }
    }

    float Calculate(Color color, float speed)
    {
        if (color.a >= 0.9f && isPlus)
            isPlus = false;
        else if (color.a < 0.1f && !isPlus)
            isPlus = true;

        switch (isPlus)
        {
            case true:
                color.a += speed;
                break;
            case false:
                color.a -= speed;
                break;
        }
        return AlphaValue = color.a;
    }

}
