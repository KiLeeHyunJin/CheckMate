using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour
{
    [SerializeField] InGameManager inGame;
    [SerializeField] Image comboImg;
    [SerializeField] Sprite[] comboSprite;
    [SerializeField] float UpScale;
    [SerializeField] float DownDuring;
    [SerializeField] float UpDuring;

    [SerializeField] float ComboTxtAlphaDownSpeed;
    [SerializeField] float ComboTxtAlphaUpSpeed;

    [SerializeField] float ComboTxtDownSpeed;
    [SerializeField] float ComboTxtUpSpeed;
    float time = 0;
    bool isStart = false;
    bool isEnd = false;


    int currentArray;
    // Update is called once per frame

    private void Start()
    {
        currentArray = -1;
        if (inGame == null)
            inGame = GetComponent<InGameManager>();
        if (comboImg != null)
            if (comboImg.gameObject.activeSelf)
                comboImg.gameObject.SetActive(false);

        if (DownDuring <= 0)
            DownDuring = 0.7f;
        if (UpDuring <= 0)
            UpDuring = 0.7f;
    }
    void Update()
    {
        ActiveCombo();
        if(isStart)
            StartComboEffect();
        else if(isEnd)
            EndComboEffect();
    }
    void ActiveCombo()
    {
        if (comboImg != null)
            SpriteReturn();
    }

    void SpriteReturn()
    {
        if (inGame != null)
        {
            Sprite sprite = null;
            int count = inGame.comboNum;
            if(count >= 100)
            {
                if(!isStart && !isEnd)
                {
                    int arrNum = (count / 100) - 1;
                    if (arrNum > currentArray && arrNum < comboSprite.Length)
                    {
                        currentArray = arrNum;
                        sprite = comboSprite[arrNum];
                        comboImg.sprite = sprite;
                        EffectActive();
                        //StartCoroutine(StartComboImage());
                    }
                    else if (currentArray > 0 && arrNum == 0)
                    {
                        currentArray = arrNum;
                        sprite = comboSprite[arrNum];
                        comboImg.sprite = sprite;
                        EffectActive();
                        //StartCoroutine(StartComboImage());
                    }
                }
            }
        }
    }
    void EffectActive()
    {
        comboImg.gameObject.SetActive(true);
        comboImg.SetNativeSize();
        comboImg.rectTransform.localScale = new Vector3(0.7f, 0.7f, 1);
        Color color = comboImg.color;
        color.a = 0;
        comboImg.color = color;
        isStart = true;
    }
    void StartComboEffect()
    {
        Color color = comboImg.color;

        if (comboImg.rectTransform.localScale.x < UpScale || color.a <= 1)
        {

            if (comboImg.rectTransform.localScale.x < UpScale)
            {
                Vector3 vector3 = comboImg.rectTransform.localScale;
                vector3 = new Vector3(vector3.x + (ComboTxtUpSpeed * Time.deltaTime), vector3.y + (ComboTxtUpSpeed * Time.deltaTime),0);
                comboImg.rectTransform.localScale = vector3;
            }
            else
            {
                comboImg.rectTransform.localScale = Vector3.one * UpScale;
            }

            if (color.a < 1)
            {
                Debug.Log("AddAlphaValue : " + color.a);
                color.a += ComboTxtAlphaUpSpeed * Time.deltaTime;
                comboImg.color = color;
            }
            Debug.Log("AlphaValue : " + color.a);

            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            isEnd = true;
            isStart = false;
        }
    }
    void EndComboEffect()
    {
        if (time < DownDuring)
        {
            Color color = comboImg.color;

            if (comboImg.rectTransform.localScale.x > 1)
            {
                Vector3 vector3 = comboImg.rectTransform.localScale;
                vector3 = new Vector3(vector3.x - (ComboTxtDownSpeed * Time.deltaTime), vector3.x - (ComboTxtDownSpeed * Time.deltaTime), 0);
                comboImg.rectTransform.localScale = vector3;
            }
            else
            {
                comboImg.rectTransform.localScale = Vector3.one;
            }

            if (comboImg.color.a > 0)
            {
                Debug.Log("MinusAlphaValue : " + color.a);
                color.a -= ComboTxtAlphaDownSpeed * Time.deltaTime;
                comboImg.color = color;
            }
            Debug.Log("AlphaValue : " + color.a);

            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            isEnd = false;
            comboImg.gameObject.SetActive(false);
        }
    }

    IEnumerator StartComboImage()
    {
        
        yield break;
    }

}
