using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPoint : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem ps;
    private Vector3 posOffset;
    private Collider collider;
    private Light light;
    private Transform cam;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider>();
        light = GetComponent<Light>();
        ps = GetComponent<ParticleSystem>();
        posOffset = transform.position;
        cam = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update()
    {
        transform.LookAt(transform.position - cam.forward, cam.up);
        Float();
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
        spriteRenderer.enabled = true;
        light.enabled = true;
        ps.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Heal();
            collider.enabled = false;
            spriteRenderer.enabled = false;
            light.enabled = false;
            ps.Stop();
            StartCoroutine("Spawn");
        }
    }
}
