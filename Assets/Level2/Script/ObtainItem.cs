using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainItem : MonoBehaviour
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

    public void PickUp()
    {
        Debug.Log("Pick up " + item.name);
        bool wasok;
        wasok = IInventory.instance.Add(item);
        if (wasok)
        {
            Destroy(gameObject);
        }
    }
}
