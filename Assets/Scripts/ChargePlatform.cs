using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePlatform : MonoBehaviour, IInteractableObject
{
    public GameObject[] battery;
    public bool charged = false;
    public bool smallChargePlatformr = true;

    private Light light;
    private int batteryNum = 0;

    void Start()
    {
        light = GetComponent<Light>();
    }

    public void Interact(PlayerController player)
    {
        if (smallChargePlatformr)
        {
            if (charged)
            {
                Debug.Log("collect");
                charged = false;
                battery[0].GetComponent<Battery>().PickUp(player.inventory);
                light.enabled = false;
            }
        }
    }

    public void PlaceBattery()
    {
        if (smallChargePlatformr)
        {
            charged = true;
            battery[0].SetActive(true);
            light.enabled = true;
        }
        else if (!charged) // big charge platform
        {
            battery[batteryNum].SetActive(true);
            batteryNum++;

            if (batteryNum == 4)
            {
                charged = true;
                light.enabled = true;
            }
        }
    }
}
