using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCell : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private MeshRenderer meshRenderer;
    private Collider collider;
    private Light light;
    private Vector3 posOffset;


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        light = GetComponent<Light>();
        posOffset = transform.position;

        StartCoroutine("Spawn");
    }

    void Update()
    {
        Spin();
        Float();
    }

    private void Spin()
    {
        transform.Rotate(Vector3.up * 150 * Time.deltaTime);
    }

    private void Float()
    {
        Vector3 tempPos = posOffset;
        tempPos.y = Mathf.Sin(2 * Mathf.PI * frequency * Time.time) * amplitude + posOffset.y;
        transform.position = tempPos;
    }

    private IEnumerator Spawn()
    {
        // spawn energy cell after 30 seconds
        yield return new WaitForSeconds(30.0f);
        collider.enabled = true;
        meshRenderer.enabled = true;
        light.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.CanGetEnergyCell())
            {
                player.GetEnergyCell();
                collider.enabled = false;
                meshRenderer.enabled = false;
                light.enabled = false;
                StartCoroutine("Spawn");
            }
        }
    }
}
