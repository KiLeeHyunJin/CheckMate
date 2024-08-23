using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MissionData
{
    public DataType.ItemType missionType;
    public int MissionValue;
    public float MissionTime;
} 

[Serializable]
public class ClearLevelData
{
    public int lowLevel;
    public int middleLevel;
    public int highLevel;
}

[Serializable]
public class StageDataSetting
{
    //public MissionData mission;
    public ClearLevelData level;

    public int highScore;

    [Header("Clear")]
    public bool isFirst;
    public bool isClear;
    public int clearLevel;

    [Header("LevelScore")]
    public ulong Level1Score;
    public ulong Level2Score;
    public ulong Level3Score;

    [Header("Exp")]
    public int Level1Exp;
    public int Level2Exp;
    public int Level3Exp;

    [Header("Reward")]
    public RewardEntry clearReward;
    public RewardEntry lv3Reward;
    public RewardEntry lv2Reward;
    public RewardEntry lv1Reward;
}

[Serializable]
public class GameStageData
{
    [SerializeField] StageDataSetting[] m_1StageData = new StageDataSetting[10];
    [SerializeField] StageDataSetting[] m_2StageData = new StageDataSetting[10];

    int clear1StageNum;
    int clear2StageNum;
    public void Start()
    { 
        //for (int i = 0; i < stageData.Length; i++)
        //{
        //    stageData[i] = new StageDataSetting();
        //}
    }

    public void LoadToStageData(int _arrayNum,bool _isClear, int _clearLevel, int _highScore, int _chapter, bool _isFirst)
    {
        if(_chapter == 1)
        {
            m_1StageData[_arrayNum].isClear = _isClear;
            m_1StageData[_arrayNum].clearLevel = _clearLevel;
            m_1StageData[_arrayNum].highScore = _highScore;
            m_1StageData[_arrayNum].isFirst = _isFirst;
           clear1StageNum = GetClearStageLenth(_chapter);
            Debug.Log( "Load "+ _arrayNum + " Stage Data_HighScore" + _highScore);
            return;
        }

        else if (_chapter == 2)
        {
            m_2StageData[_arrayNum].isClear = _isClear;
            m_2StageData[_arrayNum].clearLevel = _clearLevel;
            m_2StageData[_arrayNum].highScore = _highScore;
            m_2StageData[_arrayNum].isFirst = _isFirst;

            clear2StageNum = GetClearStageLenth(_chapter);
        }
    }

    public void SaveToStageData(List<int> _arrayNum, List<bool> _isClear, List<int> _clearLevel, List<int> _highScore, int _chapterNum)
    {
        if(_chapterNum == 1)
        {
            for (int i = 0; i < _arrayNum.Count; i++)
            {
                //if (_arrayNum[i] != null)
                {
                    _arrayNum.Add(i);
                    _isClear.Add(m_1StageData[i].isClear);
                    _clearLevel.Add(m_1StageData[i].clearLevel);
                    _highScore.Add(m_1StageData[i].highScore);
                }
            }
            return;
        }
        else if (_chapterNum == 2)
        {
            for (int i = 0; i < _arrayNum.Count; i++)
            {
                //if (_arrayNum[i] != null)
                {
                    _arrayNum.Add(i);
                    _isClear.Add(m_2StageData[i].isClear);
                    _clearLevel.Add(m_2StageData[i].clearLevel);
                    _highScore.Add(m_2StageData[i].highScore);
                }
            }
            return;
        }
    }

    public bool GetChpaterClearCheck(int _chapterNum)
    {
        if(_chapterNum == 1)
        {
            int stageLenth = m_1StageData.Length;
            if (m_1StageData[stageLenth].isClear)
                return true;
        }
        else if (_chapterNum == 2)
        {
            int stageLenth = m_2StageData.Length;
            if (m_2StageData[stageLenth].isClear)
                return true;
        }
        return false;
    }

    public void SetStageHightScore(int _stageNum, int _highScore, int _chapterNum)
    {
        if(_chapterNum == 1)
        {
            if (m_1StageData.Length > _stageNum)
                m_1StageData[_stageNum].highScore = _highScore;
            return;
        }
        else if (_chapterNum == 2)
            if (m_2StageData.Length > _stageNum)
                m_2StageData[_stageNum].highScore = _highScore;

    }

    public void SetStageData(int _stageNum, bool _clear, int _clearLevel, int _highScore, int _chapterNum)
    {
        if(_chapterNum == 1)
        {
            if (m_1StageData.Length < _stageNum)
                m_1StageData[_stageNum].isClear = _clear;
            m_1StageData[_stageNum].clearLevel = _clearLevel;
            m_1StageData[_stageNum].highScore = _highScore;
            return;
        }
        else if (_chapterNum == 2)
        {
            if (m_2StageData.Length < _stageNum)
                m_2StageData[_stageNum].isClear = _clear;
            m_2StageData[_stageNum].clearLevel = _clearLevel;
            m_2StageData[_stageNum].highScore = _highScore;
        }
    }
    public void GetReward(int _stageNum, int _chapterNum, int _clearLevel)
    {
        StageDataSetting stageData = null;
        int returnEXP = 0;

        if (_chapterNum == 1)
            stageData = m_1StageData[_stageNum];
        else if (_chapterNum == 2)
            stageData = m_2StageData[_stageNum];

        if (!(stageData.isClear))
            return;
    }
    public RewardEntry FirstClearReward(int _stageNum, int _chapterNum)
    {
        StageDataSetting stageData = null;
        RewardEntry reward = null;

        if (_chapterNum == 1)
            stageData = m_1StageData[_stageNum];
        else if (_chapterNum == 2)
            stageData = m_2StageData[_stageNum];
        if(stageData != null)
        {
            reward = stageData.clearReward;
        }
        return reward;
    }
    public int GetStageEXP(int _stageNum, int _chapterNum, int _clearLevel)
    {
        StageDataSetting stageData = null;
        int returnEXP = 0;

        if (_chapterNum == 1)
            stageData = m_1StageData[_stageNum];
        else if(_chapterNum == 2)
            stageData = m_2StageData[_stageNum];

        if (!(stageData.isClear))
            return 0;

        switch (_clearLevel)
        {
            case 0:
                returnEXP = stageData.Level1Exp;
                break;
            case 1:
                returnEXP = stageData.Level2Exp;
                break;
            case 2:
                returnEXP = stageData.Level3Exp;
                break;
            case 3:
                returnEXP = stageData.Level3Exp;
                break;
            default:
                break;
        }
        return returnEXP;
    }

    public int GetClearChapterNum()
    {
        if (m_1StageData[m_1StageData.Length - 1].isClear)
            return 2;
        return 1;
    }
    public RewardEntry CheckClearReward(int _clearLevel, int _stageNum, int _chapterNum)
    {
        StageDataSetting stageData = null;
        RewardEntry reward = null;
        if (_chapterNum == 1)
            stageData = m_1StageData[_stageNum];
        else
            stageData = m_2StageData[_stageNum];

        if (stageData != null)
        {
            switch (_clearLevel)
            {
                case 1:
                    reward = stageData.lv1Reward;
                    break;
                case 2:
                    reward = stageData.lv2Reward;
                    break;
                case 3:
                    reward = stageData.lv3Reward;
                    break;
                default:
                    break;
            }
            #region
            //if (_score >= stageData.Level1Score && stageData.clearLevel <= 1)
            //{
            //    stageData.clearLevel = 1;
            //    reward = stageData.lv1Reward;
            //}
            //else if (_score >= stageData.Level2Score && stageData.clearLevel <= 2)
            //{
            //    stageData.clearLevel = 2;
            //    reward = stageData.lv2Reward;
            //}
            //else if (_score >= stageData.Level3Score && stageData.clearLevel <= 3)
            //{
            //    stageData.clearLevel = 3;
            //    reward = stageData.lv3Reward;
            //}
            #endregion
        }
        return reward;
    }
    public int[] CheckClear(ulong _score, int _stageNum, int _chapterNum)
    {
        StageDataSetting stageData = null;
        int rewardGold = 0;
        int rewardExp = 0;
        int[] returnvalue = {rewardExp, rewardGold};

        if(_chapterNum == 1)
            stageData = m_1StageData[_stageNum];
        else
            stageData = m_2StageData[_stageNum];

        if(stageData != null)
        {
            if(_score >= stageData.Level1Score && stageData.clearLevel <= 1)
            {
                stageData.clearLevel = 1;
                returnvalue[0] = stageData.Level1Exp;
                //returnvalue[1] = stageData.lv1Gold;
            }
            else if(_score >= stageData.Level2Score && stageData.clearLevel <= 2)
            {
                stageData.clearLevel = 2;
                returnvalue[0] = stageData.Level2Exp;
                //returnvalue[1] = stageData.lv2Gold;
            }
            else if (_score >= stageData.Level3Score && stageData.clearLevel <= 3)
            {
                stageData.clearLevel = 3;
                returnvalue[0] = stageData.Level2Exp;
                //returnvalue[1] = stageData.lv3Gold;
            }
        }

        return returnvalue;
    }

    int GetClearStageLenth(int _chapterNum)
    {
        if(_chapterNum == 1)
        {
            int clearStageNum = 0;
            for (int i = 0; i < m_1StageData.Length; i++)
            {
                if (!m_1StageData[i].isClear)
                    break;
                ++clearStageNum;
            }
            return clearStageNum;
        }
        else if (_chapterNum == 2)
        {
            int clearStageNum = 0;
            for (int i = 0; i < m_2StageData.Length; i++)
            {
                if (!m_2StageData[i].isClear)
                    break;
                ++clearStageNum;
            }
            return clearStageNum;
        }
        return 0;
    }

    public int GetClearStageNum(int _chapterNum)
    {
        if (_chapterNum == 1)
        {
            return clear1StageNum = GetClearStageLenth(_chapterNum);
        }
        return clear2StageNum = GetClearStageLenth(_chapterNum);
    }
    public int GetStageLenth(int _chapterNum)
    {
        if(_chapterNum == 1)
            return m_1StageData.Length;
        return m_2StageData.Length;
    }
    public StageDataSetting GetStageData(int _num, int _chapterNum)
    {
        if(_chapterNum == 1)
        {
            if (m_1StageData.Length > _num)
            {
                if (m_1StageData[_num] != null)
                {
                    return m_1StageData[_num];
                }
            }
        }
        else if(_chapterNum == 2)
        {
            if (m_2StageData.Length > _num)
            {
                if (m_2StageData[_num] != null)
                {
                    return m_2StageData[_num];
                }
            }
        }
        return null;
    }

}


