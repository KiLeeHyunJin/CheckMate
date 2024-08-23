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

    public void InitArray()//��ġ �ʱ�ȭ
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
                allTile[i].objData.CurrentCount = allTile[i].copy.Length; //���� ���� ��ü�� ��ġ�� �� ��ȣ 
                allTile[i].objData.CopyCount = 0; //���� ���� �տ��ִ� ��ü ��ȣ
                allTile[i].objData.FrontCount = 0;

            }
        }

    }

    public void ReturnObject()//������Ʈ ȸ��
    {
        for (int i = 0; i < allTile.Count; i++) //Ÿ�� �������� �ݺ�// 5���� Ÿ�� �ݺ�
        {
            TileObject tile = allTile[i];
            if (tile == null || tile.copy == null)
                continue;
            //for (int j = 0; j < tile.copy.Length; j++) //������ ��ü�� �ݺ�// 20���� �ν��Ͻ� �ݺ�
            {
                if (tile.objData.CurrentCount >= tile.objData.TotalCount)
                    break;
                if (tile.copy[tile.objData.FrontCount].transform.position.x < (m_fReturnXPos))
                {
                    tile.copy[tile.objData.FrontCount].gameObject.SetActive(true);

                    tile.copy[tile.objData.FrontCount].transform.localPosition = tile.objData.objectPosition[tile.objData.CurrentCount].position; //���纻�� ������Ʈ ��ġ�� �̵�
                    
                    Vector3 pos = tile.copy[tile.objData.FrontCount].transform.position;
                    pos.y = tile.objData.objectPosition[tile.objData.CurrentCount].position.y;
                    tile.copy[tile.objData.FrontCount].transform.position = pos;

                    tile.objData.CurrentCount++; //������ǥ ������Ʈ ��ȣ (��ġ)
                    tile.objData.FrontCount++;
                    if (tile.objData.FrontCount >= tile.copy.Length)
                        tile.objData.FrontCount = 0;
                }

                //if (tile.objData.CopyCount >= tile.copy.Length - 1) //�迭�� �������̸� ó������
                //{
                //    tile.objData.CopyCount = 0;
                //}
                //else
                //{
                //    tile.objData.CopyCount++;
                //}
                //tile.copy[tile.objData.CopyCount].transform.position = tile.objData.objectPosition[tile.objData.CurrentCount].transform.position; //���纻�� ������Ʈ ��ġ�� �̵�
                //tile.objData.CurrentCount++; //������ǥ ������Ʈ ��ȣ (��ġ)
            }
        }
    }

    public void ObjectInstantiate(SampleScrolling _scrolling)//��ü ����
    {
        GameItemDataBase dataBase = GameItemDataBase.Instance;
        scrolling = _scrolling;
        normalTile.objData.prefab = dataBase.GetTile(DataType.TileType.NormalTile);
        upTile.objData.prefab = dataBase.GetTile(DataType.TileType.UpTile);
        downTile.objData.prefab = dataBase.GetTile(DataType.TileType.DownTile);
        SetItemInList();
    }

    void SetItemInList()//����Ʈ�� �߰�
    {
        if (normalTile.objData.objectPosition != null)
            allTile.Add(normalTile);

        if (upTile.objData.objectPosition != null)
            allTile.Add(upTile);

        if (downTile.objData.objectPosition != null)
            allTile.Add(downTile);
        InitItem();
    }

    void InitItem()//����Ʈ �ʱ�ȭ
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



    void CreateObjArray(TileObject tiles)//��ü ����
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
        tiles.objData.CurrentCount = tiles.copy.Length; //���� ���� ��ü�� ��ġ�� �� ��ȣ 
        tiles.objData.CopyCount = 0; //���� ���� �տ��ִ� ��ü ��ȣ
    }
}
