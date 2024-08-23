using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEating : MonoBehaviour
{
    [SerializeField] BirdMagnet Owner;
    CircleCollider2D circleCollider;
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!InGameManager.instance.isGameover)
        {
            if(Owner.isMagnet)
            {
                BaseJelly Temp = collision.GetComponent<BaseJelly>();
                if (Temp != null)
                {
                    int score = Temp.GetScore();
                    collision.gameObject.layer = 13;
                    InGameManager.instance.UpdateScore(score, Temp.type);
                    SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Jelly);
                    collision.GetComponent<Animator>().SetTrigger("Die");
                    Invoke("OnDisable", 5f);
                }
            }
        }
    }
}
