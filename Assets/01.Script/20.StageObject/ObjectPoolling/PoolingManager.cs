using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] ObjectPooling startZone;
    [SerializeField] ObjectPooling[] allList;
    List<ObjectPooling> poolings = new List<ObjectPooling>();
    [SerializeField] ObjectPooling currentPool;
    // Start is called before the first frame update
    void Start()
    {
        AddList();
        StartZoneListCheck();
        currentPool = startZone;
    }
    void AddList()
    {
        for (int i = 0; i < allList.Length; i++)
        {
            if (allList[i] != null)
                poolings.Add(allList[i]);
        }
    }
    void StartZoneListCheck()
    {
        bool isFind = false;
        for (int i = 0; i < poolings.Count; i++)
        {
            if (poolings[i] == startZone)
                isFind = true;
        }
        if (isFind != true)
            poolings.Add(startZone);
    }
    // Update is called once per frame
    void Update()
    {
        if(currentPool.isCurrentEnd)
        {
            reCount:
            int num = Random.Range(0, poolings.Count);
            if(poolings[num] != currentPool)
            {
                currentPool = poolings[num];
                currentPool.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("현재 동일한 패턴이 선택되었습니다. 재설정에 들어갑니다. -CurrentPool Num :" + num);
                goto reCount;
            }
        }
    }
}
