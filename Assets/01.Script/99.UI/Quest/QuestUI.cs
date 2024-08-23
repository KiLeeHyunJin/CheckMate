using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    QuestData questData;
    [SerializeField] UserDataController userData;

    [Header("QuestInfo")]
    [SerializeField] TextMeshProUGUI[] textMeshPro;

    [Header("ClearPopUp")]
    [SerializeField] GameObject clearPopUp;
    [SerializeField] TextMeshProUGUI clearPopUptext;

    [Header("Reward")]
    [SerializeField] RectTransform[] rectTransforms;
    [SerializeField] RewardIconEntry rewardEntry;

    RewardIconEntry[] currentArrayEntry;

    StageQuestEntry currentQuest;
    QuestEntry[] currentEntry;
    QuestEntry[] entries;



    // Start is called before the first frame update
    void Start()
    {
        if (userData == null)
            userData = FindObjectOfType<UserDataController>();

        questData = userData.questData;
        InitText();
        if(clearPopUp != null)
            clearPopUp.SetActive(false);
        //Invoke("InitText", 1);
    }
    void InitText()
    {
        TextObjectOff();
        currentQuest = questData.GetCurrenQuest();
        TextObjectOn();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        InitQuestMessage();
        ClearPopUp();
    }

    void ClearPopUp()
    {
        if (clearPopUp == null)
            return;
        currentQuest = questData.GetCurrenQuest();
        bool clearCheck = currentQuest.isReward;
        if(clearCheck)
            clearPopUp.SetActive(true);
        questData.GiveQuestReward();
        RewardUI();
    }

    public void CloseQuestPopUp()
    {
        if (clearPopUp == null)
            return;
        questData.NextQuest(); //인덱스 번호 증가 및 새로운 퀘스트 전달 및 카운트 초기화
        GameManager.isQuestClear = false;
        RewardEntryAllOff();
        clearPopUp.SetActive(false);
    }

    void RewardUI()
    {
        if(rewardEntry != null)
        {
            RewardEntryAllOff();

            RewardEntry[] entry = questData.GetRewardEntryList();

            if (entry == null)
                return;

            for (int i = 0; i < entry.Length; i++)
            {
                currentArrayEntry = new RewardIconEntry[entry.Length];

                if (rectTransforms[i] != null || entry[i] != null)
                {
                    RewardEntry reward = entry[i];
                    currentArrayEntry[i] = Instantiate(rewardEntry, rectTransforms[i]);
                    currentArrayEntry[i].SetData(reward.rewardCount, false, reward.rewardType);
                }
            }
        }
    }

    void RewardEntryAllOff() //보상 객체 전부 파괴 및 위치 전부 Off
    {
        if(rectTransforms != null)
        {
            for (int i = 0; i < rectTransforms.Length; i++)
            {
                if (currentArrayEntry != null)
                {
                    for (int j = 0; j < currentArrayEntry.Length; j++)
                    {
                        if (currentArrayEntry[i] != null)
                            Destroy(currentArrayEntry[j].gameObject);
                    }
                    currentArrayEntry = null;
                }

                if (rectTransforms[i] != null)
                    rectTransforms[i].gameObject.SetActive(false);
            }
        }
    }

    void InitQuestMessage()
    {
        if (currentQuest == null || textMeshPro != null)
            return;

        if(currentEntry == null)
            currentEntry = currentQuest.questEntries;
        else
            entries = currentQuest.questEntries;
        Re:
        if(currentEntry == entries)
        {
            if (currentEntry != null)
            {
                for (int i = 0; i < currentEntry.Length; i++)
                {
                    string questText = "";
                    DataType.QuestType questType = currentEntry[i].type;
                    QuestMessage(i, currentEntry[i], questType, questText);
                }
            }
        }
        else
        {
            InitText();
            currentEntry = entries;
            goto Re;
        }
    }

    void QuestMessage(int _index, QuestEntry _entry,DataType.QuestType _type, string _text)
    {
        if (textMeshPro.Length < _index)
            return;

        int CurrentCount = _entry.iCurrentCount;
        int TargetCount = _entry.iTargetCount;
        TextMeshProUGUI textMeshEntry = textMeshPro[_index];
        switch (_type)
        {
            case DataType.QuestType.Slide:
                _text += "슬라이드 횟수를 달성하시오";
                break;
            case DataType.QuestType.Jump:
                _text += "점프 횟수를 달성하시오";
                break;
            case DataType.QuestType.Double:
                _text += "더블 점프 횟수를 달성하시오";
                break;
            case DataType.QuestType.Fever:
                _text += "피버 횟수를 달성하시오";
                break;
            case DataType.QuestType.Skill:
                _text += "스킬 횟수를 달성하시오";
                break;
            case DataType.QuestType.Buy:
                _text += "아이템 구매 횟수를 달성하시오";
                break;
            case DataType.QuestType.LevelUp:
                _text += "플레이어 레벨 업을 달성하시오";
                break;
            case DataType.QuestType.ItemLevelUp:
                _text += "아이템 레벨 업을 달성하시오";
                break;
            case DataType.QuestType.StageScore:
                if (userData != null)
                {
                    CurrentCount = (int)userData.GetStageData().GetStageData(_entry.iStageNum, _entry.iChapterNum).highScore;
                }
                _text += _entry.iChapterNum + " 챕터 " + _entry.iStageNum + " 스테이지" + "의 목표점수를 달성하시오";
                break;
            case DataType.QuestType.END:
                _text += "This Quest is Null Data.";
                break;
            default:
                _text += "This Quest is Null Data.";
                break;
        }
        if ( _entry.isClear || CurrentCount >= TargetCount)
        {
            textMeshEntry.fontStyle = FontStyles.Strikethrough;
            _text += " (" + TargetCount + " / " + TargetCount + ") ";
        }
        else
        {
            textMeshEntry.fontStyle = FontStyles.Normal;
            _text += " (" + CurrentCount + " / " + TargetCount + ") ";
        }

        textMeshEntry.text = _text;
    }

    void TextObjectOff()
    {
        for (int i = 0; i < textMeshPro.Length; i++)
            textMeshPro[i].gameObject.SetActive(false);
    }

    void TextObjectOn()
    {
        if (textMeshPro == null || currentQuest == null || currentQuest.questEntries == null)
            return;
        for (int i = 0; i < currentQuest.questEntries.Length; i++)
        {
            if (textMeshPro.Length > i)
                textMeshPro[i].gameObject.SetActive(true);
            else
                break;
        }
    }
}
