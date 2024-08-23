using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] UserDataController userData;
    [SerializeField] InGameManager inGameManager;

    [SerializeField] Button nextStageButton;

    [SerializeField] int stageNum;
    [SerializeField] int chapterNum;

    GameStageData gameStageData;
    StageDataSetting stageData;

    bool isClearStage = false;
    int stageLenth;
    private void Start()
    {
       
        if (inGameManager == null)
            inGameManager = FindObjectOfType<InGameManager>();
    }
    private void OnEnable()
    {
        stageLenth = 0;
        stageNum = 0;
        chapterNum = 1;

        if (gameManager == null)
            gameManager = GameManager.instance;

        stageNum = gameManager.GetStageNum();
        chapterNum = gameManager.GetChapterNum();

        if (userData == null)
            userData = UserDataController.Instance;


        gameStageData = userData.GetStageData();

        if (gameStageData != null)
        {
             stageData = gameStageData.GetStageData(stageNum, chapterNum);

            if (stageData != null)
            {
                isClearStage = stageData.isClear;
                stageLenth = 3;//gameStageData.GetStageLenth(chapterNum) - 1;

                if (isClearStage)
                {
                    if (stageNum < stageLenth)
                    {
                        if (nextStageButton != null)
                        {
                            nextStageButton.interactable = true;
                            return;
                        }
                    }
                }
            }
        }
        if(nextStageButton != null)
            nextStageButton.interactable = false;
    }

    public void Restart()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        string stageName = "OvenBreak";
        if (stageNum == 99)
            stageNum = 0;
        else if (stageNum > 0)
            stageName += " " + stageNum.ToString();
        SceneManager.LoadScene(stageName);
    }

    public void NextStage()
    {
        if (isClearStage)
        {
            if (stageNum == 99)
                stageNum = 0;

            if (stageNum < stageLenth)
            {
                int nextStageNum = stageNum + 1;
                if (gameManager == null)
                    gameManager = GameManager.instance;
                if(stageLenth > nextStageNum)
                {
                    gameManager.SetStageNum(nextStageNum);
                    string stageName = "OvenBreak";
                    stageName += " " + nextStageNum.ToString();
                    SceneManager.LoadScene(stageName);
                }
                //Restart();
            }
        }
    }

}
