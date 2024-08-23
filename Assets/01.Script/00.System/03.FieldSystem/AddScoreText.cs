using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddScoreText : MonoBehaviour
{
    [SerializeField] GameObject StartPos;
    [SerializeField] TextMeshProUGUI[] texts;
    [SerializeField] float moveUpSpeed;
    [SerializeField] float duringTime;
    [SerializeField] float minusAlphaValue;
    [Header("Type Color")]
    [SerializeField] Color BlackColor;
    [SerializeField] Color BlueColor;
    [SerializeField] Color GreenColor;
    [SerializeField] Color RedColor;
    [SerializeField] Color WhiteColor;
    [SerializeField] Color YellowColor;
    [SerializeField] Color HeartColor;
    [SerializeField] Color SpadeColor;
    [SerializeField] Color CloverColor;
    [SerializeField] Color DiamondColor;
    [SerializeField] Color SmallTimeColor;
    [SerializeField] Color BigTimeColor;
    [SerializeField] Color CoinColor;
    float leftMoveSpeed;
    float[] textTime;
    bool[] textActiveCheck;
    int textIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        textTime = new float[texts.Length];
        textActiveCheck = new bool[texts.Length];

        if (duringTime <= 0)
            duringTime = 1;
        if (moveUpSpeed <= 0)
            moveUpSpeed = 2;
        if (minusAlphaValue <= 0)
            minusAlphaValue = 0.08f;
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
            textActiveCheck[i] = false;
        }

        leftMoveSpeed = ( moveUpSpeed / 3 ) * -1;
    }
    private void Update()
    {
        float time = Time.deltaTime;
        for (int i = 0; i < texts.Length; i++)
        {
            if(textActiveCheck[i])
                UpdateCheck(i, time);
        }
    }

    public void SetAddScoreText(int _scoreValue, DataType.JellyType jellyType)
    {
        if (textIdx == texts.Length)
            textIdx = 0;
        texts[textIdx].transform.position = Camera.main.WorldToScreenPoint(StartPos.transform.position);
        textActiveCheck[textIdx] = true;
        Color color = ReturnJellyTypeColor(jellyType);
        texts[textIdx].text = _scoreValue.ToString();
        texts[textIdx].color = color;
        texts[textIdx].gameObject.SetActive(true);
        textIdx++;
    }

    void UpdateCheck(int _idx, float time)
    {
        if (textTime[_idx] > duringTime)
        {
            ReturnPos(_idx);
            return;
        }
        textTime[_idx] += time;
        Color sample = texts[_idx].color;
        sample.a -= minusAlphaValue;
        float leftcalc = leftMoveSpeed * (1 - sample.a) * time;
        float upcalc = moveUpSpeed * sample.a * time;
        texts[_idx].color = sample;
        texts[_idx].transform.Translate(leftcalc, upcalc, 0);
    }
    Color ReturnJellyTypeColor(DataType.JellyType jellyType)
    {
        Color returnColor = Color.white;
        switch (jellyType)
        {
            case DataType.JellyType.Black:
                returnColor = BlackColor;
                break;
            case DataType.JellyType.Blue:
                returnColor = BlueColor;
                break;
            case DataType.JellyType.Green:
                returnColor = GreenColor;
                break;
            case DataType.JellyType.Red:
                returnColor = RedColor;
                break;
            case DataType.JellyType.White:
                returnColor = WhiteColor;
                break;
            case DataType.JellyType.Yellow:
                returnColor = YellowColor;
                break;
            case DataType.JellyType.Heart:
                returnColor = HeartColor;
                break;
            case DataType.JellyType.Spade:
                returnColor = SpadeColor;
                break;
            case DataType.JellyType.Clover:
                returnColor = CloverColor;
                break;
            case DataType.JellyType.Diamond:
                returnColor = DiamondColor;
                break;
            case DataType.JellyType.SmallTime:
                returnColor = SmallTimeColor;
                break;
            case DataType.JellyType.BigTime:
                returnColor = BigTimeColor;
                break;
            case DataType.JellyType.Coin:
                returnColor = CoinColor;
                break;
            default:
                break;
        }
        return returnColor;
    }

    void ReturnPos(int _idx)
    {
        texts[_idx].gameObject.SetActive(false);
        texts[_idx].text = "";
        texts[_idx].color = Color.white;
        textActiveCheck[_idx] = false;
        textTime[_idx] = 0;
    }
}
