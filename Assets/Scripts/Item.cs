using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IInventoryItem
{
    public string itemName;
    public Texture itemTexture;    

    public void PickUp(Inventory inventory)
    {
        inventory.Add(this);
        gameObject.SetActive(false);
    }

    public abstract bool Use(IInteractableObject obj);
}
