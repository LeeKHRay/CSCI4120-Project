using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cam;

    private RaycastHit hit;
    private Vector3 offset;

    void Start()
    {
        offset = cam.localPosition;
    }

    void Update()
    {
        if (Physics.Linecast(transform.position, transform.position + transform.rotation * offset, out hit))
        {
            cam.localPosition = new Vector3(0, 0, -Vector3.Distance(transform.position, hit.point));
            
        }
        else
        {
            // move to original position smoothly
            cam.localPosition = Vector3.Lerp(cam.localPosition, offset, Time.deltaTime);
        }
    }
}
