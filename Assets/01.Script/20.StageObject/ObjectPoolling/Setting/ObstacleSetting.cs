using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSetting : MonoBehaviour
{
    [Header("MoveObstacle")]
    [HideInInspector] public ObstacleObject Mv_Arrow = new ObstacleObject();
    [HideInInspector] public ObstacleObject Mv_BrownGrass = new ObstacleObject();
    [HideInInspector] public ObstacleObject Mv_Character = new ObstacleObject();
    [HideInInspector] public ObstacleObject Mv_DownStreet = new ObstacleObject();
    [HideInInspector] public ObstacleObject Mv_FireBall = new ObstacleObject();
    [HideInInspector] public ObstacleObject Mv_GreenGrass = new ObstacleObject();
    [HideInInspector] public ObstacleObject Mv_OliveGrass = new ObstacleObject();
    [HideInInspector] public ObstacleObject Mv_PinkDown = new ObstacleObject();
    [HideInInspector] public ObstacleObject Mv_Yellow = new ObstacleObject();
    [HideInInspector]  public ObstacleObject Mv_DropTile = new ObstacleObject();

    [Header("StandObstacle")]
    [HideInInspector] public ObstacleObject St_BlueBottle = new ObstacleObject();
    [HideInInspector] public ObstacleObject St_BrownGrass = new ObstacleObject();
    [HideInInspector] public ObstacleObject St_GreenGrass = new ObstacleObject();
    [HideInInspector] public ObstacleObject St_OliveGrass = new ObstacleObject();
    [HideInInspector] public ObstacleObject St_RedBottle = new ObstacleObject();

    [SerializeField] GameObject parentObj;
    public List<ObstacleObject> stopObstacles = new List<ObstacleObject>();
    public List<ObstacleObject> moveObstacles = new List<ObstacleObject>();
    [SerializeField] int m_iCount;
    public float m_fReturnXPos;
    SampleScrolling scrolling;

    public void InitArray()//위치 초기화
    {
        Vector3 scale = Vector3.one;
        for (int i = 0; i < stopObstacles.Count; i++)
        {
            if(stopObstacles[i].copy != null)
            {
                for (int j = 0; j < stopObstacles[i].copy.Length; j++)
                {
                    scale = stopObstacles[i].objData.objectPosition[j].localScale;
                    stopObstacles[i].copy[j].transform.localScale = scale;
                    stopObstacles[i].copy[j].transform.position = stopObstacles[i].objData.objectPosition[j].position;

                    BaseObstacle baseObj = stopObstacles[i].copy[j].GetComponent<BaseObstacle>();
                    if(baseObj != null)
                    {
                        InitObj(baseObj);
                    }
                }
                ObjListInit(stopObstacles, i);
            }
        }

        for (int i = 0; i < moveObstacles.Count; i++)
        {
            if(moveObstacles[i].copy != null)
            {
                for (int j = 0; j < moveObstacles[i].copy.Length; j++)
                {
                    scale = moveObstacles[i].objData.objectPosition[j].localScale;
                    moveObstacles[i].copy[j].transform.localScale = scale;
                    moveObstacles[i].copy[j].transform.position = moveObstacles[i].objData.objectPosition[j].position;

                    BaseObstacle baseObj = moveObstacles[i].copy[j].GetComponent<BaseObstacle>();
                    if (baseObj != null)
                    {
                        InitObj(baseObj);
                    }
                }
                ObjListInit(moveObstacles, i);
            }
        }
    }

    void ObjListInit(List<ObstacleObject> list, int _idex)
    {
        list[_idex].objData.CurrentCount = list[_idex].copy.Length; //현재 복사 객체가 위치한 끝 번호 
        list[_idex].objData.CopyCount = 0; //현재 가장 앞에있는 객체 번호
    }

    void InitObj(BaseObstacle Obj)
    {
        Obj.gameObject.SetActive(true);
        Obj.Init();
    }

    public void ReturnObject()//오브젝트 회수
    {

        for (int i = 0; i < stopObstacles.Count; i++)
        {
            ObstacleObject obstacle = stopObstacles[i];
            if (obstacle == null || obstacle.copy == null || obstacle.copy[obstacle.objData.CopyCount] == null)
                continue;
            BaseObstacle obj = obstacle.copy[obstacle.objData.CopyCount].GetComponent<BaseObstacle>();
            if (obstacle == null || obj == null)
                continue;
           // m_fReturnXPos = Owner.transform.position.x - 
            if (obj.transform.position.x < (m_fReturnXPos))
            {
                if (obstacle.objData.CurrentCount < obstacle.objData.TotalCount)
                {
                    if (obstacle.objData.CopyCount >= obstacle.copy.Length - 1) //배열이 마지막이면 처음부터
                        obstacle.objData.CopyCount = 0;
                    else
                        obstacle.objData.CopyCount++;
                    obstacle.copy[obstacle.objData.CopyCount].gameObject.SetActive(true);

                    obstacle.copy[obstacle.objData.CopyCount].transform.localPosition = obstacle.objData.objectPosition[obstacle.objData.CurrentCount].position; //복사본을 오브젝트 위치로 이동
                    obstacle.objData.CurrentCount++; //다음목표 오브젝트 번호 (위치)
                    obj.Init();//초기화
                }
            }
        }
    }

    public void ObjectInstantiate(SampleScrolling _scrolling)
    {
        GameItemDataBase dataBase = GameItemDataBase.Instance;
        scrolling = _scrolling;
        Mv_Arrow.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.Arrow);
        Mv_BrownGrass.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.BrownGrass);
        Mv_Character.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.Character);
        Mv_Character.objData.prefab.transform.localScale = new Vector3(0.7f,0.7f,1);
        Mv_DownStreet.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.DownStreetFireBall);
        Mv_FireBall.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.FireBall);
        Mv_GreenGrass.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.GreenGrass);
        Mv_OliveGrass.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.OliveGrass);
        Mv_PinkDown.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.PinkDown);
        Mv_Yellow.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.YellowDown);
        Mv_DropTile.objData.prefab = dataBase.GetMoveObstacle(DataType.MoveObstacleType.DropTile);

        St_BlueBottle.objData.prefab = dataBase.GetStopObstacle(DataType.StopObstacleType.BlueBottle);
        St_BrownGrass.objData.prefab = dataBase.GetStopObstacle(DataType.StopObstacleType.BrownGrass);
        St_GreenGrass.objData.prefab = dataBase.GetStopObstacle(DataType.StopObstacleType.GreenGrass);
        St_OliveGrass.objData.prefab = dataBase.GetStopObstacle(DataType.StopObstacleType.OliveGrass);
        St_RedBottle.objData.prefab = dataBase.GetStopObstacle(DataType.StopObstacleType.RedBottle);
        SetItemInList();
    }

    void SetItemInList()
    {
        if (St_BlueBottle.objData.objectPosition != null)
            stopObstacles.Add(St_BlueBottle);

        if (St_BrownGrass.objData.objectPosition != null)
            stopObstacles.Add(St_BrownGrass);

        if (St_GreenGrass.objData.objectPosition != null)
            stopObstacles.Add(St_GreenGrass);

        if (St_OliveGrass.objData.objectPosition != null)
            stopObstacles.Add(St_OliveGrass);

        if (St_RedBottle.objData.objectPosition != null)
            stopObstacles.Add(St_RedBottle);


        if (Mv_Arrow.objData.objectPosition != null)
            moveObstacles.Add(Mv_Arrow);

        if (Mv_BrownGrass.objData.objectPosition != null)
            moveObstacles.Add(Mv_BrownGrass);

        if (Mv_Character.objData.objectPosition != null)
            moveObstacles.Add(Mv_Character);

        if (Mv_DownStreet.objData.objectPosition != null)
            moveObstacles.Add(Mv_DownStreet);

        if (Mv_FireBall.objData.objectPosition != null)
            moveObstacles.Add(Mv_FireBall);

        if (Mv_GreenGrass.objData.objectPosition != null)
            moveObstacles.Add(Mv_GreenGrass);

        if (Mv_OliveGrass.objData.objectPosition != null)
            moveObstacles.Add(Mv_OliveGrass);

        if (Mv_PinkDown.objData.objectPosition != null)
            moveObstacles.Add(Mv_PinkDown);

        if (Mv_Yellow.objData.objectPosition != null)
            moveObstacles.Add(Mv_Yellow);

        if (Mv_DropTile.objData.objectPosition != null)
            moveObstacles.Add(Mv_DropTile);

        InitItem(moveObstacles);
        InitItem(stopObstacles);
    }

    void InitItem(List<ObstacleObject> _allObj)//리스트 초기화
    {
        for (int i = 0; i < _allObj.Count; i++)
        {
            _allObj[i].objData.CurrentCount = _allObj[i].objData.CopyCount = 0;
            _allObj[i].objData.FrontCount = 0;
            if (_allObj[i].objData.objectPosition == null || _allObj[i].objData.objectPosition.Count == 0)
            {
                _allObj[i].objData.TotalCount = -1;
                _allObj[i].copy = null;
                _allObj[i].objData.objectPosition = null;
            }
            else
            {
                _allObj[i].objData.TotalCount = _allObj[i].objData.objectPosition.Count;
                CreateObjArray(_allObj[i]);
            }
        }
    }

    void CreateObjArray(ObstacleObject _Obj)//객체 생성
    {
        if (_Obj.objData.TotalCount >= m_iCount)
        {
            _Obj.copy = new GameObject[m_iCount];
        }
        else
        {
            if(_Obj.objData.prefab != null)
            {
                if (_Obj.objData.TotalCount > 0)
                    _Obj.copy = new GameObject[_Obj.objData.TotalCount];
            }
            else
            {
                _Obj.copy = null;
                _Obj.objData.objectPosition = null;
                return;
            }
        }
        for (int i = 0; i < _Obj.copy.Length; i++)
        {
            GameObject obj = Instantiate(_Obj.objData.prefab, _Obj.objData.objectPosition[i].position, Quaternion.identity);
            obj.transform.parent = parentObj.transform;
            _Obj.copy[i] = obj;
        }
        _Obj.objData.CurrentCount = _Obj.copy.Length; //현재 복사 객체가 위치한 끝 번호 
        _Obj.objData.CopyCount = 0; //현재 가장 앞에있는 객체 번호
    }
}
