using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    [SerializeField] GameObject newParents;
    BoxCollider2D boxCollider2;
    void Start()
    {
        boxCollider2 = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.parent = newParents.transform;
        collision.gameObject.SetActive(false);
    }
}
