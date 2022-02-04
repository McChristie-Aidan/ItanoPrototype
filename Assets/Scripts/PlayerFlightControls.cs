using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlightControls : MonoBehaviour
{
    public float forwardSpeed = 25f, strafeSpeed = 7.5f, hoverSpeed = 5f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;


    public float verticleLookSpeed = 7f, horizontalLookSpeed = 10f, rollSpeed = 10f;
    private float activeRollSpeed;

    private Vector3 lookRotation;

    private Vector2 movementInput;

    private float strafeInput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleMovement();
        /*
         * use this for total control over speed
         */
        //transform.position += this.transform.forward * activeForwardSpeed * Time.deltaTime;
        /*
         * use this for auto move forward
         */
        transform.position += this.transform.forward * forwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);
    }
    private void FixedUpdate()
    {

    }

    void HandleMovement()
    {
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, movementInput.y * forwardSpeed, forwardAcceleration * Time.deltaTime);
        /*
         * for strafing movement
         */
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, strafeInput * strafeSpeed, strafeSpeed * Time.deltaTime);
    }
    void HandleRotation()
    {
        activeRollSpeed = Mathf.Lerp(activeRollSpeed, movementInput.x * rollSpeed, rollSpeed * Time.deltaTime);
        lookRotation = new Vector3(
            lookRotation.x * verticleLookSpeed,
            lookRotation.y * horizontalLookSpeed,
            lookRotation.z * rollSpeed) * Time.deltaTime;
        lookRotation.z = -activeRollSpeed;

        this.transform.Rotate(lookRotation, Space.Self);

    }
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        //Debug.Log("Moving");
        movementInput = callbackContext.ReadValue<Vector2>();
        lookRotation.z = -callbackContext.ReadValue<Vector2>().x;
    }

    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        //Debug.Log("looking");
        /*
         * scale look rotation by distance from center
         */
        float xDist = Mouse.current.position.ReadValue().x - Screen.width/2;
        float yDist = Mouse.current.position.ReadValue().y - Screen.height/2;

        Vector2.Distance(Mouse.current.position.ReadValue(), new Vector2(Screen.width/2 , Screen.height/2));
        lookRotation.x = -(callbackContext.ReadValue<Vector2>().y) * xDist;
        lookRotation.y = callbackContext.ReadValue<Vector2>().x * yDist;
    }

    public void OnStrafe(InputAction.CallbackContext callbackContext)
    {
        strafeInput = callbackContext.ReadValue<float>();
    }

    public void OnYaw(InputAction.CallbackContext callbackContext)
    {

    }
    public void OnPitch(InputAction.CallbackContext callbackContext)
    {

    }
    public void OnRoll(InputAction.CallbackContext callbackContext)
    {

    }
}
