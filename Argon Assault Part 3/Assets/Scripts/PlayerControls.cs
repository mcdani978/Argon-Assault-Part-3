using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float acceleration = 5f; // Acceleration factor
    [SerializeField] float deceleration = 5f; // Deceleration factor
    [SerializeField] float maxSpeed = 15f; // Maximum speed limit
    [SerializeField] float xRange = 5f;
    [SerializeField] float yRange = 3.5f;

    private Vector2 currentSpeed = Vector2.zero; // Stores current movement speed

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        movement.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = movement.ReadValue<Vector2>();

        // Smooth acceleration for the x and y axes
        currentSpeed.x = Mathf.MoveTowards(currentSpeed.x, inputVector.x * maxSpeed, acceleration * Time.deltaTime);
        currentSpeed.y = Mathf.MoveTowards(currentSpeed.y, inputVector.y * maxSpeed, acceleration * Time.deltaTime);

        // Deceleration if no input is detected
        if (inputVector.x == 0)
        {
            currentSpeed.x = Mathf.MoveTowards(currentSpeed.x, 0, deceleration * Time.deltaTime);
        }

        if (inputVector.y == 0)
        {
            currentSpeed.y = Mathf.MoveTowards(currentSpeed.y, 0, deceleration * Time.deltaTime);
        }

        float xOffset = currentSpeed.x * Time.deltaTime;
        float yOffset = currentSpeed.y * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;

        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float rawYPos = transform.localPosition.y + yOffset;

        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }
}
