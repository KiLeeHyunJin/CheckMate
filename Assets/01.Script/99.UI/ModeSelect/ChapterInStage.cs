using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterInStage : MonoBehaviour
{
    [SerializeField] GameObject chapter1BG;
    [SerializeField] GameObject chapter2BG;

    int chapterNum;
    bool isCheck;
    // Start is called before the first frame update
    void Start()
    {
        isCheck = true;
        chapter1BG.SetActive(false);
        chapter2BG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheck) 
        { 
            chapterNum = GameManager.instance.GetChapterNum(); 
            if(chapterNum == 1 || chapterNum == 99)
            {
                chapter1BG.SetActive(true);
                if(chapterNum == 99)
                {
                    GameManager.instance.SetChapterNum(1);
                }
            }
            else
            {
                chapter2BG.SetActive(true);
            }

            isCheck = false; 
        }
    }
}
