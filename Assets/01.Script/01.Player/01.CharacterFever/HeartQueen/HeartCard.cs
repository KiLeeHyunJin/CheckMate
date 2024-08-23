using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCard : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Vector2 startPos;
    [SerializeField] GameObject target;
    [SerializeField] Vector2[] m_points = new Vector2[4];
    Vector2 targetPos;
    [SerializeField] Vector2 moveSpeed;
    [SerializeField] Vector2 moveDirection;
    [SerializeField] float startScale;
    [SerializeField] float endScale;
    [SerializeField] float m_speed;
    [SerializeField] float rotSpeed;
    [SerializeField] float targetDistance;
    [SerializeField] bool isTouch;
    BaseJelly[] Coin;
    CircleCollider2D collider2D;
    int coinIndex;
    bool isSetting;
    float m_timerCurrent;
    float m_timerMax = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        collider2D = GetComponent<CircleCollider2D>();
    }
    void Start()
    {
        if (m_speed <= 0)
            m_speed = 5f;
        if (rotSpeed <= 0)
            rotSpeed = 10;
        if (anim == null)
            anim = GetComponent<Animator>();
        if (targetDistance <= .5f)
            targetDistance = 1;
    }

    public void Setting(Vector2 StartPos,  GameObject _target, float _ySpeed, float _xSpeed, int _coinIndex, BaseJelly[] _Coin)
    {
        m_timerCurrent = 0;
        startPos = StartPos;
        moveSpeed.y = _ySpeed;
        moveSpeed.x = _xSpeed;
        target = _target;
        isTouch = false;
        isSetting = true;
        coinIndex = _coinIndex;
        Coin = _Coin;
        targetPos = target.transform.position;

        if (anim != null)
        {
            anim.SetTrigger("Idle");
        }

        transform.position = m_points[0] = startPos;

        m_points[1] = startPos +
            (startScale * (Random.Range(-2f, 2.0f) * Vector2.up)) + // Y (아래쪽 조금, 위쪽 전체)
            (startScale * (Random.Range(-5.0f, -0.3f) * Vector2.right));

        m_points[2] = targetPos +
            (endScale * Random.Range(-1.0f, 1.0f) * Vector2.up) +// Y (위, 아래 전체)
            (endScale * Random.Range(-1.0f, 1.0f) * Vector2.right) 
            - new Vector2(7,0);// Y (위, 아래 전체)

        m_points[3] = targetPos;

        m_timerMax = Random.Range(0.7f, 1.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSetting)
        {
            gameObject.SetActive(false);
            return;
        }

        if(!(InGameManager.instance.isGameover))
        {
            if (target == null)
                return;

            if (!isTouch)
            {
                m_timerCurrent += Time.deltaTime * m_speed;

                m_points[3] = targetPos;
                transform.position = new Vector3(
                    CubicBezierCurve(m_points[0].x, m_points[1].x, m_points[2].x, m_points[3].x),
                    CubicBezierCurve(m_points[0].y, m_points[1].y, m_points[2].y, m_points[3].y)
                    );

                transform.Rotate(new Vector3(0, 0, 10) * Time.deltaTime);

                if(targetDistance >= (new Vector3(targetPos.x , targetPos.y, 0) - transform.position).magnitude)
                {
                    isTouch = true;
                    isSetting = false;

                    if(Coin != null && Coin.Length > coinIndex && Coin[coinIndex] != null)
                    {
                        Coin[coinIndex].transform.position = targetPos;
                        if(Coin[coinIndex].gameObject.activeSelf == false)
                            Coin[coinIndex].gameObject.SetActive(true);
                        Coin[coinIndex].Init();
                    }

                    if (anim != null)
                        anim.SetTrigger("Bomb");
                    else
                        gameObject.SetActive(false);
                }
            }
            else
                Bomb();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (InGameManager.instance.isGameover)
            return;
        if(collision != null)
        {
            if(collision.gameObject.tag == "Obstacle")
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
    private float CubicBezierCurve(float a, float b, float c)
    {
        float t = m_timerCurrent / m_timerMax; // (현재 경과 시간 / 최대 시간)
        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);

        float abbc = Mathf.Lerp(ab, bc, t);

        return abbc;
    }
    private float CubicBezierCurve(float a, float b, float c, float d)
    {
        float t = m_timerCurrent / m_timerMax; // (현재 경과 시간 / 최대 시간)
        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }

    void Bomb()
    {
        if(anim != null)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                return;
        }
        else
            gameObject.SetActive(false);
    }
}
