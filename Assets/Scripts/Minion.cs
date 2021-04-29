using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    public Transform bulletPos;
    public float rotationDamping = 6.0f;
    public float viewDistance = 20.0f;
    public float viewAngle = 120.0f;
    public int lifePoint = 100;
    public ParticleSystem explosion;
    public ParticleSystem smoke;
    public LayerMask ignoreLayer;

    protected Animator animator;
    protected Rigidbody rb;
    protected Weapon weapon;
    protected NavMeshAgent agent;
    protected Transform target;
    protected AudioSource audioSource;
    protected int state = 0;

    private Vector3 prevDir;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        prevDir = transform.forward * 10;

        StartCoroutine("AttackOrMove");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        Vector3 velocity = transform.InverseTransformDirection(agent.desiredVelocity); // convert velocity from world coordinates to local
        animator.SetFloat("Speed", NormalizedSpeed(velocity.magnitude));
        animator.SetFloat("TurningSpeed", AngularVelocity());

        switch (state) {
            case 0:
                animator.SetBool("Aiming", false);
                if (ScanPlayer(target.transform.position - transform.position))
                {
                    state = 1;
                    agent.isStopped = true;
                }
                break;
            case 1:
                animator.SetBool("Aiming", true);
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
                    yield return new WaitForSeconds(0.1f);
                    break;

                case 1:
                    transform.LookAt(target.position);
                    animator.SetTrigger("Shoot");
                    weapon.Shoot();
                    yield return new WaitForSeconds(3.0f);
                    break;

                case 2:                    
                    yield break;
            }
        }
    }

    protected bool ScanPlayer(Vector3 heading)
    {
        if (Vector3.Dot(heading.normalized, transform.forward) > Mathf.Cos(viewAngle / 2 * Mathf.Deg2Rad) && Vector3.Magnitude(heading) < viewDistance)
        {
            Debug.DrawLine(bulletPos.position - heading.normalized, bulletPos.position - heading.normalized + heading.normalized * viewDistance, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(bulletPos.position - heading.normalized, heading, out hit, viewDistance, ~ignoreLayer))
            {
                if (hit.collider.tag.Equals("Player")) // check if there is no occluders
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected float NormalizedSpeed(float speed)
    {
        return speed / agent.speed;
    }

    private float AngularVelocity()
    {
        Vector3 currDir = transform.forward * 10;
        float angularVelocity = Vector3.Angle(currDir, prevDir) / Time.deltaTime;

        if (Vector3.Cross(prevDir, currDir).y < 0) // turn left
        {
            prevDir = currDir;
            return -angularVelocity / 180.0f;
        }

        // turn right
        prevDir = currDir;
        return angularVelocity / 180.0f;
    }

    public virtual void Damage(int damage)
    {
        if (lifePoint > 0)
        {
            lifePoint -= damage;
            if (lifePoint <= 0)
            {
                state = 2;
                StartCoroutine("Die");
            }
        }
    }

    protected virtual IEnumerator Die()
    {
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);
        agent.isStopped = true;
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.0f);
        explosion.Play();
        smoke.Play();
        audioSource.Play();
        Destroy(gameObject, 1.0f);
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    void OnCollisionEnter(Collision collision)
    {

    }
}
