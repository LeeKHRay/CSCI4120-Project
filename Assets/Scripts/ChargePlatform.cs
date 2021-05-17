using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePlatform : MonoBehaviour, IInteractableObject
{
    public GameObject[] battery;
    public bool charged = false;
    public bool smallChargePlatformr = true;

    private int batteryNum = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(PlayerController player)
    {
        if (smallChargePlatformr)
        {
            if (player.itemName.Equals("Battery") && player.itemNum > 0)
            {
                charged = !charged;
                if (charged)
                {
                    battery[0].SetActive(true);
                    player.PutItem();
                }
                else
                {
                    player.GetItem(battery[0].GetComponent<Item>(), false);
                    battery[0].SetActive(false);
                }
            }
            else if (charged) // take away battery
            {
                charged = false;
                player.GetItem(battery[0].GetComponent<Item>(), false);
                battery[0].SetActive(false);
            }
        }
        else if (!charged) // big charge platform
        {
            if (player.itemName.Equals("Battery") && player.itemNum > 0)
            {
                battery[batteryNum].SetActive(true);
                batteryNum++;
                player.PutItem();

                if (batteryNum == 4)
                {
                    charged = true;
                }
            }
        }
    }
}
