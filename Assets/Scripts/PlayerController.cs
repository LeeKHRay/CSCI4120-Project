using System;
using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    public float groundDistance = 0.3f;
    public LayerMask whatIsGround;
    public float jumpForce = 200;
    public float speed = 5.0f;
    public Transform mainCam;
    public Transform aimingCam;
    public GameObject crosshair;
    public Inventory inventory;
    public int lifePoint = 500;
    public int energyCellNum = 3;
    public ParticleSystem thruster;
    public ParticleSystem explosion;
    public ParticleSystem smoke;
    public ParticleSystem healingEffect;

    private int maxLifePoint;
    private Rigidbody rb;
    private Animator animator;
    private LaserGun laserGun;
    private AudioSource audioSource;
    private bool isRecharging = false;

    private IInteractableObject interactableObject = null;
    private IInventoryItem inventoryItem = null;

    public GameObject interact;
    public GameObject Puzzle1;
    public GameObject Puzzle2;
    public GameObject Puzzle3;
    public GameObject Puzzle4;
    public GameObject Hint1;
    public GameObject Hint2;
    public GameObject Hint3;

    void Start()
    {
        maxLifePoint = lifePoint;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        laserGun = GetComponent<LaserGun>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canMove && lifePoint > 0)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, whatIsGround))
            {
                animator.SetBool("Grounded", true);
            }
            else
            {
                animator.SetBool("Grounded", false);
            }

            // aiming
            if (Input.GetMouseButtonDown(1))
            {
                float angle = mainCam.eulerAngles.x;
                if (angle < 16)
                {
                    angle = 20 * angle / 15;
                }
                else if (angle > 349)
                {
                    angle = -20 * (360 - angle) / 10;
                }
                SetAimingCameraAngle(angle); // reset aiming camera angle
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
            if (Input.GetMouseButtonUp(1))
            {
                float angle = aimingCam.eulerAngles.x;
                if (angle < 21)
                {
                    angle = 15 * angle / 20;
                }
                else if (angle > 339)
                {
                    angle = -10 * (360 - angle) / 20;
                }
                SetMainCameraAngle(angle);
            }

            // shoot
            if (animator.GetBool("Aiming") && Input.GetMouseButtonDown(0) && laserGun.CanShoot())
            {
                animator.SetTrigger("Shoot");
                laserGun.Shoot();
            }

            // recharge
            if (Input.GetKeyDown(KeyCode.R) && !isRecharging && energyCellNum > 0)
            {
                energyCellNum--;
                StartCoroutine("Recharge");
            }

            // jump
            if (animator.GetBool("Grounded") && Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
                thruster.Play();
                rb.AddForce(Vector3.up * jumpForce);
            }
        }

        // interact with object or pickup item
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactableObject != null)
            {
                interactableObject.Interact(this);
            }
            else if (inventoryItem != null)
            {
                Debug.Log("inventoryItem");
                inventoryItem.PickUp(inventory);
                inventoryItem = null;
            }
        }

        // use item
        if (Input.GetKeyDown(KeyCode.F))
        {
            inventory.UseItem(interactableObject);
        }
    }

    void LateUpdate()
    {
        if (canMove && lifePoint > 0)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            animator.SetFloat("ZSpeed", v);
            animator.SetFloat("XSpeed", h);
            animator.SetFloat("TurningSpeed", x);

            Vector3 camForward = Vector3.Scale(mainCam.forward, new Vector3(1, 0, 1)).normalized;
            rb.velocity = Vector3.ClampMagnitude(v * camForward + h * mainCam.right, 1f) * speed + transform.up * rb.velocity.y;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 80 * x * Time.fixedDeltaTime, 0));

            SetMainCameraAngle(mainCam.eulerAngles.x - 80 * y * Time.fixedDeltaTime);
            SetAimingCameraAngle(aimingCam.eulerAngles.x - 80 * y * Time.fixedDeltaTime);

        }
        else if (!canMove)
        {
            animator.SetFloat("ZSpeed", 0);
            animator.SetFloat("XSpeed", 0);
            animator.SetFloat("TurningSpeed", 0);
            rb.velocity = Vector3.zero;
        }
    }

    private IEnumerator Recharge()
    {
        isRecharging = true;
        animator.SetTrigger("Recharge");
        laserGun.Clear();
        yield return new WaitForSeconds(2.667f); // wait until recharge animation finishes
        laserGun.Recharge();
        isRecharging = false;
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

    private void SetMainCameraAngle(float angle)
    {
        Vector3 rotation = mainCam.eulerAngles;
        rotation.x = angle;
        if (rotation.x > 15 && rotation.x < 180)
        {
            rotation.x = 15;
        }
        else if (rotation.x < 350 && rotation.x > 180)
        {
            rotation.x = 350;
        }
        mainCam.eulerAngles = rotation;
    }

    private void SetAimingCameraAngle(float angle)
    {
        Vector3 rotation = aimingCam.eulerAngles;
        rotation.x = angle;
        if (rotation.x > 20 && rotation.x < 180)
        {
            rotation.x = 20;
        }
        else if (rotation.x < 340 && rotation.x > 180)
        {
            rotation.x = 340;
        }
        aimingCam.eulerAngles = rotation;
    }

    public void Damage(int damage)
    {
        if (lifePoint > 0)
        {
            lifePoint -= damage;
            if (lifePoint <= 0)
            {
                StartCoroutine("Die");
            }
        }
    }

    private IEnumerator Die()
    {
        mainCam.GetComponent<Camera>().enabled = true;
        aimingCam.GetComponent<Camera>().enabled = false;
        crosshair.SetActive(false);
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.0f);
        explosion.Play();
        smoke.Play();
        audioSource.Play();
    }

    public bool CanGetEnergyCell()
    {
        return energyCellNum < 3;
    }

    public void GetEnergyCell()
    {
        energyCellNum++;
    }

    public void Heal()
    {
        StartCoroutine("HealCoroutine");
    }

    private IEnumerator HealCoroutine()
    {
        healingEffect.Play();
        for (int i = 0; i < 10; i++)
        {
            // avoid healing after die
            if (lifePoint <= 0)
            {
                yield break;
            }

            if (lifePoint < maxLifePoint)
            {
                lifePoint += 5;
            }
            yield return new WaitForSeconds(1);
        }
        healingEffect.Stop();
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractableObject obj = other.GetComponent<IInteractableObject>();
        if (obj != null)
        {
            interactableObject = obj;
        }
        else
        {
            IInventoryItem item = other.GetComponent<IInventoryItem>();
            if (item != null)
            {
                inventoryItem = item;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractableObject obj = other.GetComponent<IInteractableObject>();
        if (obj != null)
        {
            interactableObject = null;
        }
        else
        {
            IInventoryItem item = other.GetComponent<IInventoryItem>();
            if (item != null)
            {
                inventoryItem = null;
            }
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        // Debug.Log("In");
        if (Input.GetKeyDown("e") && collider.gameObject.tag == "Puzzle1")
        {
            Puzzle1.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown("e") && collider.gameObject.tag == "Puzzle2")
        {
            Puzzle2.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown("e") && collider.gameObject.tag == "Puzzle3")
        {
            Puzzle3.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown("e") && collider.gameObject.tag == "Puzzle4")
        {
            Puzzle4.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown("e") && collider.gameObject.tag == "Hint1")
        {
            Hint1.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown("e") && collider.gameObject.tag == "Hint2")
        {
            Hint2.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown("e") && collider.gameObject.tag == "Hint3")
        {
            Hint3.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown("e"))  // can be e
        {
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            collider.gameObject.GetComponent<ItemPickUp>().Pickup(interact);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Special")
        {
            interact = collision.gameObject;
        }
        else if (collision.gameObject.tag == "DeathFloor")
        {
            Damage(lifePoint);
        }
    }
}
