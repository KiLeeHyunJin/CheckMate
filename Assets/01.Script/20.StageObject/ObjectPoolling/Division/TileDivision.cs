using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDivision : MonoBehaviour
{
    TilePos[] tiles = null;
    [SerializeField] TileSetting pool;
    [SerializeField] bool isOff;
    [Range(0,1)]
    [SerializeField] float setColorAlphaValue;

    private void Awake()
    {
        GetObject();
    }

    public void GetObject()
    {
        if (pool == null)
            pool = GetComponent<TileSetting>();

        tiles = GetComponentsInChildren<TilePos>();

        if(pool != null)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                SpriteRenderer Temp = tiles[i].gameObject.GetComponent<SpriteRenderer>();
                if (!isOff)
                {
                    if (Temp != null)
                    {
                        Temp.enabled = !isOff;
                    }
                }
                else
                {
                    SetAlpha(Temp);
                }
                ObjectSort(tiles[i].type, i);
            }
        }
        XPosSort();
    }
    void SetAlpha(SpriteRenderer temp)
    {
        if (temp != null)
        {
            Color color = temp.color;
            color.a = setColorAlphaValue;
            temp.color = color;
        }
    }
    void XPosSort()
    {
        if(pool.downTile.objData.objectPosition.Count > 0)
        {
            pool.downTile.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.upTile.objData.objectPosition.Count > 0)
        {
            pool.upTile.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.normalTile.objData.objectPosition.Count > 0)
        {
            pool.normalTile.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
    }
    void ObjectSort(DataType.TileType _enum, int _index)
    {
        if (tiles[_index] == null)
            return;
        objectPosition objPos = new objectPosition();
        objPos.localScale = tiles[_index].transform.localScale;
        objPos.position = tiles[_index].transform.position;
        objPos.rotation = tiles[_index].transform.rotation;
        objPos.isActive = true;

        switch (_enum)
        {
            case DataType.TileType.NormalTile:
                pool.normalTile.objData.objectPosition.Add(objPos);
                break;
            case DataType.TileType.UpTile:
                pool.upTile.objData.objectPosition.Add(objPos);
                break;
            case DataType.TileType.DownTile:
                pool.downTile.objData.objectPosition.Add(objPos);
                break;
            case DataType.TileType.End:
                break;
        }
        Destroy(tiles[_index].gameObject);
    }
}
