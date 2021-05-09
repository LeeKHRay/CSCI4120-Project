using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPoint : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 posOffset;
    private Transform cam;

    void Start()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Heal();
            Destroy(gameObject);
        }
    }
}
