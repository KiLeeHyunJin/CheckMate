using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorial : MonoBehaviour
{
    [SerializeField] BoxCollider2D collider2D;
    [SerializeField] TutorialManager tutorialManager;
    // Start is called before the first frame update
    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        if (!collider2D.isTrigger)
            collider2D.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            tutorialManager.ActiveJumpInfo();
    }
}
