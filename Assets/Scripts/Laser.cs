using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 10;
    public ParticleSystem spark;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Vanish");
    }

    private IEnumerator Vanish()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        spark.Play();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<PlayerController>().Damage(damage);
        }
        else if (collision.collider.tag == "Minion")
        {
            collision.collider.GetComponent<Minion>().Damage(damage);
        }
        else if (collision.collider.tag == "Boss")
        {
            collision.collider.GetComponent<Boss>().Damage(damage);
        }
        else if (collision.collider.tag == "Shield")
        {
            collision.collider.GetComponent<Shield>().Damage(damage, collision);
        }
        Destroy(gameObject, 2.0f);
    }
}
