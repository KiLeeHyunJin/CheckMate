using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDivision : MonoBehaviour
{
    Mv_ObPos[] moveObjs = null;
    St_ObPos[] standObjs = null;
    [SerializeField] ObstacleSetting pool;
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
            pool = GetComponent<ObstacleSetting>();

        moveObjs = GetComponentsInChildren<Mv_ObPos>();
        standObjs = GetComponentsInChildren<St_ObPos>();

        if(pool != null)
        {
            if(isOff)
            {
                for (int i = 0; i < moveObjs.Length; i++)
                {
                    MoveObjectSort(moveObjs[i].type, i);
                    if(moveObjs[i] != null)
                        moveObjs[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < standObjs.Length; i++)
                {
                    StandObjectSort(standObjs[i].type, i);
                    if(standObjs[i]  != null)
                        standObjs[i].gameObject.SetActive(false);
                }
            }
            else
            {
                SpriteRenderer[] temp;
                for (int i = 0; i < moveObjs.Length; i++)
                {
                    MoveObjectSort(moveObjs[i].type, i);
                    temp = moveObjs[i].GetComponentsInChildren<SpriteRenderer>();
                    SetAlpha(temp);
                }

                for (int i = 0; i < standObjs.Length; i++)
                {
                    StandObjectSort(standObjs[i].type, i);
                    temp = standObjs[i].GetComponentsInChildren<SpriteRenderer>();
                    SetAlpha(temp);
                }
            }
        }
        XPosSort();
    }
    void SetAlpha(SpriteRenderer[] temp)
    {
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] != null)
            {
                Color color = temp[i].color;
                color.a = setColorAlphaValue;
                temp[i].color = color;
            }
        }
    }

    void XPosSort()
    {
        if (pool.Mv_Arrow.objData.objectPosition.Count > 0)
        {
            pool.Mv_Arrow.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_Character.objData.objectPosition.Count > 0)
        {
            pool.Mv_Character.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_DropTile.objData.objectPosition.Count > 0)
        {
            pool.Mv_DropTile.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_OliveGrass.objData.objectPosition.Count > 0)
        {
            pool.Mv_OliveGrass.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_PinkDown.objData.objectPosition.Count > 0)
        {
            pool.Mv_PinkDown.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_BrownGrass.objData.objectPosition.Count > 0)
        {
            pool.Mv_BrownGrass.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_FireBall.objData.objectPosition.Count > 0)
        {
            pool.Mv_FireBall.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_DownStreet.objData.objectPosition.Count > 0)
        {
            pool.Mv_DownStreet.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_Yellow.objData.objectPosition.Count > 0)
        {
            pool.Mv_Yellow.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.Mv_GreenGrass.objData.objectPosition.Count > 0)
        {
            pool.Mv_GreenGrass.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }

        if (pool.St_BlueBottle.objData.objectPosition.Count > 0)
        {
            pool.St_BlueBottle.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.St_GreenGrass.objData.objectPosition.Count > 0)
        {
            pool.St_GreenGrass.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.St_BrownGrass.objData.objectPosition.Count > 0)
        {
            pool.St_BrownGrass.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.St_OliveGrass.objData.objectPosition.Count > 0)
        {
            pool.St_OliveGrass.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
        if (pool.St_RedBottle.objData.objectPosition.Count > 0)
        {
            pool.St_RedBottle.objData.objectPosition.Sort((x1, x2) => x1.position.x.CompareTo(x2.position.x));
        }
    }
    void MoveObjectSort(DataType.MoveObstacleType _enum, int _index)
    {
        if (moveObjs == null || moveObjs.Length < _index || moveObjs[_index] == null)
            return;
        objectPosition objPos = new objectPosition();
        objPos.localScale = moveObjs[_index].transform.localScale;
        objPos.position = moveObjs[_index].transform.position;
        objPos.rotation = moveObjs[_index].transform.rotation;
        objPos.isActive = true;

        switch (_enum)
        {
            case DataType.MoveObstacleType.Arrow:
                pool.Mv_Arrow.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.BrownGrass:
                pool.Mv_BrownGrass.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.Character:
                pool.Mv_Character.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.DownStreetFireBall:
                pool.Mv_DownStreet.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.DropTile:
                pool.Mv_DropTile.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.FireBall:
                pool.Mv_FireBall.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.GreenGrass:
                pool.Mv_GreenGrass.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.OliveGrass:
                pool.Mv_OliveGrass.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.PinkDown:
                pool.Mv_PinkDown.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.YellowDown:
                pool.Mv_Yellow.objData.objectPosition.Add(objPos);
                break;
            case DataType.MoveObstacleType.END:
                break;
            default:
                break;
        }
        Destroy(moveObjs[_index].gameObject);
    }
    void StandObjectSort(DataType.StopObstacleType _enum, int _index)
    {
        if (standObjs == null || standObjs.Length < _index || standObjs[_index] == null)
            return;
        objectPosition objPos = new objectPosition();
        objPos.localScale = standObjs[_index].transform.localScale;
        objPos.position = standObjs[_index].transform.position;
        objPos.rotation = standObjs[_index].transform.rotation;
        objPos.isActive = true;
        switch (_enum)
        {
            case DataType.StopObstacleType.BlueBottle:
                pool.St_BlueBottle.objData.objectPosition.Add(objPos);
                break;
            case DataType.StopObstacleType.BrownGrass:
                pool.St_BrownGrass.objData.objectPosition.Add(objPos);
                break;
            case DataType.StopObstacleType.GreenGrass:
                pool.St_GreenGrass.objData.objectPosition.Add(objPos);
                break;
            case DataType.StopObstacleType.OliveGrass:
                pool.St_OliveGrass.objData.objectPosition.Add(objPos);
                break;
            case DataType.StopObstacleType.RedBottle:
                pool.St_RedBottle.objData.objectPosition.Add(objPos);
                break;
            case DataType.StopObstacleType.END:
                break;
            default:
                break;
        }
        Destroy(standObjs[_index].gameObject);
    }
}
