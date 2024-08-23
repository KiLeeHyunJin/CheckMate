using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBearJelly : BaseJelly
{

    override protected void Go()
    {
        base.Go();
    }
    override protected void Tick()
    {
        if (InGameManager.instance.ismagatic && !isCrashed)
        {
            if(PetScript.instance != null)
            {
                target = PetScript.instance.transform.position;
                if (Vector3.Distance(target, transform.position) < dis)
                    transform.position = Vector3.MoveTowards(transform.position, target, 0.35f);
            }
        }
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
