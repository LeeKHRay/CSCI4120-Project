using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingFloorConsole : MonoBehaviour, IInteractableObject
{
    public Animator animator;

    private bool rise = false;

    public void Interact(PlayerController player)
    {
        rise = !rise;
        animator.SetBool("Rise", rise);
        if (rise)
        {
            StartCoroutine("SinkFloor");
        }
        else
        {
            StopCoroutine("SinkFloor");
        }
    }

    private IEnumerator SinkFloor()
    {
        yield return new WaitForSeconds(40);
        rise = false;
        animator.SetBool("Rise", false);
    }
}
