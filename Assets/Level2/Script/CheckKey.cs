﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKey : MonoBehaviour
{
    public GameObject[] save;
    public Door exit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckAns())
        {
            exit.Open();
        }
    }

    bool CheckAns()
    {
        for (int i = 0; i < save.Length; i++)
        {
            if (!save[i].GetComponent<AcceptItem>().correct)
            {
                return false;
            }
        }
        return true;
    }
}