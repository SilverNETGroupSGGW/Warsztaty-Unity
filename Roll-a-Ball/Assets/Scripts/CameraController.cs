using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    Vector3 velocity;

    public float smoothTime = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp( transform.position, target.position + offset, ref velocity, smoothTime );
    }
}
