using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    void PickUp(Inventory inventory);

    bool Use(IInteractableObject obj);
}
