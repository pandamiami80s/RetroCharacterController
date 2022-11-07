using UnityEngine;

/// <summary>
/// 2022 11 07
/// Retro player movement controller. Player movement implemented using CharacterController (No rigidbody attached)
/// 
/// Setup:
///     * Set CharacterController Layer to player (Avoid isGround detection on player collider)
/// 
/// 
///     * Groundcheck size is also part of model somehow but not playe
///     
///     * Movement (Strafing)  
///     * Look rotation
///     * Jump
///     * Crouch
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CharacterController characterController;

    [Header("Movement")]
    [SerializeField] float movementSpeed = 2.0f;

    [Header("Jumping")]
    // Check if player on ground is essential part in this logic
    [SerializeField] Transform groundCheckSphere;
    // Must match player radius for best results
    [SerializeField] float groundCheckRadius = 0.25f;
    // Set to "default" layer
    [SerializeField] LayerMask groundCheckMask;
    bool isGroudned = false;
    Vector3 velocity;
    [SerializeField] float gravity = -9.8f;
    [Space]
    [SerializeField] bool isCanJump = true;
    [SerializeField] float jumpHeight = 1.0f;

    [Header("Crouch")]
    [SerializeField] Transform crouchCheckSphere;
    [SerializeField] Transform playerCamera;
    bool isNoSpace = false;
    [Space]
    [SerializeField] float crouchHeightOffset = 1.0f;
    float crouchHeightDefault;
    [SerializeField] float crouchCameraOffset = 0.25f;
    Vector3 crouchCenterDefault;
    [SerializeField] float crouchSpeed = 1.5f;
    float crouchSpeedDefault;
    Vector3 crouchCameraDefault;
    



    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        // Crouch store default values to revert back
        crouchSpeedDefault = movementSpeed;
        crouchHeightDefault = characterController.height;
        crouchCenterDefault = characterController.center;
        crouchCameraDefault = playerCamera.transform.localPosition;
    }

    public void MovePlayer(Vector3 movement)
    {
        isGroudned = Physics.CheckSphere(groundCheckSphere.position, groundCheckRadius, groundCheckMask);

        // Jump
        if (isCanJump && isGroudned && 0 < movement.y)
        {
            Debug.Log("Jump");

            // Formula  
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity (Free fall simulation)
        if (isGroudned && velocity.y < 0)
        {
            Debug.Log("Reset velocity");

            velocity.y = -1.0f;
        }
        else
        {
            Debug.Log("Free fall");
            // Can use like in DN3D fall.voc here

            velocity.y += gravity * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);

        // Crouch
        isNoSpace = Physics.CheckSphere(crouchCheckSphere.position, groundCheckRadius, groundCheckMask);
        if (isGroudned && movement.y < 0)
        {
            Debug.Log("Crouch");

            movementSpeed = crouchSpeed;
            // CharacterController
            characterController.height = crouchHeightDefault - crouchHeightOffset;
            characterController.center = new Vector3(
                characterController.center.x,
                crouchCenterDefault.y - crouchHeightOffset / 2,
                characterController.center.z);
            // Camera
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                crouchCameraDefault.y - crouchCameraOffset,
                playerCamera.transform.localPosition.z);
        }
        else if (isNoSpace == false)
        {
            Debug.Log("Standnig");

            movementSpeed = crouchSpeedDefault;
            // CharacterController
            characterController.height = crouchHeightDefault;
            characterController.center = crouchCenterDefault;
            // Camera
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                crouchCameraDefault.y,
                playerCamera.transform.localPosition.z);
        }

        // Move in looking direction 
        Vector3 directionRight = transform.right.normalized * movement.x;
        Vector3 directionForward = transform.forward.normalized * movement.z;
        Vector3 movementDirection = directionRight + directionForward;
        characterController.Move(movementDirection * movementSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheckSphere.transform.position, groundCheckRadius);
        Gizmos.DrawSphere(crouchCheckSphere.transform.position, groundCheckRadius);
    }
}

/*
    isGroudned = Physics.BoxCast(groundCheck.transform.position, boxSize, Vector3.down, out m_Hit, Quaternion.identity, groundCheckDist, groundCheckMask);
    Debug.Log(isGroudned);
    if (isGroudned)
    {
        Gizmos.DrawWireCube(groundCheck.transform.position + Vector3.down * m_Hit.distance, boxSize * 2);
    }
    else
    {
        Gizmos.DrawWireCube(groundCheck.transform.position + Vector3.down * groundCheckDist, boxSize * 2);
}
*/