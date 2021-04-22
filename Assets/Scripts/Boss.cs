using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Minion
{
    public EnergyBallCannon energyBallCannon;
    public BlackHoleCannon blackHoleCannon;
    public ParticleSystem[] teleport;
    public Shield shield;

    private bool escaped = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine("AttackOrMove");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        Vector3 velocity = transform.InverseTransformDirection(agent.desiredVelocity); // convert velocity from world coordinates to local
        animator.SetFloat("ZSpeed", NormalizedSpeed(velocity.z));
        animator.SetFloat("XSpeed", NormalizedSpeed(velocity.x));

        switch (state) {
            case 0:
                if (ScanPlayer(target.transform.position - transform.position))
                {
                    state = 1;
                    agent.isStopped = true;
                }
                break;
            case 1:
                if (ScanPlayer(target.transform.position - transform.position))
                {
                    Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
                }
                else
                {
                    state = 0;
                    agent.isStopped = false;
                }
                break;
        }
    }

    private IEnumerator AttackOrMove()
    {
        while (true)
        {
            switch (state)
            {
                case 0:
                    animator.SetBool("Aiming", false);
                    yield return new WaitForSeconds(0.1f);
                    break;

                case 1:
                    animator.SetBool("Aiming", true);
                    transform.LookAt(target.position);
                    animator.SetTrigger("Shoot");
                    weapon.Shoot();
                    yield return new WaitForSeconds(0.3f);
                    weapon.Shoot();
                    yield return new WaitForSeconds(0.3f);
                    weapon.Shoot();
                    yield return new WaitForSeconds(3.0f);
                    break;

                case 2:
                    yield break;
            }
        }
    }

    private IEnumerator EnergyBallCannonAttack()
    {
        yield return new WaitForSeconds(3.0f);
        while (true)
        {
            switch (state)
            {
                case 0:
                    yield return new WaitForSeconds(0.1f);
                    break;

                case 1:
                    energyBallCannon.Shoot();
                    yield return new WaitForSeconds(6.0f);
                    break;

                case 2:
                    yield break;
            }
        }
    }

    private IEnumerator BlackHoleCannonAttack()
    {
        yield return new WaitForSeconds(5.0f);
        while (true)
        {
            switch (state)
            {
                case 0:
                    yield return new WaitForSeconds(0.1f);
                    break;

                case 1:
                    blackHoleCannon.Shoot(target);
                    yield return new WaitForSeconds(20.0f);
                    break;

                case 2:
                    yield break;
            }
        }
    }

    public void Escape()
    {
        StartCoroutine("EscapeCoroutine");
    }

    private IEnumerator EscapeCoroutine()
    {
        escaped = true;
        foreach (ParticleSystem ps in teleport)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(2.0f);
        state = 0;
        gameObject.SetActive(false);
    }

    public void Return()
    {
        // create shield
        shield.durability = 50;
        shield.gameObject.SetActive(true);
        rb.isKinematic = true; // set isKinematic to true to ensure collision point on shield is not shifted
        
        escaped = false;
        transform.LookAt(target);
        energyBallCannon.gameObject.SetActive(true);
        blackHoleCannon.gameObject.SetActive(true);
        StartCoroutine("AttackOrMove");
        StartCoroutine("EnergyBallCannonAttack");
        StartCoroutine("BlackHoleCannonAttack");
    }

    public override void Damage(int damage)
    {
        if (!escaped)
        {
            base.Damage(damage);
        }
    }

    protected override IEnumerator Die()
    {
        agent.isStopped = true;
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.0f);
        explosion.Play();
        smoke.Play();
        Destroy(blackHoleCannon.gameObject, 1.0f);
        Destroy(gameObject, 1.0f);
    }

    public void RecreateShield()
    {
        StartCoroutine("CreateShield");
    }

    private IEnumerator CreateShield()
    {
        rb.isKinematic = false;
        for (float i = 0; i < 10; i += 0.2f)
        {
            shield.durability += 1;
            yield return new WaitForSeconds(0.2f);
        }
        shield.durability = 50;
        shield.gameObject.SetActive(true);
        rb.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision)
    {

    }
}
