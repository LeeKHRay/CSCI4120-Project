using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DropMinion : MonoBehaviour
{
    public Minion t;
    public GameObject spawnObject;
    public GameObject activateObject;
    public bool first = true;

    // Update is called once per frame
    void Update()
    {
        if (t.isDead && first)
        {
            first = false;
            StartCoroutine("Spawn");
        }
    }
    IEnumerator Spawn()
    {
        Transform t = transform;
        yield return new WaitForSeconds(1.0f);
        Instantiate(spawnObject, t.position + t.up * 1.0f, t.rotation);
        activateObject.SetActive(true);
    }
}
