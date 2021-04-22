using System;
using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float groundDistance = 0.3f;
    public LayerMask whatIsGround;
    public float jumpForce = 200;
    public float speed = 5.0f;
    public Transform mainCam;
    public Transform aimingCam;
    public GameObject crosshair;
    public int lifePoint = 500;
    public ParticleSystem thruster;
    public ParticleSystem explosion;
    public Rigidbody rb;

    private Animator animator;
    private Weapon weapon;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (lifePoint > 0)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, whatIsGround))
            {
                animator.SetBool("Grounded", true);
                animator.applyRootMotion = true;
            }
            else
            {
                animator.SetBool("Grounded", false);
            }

            // aiming
            if (Input.GetMouseButtonDown(1))
            {
                SetAimingCameraAngle(0); // reset aiming camera angle
            }
            if (Input.GetMouseButton(1))
            {
                animator.SetBool("Aiming", true);
                speed = 2.5f; // walk slowly when aiming
                SwitchCamera();
            }
            else
            {
                animator.SetBool("Aiming", false);
                speed = 5.0f;
                SwitchCamera();
            }

            // shoot
            if (animator.GetBool("Aiming") && Input.GetMouseButtonDown(0) && weapon.CanShoot())
            {
                animator.SetTrigger("Shoot");
                weapon.Shoot();
            }

            // reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                animator.SetTrigger("Reload");
                weapon.Reload();
            }
        }        
    }

    // use FixedUpdate() for Physics calculations
    void FixedUpdate()
    {
        if (lifePoint > 0)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            animator.SetFloat("ZSpeed", v);
            animator.SetFloat("XSpeed", h);
            animator.SetFloat("TurningSpeed", x);

            Vector3 camForward = Vector3.Scale(mainCam.forward, new Vector3(1, 0, 1)).normalized;
            rb.velocity = (v * camForward + h * mainCam.right) * speed + transform.up * rb.velocity.y;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 80 * x * Time.fixedDeltaTime, 0));

            SetAimingCameraAngle(aimingCam.transform.eulerAngles.x - 80 * y * Time.fixedDeltaTime);

            // jump
            if (animator.GetBool("Grounded") && Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
                thruster.Play();
                rb.AddForce(Vector3.up * jumpForce);
            }
        }
    }

    private void SwitchCamera()
    {
        if (animator.GetBool("Aiming"))
        {
            mainCam.GetComponent<Camera>().enabled = false;
            aimingCam.GetComponent<Camera>().enabled = true;
            crosshair.SetActive(true);
        }
        else
        {
            mainCam.GetComponent<Camera>().enabled = true;
            aimingCam.GetComponent<Camera>().enabled = false;
            crosshair.SetActive(false);
        }
    }

    private void SetAimingCameraAngle(float angle)
    {
        Vector3 rotation = aimingCam.transform.eulerAngles;
        rotation.x = angle;
        if (rotation.x > 20 && rotation.x < 180)
        {
            rotation.x = 20;
        }
        else if (rotation.x < 340 && rotation.x > 180)
        {
            rotation.x = 340;
        }
        aimingCam.transform.eulerAngles = rotation;
    }

    public void Damage(int damage)
    {
        if (lifePoint > 0)
        {
            lifePoint -= damage;
            if (lifePoint <= 0)
            {
                mainCam.GetComponent<Camera>().enabled = true;
                aimingCam.GetComponent<Camera>().enabled = false;
                crosshair.SetActive(false);
                explosion.Play();
                animator.SetTrigger("Dead");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {

    }
}
