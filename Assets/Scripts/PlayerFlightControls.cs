using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlightControls : MonoBehaviour
{
    public float minForwardSpeed = 10f, maxForwardSpeed = 25f, strafeSpeed = 7.5f;
    private float activeForwardSpeed, activeStrafeSpeed;
    private float forwardAcceleration = 5f;


    public float pitchSpeed = 7f, yawSpeed = 10f, rollSpeed = 10f;
    private float activeRollSpeed;

    private Vector3 lookRotation;

    private Vector2 movementInput;
    private float strafeInput;

    private void Start()
    {
        activeForwardSpeed = minForwardSpeed;
    }
    // Update is called once per frame
    void Update()
    {


        GetLookRotation();
        HandleRotation();
        HandleMovement();
        /*
         * use this for total control over speed
         */
        //transform.position += this.transform.forward * activeForwardSpeed * Time.deltaTime;
        /*
         * use this for auto move forward
         */
        transform.position += this.transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;
    }
    private void LateUpdate()
    {
        //transform.Translate(this.transform.forward * activeForwardSpeed * Time.deltaTime);
        //transform.Translate(this.transform.right * activeStrafeSpeed * Time.deltaTime);
    }

    void GetLookRotation()
    {
        float xDist = Mouse.current.position.ReadValue().x - Screen.width / 2;
        float yDist = Mouse.current.position.ReadValue().y - Screen.height / 2;

        lookRotation.x = -(yDist * 2) * Time.deltaTime;
        lookRotation.y = xDist * Time.deltaTime;
    }
    void HandleMovement()
    {
        activeForwardSpeed += (forwardAcceleration * movementInput.y) * Time.deltaTime;
        
        /*
         * for strafing movement
         */
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, strafeInput * strafeSpeed, strafeSpeed * Time.deltaTime);
    }
    void HandleRotation()
    {
        activeRollSpeed = Mathf.Lerp(activeRollSpeed, movementInput.x * rollSpeed, rollSpeed * Time.deltaTime);
        lookRotation = new Vector3(
            lookRotation.x * pitchSpeed,
            lookRotation.y * yawSpeed,
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
        //float xDist = Mouse.current.position.ReadValue().x - Screen.width/2;
        //float yDist = Mouse.current.position.ReadValue().y - Screen.height/2;

        //lookRotation.x = xDist * Time.deltaTime;
        //lookRotation.y = yDist * Time.deltaTime;
        //lookRotation.x = -(callbackContext.ReadValue<Vector2>().y);
        //lookRotation.y = callbackContext.ReadValue<Vector2>().x;
    }

    public void OnStrafe(InputAction.CallbackContext callbackContext)
    {
        strafeInput = callbackContext.ReadValue<float>();
    }
}
