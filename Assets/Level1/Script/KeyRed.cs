using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRed : Item
{
    public override bool Use(IInteractableObject obj)
    {
        if (obj != null)
        {
            if (obj is KeyPlacer)
            {
                ((KeyPlacer)obj).PlaceKey(this);
                return true;
            }
        }
        return false;
    }
}
