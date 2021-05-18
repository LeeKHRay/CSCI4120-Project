using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Item
{
    public override bool Use(IInteractableObject obj)
    {
        if (obj != null)
        {
            if (obj is ChargePlatform)
            {
                ((ChargePlatform)obj).PlaceBattery();
                return true;
            }
        }
        return false;
    }
}
