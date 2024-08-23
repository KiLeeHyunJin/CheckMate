using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{
    private void OnEnable()
    {
        string resultScore = InGameManager.instance.Getscore();
        if (resultScore != null)
            return;
        Text text = gameObject.GetComponent<Text>();
        if(text != null)
        {
            text.text = resultScore;
            return;
        }
        TextMeshProUGUI textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
        if(textMeshProUGUI !=  null)
            textMeshProUGUI.text = resultScore;
    }
}
