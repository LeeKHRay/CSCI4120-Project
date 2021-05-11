using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public Camera aimingCam = null;
    public GameObject laser;
    public Transform laserPos;
    public float shootForce = 1000f;
    public int maxEnergy = 20;
    public AudioSource audioSource;

    private int energy;

    void Start()
    {
        energy = maxEnergy;
    }

    public virtual void Shoot(int damage = 10)
    {
        GameObject bulletObj;
        if (aimingCam != null) // for player
        {
            Vector3 dir;
            bulletObj = Instantiate(laser, laserPos.position, aimingCam.transform.rotation * laser.transform.rotation);
            RaycastHit hit;
            if (Physics.Raycast(aimingCam.transform.position, aimingCam.transform.forward, out hit, aimingCam.farClipPlane))
            {
                dir = (hit.point - laserPos.position).normalized;
            }
            else
            {
                Vector3 point = aimingCam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, aimingCam.farClipPlane));
                dir = (point - laserPos.position).normalized;
            }
            Debug.DrawLine(laserPos.position, laserPos.position + dir * 1000, Color.red);
            bulletObj.GetComponent<Rigidbody>().AddForce(dir * shootForce);
            energy--;
        }
        else // for enemy
        {
            bulletObj = Instantiate(laser, laserPos.position, transform.rotation * laser.transform.rotation);
            bulletObj.GetComponent<Laser>().SetDamage(damage);
            bulletObj.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void Clear()
    {
        energy = 0;
    }

    public void Recharge()
    {
        energy = maxEnergy;
    }

    public bool CanShoot()
    {
        return energy > 0;
    }

    public int GetEnergy()
    {
        return energy;
    }
}
