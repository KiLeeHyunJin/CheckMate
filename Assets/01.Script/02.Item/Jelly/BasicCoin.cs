using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCoin : BaseJelly
{
    CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }
    public override void Init()
    {
        isDie = isCrashed = false;
        if (animator != null)
        {
            animator.SetTrigger("Init");
        }
    }
    protected override void Tick()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * 900, 0));

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCrashed)
        {
            isCrashed = true;
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Pet")
            {
                gameObject.layer = 13;
                AddData();
                SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Coin);
                if(animator != null)
                    animator.SetTrigger("Die");
            }
        }
    }
}
