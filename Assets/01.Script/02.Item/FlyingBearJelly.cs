using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBearJelly : BaseJelly
{
    bool isCrashed;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isCrashed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCrashed)
        {
            isCrashed = true;
            if (collision.gameObject.tag == "Player")
            {
                gameObject.layer = 13;
                //int addScore = GetScore();
                //InGameManager.instance.UpdateScore(addScore);
                AddData();
                SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Jelly);
                animator.SetTrigger("Die");
            }
        }
    }
}
