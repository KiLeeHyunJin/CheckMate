using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageClick : MonoBehaviour
{
    [SerializeField] BGMmanager bGMmanager;
    [SerializeField] ModeReadyPlayerButton modeReady;
    [SerializeField] RectTransform clickUI;
    [SerializeField] float readySpeed;
    [SerializeField] Vector2 difPos;
    float clickUIXPos = 2000;
    [SerializeField] TextMeshProUGUI currentStageTxt;
    [SerializeField] TextMeshProUGUI currentStageHighScoreTxt;

    [SerializeField] GameObject character;
    [SerializeField] RectTransform[] stages;
    [SerializeField] Sprite selectStageImage;
    [SerializeField] RectTransform contents;
    [SerializeField] Image aim;
    [SerializeField] Animator aimAnim;
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] Vector2 aimPosition;
    [SerializeField] Vector2 destination;
    [SerializeField] Vector2 startPosition;
    [SerializeField] Vector2 currentPosition;
    [SerializeField] Vector2 movePosition;

    Vector2 currentCharacterPosition;
    Vector2 moveCharacterPosition;
    Vector2 destinationCharacterPosition;
    Vector2 startCharacterPosition;
    [Range(0.1f, 1)]
    [SerializeField] float AnimSpeed;
    [SerializeField] float moveCharacterSpeed;
    [SerializeField] float CharacterDistance;
    [SerializeField] int SelectCharacterStageNum;
    [SerializeField] int CharacterCurrentStageNum;
    [SerializeField] int CharacterMoveStageNum;
    [SerializeField] bool isCharacterMove;


    [SerializeField] float moveSpeed;
    [SerializeField] float distance;
    [SerializeField] int selectStageNum;
    int beforeSelectStageNum;
    [SerializeField] float aimDistanceY;

    [SerializeField] int currentStageNum;
    [SerializeField] int moveStageNum;
    [SerializeField] bool isMove;

    [SerializeField] float currentDistanceMag;
    [SerializeField] float currentDistance;
    [SerializeField] float AnimSpeedCheck;
    [SerializeField] float OkDistance;

    [SerializeField] Sprite normalImage;
    [SerializeField] Sprite selectImage;
    [SerializeField] Button nextStageButton;
    [SerializeField] UserDataController dataController;

    bool isStageClick = false;
    void Start()
    {
        int characterNum = GameManager.instance.GetPlayCharacterNum();
        DataType.CharacterType OnCharacterType;
        if (characterNum != 99 && UserDataController.Instance.characterSystem.CharacterChecking(characterNum))
        {
            CharacterEntry Temp = UserDataController.Instance.characterSystem.Entries[characterNum];
            OnCharacterType = Temp.Character.CharacterType;
            aim.sprite = CharacterImage(OnCharacterType);
        }
        else
        {
            aim.sprite = CharacterImage(DataType.CharacterType.HeartQueen);
        }
        currentStageNum = selectStageNum = CharacterMoveStageNum = SelectCharacterStageNum = CharacterCurrentStageNum = currentStageNum = GameManager.instance.GetStageNum();
        if (currentStageNum == 99)
            currentStageNum = 0;
        Image image = stages[currentStageNum].GetComponent<Image>();
        if (image != null)
            image.sprite = selectStageImage;
        if (moveSpeed <= 0)
            moveSpeed = 1;
        moveSpeed *= 80f;
        if (distance <= 0)
            distance = 1f;
        if (clickUI != null)
        {
            clickUI.anchoredPosition = new Vector3(clickUIXPos, 0, 0);
            clickUI.gameObject.SetActive(false);
            if (character != null)
                character.SetActive(false);
        }
        if (aim != null)
            aimPosition = aim.rectTransform.anchoredPosition;

        aimPosition.y += aimDistanceY;

        contents.anchoredPosition = (stages[currentStageNum].anchoredPosition * -1) + aimPosition ;
        aim.rectTransform.anchoredPosition = (stages[currentStageNum].anchoredPosition) + difPos;
    }
    Sprite CharacterImage(DataType.CharacterType OnCharacterType)
    {
        int Lenth = UserDataController.Instance.dataBase.GetCharacterLenth();
        if (aimAnim != null)
        {
            if (OnCharacterType == DataType.CharacterType.HeartQueen)
            {
                aimAnim.runtimeAnimatorController = controller;
                aimAnim.speed = AnimSpeed;
                aim.SetNativeSize();
            }
        }
        for (int i = 0; i < Lenth; i++)
        {
            if (UserDataController.Instance.dataBase.GetCharacter(i).CharacterType == OnCharacterType)
            {
                CharacterBase characterBase = UserDataController.Instance.dataBase.GetCharacter(i);
                return characterBase.CharacterSprite;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        int stageNum = GameManager.instance.GetStageNum();
        int last = dataController.GetStageData().GetClearStageNum(GameManager.instance.GetChapterNum());

        if (stageNum >= last)
        {
            nextStageButton.interactable = false;
        }
        else
        {
            nextStageButton.interactable = true;    
        }

        if(selectStageNum  != beforeSelectStageNum)
        {

        }

        if (isMove)
        {
            if (currentStageNum != selectStageNum)
            {
                MoveCharacter();
            }
            else
            {
                isMove = false;
            }
        }

        if (isCharacterMove)
        {
            if (CharacterCurrentStageNum != SelectCharacterStageNum)
            {
                MoveUserCharacter();
            }
            else
            {
                isCharacterMove = false;
            }
        }
        if (CharacterCurrentStageNum == CharacterMoveStageNum)
        {
            aimAnim.SetBool("run", false);
            aim.SetNativeSize();
            isMove = isCharacterMove = false;

            if(isStageClick)
                ClickUIActive();
        }
        else
        {
            aim.SetNativeSize();
        }
        AnimSpeedCheck = aimAnim.speed;
    }

    public void OnOffStageInfo()
    {
        if (clickUI != null)
        {
            isStageClick = true;
        }
        else
            Debug.Log("clickUI is null");
    }
    void ClickUIActive()
    {
        if (clickUI.gameObject.activeSelf)
        {
            SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);

            clickUI.anchoredPosition = new Vector3(clickUIXPos, 0, 0);
            clickUI.gameObject.SetActive(false);
            if (character != null)
                character.SetActive(false);
            if (modeReady != null)
                modeReady.SetOnStage(true);
            if (bGMmanager != null)
                bGMmanager.PlayOnMusic(BGMmanager.MusicName.bgm_Stage);
        }
        else
        {
            OpenStageInfo();
            if (bGMmanager != null)
                bGMmanager.PlayOnMusic(BGMmanager.MusicName.bgm_Ready);
        }
        isStageClick = false;
    }

    IEnumerator moveClickUI()
    {
        Vector3 Temp = new Vector3(-20, 0,0);
        Vector3 Current = Vector3.right;
        while(true)
        {
            Current = clickUI.anchoredPosition;
            Current += Temp * Time.deltaTime * readySpeed ;
            clickUI.anchoredPosition = Current;
            if (clickUI.anchoredPosition.x <= 0)
            {
                clickUI.anchoredPosition = new Vector3(0, 0, 0);
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void NextStageButton()
    {
        int getStageNum = GameManager.instance.GetStageNum();
        if (getStageNum != 99)
        {
            SetButtonNum(++getStageNum);
        }
    }
    public void SetButtonNum(int _index)
    {
        contents.anchoredPosition = (stages[CharacterCurrentStageNum].anchoredPosition * -1) + aimPosition;
        aim.rectTransform.anchoredPosition = (stages[CharacterCurrentStageNum].anchoredPosition) + difPos;

        if (_index >= 0)
        {
            int num = GameManager.instance.GetStageNum();
            Image currentStageNum = stages[num].GetComponent<Image>();
            if (currentStageNum != null)
                currentStageNum.sprite = normalImage;

            GameManager.instance.SetStageNum(_index); //스테이지 번호 저장

            SelectCharacterStageNum = _index; //캐릭터 스테이지 번호 저장
            selectStageNum = _index; //스테이지 번호 저장

            isMove = true; //움직임 차이 검사
            isCharacterMove = true; //에임 움직임 시작

            startPosition = contents.anchoredPosition;
            startCharacterPosition = aim.rectTransform.anchoredPosition;
            Image image = stages[beforeSelectStageNum].GetComponent<Image>();
            if (image != null)
                image.sprite = normalImage;
            image = stages[_index].GetComponent<Image>();
            if(image != null)
                image.sprite = selectStageImage;

            beforeSelectStageNum = _index;
            aimAnim.SetBool("run", true);
        }
        else
            Debug.Log("_index is " + _index + " StageClick.85 Line");
    }


    void MoveUserCharacter()
    {
    CharacterGo:
        int Temp = SelectCharacterStageNum - CharacterCurrentStageNum; //현재와 선택된 스테이지 번호의 차이를 구한다.
        int Dest = Temp;    //복사한다.
        Dest = Mathf.Abs(Dest); //절대값으로 변경한다.
        if (Dest > 1)  //절대값이 2이상이면
        {
            CharacterMoveStageNum = (CharacterCurrentStageNum + (Temp / Dest)); //현재 위치 +- 1
        }
        else
        {
            CharacterMoveStageNum = CharacterCurrentStageNum + Temp; //현재위치 +- 1
        }
        destinationCharacterPosition = ((stages[CharacterMoveStageNum].anchoredPosition) + difPos); //목표 위치 = 해당 버튼의 위치
        currentCharacterPosition = aim.rectTransform.anchoredPosition; //현재 캐릭터 위치 = 에임 위치
        moveCharacterPosition = ((destinationCharacterPosition - startCharacterPosition).normalized) * moveSpeed; //움직이는 캐릭터 위치 = (목표 - 시작위치) 균일화 * 속도
        Vector3 localScale = aim.rectTransform.localScale;
        if(moveCharacterPosition.x * aim.rectTransform.localScale.x < 0) //하나라도 음수라면
        {
            localScale.x *= -1;
            aim.rectTransform.localScale = localScale;
        }

        //if (moveCharacterPosition.x > 0)
        //{
        //    aim.rectTransform.localScale = localScale;
        //}
        //else
        //{
        //    if (aim.rectTransform.localScale.x > 0)
        //    {
        //        localScale.x *= -1;
        //    }
        //    aim.rectTransform.localScale = localScale;
        //}

        float Distance = Vector2.Distance(aim.rectTransform.anchoredPosition, destinationCharacterPosition);

        if (CharacterCurrentStageNum != CharacterMoveStageNum && Distance <= OkDistance)
        {
            aim.rectTransform.anchoredPosition = destinationCharacterPosition;
            currentCharacterPosition = aim.rectTransform.anchoredPosition;
            CharacterCurrentStageNum = CharacterMoveStageNum;
            startCharacterPosition = aim.rectTransform.anchoredPosition;
            if (CharacterCurrentStageNum != SelectCharacterStageNum)
                goto CharacterGo;
        }
        aim.rectTransform.anchoredPosition += moveCharacterPosition * Time.deltaTime;

    }

    void StageDifferentCheck()
    {
    go:
        int Temp = selectStageNum - currentStageNum; // 현재와 선택 번호 차이 
        int Dest = Temp;
        Dest = Mathf.Abs(Dest);
        if (Dest > 1)
        {
            moveStageNum = (currentStageNum + (Temp / Dest));
        } //moveStageNum = 움직일 넘버 전달
        else
        {
            moveStageNum = currentStageNum + Temp;
        }

        destination = ((stages[moveStageNum].anchoredPosition) * -1) + aimPosition + difPos; //목표 = 스테이지 위치 + 에임 포지션
        currentPosition = contents.anchoredPosition; //
        movePosition = ((destination - startPosition).normalized) * moveSpeed;
        //movePosition.x = stages[moveStageNum].anchoredPosition.x - contents.anchoredPosition.x;
        //movePosition.y = stages[moveStageNum].anchoredPosition.y - contents.anchoredPosition.y;

        currentDistance = Vector2.Distance(contents.anchoredPosition, destination);
        currentDistanceMag = movePosition.magnitude * Time.deltaTime + distance;

        if (currentStageNum != moveStageNum && currentDistance <= OkDistance)
        {
            contents.anchoredPosition = destination;
            currentPosition = contents.anchoredPosition;
            currentStageNum = moveStageNum;
            startPosition = contents.anchoredPosition;
            if (currentStageNum != selectStageNum)
                goto go;
        }
    }

    void MoveCharacter()
    {
        StageDifferentCheck();
        contents.anchoredPosition += movePosition * Time.deltaTime;
    }

    void OpenStageInfo()
    {
        clickUI.gameObject.SetActive(true);
        StartCoroutine(moveClickUI());
        if (modeReady != null)
            modeReady.DefaultWinodw();
        if (character != null)
            character.SetActive(true);
        int chapterNum = GameManager.instance.GetChapterNum();
        if (currentStageTxt != null)
            currentStageTxt.text = "Stage " + chapterNum + " - " + (selectStageNum + 1);
        if (currentStageHighScoreTxt != null)
            currentStageHighScoreTxt.text = UserDataController.Instance.GetStageData().GetStageData(selectStageNum, chapterNum).highScore.ToString();

    }
}
