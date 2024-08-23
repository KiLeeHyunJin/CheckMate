using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySetting : MonoBehaviour
{
    SampleScrolling scrolling;

    //[Header("Jellys")]
    /*[HideInInspector]*/ public JellyObject clovers = new JellyObject();
    /*[HideInInspector]*/ public JellyObject spades = new JellyObject();
    /*[HideInInspector]*/ public JellyObject hearts = new JellyObject();
    /*[HideInInspector]*/ public JellyObject diamonds = new JellyObject();

    //[Header("Coins")]
    /*[HideInInspector]*/ public JellyObject blacks = new JellyObject();
    /*[HideInInspector]*/ public JellyObject whites = new JellyObject();
    /*[HideInInspector]*/ public JellyObject blues = new JellyObject();
    /*[HideInInspector]*/ public JellyObject reds = new JellyObject();
    /*[HideInInspector]*/ public JellyObject yellows = new JellyObject();
    /*[HideInInspector]*/ public JellyObject greens = new JellyObject();

    //[Header("Item")]
    /*[HideInInspector]*/ public JellyObject smalls = new JellyObject();
    /*[HideInInspector]*/ public JellyObject bigs = new JellyObject();
    /*[HideInInspector]*/ public JellyObject coins = new JellyObject();

    [HideInInspector] public List<JellyObject> allItem = new List<JellyObject>();

    [SerializeField] GameObject parentObj;
    public float m_fReturnXPos;
    [SerializeField] int m_jellyCount;
    [SerializeField] int m_feverCount;

    public void InitArray()//위치 초기화
    {
        Vector3 scale = Vector3.one;
        for (int i = 0; i < allItem.Count; i++)
        {
            if(allItem[i].copy != null)
            {
                for (int j = 0; j < allItem[i].copy.Length; j++)
                {
                    scale = allItem[i].objData.objectPosition[j].localScale;
                    allItem[i].copy[j].transform.position = allItem[i].objData.objectPosition[j].position;
                    allItem[i].copy[j].transform.localScale = scale;
                }
                allItem[i].objData.CurrentCount = allItem[i].copy.Length; //현재 복사 객체가 위치한 끝 번호 
                allItem[i].objData.CopyCount = 0; //현재 가장 앞에있는 객체 번호
                allItem[i].objData.FrontCount = 0;
            }
        }
    }
    public void ReturnObject()//오브젝트 회수
    {
        for (int i = 0; i < allItem.Count; i++)
        {
            JellyObject jelly = allItem[i];
            if (jelly == null || jelly.copy == null)
                continue;
            //BaseJelly obj = jelly.copy[jelly.objData.CopyCount];
            for (int j = 0; j < 3; j++)
            {
                BaseJelly obj = jelly.copy[jelly.objData.FrontCount];

                if (obj == null)
                    continue;

                if (jelly.objData.CurrentCount >= jelly.objData.TotalCount)
                    break;

                if (obj.transform.position.x < (m_fReturnXPos) || obj.isDie)/* || obj.isDie == true*/
                {
                    OnRestetObj(jelly, jelly.objData.FrontCount);
                }
                jelly.objData.FrontCount++;
                if (jelly.objData.FrontCount >= jelly.copy.Length)
                    jelly.objData.FrontCount = 0;
            }
            #region
            //if (obj == null)
            //    continue;
            //if (obj.transform.position.x < (m_fReturnXPos)/* || obj.isDie == true*/)
            //{
            //    Debug.Log(jelly.objData.CopyCount + "Num Pos : " + obj.transform.position.x + "return value" + m_fReturnXPos);
            //    if (jelly.objData.CurrentCount < jelly.objData.TotalCount)
            //    {
            //        if (jelly.objData.CopyCount >= jelly.copy.Length - 1) //배열이 마지막이면 처음부터
            //        {
            //            jelly.objData.CopyCount = 0;
            //        }
            //        else
            //        {
            //            jelly.objData.CopyCount++;
            //        }
            //        jelly.copy[jelly.objData.CopyCount].transform.position = jelly.objData.objectPosition[jelly.objData.CurrentCount].transform.position; //복사본을 오브젝트 위치로 이동
            //        jelly.objData.CurrentCount++; //다음목표 오브젝트 번호 (위치)
            //        jelly.copy[jelly.objData.CopyCount].gameObject.SetActive(true);
            //        jelly.copy[jelly.objData.CopyCount].Init();//초기화
            //    }
            //}
            #endregion
        }
    }
    void OnRestetObj(JellyObject jelly,int j)
    {
        if (jelly.copy[j] == null || jelly.objData.objectPosition[jelly.objData.CurrentCount].isActive == false)
            return;
        jelly.copy[j].gameObject.SetActive(true);
        jelly.copy[j].transform.localPosition = jelly.objData.objectPosition[jelly.objData.CurrentCount].position; //복사본을 오브젝트 위치로 이동
        jelly.copy[j].Init();//초기화
        jelly.objData.CurrentCount++; //다음목표 오브젝트 번호 (위치)
    }
    public void ObjectInstantiate(SampleScrolling _scrolling) //복사본 할당
    {
        GameItemDataBase dataBase = GameItemDataBase.Instance;
        scrolling = _scrolling;

        clovers.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Clover);
        diamonds.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Diamond);
        hearts.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Heart);
        spades.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Spade);

        whites.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.White);
        blacks.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Black);
        blues.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Blue);
        reds.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Red);
        greens.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Green);
        reds.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Red);
        yellows.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Yellow);

        bigs.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.BigTime);
        smalls.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.SmallTime);
        coins.objData.prefab = dataBase.GetFieldItem(DataType.JellyType.Coin);

        SetItemInList();
    }

    void SetItemInList() // 반복할 물체 리스트에 저장
    {
        if (clovers.objData.objectPosition != null)
            allItem.Add(clovers);

        if (spades.objData.objectPosition != null)
            allItem.Add(spades);

        if (hearts.objData.objectPosition != null)
            allItem.Add(hearts);

        if (diamonds.objData.objectPosition != null)
            allItem.Add(diamonds);

        if (blacks.objData.objectPosition != null)
            allItem.Add(blacks);

        if (whites.objData.objectPosition != null)
            allItem.Add(whites);

        if (blues.objData.objectPosition != null)
            allItem.Add(blues);

        if (reds.objData.objectPosition != null)
            allItem.Add(reds);

        if (yellows.objData.objectPosition != null)
            allItem.Add(yellows);

        if (greens.objData.objectPosition != null)
            allItem.Add(greens);

        if (smalls.objData.objectPosition != null)
            allItem.Add(smalls);

        if (bigs.objData.objectPosition != null)
            allItem.Add(bigs);

        if (coins.objData.objectPosition != null)
            allItem.Add(coins);
        InitItem();
    }

    void InitItem()//객체 값 초기화
    {
        for (int i = 0; i < allItem.Count; i++)
        {
            allItem[i].objData.CurrentCount = allItem[i].objData.CopyCount = 0;

            if (allItem[i].objData.objectPosition == null || allItem[i].objData.objectPosition.Count == 0)
            {
                allItem[i].objData.TotalCount = -1;
                allItem[i].copy = null;
            }
            else
            {
                allItem[i].objData.TotalCount = allItem[i].objData.objectPosition.Count;
                CreateJellyArray(allItem[i]);
            }
        }
    }
    //    Black, //0
    //    Blue, // 1
    //    Green, //2
    //    Red, //3
    //    White, //4
    //    Yellow, //5

    //    Spade, //6
    //    Clover,//7
    //    Heart,//8
    //    Diamond,//9

    //    SmallTime,//10
    //    BigTime,//11

    //    Coin//12
    void CreateJellyArray(JellyObject baseJellies)//객체 생성
    {
        DataType.JellyType type = baseJellies.objData.prefab.GetComponent<BaseJelly>().type;//젤리 타입 저장
        int num;

        if (type < DataType.JellyType.Spade) // 타입이 코인
            num = m_jellyCount;
        else if (type > DataType.JellyType.SmallTime) // 타입이 피버젤리
            num = m_feverCount;
        else //타입이 기타 아이템
            num = m_jellyCount;

        if (baseJellies.objData.TotalCount >= num) //디폴트 개수보다 많다면
        {
                baseJellies.copy = new BaseJelly[num]; //배열 확장
        }
        else
        {
            bool nullValue = false;

            if (baseJellies.objData.prefab != null)
            {
                if (baseJellies.objData.TotalCount > 0)
                    baseJellies.copy = new BaseJelly[baseJellies.objData.TotalCount];//복제할 위치에 오브젝트 풀링 배열 생성
                else
                    nullValue = true; //데이터가 없는 상태로 변경
            }
            else
                nullValue = true;//데이터가 없는 상태로 변경

            if (nullValue)//실패
            {
                baseJellies.copy = null;
                baseJellies.objData.objectPosition = null;
                return;
            }
        }

        for (int i = 0; i < baseJellies.copy.Length; i++) //객체 복사 생성
        {
            GameObject obj = Instantiate(baseJellies.objData.prefab, baseJellies.objData.objectPosition[i].position, Quaternion.identity);
            obj.transform.parent = parentObj.transform;

            if (obj.GetComponent<BaseJelly>() == null)
                Debug.Log(baseJellies.objData.prefab + "의 GetComponent<BaseJelly>가 NULL입니다");
            else
                baseJellies.copy[i] = obj.GetComponent<BaseJelly>();

        }
        baseJellies.objData.CurrentCount = baseJellies.copy.Length; //현재 복사 객체가 위치한 끝 번호 
        baseJellies.objData.CopyCount = 0; //현재 가장 앞에있는 객체 번호
    }
}
