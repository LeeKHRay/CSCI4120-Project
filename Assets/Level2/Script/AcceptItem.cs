using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptItem : MonoBehaviour
{

    public GameObject red;
    public GameObject green;
    public GameObject blue;
    public Transform pos;
    public bool hasObj = false;

    public string ans;
    public string whatitem = "";
    public bool correct = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        correct = (ans == whatitem);
    }

    public void TakeAway()
    {
        hasObj = false;
        whatitem = "";
    }

    public bool Accept(InventoryItem temp)
    {
        if (!correct)
        {
            if (!hasObj)
            {
                if (temp.name == "KeyRed")
                {
                    Instantiate(red, pos.position + pos.up * 2.0f + new Vector3(0, 0, -1), pos.rotation);
                    whatitem = "red";
                }
                else if (temp.name == "KeyGreen")
                {
                    Instantiate(green, pos.position + pos.up * 2.0f + new Vector3(0, 0, -1), pos.rotation);
                    whatitem = "green";
                }
                else if (temp.name == "KeyBlue")
                {
                    Instantiate(blue, pos.position + pos.up * 2.0f + new Vector3(0, 0, -1), pos.rotation);
                    whatitem = "blue";
                }
                else
                {
                    return false;
                }
                hasObj = true;
                return true;
            }
        }
        return false;
    }
}
