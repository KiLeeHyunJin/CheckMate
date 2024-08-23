using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameFeverItem : MonoBehaviour
{
    public struct FeverCoin
    {
        public bool isActive;
        public DataType.FeverCoinType type;
        public Image feverImage;
    }
    [SerializeField] Image[] feverImages;
    FeverCoin[] feverImagesEntries = null;
    public static InGameFeverItem instance;
    [SerializeField] InGameManager inGameManager;
    [SerializeField] PlayerEffectController playerEffectControll;
    [SerializeField] UserDataController userData;
    [SerializeField] int feverCoinGetCount;
    [SerializeField] float flickrSpeed;
    [SerializeField] float AlphaValue;
    int feverCoinCount = 0;
    bool isPlus;
    public bool isReset { get; set; }
    public int feverCount { get; set; }
    public bool isFever { get; set; }
    void Start()
    {
        if (flickrSpeed == 0)
            flickrSpeed = 1.2f;
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        isFever = false;
        feverImagesEntries = new FeverCoin[(int)DataType.FeverCoinType.END];
        feverCoinCount = feverImagesEntries.Length;
        if (userData == null)
            userData = UserDataController.Instance;
        for (int i = 0; i < feverImages.Length; i++)
        {
            Color color = feverImages[i].color;
            color.a = 0;
            feverImages[i].color = color;
        }

        for (int i = 0; i < feverImagesEntries.Length; i++)
        {
            feverImagesEntries[i].feverImage = feverImages[i];
            feverImagesEntries[i].type = (DataType.FeverCoinType)i;
            feverImagesEntries[i].isActive = false;
            if(feverImagesEntries[i].feverImage.gameObject.activeSelf)
                feverImagesEntries[i].feverImage.gameObject.SetActive(true);
        }
        if (playerEffectControll != null)
            playerEffectControll.feverItem = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameManager.instance.isFeverState)
        {
            if (!isReset)
            {
                BoostState();
                return;
            }
        }
        if (CheckFever())
        {
            if (playerEffectControll != null)
            {
                //playerCharacter.GetFeverEffect();//피버 발동
                playerEffectControll.SuccessFever();
                FeverReset();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
            AbsoluteFever();

    }
    public void BoostState()
    {
        if(feverImagesEntries != null)
        {
            Color color = new Color();
            bool isCheck = true;
            float speed = Time.deltaTime * flickrSpeed;
            for (int i = 0; i < feverImagesEntries.Length; i++)
            {
                if(feverImagesEntries[i].feverImage != null)
                {
                    if (i == 0 && feverImagesEntries[i].feverImage != null)
                    {
                        color = feverImagesEntries[0].feverImage.color;
                        color.a = Calculate(color, speed);
                        isCheck = false;
                    }
                    else if(isCheck && feverImagesEntries[i].feverImage != null)
                    {
                        color = feverImagesEntries[i].feverImage.color;
                        color.a = Calculate(color, speed);
                        isCheck = false;
                    }
                    feverImagesEntries[i].feverImage.color = color;
                }
            }
        }


    }
    float Calculate(Color color,float speed)
    {
        if (color.a >= 0.9f && isPlus)
        {
            isPlus = false;
        }
        else if (color.a < 0.1f && !isPlus)
        {
            isPlus = true;
        }
        switch (isPlus)
        {
            case true:
                color.a += speed;
                break;
            case false:
                color.a -= speed;
                break;
        }
        return AlphaValue = color.a;
    }

    public void AbsoluteFever()
    {
        int Temp = 0;
        for (int i = 0; i < feverImagesEntries.Length; i++)
        {
            if (feverImagesEntries[i].type == (DataType.FeverCoinType)i)
            {
                feverImagesEntries[i].isActive = true;
            }
            if (feverImagesEntries[i].isActive == true)
            {
                ++Temp;
            }
            if (Temp == feverCoinCount)
            {
                isFever = true;
            }
        }
    }
    public bool CheckFever()
    {
        for (int j = 0; j < feverImagesEntries.Length; j++)
        {
            feverImagesActive(j);
        }
        //for (int i = 0; i < feverImagesEntries.Length; i++)
        //{
        //    if(i> feverCoinCount)
        //        feverImagesEntries[i].feverImage.SetActive(true);
        //    else
        //        feverImagesEntries[i].feverImage.SetActive(false);
        //}
        for (int i = 0; i < feverImagesEntries.Length; i++)
        {
            if (feverImagesEntries[i].isActive == false)
                return false;
        }
        return true;
    }
    public void AddFeverItem(DataType.FeverCoinType _feverCoinType)
    {
        if(playerEffectControll.isFever)
        {
            if (!playerEffectControll.isReset)
                return;
        }
        feverCoinGetCount = 0;
        for (int i = 0; i < feverImagesEntries.Length; i++)
        {
            if(feverImagesEntries[i].type == _feverCoinType)
            {
                feverImagesEntries[i].isActive = true;
            }
            if(feverImagesEntries[i].isActive == true)
            {
                ++feverCoinGetCount;
            }
        }
        if (feverCoinGetCount == feverCoinCount)
        {
            isFever = true;
        }
    }
    public void FeverReset()
    {
        for (int i = 0; i < feverImagesEntries.Length; i++)
        {
            feverImagesEntries[i].isActive = false;
        }
        feverCoinGetCount = 0;
        isPlus = true;
    }
    void feverImagesActive(int _num)
    {
        Color color = feverImagesEntries[_num].feverImage.color;

        if (feverImagesEntries[_num].isActive == true)
        {
            color.a = 1;
        }
        else
        {
            color.a = 0;
        }

        feverImagesEntries[_num].feverImage.color = color;

        //if (_num < feverCoinGetCount)
        //    feverImagesEntries[_num].feverImage.SetActive(true);
        //else
        //    feverImagesEntries[_num].feverImage.SetActive(false);
        //feverImagesEntries[_num].isActive = true;
    }
}
