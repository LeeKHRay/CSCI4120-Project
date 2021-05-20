using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlackHoleCannon : MonoBehaviour
{
    public int rotateDir;
    public GameObject bullet;
    public Transform origin;
    public GameObject translateVFX;
    public Transform boss;

    private MeshRenderer meshRenderer;
    private ParticleSystem flame;
    private Vector3 destination;
    private VisualEffect originTranslateVFX = null;
    private VisualEffect destinationTranslateVFX = null;
    private AudioSource audioSource;
    private bool shouldAttack = false;
    private bool shouldReturn = false;

    void Start()
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        flame = transform.GetChild(1).GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RotateCannon();

        if (shouldAttack)
        {
            StartCoroutine("CreateBlackHole");
            shouldAttack = false;
        }
        else if (shouldReturn)
        {
            StartCoroutine("Return");
            shouldReturn = false;
        }
    }

    public void Shoot(Transform target)
    {
        destination = new Vector3(target.position.x, 4f, target.position.z);
        shouldAttack = true;
    }

    private IEnumerator CreateBlackHole()
    {
        originTranslateVFX = Instantiate(translateVFX, transform.position, Quaternion.identity).GetComponent<VisualEffect>();
        originTranslateVFX.SetVector3("TargetOffset", destination - transform.position);
        originTranslateVFX.Play();
        meshRenderer.enabled = false;
        flame.Stop();

        yield return new WaitForSeconds(3.0f);

        transform.parent = null;
        transform.position = destination;
        Destroy(originTranslateVFX.gameObject);
        meshRenderer.enabled = true;
        flame.Play();
        Vector3 spawnPoint = new Vector3(transform.position.x, 0.5f, transform.position.z);
        transform.rotation = Quaternion.LookRotation(Vector3.down);

        yield return new WaitForSeconds(1.0f);

        Instantiate(bullet, spawnPoint, bullet.transform.rotation);
        audioSource.Play();

        yield return new WaitForSeconds(1.0f);

        shouldReturn = true;
    }

    private IEnumerator Return()
    {
        destinationTranslateVFX = Instantiate(translateVFX, transform.position, Quaternion.identity).GetComponent<VisualEffect>();
        destinationTranslateVFX.Play();
        meshRenderer.enabled = false;
        flame.Stop();

        yield return new WaitForSeconds(3.0f);

        transform.parent = boss;
        transform.position = origin.position;
        Destroy(destinationTranslateVFX.gameObject);
        meshRenderer.enabled = true;
        flame.Play();
        transform.rotation = Quaternion.LookRotation(boss.forward);
    }

    private void RotateCannon()
    {
        transform.Rotate(rotateDir * Vector3.forward * 500 * Time.deltaTime);
        if (originTranslateVFX != null)
        {
            originTranslateVFX.SetVector3("InitRotation", new Vector3(0, 180, -transform.rotation.eulerAngles.z));
            originTranslateVFX.SetVector3("FinalRotation", new Vector3(-90, 0, -transform.rotation.eulerAngles.z));
        }
        if (destinationTranslateVFX != null)
        {
            destinationTranslateVFX.SetVector3("TargetOffset", origin.position - transform.position);
            destinationTranslateVFX.SetVector3("InitRotation", new Vector3(-90, 0, transform.rotation.eulerAngles.y));
            destinationTranslateVFX.SetVector3("FinalRotation", new Vector3(0, boss.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.y));
        }
    }
}
