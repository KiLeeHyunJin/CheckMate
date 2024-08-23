using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField] bool isChapter;
    [SerializeField] SceneChanger sceneChanger;
    [SerializeField] UserDataController userData;
    [SerializeField] GameObject nullCharacterWindow;
    [SerializeField] StageClick moveCharaterStageNum;
    [SerializeField] GameObject disableImage;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI clearLevel;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] int stageNum;
    [SerializeField] int chapterNum;
    [SerializeField] Sprite disImage;
    Image thisImage;
    public string ChapterSceneName;
    public string StageSceneName;
    bool isCheckEnter;
    bool isCheckCharacter;
    bool isNoSelectLobbyCharacter;
    void Awake()
    {
        if (disableImage == null)
            disableImage = gameObject;
        if (userData == null)
            userData = UserDataController.Instance;
        if (button == null)
            button = gameObject.GetComponent<Button>();
        //button.gameObject.SetActive(false);
        disableImage.SetActive(false);
        //disableImage.SetActive(true);
        thisImage = GetComponent<Image>();

    }
    private void Start()
    {
        isCheckCharacter = isCheckEnter = false;
    }
    void CheckCharacter()
    {
        if (isChapter)
        {
            CharacterEntry[] characters = userData.characterSystem.Entries;
            if (characters != null)
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    if (characters[i] != null)
                    {
                        if (characters[i].Character.CharacterType == DataType.CharacterType.HeartQueen)
                        {
                            if(userData.characterType != DataType.CharacterType.END)
                            {
                                isNoSelectLobbyCharacter = false;
                            }
                            else
                            {
                                isNoSelectLobbyCharacter = true;
                            }
                            isCheckEnter = true;
                            return;
                        }
                    }
                }
            }
        }
    }
    private void LateUpdate()
    {
        if(!isCheckCharacter)
        {
            CheckCharacter();
        }
    }
    public void SetStageData(int _clearLevel, int _highScore)
    {
        disableImage.SetActive(false);
        button.gameObject.SetActive(true);
        //if(clearLevel != null)
        //    clearLevel.text = _clearLevel.ToString();
        //if(score != null)
        //    score.text = _highScore.ToString();
    }
    public void SetChapterData()
    {
        button.gameObject.SetActive(true);
        disableImage.SetActive(false);
    }
    public void DisableButton()
    {
        //if (disableImage != null)
        //    disableImage.SetActive(true);
        disableImage.SetActive(false);
        thisImage.sprite = disImage;
        //Image image = this.GetComponent<Image>();
        //if(image != null)
        //    image.enabled = false;
        //Animator animator = this.GetComponent<Animator>();
        //if(animator != null)
        //    animator.enabled = false;
    }
    public void ChapterEnter()
    {
        if (isCheckEnter/* && !isNoSelectLobbyCharacter*/)//캐릭터가 있으면 통과
        {
            SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

            //transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            GameManager.instance.SetChapterNum(chapterNum);
            if (ChapterSceneName != "null" || ChapterSceneName != "")
            {
                GameManager.instance.SetSceneType(DataType.SceneType.mode_stage, DataType.SceneType.mode_select);
                SceneManager.LoadScene(ChapterSceneName);
            }
        }
        else//없으면 팝업창 활성화
        {
            if(!isCheckEnter)
            {
                if (nullCharacterWindow != null)
                    nullCharacterWindow.SetActive(true);
            }
            else
            {
                //로비에 메인 캐릭터를 세워놓지 않았습니다.
            }

        }
    }

    public void CloseNotHaveCharacterWindowButton()
    {
        if (nullCharacterWindow != null)
            nullCharacterWindow.SetActive(false);
    }
    public void StageEnter()
    {

        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        //if (moveCharaterStageNum != null)
        //{
        //    moveCharaterStageNum.SetButtonNum(stageNum);
        //}
        //transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        GameManager.instance.SetStageNum(stageNum);
        if (StageSceneName != "null" || StageSceneName != "")
        {
            SceneManager.LoadScene(StageSceneName);
        }

    }
}
