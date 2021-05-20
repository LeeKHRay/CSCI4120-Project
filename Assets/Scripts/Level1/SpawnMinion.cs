using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    public Transform minionSpawnPoint;
    public GameObject minion;

    private ParticleSystem[] particleSystems;
    private bool spawned = false;


    void Start()
    {
        particleSystems = minionSpawnPoint.GetComponentsInChildren<ParticleSystem>();
    }

    private IEnumerator Spawn()
    {
        particleSystems[0].Play();
        particleSystems[1].Play();
        yield return new WaitForSeconds(2.0f);
        minion.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!spawned && other.tag == "Player")
        {
            spawned = true;
            StartCoroutine("Spawn");
        }
    }
}
