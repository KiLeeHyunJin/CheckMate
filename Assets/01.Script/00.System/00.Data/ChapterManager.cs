using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
public enum  StageOrChapter
{ 
    Chapter,
    Stage,
}

public class ChapterManager : MonoBehaviour
{
    [SerializeField] StageOrChapter ManagerState;
    [SerializeField] StageSelect[] chapterButton;
    [SerializeField] StageSelect[] stageChapter1Button;
    [SerializeField] StageSelect[] stageChapter2Button;
    StageSelect[] currentStageButton;
    UserDataController Owner;
    [SerializeField] int clearStageNum;
    [SerializeField] int clearChapterNum;
    [SerializeField] int stageMaxNum;
    [SerializeField] int chapterNum;
    [SerializeField] bool isCheck = true;
    void Start()
    {
        Owner = UserDataController.Instance;
        stageMaxNum = Owner.GetStageData().GetStageLenth(chapterNum);
        if (chapterNum < 1)
        {
            chapterNum = 1;
            currentStageButton = stageChapter1Button;
        }
        else if (chapterNum < 2)
        {
            chapterNum = 2;
            currentStageButton = stageChapter2Button;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            isCheck = false;
            if (clearChapterNum == 1)
                clearChapterNum = 2;
            else
                clearChapterNum = 1;
            GameManager.instance.SetChapterNum(clearChapterNum);
        }
        if (isCheck)
        {
            clearStageNum = Owner.GetStageData().GetClearStageNum(chapterNum);
            clearChapterNum = Owner.GetStageData().GetClearChapterNum();
        }
        switch (ManagerState)
        {
            case StageOrChapter.Chapter:
                ActiveChapterButton();
                break;
            case StageOrChapter.Stage:
                ActiveStageButton();
                break;
            default:
                break;
        }
    }
    void ActiveStageButton()
    {
        if (currentStageButton != null)
        {
            if (currentStageButton.Length > 0)
            {
                if (clearStageNum >= 0)
                {
                    int MaxNum = clearStageNum;
                    for (int j = 0; j < currentStageButton.Length; j++)
                    {
                        if (j < clearStageNum)
                        {
                            currentStageButton[j].SetStageData(Owner.GetStageData().GetStageData(j, chapterNum).clearLevel, Owner.GetStageData().GetStageData(j, chapterNum).highScore);
                        }
                        else if (j == clearStageNum)
                        {
                            currentStageButton[clearStageNum].SetStageData(0, 0);
                        }
                        else
                        {
                            currentStageButton[j].GetComponent<Button>().interactable = false;
                            currentStageButton[j].DisableButton();
                        }
                    }

                    //for (int i = 0; i < clearStageNum; i++)
                    //{
                    //   }
                    //if (stageMaxNum > clearStageNum)
                }
            }
        }
    }

    void ActiveChapterButton()
    {
        if (chapterButton != null)
        {
            if (chapterButton.Length > 0)
            {
                if (clearChapterNum != 0)
                {
                    int MaxNum = clearChapterNum;
                    for (int i = 0; i <= clearChapterNum; i++)
                    {
                        if ((i  - 1 ) < 0)
                            continue;
                        chapterButton[i - 1].SetChapterData();
                    }
                    //if (stageMaxNum > clearChapterNum)
                    //    chapterButton[clearChapterNum].SetChapterData();
                }
                else
                    chapterButton[0].SetChapterData();
            }
        }
    }
}
