using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StagePercentage : MonoBehaviour
{
    [SerializeField] UserDataController userData;
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI percentTxt;
    [SerializeField] Slider slider;
    [SerializeField] Image fill;
    [SerializeField] float stageLenth;
    [SerializeField] float clearStage;
    [SerializeField] float percentage;
    [SerializeField] int chapter;

    private void LateUpdate()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        if (userData == null)
            userData = FindObjectOfType<UserDataController>();

        if(gameManager != null)
            chapter = gameManager.GetChapterNum();

        if (chapter == 99 && chapter == 0)
            chapter = 1;

        if(userData != null)
        {
            stageLenth = userData.GetStageData().GetStageLenth(chapter);
            clearStage = userData.GetStageData().GetClearStageNum(chapter);

            percentage = clearStage / (stageLenth);

            if(percentTxt != null)
            {
                percentTxt.text = (percentage * 100).ToString() + "%";
            }

            if (slider != null)
            {
                if (percentage != 0)
                    slider.value = percentage;
                else
                    fill.GetComponent<Image>().enabled = false;
            }
        }
    }
}
