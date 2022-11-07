using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 2022 11 07
/// Player input from mouse and keyboard
/// </summary>

public class PlayerInput : MonoBehaviour
{
    [System.Serializable] public class RotationInputEvent : UnityEvent<Vector3>
    {
        //
    }
    public RotationInputEvent rotationInputEvent;
    [System.Serializable] public class MovementInputEvent : UnityEvent<Vector3>
    {
        //
    }
    public MovementInputEvent movementInputEvent;



    void Update()
    {
        rotationInputEvent.Invoke(MouseInput());
        movementInputEvent.Invoke(KeyboardInput());
    }

    Vector3 MouseInput()
    {
        float inputX = Input.GetAxis("Mouse X");
        float inputY = Input.GetAxis("Mouse Y");

        return new Vector3(inputX, inputY, 0);
    }

    Vector3 KeyboardInput()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = 0;
        if (Input.GetButton("Jump"))
        {
            inputY = 1;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            inputY = -1;
        }
        float inputZ = Input.GetAxis("Vertical");

        return new Vector3(inputX, inputY, inputZ);
    }
}
