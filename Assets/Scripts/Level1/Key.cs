using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public string color;

    public override bool Use(IInteractableObject obj)
    {
        if (obj != null)
        {
            if (obj is KeyPlacer)
            {
                return ((KeyPlacer) obj).PlaceKey(color);
            }
        }
        return false;
    }
}
