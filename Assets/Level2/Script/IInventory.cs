using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
    // Start is called before the first frame update

    #region Singleton

    public static IInventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();

    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public List<InventoryItem> items = new List<InventoryItem>();

    public bool Add(InventoryItem item)
    {
        if (!item.isDefault)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room");
                return false;
            }
            items.Add(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        return true;
    }

    public void Remove(InventoryItem item) {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
