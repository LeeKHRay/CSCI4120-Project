using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePickUp : MonoBehaviour
{
    public Item key;
    public Inventory i;

    public void Click()
    {
        key.PickUp(i);
        gameObject.SetActive(false);
    }
}
