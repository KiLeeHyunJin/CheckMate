using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingScript : MonoBehaviour
{
    float speed;
    public float relaitiveSpeed;
    int startIndex;
    int endIndex;
    public float dis;
    public Transform[] sprites;

    private void Start()
    {
        speed = InGameManager.instance.Getspeed();
        startIndex = 0;
        endIndex = 2;
    }

    private void LateUpdate()
    {
        Move();
        Scrolling();
    }

    private void Move()
    {
        if (InGameManager.instance.isGameover || InGameManager.instance.isCount) return;
        transform.Translate(Vector3.right * (speed - relaitiveSpeed) * Time.deltaTime);

    }

    private void Scrolling()
    {
        if (sprites[startIndex].position.x < Camera.main.transform.position.x - dis)
        {
            Vector3 backSpritePos = sprites[endIndex].localPosition;
            sprites[startIndex].transform.localPosition = backSpritePos + Vector3.right * dis;

            int endIndexSave = endIndex;
            endIndex = startIndex;
            startIndex = (endIndexSave - 1 == -1) ? sprites.Length - 1 : endIndexSave - 1;
        }
    }

}
