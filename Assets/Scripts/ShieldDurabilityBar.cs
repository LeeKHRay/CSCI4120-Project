using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDurabilityBar : MonoBehaviour
{
    public Shield shield;

    private Slider shieldDurabilityBar;
    private Transform cam;

    void Start()
    {
        shieldDurabilityBar = GetComponent<Slider>();
        cam = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update()
    {
        shieldDurabilityBar.value = shield.durability;
        transform.LookAt(transform.position - cam.forward, cam.up);
    }

    public void Enhance()
    {
        shieldDurabilityBar.maxValue = 100;
    }
}
