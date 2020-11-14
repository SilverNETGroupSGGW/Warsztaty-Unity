using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 50;

    Rigidbody rb;
    Vector3 movementDirection;

    public GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start" );

        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PollInput();
    }

    void FixedUpdate()
    {
        Move( movementDirection );
    }

    void OnTriggerEnter( Collider other )
    {
        if( other.CompareTag( "Point" ) )
        {
            gameManager.OnPointCollect();
            Destroy( other.gameObject );
        }
    }

    void PollInput()
    {
        float horizontal = Input.GetAxis( "Horizontal" );
        float vertical = Input.GetAxis( "Vertical" );

        movementDirection = new Vector3( horizontal, 0, vertical ).normalized;
    }

    void Move( Vector3 movementDirection )
    {
        rb.AddForce( movementDirection * (speed * Time.fixedDeltaTime) );
    }
}
