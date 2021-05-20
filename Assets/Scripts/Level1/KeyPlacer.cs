using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPlacer : MonoBehaviour, IInteractableObject
{
    public GameObject red;
    public GameObject green;
    public GameObject blue;
    public string correctColor = "";
    public bool correct = false;
    public bool pass = false;

    private string keyColor = "";

    public void Interact(PlayerController player) 
    {
        if (keyColor != "" && !pass)
        {
            switch (keyColor) {
                case "Red":
                    red.GetComponent<Key>().PickUp(player.inventory);
                    break;

                case "Green":
                    green.GetComponent<Key>().PickUp(player.inventory);
                    break;

                case "Blue":
                    blue.GetComponent<Key>().PickUp(player.inventory);
                    break;
            }
            keyColor = "";
            correct = false;
        }
    }

    public bool PlaceKey(string color)
    {
        if (keyColor == "")
        {
            keyColor = color;
            switch (keyColor)
            {
                case "Red":
                    red.SetActive(true);
                    break;

                case "Green":
                    green.SetActive(true);
                    break;

                case "Blue":
                    blue.SetActive(true);
                    break;
            }

            correct = (keyColor == correctColor);
            return true;
        }
        return false;
    }
}
