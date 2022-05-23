using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 offset = new Vector3(0, 0, -10f);
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.25f;

    [SerializeField]
    Transform Target; 
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = Target.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
