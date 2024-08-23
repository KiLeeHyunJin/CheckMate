using UnityEngine;

public class BackGroundScrolling : MonoBehaviour
{
    [SerializeField] public float speed = 1f;
    [SerializeField] float moveYSpeed;
    [SerializeField] GameObject start;
    [SerializeField] GameObject end;
    [SerializeField] InGameManager inGameManager;
    float moveYPos;

    Vector2 startPosition;
    Vector2 endPosition;

    private void Start()
    {
        startPosition = start.transform.position;
        endPosition = end.transform.position;
        if (inGameManager == null)
            inGameManager = InGameManager.instance;
        if (moveYSpeed <= 0)
            moveYSpeed = 0.55f;
    }
    // Update is called once per frame
    void Update()
    {
        if (inGameManager.isGameover || inGameManager.isCount || inGameManager.isTuto)
            return;
        transform.Translate(-1 * speed * Time.deltaTime, moveYPos * moveYSpeed, 0);
        if(transform.position.x <= endPosition.x)
        {
            ScrollEnd();
        }
    }
    void ScrollEnd()
    {
        transform.Translate(-1 * (endPosition.x - startPosition.x),0, 0);
    }
    public void GetMoveYPos(float value)
    {
        moveYPos = value;
    }
}
