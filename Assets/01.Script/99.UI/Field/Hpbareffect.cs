using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbareffect : MonoBehaviour
{
    public float Num;
    public Image PgameObject;
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //앵커 조절하여고치기
        rectTransform.anchoredPosition = new Vector2(PgameObject.fillAmount * Num, 0.0f);
    }
}
