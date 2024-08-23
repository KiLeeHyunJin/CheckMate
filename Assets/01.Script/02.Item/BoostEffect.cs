using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    Vector3 targetpos;

    private void OnEnable()
    {
        if (PlayerCharacter.instance.isbig) transform.localScale = new Vector3(transform.localScale.x, 3f, transform.localScale.z);
        targetpos = PlayerCharacter.instance.transform.position;
        transform.position = new Vector3(targetpos.x + 0.2f, targetpos.y + 1.27f, targetpos.z);
    }
    private void OnBecameInvisible()
    {
        transform.localScale = new Vector3(transform.localScale.x, 1.5f, transform.localScale.z);
        gameObject.SetActive(false);
    }
}
