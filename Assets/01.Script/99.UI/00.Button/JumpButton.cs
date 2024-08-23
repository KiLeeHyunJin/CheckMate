using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public void OnClick()
    {
        PlayerCharacter.instance.OnClickJumpButton();
    }
}
