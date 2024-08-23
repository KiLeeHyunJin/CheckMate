using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultbestScore : MonoBehaviour
{
    private void OnEnable()
    {
        Text Score = gameObject.GetComponent<Text>();
        TextMeshProUGUI ScoreMeshPro = gameObject.GetComponent<TextMeshProUGUI>();

        if (Score != null)
            Score.text = InGameManager.instance.GetBestscore();
        else if(ScoreMeshPro != null)
            ScoreMeshPro.text = InGameManager.instance.GetBestscore();
    }
}
