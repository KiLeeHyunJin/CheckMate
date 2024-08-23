using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetting : MonoBehaviour
{
    [Header("Tile")]
    [HideInInspector] public TileObject normalTile = new TileObject();
    [HideInInspector] public TileObject upTile = new TileObject();
    [HideInInspector] public TileObject downTile = new TileObject();
    [SerializeField] GameObject parentObj;
    SampleScrolling scrolling;
    public List<TileObject> allTile = new List<TileObject>();
    [SerializeField] int m_iCount;
    public float m_fReturnXPos;

    public void InitArray()//위치 초기화
    {
        Vector3 scale = Vector3.one;
        for (int i = 0; i < allTile.Count; i++)
        {
            if(allTile[i].copy != null)
            {
                for (int j = 0; j < allTile[i].copy.Length; j++)
                {
                    scale = allTile[i].objData.objectPosition[j].localScale;
                    allTile[i].copy[j].transform.localScale = scale;
                    allTile[i].copy[j].transform.position = allTile[i].objData.objectPosition[j].position;
                    Debug.Log("tilePos : " + allTile[i].objData.objectPosition[i].position);
                }
                allTile[i].objData.CurrentCount = allTile[i].copy.Length; //현재 복사 객체가 위치한 끝 번호 
                allTile[i].objData.CopyCount = 0; //현재 가장 앞에있는 객체 번호
                allTile[i].objData.FrontCount = 0;

            }
        }

    }

    public void ReturnObject()//오브젝트 회수
    {
        for (int i = 0; i < allTile.Count; i++) //타일 종류별로 반복// 5종의 타일 반복
        {
            TileObject tile = allTile[i];
            if (tile == null || tile.copy == null)
                continue;
            //for (int j = 0; j < tile.copy.Length; j++) //생성된 객체들 반복// 20개의 인스턴스 반복
            {
                if (tile.objData.CurrentCount >= tile.objData.TotalCount)
                    break;
                if (tile.copy[tile.objData.FrontCount].transform.position.x < (m_fReturnXPos))
                {
                    tile.copy[tile.objData.FrontCount].gameObject.SetActive(true);

                    tile.copy[tile.objData.FrontCount].transform.localPosition = tile.objData.objectPosition[tile.objData.CurrentCount].position; //복사본을 오브젝트 위치로 이동
                    
                    Vector3 pos = tile.copy[tile.objData.FrontCount].transform.position;
                    pos.y = tile.objData.objectPosition[tile.objData.CurrentCount].position.y;
                    tile.copy[tile.objData.FrontCount].transform.position = pos;

                    tile.objData.CurrentCount++; //다음목표 오브젝트 번호 (위치)
                    tile.objData.FrontCount++;
                    if (tile.objData.FrontCount >= tile.copy.Length)
                        tile.objData.FrontCount = 0;
                }

                //if (tile.objData.CopyCount >= tile.copy.Length - 1) //배열이 마지막이면 처음부터
                //{
                //    tile.objData.CopyCount = 0;
                //}
                //else
                //{
                //    tile.objData.CopyCount++;
                //}
                //tile.copy[tile.objData.CopyCount].transform.position = tile.objData.objectPosition[tile.objData.CurrentCount].transform.position; //복사본을 오브젝트 위치로 이동
                //tile.objData.CurrentCount++; //다음목표 오브젝트 번호 (위치)
            }
        }
    }

    public void ObjectInstantiate(SampleScrolling _scrolling)//객체 복사
    {
        GameItemDataBase dataBase = GameItemDataBase.Instance;
        scrolling = _scrolling;
        normalTile.objData.prefab = dataBase.GetTile(DataType.TileType.NormalTile);
        upTile.objData.prefab = dataBase.GetTile(DataType.TileType.UpTile);
        downTile.objData.prefab = dataBase.GetTile(DataType.TileType.DownTile);
        SetItemInList();
    }

    void SetItemInList()//리스트에 추가
    {
        if (normalTile.objData.objectPosition != null)
            allTile.Add(normalTile);

        if (upTile.objData.objectPosition != null)
            allTile.Add(upTile);

        if (downTile.objData.objectPosition != null)
            allTile.Add(downTile);
        InitItem();
    }

    void InitItem()//리스트 초기화
    {
        for (int i = 0; i < allTile.Count; i++)
        {
            allTile[i].objData.CurrentCount = allTile[i].objData.CopyCount = 0;
            if (allTile[i].objData.objectPosition == null || allTile[i].objData.objectPosition.Count == 0)
            {
                allTile[i].objData.TotalCount = -1;
                allTile[i].copy = null;
            }
            else
            {
                allTile[i].objData.TotalCount = allTile[i].objData.objectPosition.Count;
                CreateObjArray(allTile[i]);
            }
        }
    }



    void CreateObjArray(TileObject tiles)//객체 생성
    {
        if (tiles.objData.TotalCount >= m_iCount)
        {
            tiles.copy = new GameObject[m_iCount];
        }
        else
        {
            if(tiles.objData.prefab != null)
            {
                if (tiles.objData.TotalCount > 0)
                    tiles.copy = new GameObject[tiles.objData.TotalCount];
            }
            else
            {
                tiles.copy = null;
                tiles.objData.objectPosition = null;
                return;
            }
        }
        for (int i = 0; i < tiles.copy.Length; i++)
        {
            //Vector3 scale = Vector3.one;
            //if (tiles.copy[i] != null)
            //  scale = tiles.copy[i].transform.localScale;
            GameObject obj = Instantiate(tiles.objData.prefab, tiles.objData.objectPosition[i].position, Quaternion.identity);
            obj.transform.parent = parentObj.transform;
            tiles.copy[i] = obj;
            //tiles.copy[i].transform.localScale = scale;
        }
        tiles.objData.CurrentCount = tiles.copy.Length; //현재 복사 객체가 위치한 끝 번호 
        tiles.objData.CopyCount = 0; //현재 가장 앞에있는 객체 번호
    }
}
