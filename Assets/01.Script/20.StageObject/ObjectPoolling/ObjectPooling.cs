using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [Header("Instantiate Count")]
    [SerializeField] GameObject readyPos;
    [SerializeField] float m_fNextReadyPos;
    [SerializeField] float m_fOffPos;
    [SerializeField] SampleScrolling scrolling;
    public GameObject EndPos;


    [SerializeField] TileSetting tiles;
    [SerializeField] ObstacleSetting obstacles;
    [SerializeField] JellySetting jellys;

    public bool isCurrentEnd;
    bool isSetting;

    // Start is called before the first frame update
    void Start()
    {
        isSetting = false;

        SetItemCount();

        jellys.InitArray();
        tiles.InitArray();
        obstacles.InitArray();
    }

    //private void OnEnable()
    //{
    //    //if (isSetting)
    //    {

    //    }
    //}

    // Update is called once per frame
    void LateUpdate()
    {
        if (InGameManager.instance.isCount || InGameManager.instance.isGameover)
            return;
        
        jellys.ReturnObject();
        tiles.ReturnObject();
        obstacles.ReturnObject();

        EndPosCheck();
    }

    void SetItemCount()
    {
        obstacles.ObjectInstantiate(scrolling);
        jellys.ObjectInstantiate(scrolling);
        tiles.ObjectInstantiate(scrolling);

        isSetting = true;
    }

    void EndPosCheck()//패턴 종료 확인
    {
        if (EndPos != null)
        {
            if (!isCurrentEnd && EndPos.transform.position.x <= m_fNextReadyPos)
            {
                isCurrentEnd = true;
            }
            else if (EndPos.transform.position.x <= m_fOffPos)
            {
                transform.position = readyPos.transform.position;
                gameObject.SetActive(false);
            }
        }
    }


    


}
