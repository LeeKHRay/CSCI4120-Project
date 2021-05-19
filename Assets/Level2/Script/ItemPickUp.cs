using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public InventoryItem item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pickup(GameObject temp)
    {
        Debug.Log("Pick up " + item.name);
        bool wasok;
        wasok = IInventory.instance.Add(item);
        if (wasok)
        {
            if (temp != null)
            {
                temp.GetComponent<AcceptItem>().TakeAway();
            }
            Destroy(gameObject);
        }
    }
}
