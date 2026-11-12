using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MainMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float verticalSpeed = 10f;

    [Header("Spin Settings")]
    [SerializeField] private float spinDuration = 0.4f;
    [SerializeField] private float pauseDuration = 1.0f;

    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 1.8f; // Multiplies normal speed by 1.8
    [SerializeField] private float boostDuration = 3.0f;   // Boost lasts 3 seconds

    private bool isSpinning = false;
    private float currentSpeedMultiplier = 1f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isSpinning) return;

        float horizontalInput = 0f;
        float verticalInput = 0f;

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
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        // Apply movement multiplied by the current boost multiplier
        Vector3 movement = new Vector3(
            horizontalInput * moveSpeed * currentSpeedMultiplier,
            verticalInput * verticalSpeed * currentSpeedMultiplier,
            0f
        );
        transform.Translate(movement * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") && !isSpinning)
        {
            StartCoroutine(SpinAndPause());
        }
        else if (other.CompareTag("Star"))
        {
            StartCoroutine(SpeedBoost());
            Destroy(other.gameObject); // Removes the star from the scene
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isSpinning)
        {
            StartCoroutine(SpinAndPause());
        }
    }

    private IEnumerator SpeedBoost()
    {
        currentSpeedMultiplier = speedMultiplier;
        yield return new WaitForSeconds(boostDuration);
        currentSpeedMultiplier = 1f; // Returns to normal speed
    }

    private IEnumerator SpinAndPause()
    {
        isSpinning = true;

        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / spinDuration;

            float smoothPercentage = Mathf.SmoothStep(0f, 1f, percentage);
            float currentAngle = Mathf.Lerp(0f, -360f, smoothPercentage);

            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        yield return new WaitForSeconds(pauseDuration);

        isSpinning = false;
    }
}