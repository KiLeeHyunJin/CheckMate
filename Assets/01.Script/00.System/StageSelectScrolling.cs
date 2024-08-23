using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectScrolling : MonoBehaviour
{
    [SerializeField] public float speed = 1f;
    [SerializeField] float moveYSpeed;
    [SerializeField] RectTransform start;
    [SerializeField] RectTransform end;
    RectTransform thisTransform;
    Vector2 OriginPos;
    float moveYPos;

    Vector2 startPosition;
    Vector2 endPosition;

    private void Awake()
    {
        startPosition = start.transform.position;
        endPosition = end.transform.position;
        thisTransform = GetComponent<RectTransform>();
        OriginPos = thisTransform.anchoredPosition;
        if (moveYSpeed <= 0)
            moveYSpeed = 0.55f;
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        thisTransform.anchoredPosition = OriginPos;
    }
    // Update is called once per frame
    void Update()
    {
        thisTransform.anchoredPosition = thisTransform.anchoredPosition + new Vector2 (-1 * speed * Time.deltaTime, 0);
        //transform.Translate(-1 * speed * Time.deltaTime, moveYPos * moveYSpeed, 0);
        if (transform.position.x <= endPosition.x)
        {
            ScrollEnd();
        }
    }
    void ScrollEnd()
    {
        transform.Translate(-1 * (endPosition.x - startPosition.x), 0, 0);
    }
    public void GetMoveYPos(float value)
    {
        moveYPos = value;
    }


}
