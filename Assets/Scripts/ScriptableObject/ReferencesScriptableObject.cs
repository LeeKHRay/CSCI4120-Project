using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "References", menuName = "ScriptableObjects/ReferencesScriptableObject", order = 1)]
public class ReferencesScriptableObject : ScriptableObject
{
    private GameObject player;

    public GameObject Player { 
        get {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            return player;
        }
    }

    public Material[] materials;
}
