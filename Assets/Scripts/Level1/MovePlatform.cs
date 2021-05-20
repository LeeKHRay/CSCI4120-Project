using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public GameObject platformPathStart;
    public GameObject platformPathEnd;
    public int speed;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Rigidbody rBody;

    private Vector3 prevPos;
    private bool playerOnTop;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        prevPos = transform.position;

        rBody = GetComponent<Rigidbody>();
        startPosition = platformPathStart.transform.position;
        endPosition = platformPathEnd.transform.position;
        StartCoroutine(Vector3LerpCoroutine(endPosition, speed));
    }

    void Update()
    {
        if (rBody.position == endPosition)
        {
            StartCoroutine(Vector3LerpCoroutine(startPosition, speed));
        }
        if (rBody.position == startPosition)
        {
            StartCoroutine(Vector3LerpCoroutine(endPosition, speed));
        }
    }

    IEnumerator Vector3LerpCoroutine(Vector3 target, float speed)
    {
        Vector3 startPosition = transform.position;
        float time = 0f;

        while (rBody.position != target)
        {
            transform.position = Vector3.Lerp(startPosition, target, (time / Vector3.Distance(startPosition, target)) * speed);
            time += Time.deltaTime;

            // move player with the same distance
            if (playerOnTop)
            {
                player.position += transform.position - prevPos;
            }
            prevPos = transform.position;

            yield return null;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            playerOnTop = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            playerOnTop = false;
        }
    }

}
