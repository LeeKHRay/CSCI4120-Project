using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera aimingCam = null;
    public GameObject bullet;
    public Transform bulletPos;
    public float shootForce = 1000f;
    public int maxAmmo = 20;

    private int ammo;

    void Start()
    {
        ammo = maxAmmo;
    }

    public virtual void Shoot()
    {
        GameObject bulletObj;
        if (aimingCam != null) // for player
        {
            Vector3 dir;
            bulletObj = Instantiate(bullet, bulletPos.position, aimingCam.transform.rotation * bullet.transform.rotation);
            RaycastHit hit;
            if (Physics.Raycast(aimingCam.transform.position, aimingCam.transform.forward, out hit, aimingCam.farClipPlane))
            {
                dir = (hit.point - bulletPos.position).normalized;
            }
            else
            {
                Vector3 point = aimingCam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, aimingCam.farClipPlane));
                dir = (point - bulletPos.position).normalized;
            }
            Debug.DrawLine(bulletPos.position, bulletPos.position + dir * 1000, Color.red);
            bulletObj.GetComponent<Rigidbody>().AddForce(dir * shootForce);
            ammo--;
        }
        else // for enemy
        {
            bulletObj = Instantiate(bullet, bulletPos.position, transform.rotation * bullet.transform.rotation);
            bulletObj.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        ammo = 0;
        yield return new WaitForSeconds(2.667f); // wait until reload animation finishes
        ammo = maxAmmo;
    }

    public bool CanShoot()
    {
        return ammo > 0;
    }

    public int ammoNum()
    {
        return ammo;
    }

    public string ammoInfo()
    {
        return ammo + "/" + maxAmmo;
    }
}
