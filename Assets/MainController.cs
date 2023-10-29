using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity = 2.0f;
    public float walkSpeed = 6.0f;
    public float runSpeed = 12f;
    [SerializeField] float gravity = -20.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.2f)] float mouseSmoothTime = 0.03f;
    public LayerMask groundLayer;

    [SerializeField] bool lockCursor = true;

    [SerializeField] KeyCode jumpKey;
    public KeyCode runKey;
    public KeyCode crouchKey;
    [SerializeField] float jumpHeight;
    bool isJumping = false;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    Vector2 currentDirection = Vector2.zero;
    Vector2 currentDirectionVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    float speed = 6f;
    float currTimeRun = 0f;
    float currTimeCrouch = 0f;
    public bool isGrounded;

    CapsuleCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        collider = GetComponent<CapsuleCollider>();
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        //get input
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        //rotates the capsule.p.Y according to X position of the mouse
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        //rotates camera.p.X according to Y position of the mouse
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
    }

    void UpdateMovement()
    {
      // get input
      Vector2 targetInputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
      targetInputDirection.Normalize();

      currentDirection = Vector2.SmoothDamp(currentDirection, targetInputDirection, ref currentDirectionVelocity, moveSmoothTime);
      //apply gravity and limit movement mid air
      if(controller.isGrounded) {
        isJumping = false;
        velocityY = 0.0f;
      }
      else if(!isJumping) {
        //falling
      }

      velocityY += gravity * Time.deltaTime;
      RaycastHit hit;
      isGrounded = Physics
        .SphereCast(transform.position, .25f, Vector3.down, out hit, controller.height / 2f + .6f,  groundLayer);
      //handle jump
      if (Input.GetKeyDown(jumpKey) && isGrounded)
      {
        isJumping = true;
        velocityY += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
      }

      if (Input.GetKey(runKey)) {
        currTimeRun += Time.deltaTime;
        speed = Mathf.Lerp(speed, runSpeed, currTimeRun / 4.0f);
      } else {
        currTimeRun += Time.deltaTime;
        speed = Mathf.Lerp(speed, walkSpeed, currTimeRun / 2.0f);
      }
      if (
          Input.GetKeyUp(runKey) || 
          Input.GetKeyDown(runKey)
        )
      {
        currTimeRun = 0f;
      }

      if (Input.GetKey(crouchKey)) {
        currTimeCrouch += Time.deltaTime;
        controller.height = Mathf.Lerp(controller.height, 0.5f, currTimeCrouch / 0.5f);
        collider.height = .5f;
      } else {
        currTimeCrouch += Time.deltaTime;
        controller.height = Mathf.Lerp(controller.height, 1.0f, currTimeCrouch / 0.5f);
        collider.height = 1f;
      }
      if (
          Input.GetKeyUp(crouchKey) ||
          Input.GetKeyDown(crouchKey) 
        ) 
      {
        currTimeCrouch = 0f;
      }

      //translate input to vector3
      Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * speed + Vector3.up * velocityY; 
      
      controller.Move(velocity * Time.deltaTime);
  }
}
