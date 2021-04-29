/*
 *  Modified from ShieldCollision.cs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Shield : MonoBehaviour
{
    public string[] collisionTag;
    public float rotateSpeed = 0.1f;
    public int maxDurability = 50;
    public int durability = 50;
    public int restoreTime = 10;
    public Boss boss;
    public ShieldDurabilityBar shieldDurabilityBar;

    private float hitTime;
    private bool enhanced = false;
    private Material mat;
    private VisualEffect ve;

    void Start()
    {
        mat = GetComponent<Renderer>().sharedMaterial;
        mat.SetFloat("_HitTime", 0);
        ve = GetComponent<VisualEffect>();
    }

    void Update()
    {
        Rotate();

        if (hitTime > 0)
        {
            float myTime = Time.fixedDeltaTime * 1000;
            hitTime -= myTime;
            if (hitTime < 0)
            {
                hitTime = 0;
            }
            mat.SetFloat("_HitTime", hitTime);
        }

        if (durability <= 0)
        {
            boss.RecreateShield();
            gameObject.SetActive(false);
        }
    }

    private void Rotate()
    {
        if (mat.mainTextureOffset.x >= 10)
        {
            mat.mainTextureOffset = new Vector2(0, 0);
        }
        else
        {
            mat.mainTextureOffset += new Vector2(rotateSpeed * Time.deltaTime, 0);
        }
    }

    public void Damage(int damage, Collision collision)
    {
        durability -= damage;
        ContactPoint[] _contacts = collision.contacts;
        for (int j = 0; j < _contacts.Length; j++)
        {
            mat.SetVector("_HitPosition", transform.InverseTransformPoint(_contacts[j].point));
            hitTime = 300;
            mat.SetFloat("_HitTime", hitTime);
        }
    }

    public void Restore()
    {
        durability = maxDurability;
        if (enhanced)
        {
            ve.Play();
        }
    }

    public void Enhance()
    {
        maxDurability = 100;
        restoreTime = 5;
        shieldDurabilityBar.Enhance();
        enhanced = true;
    }
}
