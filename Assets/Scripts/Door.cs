using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool auto = true;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        animator.SetBool("Open", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (auto)
        {
            animator.SetBool("Open", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (auto)
        {
            animator.SetBool("Open", false);
        }
    }
}
