using UnityEngine;

/// <summary>
/// 2022 11 07
/// Player rotation controller
/// </summary>

public class PlayerRotation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform playerBody;

    [Header("Settings")]
    [SerializeField] float rotationSpeedX = 100.0f;
    [SerializeField] float rotationSpeedY = 100.0f;
    [SerializeField] float rotationLookLimit = 45.0f;
    // Use to clamp Y camera rotation
    float rotationAngleY = 0;



    public void RotatePlayer(Vector3 rotation)
    {
        playerBody.Rotate(0, rotation.x * rotationSpeedX * Time.deltaTime, 0);

        // Rotate only camera Y (Inversed)
        rotationAngleY += rotation.y * rotationSpeedY * Time.deltaTime;
        rotationAngleY = Mathf.Clamp(rotationAngleY, -rotationLookLimit, rotationLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(-rotationAngleY, 0, 0);
    }
}
