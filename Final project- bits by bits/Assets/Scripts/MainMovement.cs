using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MainMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float verticalSpeed = 10f;

    [Header("Spin Settings")]
    [SerializeField] private float spinDuration = 0.4f; // Time in seconds to complete the 360 spin
    [SerializeField] private float pauseDuration = 1.0f; // Time in seconds to pause after the spin
    private bool isSpinning = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Prevent player movement input while spinning or paused
        if (isSpinning) return;

        float horizontalInput = 0f;
        float verticalInput = 0f;

        // Reading movement inputs (New Input System)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
                horizontalInput = 1f;
            else if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
                horizontalInput = -1f;

            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
                verticalInput = 1f;
            else if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
                verticalInput = -1f;
        }
        else
        {
            // Fallback for classic Input
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        // Apply 2D movement
        Vector3 movement = new Vector3(horizontalInput * moveSpeed, verticalInput * verticalSpeed, 0f);
        transform.Translate(movement * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") && !isSpinning)
        {
            StartCoroutine(SpinAndPause());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isSpinning)
        {
            StartCoroutine(SpinAndPause());
        }
    }

    private IEnumerator SpinAndPause()
    {
        isSpinning = true;

        float elapsedTime = 0f;

        // 1. Smooth 360-degree rotation loop
        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / spinDuration;

            // Smooth interpolation (Mathf.SmoothStep provides a sleek ease-in/ease-out spin)
            float smoothPercentage = Mathf.SmoothStep(0f, 1f, percentage);
            float currentAngle = Mathf.Lerp(0f, -360f, smoothPercentage);

            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            yield return null;
        }

        // Ensure rotation is perfectly back to zero
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // 2. Pause character for 1 second
        yield return new WaitForSeconds(pauseDuration);

        isSpinning = false;
    }
}