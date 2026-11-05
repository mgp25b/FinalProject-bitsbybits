using UnityEngine;
using UnityEngine.InputSystem;

public class MainMovement : MonoBehaviour
{
public float moveSpeed = 5f;
public float verticalSpeed = 5f;

    void Update()
    {
        // Always move to the right
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Move up and down using arrow keys
        float verticalInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.upArrowKey.isPressed)
            {
                verticalInput = 1f;
            }
            else if (Keyboard.current.downArrowKey.isPressed)
            {
                verticalInput = -1f;
            }
        }
        else
        {
            // Fallback for Input.GetKey if the new Input System is not available
            if (Input.GetKey(KeyCode.UpArrow))
            {
                verticalInput = 1f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                verticalInput = -1f;
            }
        }
        transform.Translate(Vector3.up * verticalInput * verticalSpeed * Time.deltaTime);
    }
}