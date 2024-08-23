using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardIconEntry : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject firstIcon;
    public Image typeImage;


    public void SetData(int _count, bool _isFirst, DataType.RewardType _rewardType)
    {
        SetText(_count);
        FirstImageSetActive(_isFirst);
        SetImageType(_rewardType);
    }

    void SetText(int _count)
    {
        if (text != null)
        {
            if(text.gameObject.activeSelf == false)
            {
                text.gameObject.SetActive(true);
            }
            text.text = _count.ToString();
        }
    }

    void FirstImageSetActive(bool _setValue)
    {
        if (firstIcon != null)
        {
            if (firstIcon.gameObject.activeSelf == false)
            {
                firstIcon.gameObject.SetActive(true);
            }
            firstIcon.SetActive(_setValue);
        }
    }

    void SetImageType(DataType.RewardType type)
    {
        if (typeImage != null)
        {
            if (typeImage.gameObject.activeSelf == false)
            {
                typeImage.gameObject.SetActive(true);
            }

            switch (type)
            {
                case DataType.RewardType.Gold:
                    {
                        if(GameItemDataBase.Instance.GoldRewardIcon != null)
                            typeImage.sprite = GameItemDataBase.Instance.GoldRewardIcon;
                    }
                    break;
                case DataType.RewardType.Diamond:
                    {
                        if(GameItemDataBase.Instance.DiamondRewardIcon != null)
                            typeImage.sprite = GameItemDataBase.Instance.DiamondRewardIcon;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void SelfGetComponent()
    {
    }
}
