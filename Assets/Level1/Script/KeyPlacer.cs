using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPlacer : MonoBehaviour, IInteractableObject
{
    public Item key;
    public bool filled = false;
    public bool finished = false;

    public GameObject red;
    public GameObject green;
    public GameObject blue;
    public GameObject save;

    public string ans;
    public string whatitem = "";
    public bool correct = false;

    public void Interact(PlayerController player)
    {
       if (filled && !finished)
       {
            filled = false;
            key.PickUp(player.inventory);
            Destroy(save);
       }
    
    }

    public void PlaceKey(Item temp)
    {
        if (!filled)
        {
            filled = true;
            Transform pos = transform;
            key = temp;
            //Debug.Log("Have");
            //Debug.Log(temp.itemName);
            if (temp.itemName == "KeyRed")
            {
                save = Instantiate(red, pos.position + pos.up * 2.0f + new Vector3(0, 0, -1), pos.rotation);
                whatitem = "red";
            }
            else if (temp.itemName == "KeyGreen")
            {
                save = Instantiate(green, pos.position + pos.up * 2.0f + new Vector3(0, 0, -1), pos.rotation);
                whatitem = "green";
            }
            else if (temp.itemName == "KeyBlue")
            {
                save = Instantiate(blue, pos.position + pos.up * 2.0f + new Vector3(0, 0, -1), pos.rotation);
                whatitem = "blue";
            }
        }
       
    }

    void Update()
    {
        correct = (ans == whatitem);
    }
}
