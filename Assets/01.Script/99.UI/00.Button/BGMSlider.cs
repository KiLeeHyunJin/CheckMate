using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("BGMslider");
    }

    public void ValueChanged()
    {
        PlayerPrefs.SetFloat("BGMslider", gameObject.GetComponent<Slider>().value);
        BGMmanager.instance.ChangeVolume(PlayerPrefs.GetFloat("BGMslider"));
    }
}

