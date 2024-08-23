using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJelly : BaseJelly
{
    [SerializeField] float value;

    protected void Go()
    {
        animator = GetComponent<Animator>();
        isCrashed = false;
        dis = 2.5f;
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
            isCrashed = true;
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Pet")
            {
                ItemEffect();
                Effect();
            }
        }
    }

    void ItemEffect()
    {
        switch (type)
        {

            case DataType.JellyType.SmallTime:
                AddTime(value);
                break;
            case DataType.JellyType.BigTime:
                AddTime(value);
                break;
            default:
                break;
        }
    }
    void AddTime(float _value)
    {
        InGameManager.instance.Healhp(_value);
    }
}
