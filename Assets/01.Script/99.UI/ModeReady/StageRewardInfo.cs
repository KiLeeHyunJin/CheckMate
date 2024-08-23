using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageRewardInfo : MonoBehaviour
{
    [SerializeField] RewardIconEntry rewardPrefab;
    [SerializeField] RectTransform[] rectTransforms;
    [SerializeField] UserDataController userData;
    [SerializeField] GameManager gameManager;

    RewardIconEntry firstRewardIcon = null;
    RewardIconEntry lv1RewardIcon = new RewardIconEntry();
    RewardIconEntry lv2RewardIcon = new RewardIconEntry();
    RewardIconEntry lv3RewardIcon = new RewardIconEntry();

    RewardEntry firstReward;
    RewardEntry lv1Reward;
    RewardEntry lv2Reward;
    RewardEntry lv3Reward;

    int ChapterNum;
    int StageNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        AllOffRect();
        InitReward();
    }

    void AllOffRect()
    {
        if(rectTransforms != null)
        {
            for (int i = 0; i < rectTransforms.Length; i++)
            {
                if(rectTransforms != null)
                    rectTransforms[i].gameObject.SetActive(false);
            }
        }
    }
    void ResetReward() //정보를 제거한다.
    {
        firstReward = null;
        lv1Reward = null;
        lv2Reward = null;
        lv3Reward = null;

        if(firstRewardIcon != null)
        {
            Destroy(firstRewardIcon.gameObject);
            firstRewardIcon = null;
        }

        RemoveIconArray(lv1RewardIcon);
        RemoveIconArray(lv2RewardIcon);
        RemoveIconArray(lv3RewardIcon);
    }
    void RemoveIconArray(RewardIconEntry rewardsArray) //배열의 내용을 제거한다.
    {
        if (rewardsArray != null)
        {
           if (rewardsArray != null)
               Destroy(rewardsArray.gameObject);
        }
        rewardsArray = null;
    }

    void InitReward() //스테이지 정보를 가져온다.
    {
        ResetReward();
        if (gameManager == null)
            gameManager = GameManager.instance;
        ChapterNum = gameManager.GetChapterNum();
        StageNum = gameManager.GetStageNum();
        if (userData == null)
            userData = UserDataController.Instance;
        if (userData.GetStageData() != null)
            GetRewardInfo();
    }


    void GetRewardInfo() //보상 아이템 정보를 가져온다.
    {
        StageDataSetting stageData = userData.GetStageData().GetStageData(StageNum, ChapterNum);
        if (stageData != null)
        {
            firstReward = stageData.clearReward;
            lv1Reward = stageData.lv1Reward;
            lv2Reward = stageData.lv2Reward;
            lv3Reward = stageData.lv3Reward;

            MakeRewardIcon(stageData.isClear);
        }
    }

    void MakeRewardIcon(bool isClear)
    {
        int count = 0;

        count = MakeRewardFirst(isClear, rectTransforms, count);

        count = MakeRewardIconArray(lv1Reward, lv1RewardIcon, rectTransforms[count], count);
        count = MakeRewardIconArray(lv2Reward, lv2RewardIcon, rectTransforms[count], count);
        MakeRewardIconArray(lv3Reward, lv3RewardIcon, rectTransforms[count], count);
    }

    int MakeRewardFirst(bool isClear, RectTransform[] rect, int count) //아이콘을 제작한다.
    {
        firstRewardIcon = new RewardIconEntry();
        //bool isClear = userData.GetStageData().GetStageData(ChapterNum, StageNum).isClear;
        if (firstReward != null && rect != null)
        {
                if(rect != null)
                {
                    firstRewardIcon = new RewardIconEntry();
                    rect[count].gameObject.SetActive(true);
                    firstRewardIcon = Instantiate(rewardPrefab, rect[count]);
                    firstRewardIcon.transform.localScale = Vector3.one;
                    count++;

                    //if (firstReward[i] == null)
                    //    continue;
                    firstRewardIcon.SetData(firstReward.rewardCount, true, firstReward.rewardType);


                    if (isClear)
                    {
                        Color color = new Color(0.7f, 0.7f, 0.7f, 1);
                        if (firstRewardIcon != null)
                        {
                            firstRewardIcon.typeImage.color = color;
                            firstRewardIcon.firstIcon.GetComponent<Image>().color = color;
                        }
                    }
                }

        }
        return count;
    }

    int MakeRewardIconArray(RewardEntry lvReward, RewardIconEntry lvRewardIcon, RectTransform rect, int count)
    {
        if (lvReward != null && rect != null)
        {
            //if (lvReward == null)
            //{
            //    lvRewardIcon = null;
            //    return count;
            //}
            count++;
            rect.gameObject.SetActive(true);
            lvRewardIcon = Instantiate(rewardPrefab, rect);
            lvRewardIcon.transform.localScale = Vector3.one;
            lvRewardIcon.SetData(lvReward.rewardCount, false, lvReward.rewardType);
        }
        return count;
    }




}
