using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverCoinData : BaseJelly
{
    [HideInInspector] public DataType.FeverCoinType FeverCoinType;
    private BoxCollider2D boxCollider2;
    protected override void Go()
    {
        animator = GetComponent<Animator>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        FeverCoinType = ((DataType.FeverCoinType) ((int)type - 6));
    }
    protected override void Tick()
    {
        base.Tick();
    }
    public override void Init()
    {
        isCrashed = false;
        isDie = false;
        if(animator != null)
            animator.SetBool("Init", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCrashed)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Pet")
            {
                isDie = isCrashed = true;

                Effect();
                AddData();
            }
        }
    }
}
