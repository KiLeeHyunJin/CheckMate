using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySelectCharacter : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (animator == null)
            return;
        animator.SetBool("Init", true);
    }

}
