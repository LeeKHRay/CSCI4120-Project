using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePointBar : MonoBehaviour
{
    public Minion enemy;

    private Slider lifePointBar;
    private Transform cam;

    void Start()
    {
        lifePointBar = GetComponent<Slider>();
        cam = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update()
    {
        lifePointBar.value = enemy.lifePoint;
        transform.LookAt(transform.position - cam.forward, cam.up);
    }
}
