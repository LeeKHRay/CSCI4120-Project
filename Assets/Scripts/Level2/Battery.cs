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
                return ((ChargePlatform)obj).PlaceBattery();
            }
        }
        return false;
    }
}
