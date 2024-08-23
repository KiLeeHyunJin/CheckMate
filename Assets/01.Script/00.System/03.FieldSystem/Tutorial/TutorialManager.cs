using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject jumpInfo;

    [SerializeField] GameObject doubleJumpInfo;

    [SerializeField] GameObject slideInfo;

    [SerializeField] Image feverInfoImg;
    [SerializeField] Sprite[] feverInfoSprite;
    [SerializeField] GameObject[] feverInfo;

    [SerializeField] InGameManager inGame;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D playerRigid;

    [HideInInspector] public bool isJump;
    [HideInInspector] public bool isDoubleJump;
    [HideInInspector] public bool isSlide;

    int feverInfoLenth;
    int currentFeverInfoCount;
    bool isClear;
    bool isCheck;

    void Start()
    {
        isCheck = false;
        if(feverInfoSprite.Length == 0)
        {
            feverInfoLenth = feverInfo.Length;
        }
        else
        {
            feverInfoLenth = feverInfoSprite.Length;
        }
        currentFeverInfoCount = 0;
    }

    private void LateUpdate()
    {
        if(!isCheck)
        {
            isClear = !(inGame.GetClearCheck());
            isCheck = true;
        }
    }

    public void ActiveJumpInfo()
    {
        if (isClear && jumpInfo != null)
        {
            jumpInfo.SetActive(true);
            SettingState(true);
            isJump = true;
        }
    }

    public void ActiveDoubleJumpInfo()
    {
        if (isClear && doubleJumpInfo != null )
        {
            doubleJumpInfo.SetActive(true);
            SettingState(true);
            isDoubleJump = true;
        }
    }

    public void ActiveSlideInfo()
    {
        if (isClear && slideInfo != null)
        {
            slideInfo.SetActive(true);
            SettingState(true);
            isSlide = true;
        }
    }

    public void ActiveFeverInfo()
    {
        if(isClear && feverInfo != null && feverInfo[currentFeverInfoCount] != null)
        {
            if(feverInfoImg != null && feverInfoSprite != null && feverInfoSprite.Length != 0)
            {
                if (feverInfoSprite[currentFeverInfoCount] != null)
                {
                    feverInfoImg.gameObject.SetActive(true);
                    feverInfoImg.sprite = feverInfoSprite[currentFeverInfoCount];
                }
                else
                    return;
            }
            else if(feverInfo != null &&feverInfo.Length != 0) 
            {
                feverInfo[currentFeverInfoCount].SetActive(true);
            }
            SettingState(true);
        }
    }

    public void NextFeverInfo()
    {
        bool isEnd = false;
        if (feverInfoImg != null && feverInfoSprite != null && feverInfoSprite.Length != 0)
        {
            re:
            currentFeverInfoCount++;
            if (feverInfoLenth > currentFeverInfoCount)
            {
                if (feverInfoSprite[currentFeverInfoCount] == null)
                    goto re;
                feverInfoImg.sprite = feverInfoSprite[currentFeverInfoCount];
            }
            else
                isEnd = true;
        }
        else
        {
            if (feverInfo == null)
                return;
            if (feverInfo[currentFeverInfoCount] != null)
                feverInfo[currentFeverInfoCount].SetActive(false);
            re:
            currentFeverInfoCount++;
            if (feverInfoLenth > currentFeverInfoCount)
            {
                if (feverInfo[currentFeverInfoCount] == null)
                    goto re;
                feverInfo[currentFeverInfoCount].SetActive(true);
            }
            else
                isEnd = true;
        }
        if(isEnd)
            SettingState(false);
    }

    public void DisJumpInfo()
    {
        if (CheckTutoState() && jumpInfo != null && isJump)
        {
            jumpInfo.SetActive(false);
            SettingState(false);
            isJump = false;
        }
    }

    public void DisDoubleJumpInfo()
    { 
        if (CheckTutoState() && doubleJumpInfo != null && isDoubleJump)
        {
            doubleJumpInfo.SetActive(false);
            SettingState(false);
            isDoubleJump = false;
        }
    }

    public void DisSlideInfo()
    {
        if (CheckTutoState() && slideInfo != null && isSlide)
        {
            slideInfo.SetActive(false);
            SettingState(false);
            isSlide = false;
        }
    }

    void SettingState(bool _state)
    {
        if(inGame != null)
            inGame.isTuto = _state;
        if(animator != null)
        {
            if (_state)
            {
                animator.speed = 0;
                Time.timeScale = 0;
            }
            else
            {
                animator.speed = 1;
                Time.timeScale = 1;
            }
        }
    }

    bool CheckTutoState()
    {
        if (inGame)
            return inGame.isTuto;
        else
            return false;
    }
}
