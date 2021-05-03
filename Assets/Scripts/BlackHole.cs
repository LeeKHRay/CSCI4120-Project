using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlackHole : MonoBehaviour
{
    public int damage = 30;
    public float attractiveForce = 100.0f;
    public float explosiveForce = 10000.0f;

    private Transform target;
    private PlayerController playerController;
    private VisualEffect blackHole;
    private VisualEffect explosion;
    private AudioSource audioSource;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = target.GetComponent<PlayerController>();
        blackHole = transform.GetChild(0).GetComponent<VisualEffect>();
        explosion = transform.GetChild(1).GetComponent<VisualEffect>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine("Explode");
    }

    void Update()
    {
        Vector3 heading = target.position - transform.position + target.up;
        playerController.AddForce(-heading.normalized * attractiveForce); // attract player
    }

    private IEnumerator Explode()
    {
        // explode after 15 seconds
        yield return new WaitForSeconds(15f);
        attractiveForce = 0;
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        blackHole.gameObject.SetActive(false);
        explosion.Play();
        audioSource.Play();
        Destroy(gameObject, 1.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            blackHole.playRate = 500;
            blackHole.Stop();
            explosion.Play();
            audioSource.Play();

            attractiveForce = 0;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            playerController.Damage(damage);
            Vector3 forceDir = new Vector3(-collision.contacts[0].normal.x, 0, -collision.contacts[0].normal.z);
            playerController.AddForce(forceDir * explosiveForce);
            Destroy(gameObject, 1.0f);
        }
    }
}
