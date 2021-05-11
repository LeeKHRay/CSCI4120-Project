using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnergyBall : MonoBehaviour
{
    public float movingSpeed = 8.0f;
    public float rotateSpeed = 90.0f;
    public int damage = 20;
    public float explosiveForce = 5000.0f;

    Vector3 currDir;
    private Transform target;
    private Rigidbody rigid;
    private VisualEffect energy;
    private VisualEffect explosion;
    private AudioSource audioSource;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rigid = GetComponent<Rigidbody>();
        energy = transform.GetChild(0).GetComponent<VisualEffect>();
        explosion = transform.GetChild(1).GetComponent<VisualEffect>();
        audioSource = GetComponent<AudioSource>();

        transform.LookAt(target);
        currDir = transform.forward;

        StartCoroutine("Explode");
    }

    void Update()
    {
        Vector3 heading = target.position - transform.position + target.up;

        currDir = Vector3.RotateTowards(currDir, heading, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
        rigid.velocity = currDir.normalized * movingSpeed;

        energy.SetVector3("TrailsDirection", -currDir.normalized); // produce tail
    }

    private IEnumerator Explode()
    {
        // explode after 10 seconds
        yield return new WaitForSeconds(10f);
        energy.playRate = 500;
        explosion.Play();
        audioSource.Play();
        Destroy(gameObject, 1.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        energy.playRate = 500;
        energy.Stop();
        explosion.Play();
        audioSource.Play();
        rigid.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;

        Vector3 forceDir = new Vector3(-collision.contacts[0].normal.x, 0, -collision.contacts[0].normal.z);
        if (collision.collider.tag == "Player")
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            player.Damage(damage);
            player.AddForce(forceDir * explosiveForce);
        }

        Destroy(gameObject, 1.0f);
    }
}
