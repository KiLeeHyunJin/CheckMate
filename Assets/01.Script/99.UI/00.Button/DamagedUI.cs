using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedUI : MonoBehaviour
{

    public static DamagedUI instance;
    Image thisImage;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        thisImage = GetComponent<Image>();

        if(thisImage != null)
        {
            thisImage.enabled = true;
            thisImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    public void Ondamaged()
    {
        StopCoroutine(Damagedeffect());
        StartCoroutine(Damagedeffect());
    }

    IEnumerator Damagedeffect()
    {
        thisImage.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.1f);
        thisImage.color = new Color(1f, 1f, 1f, 0f);
    }
}