using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class RewardEntry
{
    public DataType.RewardType rewardType;
    public int rewardCount;
    public void SetData(DataType.RewardType _rewardType ,int _rewardCount)
    {
        this.rewardType = _rewardType;
        this.rewardCount = _rewardCount;
    }
}

[Serializable]
public class QuestEntry //퀘스트 소단위
{
    public DataType.QuestType type; //퀘스트 내용
    public int iTargetCount;              //목표 값
    public int iCurrentCount;       //현재 값
    [Header("if type is Score")]
    public int iChapterNum;
    public int iStageNum;
    public bool isClear = false;    //소단위 퀘스트 클리어 여부
}

[Serializable]
public class StageQuestEntry    //퀘스트 단계
{
    public QuestEntry[] questEntries;   //소단위 퀘스트 
    public RewardEntry[] rewardEntries;
    public bool isClear = false;    //해당 퀘스트 종합 완료 여부
    public bool isReward = false;
}

[Serializable]
public class QuestData
{
    [SerializeField] StageQuestEntry[] questEntries;
    [SerializeField] StageQuestEntry currentQuest;
    [SerializeField] int currentQuestNum;
    [HideInInspector] public UserDataController Owner;
    public void NextQuest()
    {
        if (questEntries.Length - 1 > currentQuestNum)
        {
            currentQuestNum++;
            currentQuest = questEntries[currentQuestNum];
        }
        Owner.countData.Reset();
    }
    public void SetCurrentQuest(int _num)//퀘스트 배열 설정
    {
        if (questEntries.Length > _num)
        {
            currentQuest = questEntries[_num];
            currentQuestNum = _num;
            return;
        }
        Debug.Log("questEntries index is Over. questEntries index is " + questEntries.Length);
    }
    public int GetQuestIndex()
    {
        return currentQuestNum;
    }
    public RewardEntry[] GetRewardEntryList()
    {
        if(currentQuest != null || currentQuest.rewardEntries != null)
            return currentQuest.rewardEntries;
        return null;
    }
    public StageQuestEntry GetCurrenQuest()//현재 퀘스트 가져오기
    {
        if (currentQuestNum >= questEntries.Length)
            return null;
        return questEntries[currentQuestNum];
    }
     
    public void GiveQuestReward()
    {
        if(currentQuest != null || Owner != null)
        {
            if(currentQuest.questEntries != null)
            {
                if(currentQuest.rewardEntries != null)
                {
                    for (int i = 0; i < currentQuest.rewardEntries.Length; i++)
                    {
                        if(currentQuest.rewardEntries[i] != null)
                        {
                            switch (currentQuest.rewardEntries[i].rewardType)
                            {
                                case DataType.RewardType.Gold:
                                    Owner.Money += currentQuest.rewardEntries[i].rewardCount;
                                    break;
                                case DataType.RewardType.Diamond:
                                    Owner.Cash += currentQuest.rewardEntries[i].rewardCount;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }



    public bool QuestClearCheck()//퀘스트 완료 체크
    {
        bool isNullCheck = QuestNullCheck();//null 체크
        if (isNullCheck)
        {
            ListClearCheck();//소단위 클리어 체크

            for (int i = 0; i < currentQuest.questEntries.Length; i++)
            {
                if ((currentQuest.questEntries[i].isClear) == false)
                    return false;
            }
            return true;
        }
        else
            Debug.Log("currentQuest is null. QuestData.cs 50 Line");

        return false;
    }

    public bool QuestNext()//다음 퀘스트로 이동
    {
        bool isNotNull = QuestNullCheck();
        if (isNotNull)
        {
            if (currentQuestNum > questEntries.Length)
            {
                currentQuestNum++;  //인덱스 번호 증가
                currentQuest = questEntries[currentQuestNum]; //새로운 퀘스트 전달

                if (Owner == null)
                    Owner = UserDataController.Instance;

                Owner.countData.Reset(); //퀘스트 카운트 초기화

                return true;
            }
            return false;
        }
        return false;
    }

    public bool QuestNullCheck()//null 값 체크
    {
        if (currentQuest == null)
            return false;
        return true;
    }

    bool ListClearCheck()//소단위 클리어 체크
    {
        bool isfalse = false;
        for (int i = 0; i < currentQuest.questEntries.Length; i++)
        {
            QuestEntry quest = currentQuest.questEntries[i];

            int currentCount = quest.iCurrentCount;
            int clearCount = quest.iTargetCount;

            if(quest.type == DataType.QuestType.StageScore)
            {
                if(Owner.GetStageData() != null)
                {
                    if(Owner.GetStageData().GetStageData(quest.iStageNum, quest.iChapterNum) != null)
                    {
                        quest.iCurrentCount = (int)Owner.GetStageData().GetStageData(quest.iStageNum, quest.iChapterNum).highScore;
                    }
                }
            }

            if (currentCount >= clearCount) //조건을 달성했다면
            {
                quest.isClear = true;
            }
            else //조건 달성 실패일 경우
            {
                quest.isClear = false;
                isfalse = true;
            }
        }
        return !isfalse;
    }
}

