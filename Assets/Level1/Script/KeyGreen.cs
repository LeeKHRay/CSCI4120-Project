using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGreen : Item
{
    public override bool Use(IInteractableObject obj)
    {
        if (obj != null)
        {
            if (obj is KeyPlacer)
            {
                //Debug.Log("how");
                ((KeyPlacer)obj).PlaceKey(this);
                return true;
            }
        }
        return false;
    }
}