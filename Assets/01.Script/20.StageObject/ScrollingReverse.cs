using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingReverse : MonoBehaviour
{
    float speed;
    public float relaitiveSpeed;
    [SerializeField] GameObject[] obj;
    public void InitGameObject(List<GameObject> _gameObjects)
    {
        obj = new GameObject[_gameObjects.Count];

        if (_gameObjects != null)
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                obj[i] = _gameObjects[i];
            }
        }
    }
    private void FixedUpdate()
    {
        if (InGameManager.instance.isCount)
            return;

        speed = InGameManager.instance.Getspeed();
        if(obj != null)
            Move();
    }

    private void Move()
    {
        if (InGameManager.instance.isGameover) return;
        else
        {
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] == null)
                    continue;
                obj[i].transform.Translate(Vector3.left * (speed - relaitiveSpeed) * Time.deltaTime, Space.Self);
            }
        }
    }

    //IEnumerator ObjectMove()
    //{

    //}
}
