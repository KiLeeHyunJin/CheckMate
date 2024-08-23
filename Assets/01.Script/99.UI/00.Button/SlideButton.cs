using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideButton : MonoBehaviour
{
    public void OnButtonDown()
    {
        PlayerCharacter.instance.DownSlideButton();
    }
    public void OnButtonUp()
    {
        PlayerCharacter.instance.UpSlideButton();
    }
}
