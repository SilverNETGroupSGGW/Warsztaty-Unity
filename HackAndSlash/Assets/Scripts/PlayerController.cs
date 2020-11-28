    using System.Collections;
using System.Collections.Generic;
    using System.Numerics;
    using UnityEngine;
    using Quaternion = UnityEngine.Quaternion;
    using Vector3 = UnityEngine.Vector3;

    [RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    public Animator animator;

    Vector3 inputDir;
    bool inputJump;
    bool inputPunch;

    public Transform cam;
    public float speed = 5f;
    public float gravityForce = 1f;
    public float jumpForce = 15f;
    public float jumpTime = 0.4f;
    
    float rotVel;
    float rotSmoothTime = 0.1f;

    float remainingJumpTime;

    Coroutine fightingTimeoutCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PollInput();
        Move();
    }

    void PollInput()
    {
        float horizontal = Input.GetAxisRaw( "Horizontal" );
        float vertical = Input.GetAxisRaw( "Vertical" );

        inputJump = Input.GetKey( KeyCode.Space );

        inputPunch = Input.GetKey( KeyCode.Mouse0 );
        
        inputDir = new Vector3( horizontal, 0, vertical ).normalized;
    }

    void Move()
    {
        Vector3 moveDir = Vector3.zero;
        if( inputDir != Vector3.zero )
        {
            float targetAngle = Mathf.Atan2( inputDir.x, inputDir.z ) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle( transform.eulerAngles.y, targetAngle, ref rotVel, rotSmoothTime );
            
            moveDir = Quaternion.Euler( 0, targetAngle, 0 ) * Vector3.forward;
            transform.rotation = Quaternion.Euler( 0, angle, 0 );
            
            animator.SetBool( "running", true );
        }
        else
        {
            animator.SetBool( "running", false );

            if( inputPunch )
            {
                animator.SetTrigger( "punch_right" );
                animator.SetBool( "fighting", true );

                if( fightingTimeoutCoroutine != null )
                {
                    StopCoroutine( fightingTimeoutCoroutine );
                }
                fightingTimeoutCoroutine = StartCoroutine( FightAnimationTimeout() );
            }
        }

        if( inputJump && controller.isGrounded )
        {
            remainingJumpTime = jumpTime;
            animator.SetBool( "jumping", true );
        }

        Vector3 jumpVector = Vector3.zero;
        if( remainingJumpTime > 0 )
        {
            float lerpedJumpForce = Mathf.Lerp( jumpForce, gravityForce, 1 - (remainingJumpTime / jumpTime) );
            jumpVector = Vector3.up * lerpedJumpForce;

            remainingJumpTime -= Time.deltaTime;
        }
        else
        {
            animator.SetBool( "jumping", false );
        }

        Vector3 gravityDir = Vector3.down * gravityForce;
        
        controller.Move( Time.deltaTime * ( moveDir * speed + gravityDir + jumpVector ) );
    }

    IEnumerator FightAnimationTimeout()
    {
        yield return new WaitForSeconds( 2f );
        
        animator.SetBool( "fighting", false );
    }
}
