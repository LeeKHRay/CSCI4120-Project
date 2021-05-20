using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKey : MonoBehaviour
{    
    public KeyPlacer[] keyPlacers;
    public Door door;

    // Update is called once per frame
    void Update()
    {
        if (CheckAns())
        {
            door.Open();
            foreach (KeyPlacer placer in keyPlacers)
            {
                placer.pass = true;
            }
        }
    }

    bool CheckAns()
    {
        foreach (KeyPlacer placer in keyPlacers)
        {
            if (!placer.correct)
            {
                return false;
            }
        }
        return true;
    }
}
