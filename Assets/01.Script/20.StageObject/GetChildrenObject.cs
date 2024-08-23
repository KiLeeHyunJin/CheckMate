using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChildrenObject : MonoBehaviour
{
    [SerializeField] ScrollingReverse scrollingReverse;
    //GameObject[] gameObjects;
    void Start()
    {
        ObstacleSensor[] gameObjects = GetComponentsInChildren<ObstacleSensor>();
        List<GameObject> Obj = new List<GameObject>();
        Transform[] Trans = null;
        if(gameObjects.Length != 0)
        {
            //Obj = new GameObject[gameObjects.Length];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Obj.Add(gameObjects[i].gameObject);
            }
        }

        else// if(Obj == null)
        {
            Trans = GetComponentsInChildren<Transform>();
            //Obj = new GameObject[Trans.Length];

            for (int i = 0; i < Trans.Length; i++)
            {
                if (Trans[i].gameObject == this.gameObject)
                    continue;
                Obj.Add(Trans[i].gameObject);
            }
        }
        scrollingReverse.InitGameObject(Obj);
    }
}
