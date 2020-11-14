using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public float rotationSpeed = 100;
    public float bobbingHeight = 0.5f;
    public float bobbingSpeed = 10f;
    
    Vector3 spawnPos;
    
    void Start()
    {
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate( Vector3.up * (rotationSpeed * Time.deltaTime) );
        transform.position = spawnPos + Vector3.up * (Mathf.Sin( Time.timeSinceLevelLoad * bobbingSpeed ) * bobbingHeight);
    }
}
