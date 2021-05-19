using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    InventoryItem item;
    public Image icon;
    public PlayerController player;

    public void AddItem(InventoryItem newitem)
    {
        Debug.Log(newitem.name);
        item = newitem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void UseItem()
    {
        Debug.Log("hi");
        InventoryItem prev = item;
        if (item != null && player.interact != null)
        {
            // item.Use(player.interact);
            bool wasok = player.interact.GetComponent<AcceptItem>().Accept(prev);
            if (wasok)
            {
                ClearSlot();
                IInventory.instance.Remove(prev);
            }
        }
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
