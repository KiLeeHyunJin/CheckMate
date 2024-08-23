using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMagnet : MonoBehaviour
{
    CircleCollider2D collider;
    public bool isMagnet;
    [SerializeField] float dis = .1f;

    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMagnet)
            collider.enabled = true;
        else
            collider.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isMagnet)
        {
            string tagName = collision.gameObject.tag;
            if (tagName == "Jelly" || tagName == "Basicjelly")
            {
                Vector3 target = collision.transform.position;
                float Distance = Vector3.Distance(target, transform.position);
                if (Distance >= dis)
                    collision.transform.position = Vector3.MoveTowards(target, transform.position, 0.5f);
            }
        }
    }
}
