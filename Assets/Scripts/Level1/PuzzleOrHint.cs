using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOrHint : MonoBehaviour, IInteractableObject
{
    public GameObject puzzleOrHint;

    private bool uiShowed = false;
    private PlayerController player;

    public void Interact(PlayerController player)
    {
        this.player = player;
        player.canMove = uiShowed;

        uiShowed = !uiShowed;
        puzzleOrHint.SetActive(uiShowed);
        Cursor.lockState = uiShowed ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = uiShowed;
    }

    public void CloseUI()
    {
        player.canMove = true;

        uiShowed = false;
        puzzleOrHint.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
