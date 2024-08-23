using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : BaseJelly
{
    //[SerializeField] float scale;
    CircleCollider2D circleCollider2;
    BoxCollider2D boxCollider2;

    PetScript petScript;
    override protected void Go()
    {
        base.Go();
        circleCollider2 = GetComponent<CircleCollider2D>();
        if (circleCollider2 != null)
            circleCollider2.isTrigger = true;

        boxCollider2 = GetComponent<BoxCollider2D>();  
        if(boxCollider2 != null)
            boxCollider2.isTrigger = true;

        petScript = PetScript.instance;
    }

    public override void Init()
    {
        isDie = isCrashed = false;
        if(animator != null)
        {
            animator.SetTrigger("Init");
        }
    }

    override protected void Tick()
    {
        //if (!isCrashed)
        //{
        //    if (petScript != null)
        //    {
        //        target = petScript.transform.position;
        //        if (Vector3.Distance(target, transform.position) < dis)
        //            transform.position = Vector3.MoveTowards(transform.position, target, 0.35f);
        //    }
        //    else
        //        Debug.Log("PetData is Null Ref");
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCrashed)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Pet")
            {
                isCrashed = true;

                AddData();
                Effect();
            }
        }
    }
}
