using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JellyDivision : MonoBehaviour
{
    ItemPos[] jellys = null;
    [SerializeField] JellySetting pool;
    [Header("체크 시 비활성화")]
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
            pool = GetComponent<JellySetting>();

        jellys = GetComponentsInChildren<ItemPos>();

        if(pool != null)
        {
            for (int i = 0; i < jellys.Length; i++)
            {
                SpriteRenderer Temp = jellys[i].gameObject.GetComponent<SpriteRenderer>();
                if (isOff)
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
                DataType.JellyType typeNum = jellys[i].type;
                Resort(typeNum, i);
            }
        }
        XPosSort();
        //for (int i = 0; i < jellys.Length; i++)
        //{
        //    if (jellys[i] != null)
        //        jellys[i].gameObject.SetActive(false);
        //}
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
        if(pool.clovers.objData.objectPosition.Count > 0)
        {
            pool.clovers.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.spades.objData.objectPosition.Count > 0)
        {
            pool.spades.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.hearts.objData.objectPosition.Count > 0)
        {
            pool.hearts.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.diamonds.objData.objectPosition.Count > 0)
        {
            pool.diamonds.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.blacks.objData.objectPosition.Count > 0)
        {
            pool.blacks.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.whites.objData.objectPosition.Count > 0)
        {
            pool.whites.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.blues.objData.objectPosition.Count > 0)
        {
            pool.blues.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.reds.objData.objectPosition.Count > 0)
        {
            pool.reds.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.yellows.objData.objectPosition.Count > 0)
        {
            pool.yellows.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.greens.objData.objectPosition.Count > 0)
        {
            pool.greens.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.smalls.objData.objectPosition.Count > 0)
        {
            pool.smalls.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.bigs.objData.objectPosition.Count > 0)
        {
            pool.bigs.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.coins.objData.objectPosition.Count > 0)
        {
            pool.coins.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }

    }
    void SortingStart(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list.Sort();
        }
    }
    void Resort(DataType.JellyType _num,int _index)
    {
        if (jellys[_index].gameObject == null)
            return;
        objectPosition objPos = new objectPosition();
        objPos.localScale = jellys[_index].transform.localScale;
        objPos.position = jellys[_index].transform.position;
        objPos.rotation = jellys[_index].transform.rotation;
        objPos.isActive = true;

        switch (_num)
        {
            case DataType.JellyType.Black:
                pool.blacks.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Blue:
                pool.blues.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Green:
                pool.greens.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Red:
                pool.reds.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.White:
                pool.whites.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Yellow:
                pool.yellows.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Spade:
                pool.spades.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Clover:
                pool.clovers.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Heart:
                pool.hearts.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Diamond:
                pool.diamonds.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.SmallTime:
                pool.smalls.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.BigTime:
                pool.bigs.objData.objectPosition.Add(objPos);
                break;
            case DataType.JellyType.Coin:
                pool.coins.objData.objectPosition.Add(objPos);
                break;
            default:
                break;
        }
        Destroy(jellys[_index].gameObject);
    }
}
