using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float acceleration = 5f;
    [SerializeField] float deceleration = 5f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float xRange = 5f;
    [SerializeField] float yRange = 3.5f;
    [SerializeField] float controlPitchFactor = -10f; 
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlRollFactor = 30f; 

    private Vector2 currentSpeed = Vector2.zero;
    private Vector2 inputVector = Vector2.zero;

    void OnEnable()
    {
        movement.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
    }

    void Update()
    {
        inputVector = movement.ReadValue<Vector2>();
        ProcessTranslation();
        ProcessRotation();
    }

    void ProcessRotation()
    {

        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = inputVector.y * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;


        float roll = -inputVector.x * controlRollFactor;


        transform.localRotation = Quaternion.Euler(pitch, 0, roll);
    }

    void ProcessTranslation()
    {
        currentSpeed.x = Mathf.MoveTowards(currentSpeed.x, inputVector.x * maxSpeed, acceleration * Time.deltaTime);
        currentSpeed.y = Mathf.MoveTowards(currentSpeed.y, inputVector.y * maxSpeed, acceleration * Time.deltaTime);

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
