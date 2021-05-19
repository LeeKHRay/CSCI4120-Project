using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Handle : MonoBehaviour, IInteractableObject
{
    public GameObject puzzle;

    public bool panelOpened = false;

    public void Interact(PlayerController player)
    {
  
        player.canMove = panelOpened;

        // open password panel
        panelOpened = !panelOpened;
        puzzle.SetActive(panelOpened);
        Cursor.lockState = panelOpened ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = panelOpened;
      
    }
}
