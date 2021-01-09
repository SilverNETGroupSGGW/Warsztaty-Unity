using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public LayerMask raycastLayer;
    
    [Header("Settings")]
    public float speed = 100f;
    public float velocityDamping = 0.1f;
    public float rotationSmoothingTime = 0.15f;

    Rigidbody rb;
    float angleVelocity;
    
    // Input values
    Vector3 inputDir;
    Vector3 pointerPos;

    Vector3 movementDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PollInput();
    }

    void LateUpdate()
    {
        PollPointerPos();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }


    void PollInput()
    {
        // Get the movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        inputDir = new Vector3(horizontal, 0, vertical).normalized;
    }

    // TODO: add movement SFX
    void HandleMovement()
    {
        // Damp the player velocity, so they won't slip too much
        // The smaller the `velocityDamping` value, the tighter the controls feel
        rb.velocity *= (1 - velocityDamping);

        // Move the player based on input vector
        Quaternion cameraRot = Quaternion.Euler( 0, cam.transform.eulerAngles.y, 0 );
        movementDir = cameraRot * inputDir;
        rb.AddForce( movementDir * ( speed * Time.fixedDeltaTime ), ForceMode.VelocityChange );

        // Rotate the player to look at mouse
        Vector3 targetPos = pointerPos - transform.position;
        float targetAngle = Mathf.Atan2( targetPos.x, targetPos.z ) * Mathf.Rad2Deg;

        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angleVelocity,
            rotationSmoothingTime);
        
        rb.MoveRotation( Quaternion.Euler(0, smoothedAngle, 0) );
    }
    

    void PollPointerPos()
    {
        // Get world mouse position with a raycast from the camera
        // The player has a ground level plane that functions as the ray target
        Ray ray = cam.ScreenPointToRay( Input.mousePosition );
        Physics.Raycast( ray, out RaycastHit hit, 200f, raycastLayer );

        pointerPos = hit.point;
    }
}