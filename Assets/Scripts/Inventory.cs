using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Transform[] itemSlots;

    private Item[] items;
    private int itemIdx = -1;

    private IEnumerator coroutine = null;

    private int ItemIdx
    {
        set
        {
            if (itemIdx != value)
            {
                if (SelectItem(value))
                {
                    itemIdx = value;
                }
            }
        }
    }
    void Start()
    {
        items = new Item[4];
    }

    void Update()
    {
        // select item
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ItemIdx = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ItemIdx = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ItemIdx = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ItemIdx = 3;
        }
    }

    public void Add(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                itemSlots[i].GetChild(0).GetComponent<RawImage>().texture = item.itemTexture;
                break;
            }
        }

        if (items.Count(e => e != null) == 1)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = MoveInventory(true);
            StartCoroutine(coroutine);
            //transform.position += new Vector3(-200, 0, 0);
        }
    }

    public void UseItem(IInteractableObject Obj) {
        if (itemIdx != -1)
        {
            if (items[itemIdx].Use(Obj))
            {
                items[itemIdx] = null;
                itemSlots[itemIdx].GetComponent<Image>().color = Color.white;
                itemSlots[itemIdx].GetChild(0).GetComponent<RawImage>().texture = null;
                itemIdx = -1;
            }

            if (items.Count(e => e != null) == 0)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = MoveInventory(false);
                StartCoroutine(coroutine);
            }
        }

    }

    private bool SelectItem(int selectedItemIdx)
    {
        if (items[selectedItemIdx] != null)
        {
            if (itemIdx != -1)
            {
                itemSlots[itemIdx].GetComponent<Image>().color = Color.white;
            }
            itemSlots[selectedItemIdx].GetComponent<Image>().color = Color.green;
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator MoveInventory(bool show)
    {
        float t = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos;
        if (show)
        {
            endPos = startPos + new Vector3(-140, 0, 0);
        }
        else
        {
            endPos = startPos + new Vector3(140, 0, 0);
        }

        while (t < 1)
        {
            t += Time.deltaTime / 0.5f;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }
}
