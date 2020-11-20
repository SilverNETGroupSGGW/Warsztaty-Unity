using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    CharacterController controller;

    public Transform cam;
    
    public float speed = 6f;
    public float jumpForce = 3f;
    public float jumpTime = .5f;
    
    public float gravity = 1f;

    float rotSmoothTime = 0.1f;
    float rotVel;
    
    Vector3 inputDir;
    bool inputJump;
    float remainingJumpTime;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        PollInput();
        Move();
    }
    

    void PollInput()
    {
        float horizontal = Input.GetAxisRaw( "Horizontal" );
        float vertical = Input.GetAxisRaw( "Vertical" );
        
        inputDir = new Vector3( horizontal, 0, vertical ).normalized;

        inputJump = Input.GetKey( KeyCode.Space );
    }

    void Move()
    {
        Vector3 moveDir = Vector3.zero;

        // Handle direction input
        if( inputDir != Vector3.zero )
        {
            float targetAngle = Mathf.Atan2( inputDir.x, inputDir.z ) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle( transform.eulerAngles.y, targetAngle, ref rotVel, rotSmoothTime );
            
            moveDir = Quaternion.Euler( 0, targetAngle, 0 ) * Vector3.forward;
            
            transform.rotation = Quaternion.Euler( 0, angle, 0 );
        }

        // Apply gravity
        if( !controller.isGrounded )
        {
            moveDir.y -= gravity;
        }
        else
        {
            // Handle jump input
            if( inputJump )
            {
                remainingJumpTime = jumpTime;
            }
        }

        // Jump
        if( remainingJumpTime > 0 )
        {
            remainingJumpTime -= Time.deltaTime;
            moveDir.y += jumpForce;
        }
            
        controller.Move( moveDir * (speed * Time.deltaTime) );
    }
}
