using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboCountSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI comboTxt;
    [SerializeField] InGameManager inGameManager;
    bool isNotUse = false;
    [Header("DjMaxComboEffect")]
    [SerializeField] bool isActiveDJMAX;
    [SerializeField] float UpSpeed;
    [SerializeField] float MinusAlphavalue;
    [SerializeField] float AlphaSpeed;
    [SerializeField] GameObject StartObj;
    [SerializeField] GameObject EndObj;
    float shakeValue = 1;
    Vector2 StartPos;
    Vector2 EndPos;
    Vector2 OriginPos;


    [Header("ComboTxt1")]
    [SerializeField] float ComboTxtAlphaValue;
    [SerializeField] float ComboTxtAlphaSpeed;
    [SerializeField] float ComboTxtScale;
    [SerializeField] float ComboTxtSpeed;
    [SerializeField] float ComboDefaultScale;

    [Header("ComboTxt2")]
    [SerializeField] float ComboTxtAlphaSpeed2;
    [SerializeField] float ComboTxtSpeed2;
    [SerializeField] float ComboTxtScale2;
    [SerializeField] float ComboTxtRotSpeed2;
    [SerializeField] float ComboDefaultScale2;
    [SerializeField] float ComboTxtRotValue2;

    [Header("ComboTxt3")]
    [SerializeField] float ComboTxtAlphaSpeed3;
    [SerializeField] float ComboTxtSpeed3;
    [SerializeField] float ComboTxtScale3;
    [SerializeField] float ComboTxtRotSpeed3;
    [SerializeField] float ComboDefaultScale3;
    [SerializeField] float ComboTxtRotValue3;

    [Header("ComboTxt4")]
    [SerializeField] float ComboTxtAlphaSpeed4;
    [SerializeField] float ComboTxtSpeed4;
    [SerializeField] float ComboTxtScale4;
    [SerializeField] float ComboTxtRotSpeed4;
    [SerializeField] float ComboDefaultScale4;
    [SerializeField] float ComboTxtRotValue4;

    [SerializeField] int CountColorValue1;
    [SerializeField] int CountColorValue2;
    [SerializeField] int CountColorValue3;
    [SerializeField] int CountColorValue4;

    [SerializeField] Color NewCountColor1;
    [SerializeField] Color NewCountColor2;
    [SerializeField] Color NewCountColor3;
    [SerializeField] Color NewCountColor4;
    public bool isEffectCombo = false;
    float comboTxtDefaultValue = 0;
    float addTime = 0;


    // Start is called before the first frame update
    void Start()
    {


        if (comboTxt != null )
            comboTxt.text = "0";
        else
            isNotUse = true;
        if (isNotUse)
        {
            return;
        }
            OriginPos = comboTxt.rectTransform.anchoredPosition;

        if (StartObj != null)
            StartPos = StartObj.GetComponent<RectTransform>().anchoredPosition;
        else
            StartPos = OriginPos;

        if (EndPos != null)
            EndPos = EndObj.GetComponent<RectTransform>().anchoredPosition;
        else
            EndPos = OriginPos;
        OnResetCombo();
    }

    // Update is called once per frame
    void Update()
    {
        if (isNotUse)
            return;
        if (inGameManager.isTuto)
            return;
        if(isEffectCombo)
        {
            if (isActiveDJMAX)
            {
                DJMAXEffect();
            }
            else
            {
                ComboEffect();
            }
        }
    }
    void DJMAXEffect()
    {
        if (comboTxt != null)
        {
            RectTransform txtTransform = comboTxt.GetComponent<RectTransform>();
            float time = Time.deltaTime;
            Color color = comboTxt.color;

            if(txtTransform.anchoredPosition.y < EndPos.y)
            {
                txtTransform.anchoredPosition += new Vector2(0, time * UpSpeed);
            }
            else if(txtTransform.anchoredPosition != EndPos)
            {
                txtTransform.anchoredPosition = EndPos;
            }

            if(comboTxt.color.a > MinusAlphavalue)
            {
                addTime += time;
                float temp = 1;
                if (addTime < 1)
                    temp = 1 - addTime;
                color.a -= time * AlphaSpeed;
            }
            else if(color.a < MinusAlphavalue)
            {
                color.a = MinusAlphavalue;
            }
            comboTxt.color = color;
        }
    }
    void ComboEffect()
    {
        if (comboTxt != null)
        {
            RectTransform txtTransform = comboTxt.GetComponent<RectTransform>();
            float time = Time.deltaTime;
            if (comboTxt.fontSize > comboTxtDefaultValue)
            {
                float txtSpeed = 0;
                if (inGameManager.comboNum < CountColorValue2)
                {
                    txtSpeed = ComboTxtSpeed;
                }
                else if (CountColorValue2 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue3)
                {
                    txtSpeed = ComboTxtSpeed2;
                }
                else if (CountColorValue3 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue4)
                {
                    txtSpeed = ComboTxtSpeed3;
                }
                else
                {
                    txtSpeed = ComboTxtSpeed4;
                }

                comboTxt.fontSize -= txtSpeed;

                if (comboTxt.fontSize <= comboTxtDefaultValue)
                {
                    comboTxt.fontSize = comboTxtDefaultValue;
                }
            }
            Color txtColor = comboTxt.color;
            if (txtColor.a < 1)
            {
                float alphaValue;
                if (inGameManager.comboNum < CountColorValue2)
                {
                    alphaValue = ComboTxtAlphaSpeed;
                }
                else if (CountColorValue2 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue3)
                {
                    alphaValue = ComboTxtAlphaSpeed2;
                }
                else if (CountColorValue3 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue4)
                {
                    alphaValue = ComboTxtAlphaSpeed3;
                }
                else
                {
                    alphaValue = ComboTxtAlphaSpeed4;
                }
                txtColor.a += alphaValue * time;
                comboTxt.color = txtColor;
            }
            if (txtTransform.rotation.z > 0)
            {
                float rotZ = -1 * (comboTxt.color.a + 1) * Time.deltaTime;
                if (CountColorValue2 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue3)
                {
                    rotZ *= ComboTxtRotSpeed2;
                }
                else if (CountColorValue3 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue4)
                {
                    rotZ *= ComboTxtRotSpeed3;
                }
                else
                {
                    rotZ *= ComboTxtRotSpeed4;
                }
                txtTransform.Rotate(new Vector3(0, 0, rotZ));
                if (txtTransform.rotation.z < 0)
                    txtTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    public void OnResetCombo()
    {
        if (isNotUse)
            return;
        if(isActiveDJMAX)
        {
            comboTxt.text = "0";
            Color color = comboTxt.color;
            color.a = 0;
            comboTxt.color = color;
        }
        else
        {
            RectTransform rectTransform = comboTxt.GetComponent<RectTransform>();
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            Color comboColor = Color.white;
            comboTxt.fontSize = ComboDefaultScale;
            comboColor.a = ComboTxtAlphaValue;
            comboTxt.color = comboColor;
            comboTxt.text = "0";
        }

    }
    public void AddCombo()
    {
        if (isNotUse)
            return;
        if(isActiveDJMAX)
        {
            if(inGameManager.comboNum != 0)
                shakeValue = (inGameManager.comboNum / 40) * 0.2f;
            Vector2 current = OriginPos;
            current.x += (Random.Range(shakeValue * -1, shakeValue));
            comboTxt.rectTransform.anchoredPosition = StartPos;
            Color color = comboTxt.color;
            color.a = 1;
            comboTxt.color = color;
            addTime = 0;
        }
        else
        {
            Color comboColor = Color.white;
            RectTransform rectTransform = comboTxt.GetComponent<RectTransform>();
            if (CountColorValue1 > inGameManager.comboNum)
            {
                comboTxt.fontSize = ComboTxtScale;
                Debug.Log("Count 1");
                comboTxtDefaultValue = ComboDefaultScale;
            }
            else if (CountColorValue1 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue2)
            {
                comboColor = NewCountColor1;
                comboTxt.fontSize = ComboTxtScale;
                comboTxtDefaultValue = ComboDefaultScale;
            }
            else if (CountColorValue2 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue3)
            {
                comboColor = NewCountColor2;
                rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, ComboTxtRotValue2));
                comboTxt.fontSize = ComboTxtScale2;
                comboTxtDefaultValue = ComboDefaultScale2;
            }
            else if (CountColorValue3 <= inGameManager.comboNum && inGameManager.comboNum < CountColorValue4)
            {
                comboColor = NewCountColor3;
                rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, ComboTxtRotValue3));
                comboTxt.fontSize = ComboTxtScale3;
                comboTxtDefaultValue = ComboDefaultScale3;
            }
            else
            {
                comboColor = NewCountColor4;
                rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, ComboTxtRotValue4));
                comboTxt.fontSize = ComboTxtScale4;
                comboTxtDefaultValue = ComboDefaultScale4;
            }
            comboColor.a = ComboTxtAlphaValue;
            comboTxt.color = comboColor;
        }
        comboTxt.text = inGameManager.comboNum.ToString();
    }
}
