using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseJelly : MonoBehaviour
{
    public DataType.JellyType type;

    [SerializeField] int score;
    [SerializeField] int coin;
    /*[HideInInspector]*/ public bool isDie;

    protected float dis;
    [SerializeField] protected bool isCrashed;
    
    protected Vector3 target;
    protected Animator animator;
    public virtual void Init()
    {

    }
    public void SetScore(int _value)
    {
        score = _value;
    }
    public void SetCoin(int _value)
    {
        coin = _value;
    }
    private void Start()
    {
        Go();
    }
    private void Update()
    {
        Tick();
    }
    protected virtual void Go()
    {
        animator = GetComponent<Animator>();
        isCrashed = false;
        dis = 2.5f;
    }
    protected virtual void Tick()
    { }
    public virtual int GetScore()
    {
        return score;
    }
    public virtual int GetCoin()
    {
        return coin;
    }
    protected virtual void AddData()
    {
        InGameManager.instance.AddCoin(coin, type);
        InGameManager.instance.UpdateScore(score, type);
    }
    protected void Effect()
    {
        gameObject.layer = 12;
        //InGameManager.instance.UpdateScore(addScore);
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Jelly);
        //SFXmanager.instance.PlayOnGetJelly();
        AddData();
        if (animator != null)
        {
            animator.SetTrigger("Die");
            //MakeDisable();
        }
        else
        {
            MakeDisable();
        }
    }
    void MakeDisable()
    {
        gameObject.layer = 12;
        gameObject.SetActive(false);
    }
}
