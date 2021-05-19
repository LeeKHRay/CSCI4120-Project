using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneDoor : MonoBehaviour
{
    public bool auto = true;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        animator.SetBool("character_nearby", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (auto)
        {
            animator.SetBool("character_nearby", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (auto)
        {
            animator.SetBool("character_nearby", false);
        }
    }
}
