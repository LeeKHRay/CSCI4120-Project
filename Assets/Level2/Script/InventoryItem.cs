using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;
    public bool isDefault = false;

    public virtual void Use(GameObject temp)
    {
        Debug.Log("Using " + name);

    }
}