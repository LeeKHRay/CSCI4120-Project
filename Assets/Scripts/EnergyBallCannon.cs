using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallCannon : MonoBehaviour
{
    public int rotateDir;
    public GameObject bullet;
    public float shootForce = 10f;

    void Update()
    {
        RotateCannon();
    }

    public void Shoot()
    {
        Debug.DrawLine(transform.position - transform.forward, -transform.forward * 1000, Color.blue);
        GameObject bulletObj = Instantiate(bullet, transform.position - transform.forward, transform.rotation * bullet.transform.rotation);
        bulletObj.GetComponent<Rigidbody>().AddForce(-transform.forward * shootForce);
    }

    private void RotateCannon()
    {
        transform.Rotate(rotateDir * Vector3.forward * 500 * Time.deltaTime);
    }
}
