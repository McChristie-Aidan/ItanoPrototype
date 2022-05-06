using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlightControls : MonoBehaviour
{
    public float minForwardSpeed = 10f, maxForwardSpeed = 25f, strafeSpeed = 7.5f;
    public float MinForwardSpeed => minForwardSpeed;
    public float MaxForwardSpeed => maxForwardSpeed;

    private float activeForwardSpeed, activeStrafeSpeed;
    public float ActiveForwardSpeed => activeForwardSpeed;
    public float forwardAcceleration = 7f;
    public float forwardDeceleration;


    public float pitchSpeed = 7f, yawSpeed = 10f, rollSpeed = 10f;
    private float activeRollSpeed;
    public float rollAcceleration = 20f;
    public float horizontalLookAcceleration = 2f;
    public float verticalLookAccelerarion = 1f;

    public float verticalLookDeadZone = 10f;
    public float horizontalLookDeadZone = 10f;

    private Vector3 activeLookRotation;
    private Vector2 lookInput;

    private Vector2 movementInput;
    private float strafeInput;

    [SerializeField]
    private CanvasGroup pauseMenu;

    bool isGamepad;

    //death
    [HideInInspector]
    public bool isAlive = true;
    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    float deathCameraShakeAmount = 10f, deathCameraShakeLength = 2f;


    PlayerControls playerControls;
    PlayerInput playerInput;
    string currentControlScheme;

    Rigidbody rb;

    private void Awake()
    {
        //playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        activeForwardSpeed = minForwardSpeed;
        rb = GetComponent<Rigidbody>();
        pauseMenu.interactable = false;
        pauseMenu.alpha = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerInput.currentControlScheme != currentControlScheme)
        {
            OnDeviceChange(playerInput);
            currentControlScheme = playerInput.currentControlScheme;
        }
        if (isAlive)
        {
            GetLookRotation();
            HandleRotation();
            HandleMovement();
            HandleCameraEffects();
            /*
             * use this for total control over speed
             */
            //transform.position += this.transform.forward * activeForwardSpeed * Time.deltaTime;
            /*
             * use this for auto move forward
             */
            transform.position += this.transform.forward * activeForwardSpeed * Time.deltaTime;
            transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;
            //rb.velocity = this.transform.forward * activeForwardSpeed + this.transform.right * activeStrafeSpeed;
        }

    }
    private void LateUpdate()
    {
        //transform.Translate(this.transform.forward * activeForwardSpeed * Time.deltaTime);
        //transform.Translate(this.transform.right * activeStrafeSpeed * Time.deltaTime);
    }

    void GetLookRotation()
    {
        if (!isGamepad)
        {
            float xDist = Mouse.current.position.ReadValue().x;
            float yDist = Mouse.current.position.ReadValue().y;

            //float xDist = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue()).x;
            //float yDist = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue()).y;

            xDist = Utility.Map(xDist, 0, -yawSpeed, Screen.width, yawSpeed);
            yDist = Utility.Map(yDist, 0, -pitchSpeed, Screen.height, pitchSpeed);
            //xDist = Utility.Map(xDist, 0, -yawSpeed, 1, yawSpeed);
            //yDist = Utility.Map(yDist, 0, -pitchSpeed, 1, pitchSpeed);


            if (xDist < -yawSpeed / 2)
            {
                xDist = -yawSpeed / 2;
            }
            if (xDist > yawSpeed / 2)
            {
                xDist = yawSpeed / 2;
            }

            if (yDist < -pitchSpeed / 2)
            {
                yDist = -pitchSpeed / 2;
            }
            if (yDist > pitchSpeed / 2)
            {
                yDist = pitchSpeed / 2;
            }

            //Debug.Log(new Vector2(xDist, yDist).ToString());

            //activeLookRotation.x += -(yDist * 2); //time.deltatime
            //activeLookRotation.y += (xDist * 2); //time.deltatime
            activeLookRotation.x = Mathf.Lerp(activeLookRotation.x, -(yDist * 2), verticalLookAccelerarion * Time.deltaTime);
            activeLookRotation.y = Mathf.Lerp(activeLookRotation.y, xDist * 2, horizontalLookAcceleration * Time.deltaTime);

            //Debug.Log(activeLookRotation.ToString());
            //Debug.Log(lookRotation);
        }
        else
        {
            lookInput.x = Mathf.Clamp(lookInput.x, -1, 1);
            lookInput.y = Mathf.Clamp(lookInput.y, -1, 1);

            lookInput.x = Utility.Map(lookInput.x, -1, -yawSpeed, 1, yawSpeed);
            lookInput.y = Utility.Map(lookInput.y, -1, -yawSpeed, 1, yawSpeed);
            Debug.Log(lookInput.ToString());

            activeLookRotation.x = Mathf.Lerp(activeLookRotation.x, -lookInput.y * 2, verticalLookAccelerarion * Time.deltaTime);
            activeLookRotation.y = Mathf.Lerp(activeLookRotation.y, lookInput.x * 2, verticalLookAccelerarion * Time.deltaTime);
        }

        
    }
    void HandleMovement()
    {
        bool isDecelerating = false;
        if (movementInput.y < 0)
        {
            isDecelerating = true;
        }

        if (!isDecelerating)
        {
            activeForwardSpeed += (forwardAcceleration * movementInput.y) * Time.deltaTime;
        }
        else
        {
            activeForwardSpeed += (forwardDeceleration * movementInput.y) * Time.deltaTime;
        }

        activeForwardSpeed = Mathf.Clamp(activeForwardSpeed, minForwardSpeed, maxForwardSpeed);
        /*
         * for strafing movement
         */
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, strafeInput * strafeSpeed, strafeSpeed * Time.deltaTime);
    }
    void HandleRotation()
    {
        if(Time.timeScale != 0)
        {
            activeRollSpeed = Mathf.Lerp(activeRollSpeed, movementInput.x * rollSpeed, rollAcceleration * Time.deltaTime);

            activeLookRotation = new Vector3(
                activeLookRotation.x * pitchSpeed,
                activeLookRotation.y * yawSpeed,
                activeLookRotation.z * rollSpeed) * Time.deltaTime;
            activeLookRotation.z = -activeRollSpeed;

            this.transform.Rotate(activeLookRotation, Space.Self);
        }

        

    }
    void HandleCameraEffects()
    {
        //if (activeForwardSpeed > maxForwardSpeed * .8 )
        //{
        //    CameraEffects.Instance.ShakeCam(activeForwardSpeed * .02f);
        //}
        float targetFOV = activeForwardSpeed + 50;
        //float targetFOV = Mathf.Lerp(minForwardSpeed, maxForwardSpeed, Mathf.InverseLerp(60, 90, activeForwardSpeed));
        CameraEffects.Instance.SetCamFOV(targetFOV);
    }
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        //Debug.Log("Moving");
        movementInput = callbackContext.ReadValue<Vector2>();
        activeLookRotation.z = -callbackContext.ReadValue<Vector2>().x;
    }

    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        if (isGamepad)
        {
            lookInput = callbackContext.ReadValue<Vector2>();
            //lookRotation.x = -(callbackContext.ReadValue<Vector2>().y);
            //lookRotation.y = callbackContext.ReadValue<Vector2>().x;
        }
        //Debug.Log("looking");
        /*
         * scale look rotation by distance from center
         */
        //float xDist = Mouse.current.position.ReadValue().x - Screen.width/2;
        //float yDist = Mouse.current.position.ReadValue().y - Screen.height/2;

        //lookRotation.x = xDist * Time.deltaTime;
        //lookRotation.y = yDist * Time.deltaTime;
    }

    public void OnStrafe(InputAction.CallbackContext callbackContext)
    {
        strafeInput = callbackContext.ReadValue<float>();
    }
    public void OnReset()
    {
        if (SceneManagement.Instance != null)
        {
            SceneManagement.Instance.LoadCurrentLevel();
        }
    }
    public void OnPause()
    {
        AudioListener.pause = !AudioListener.pause;
        if (Time.timeScale == 1)
        {
            pauseMenu.interactable = true;
            pauseMenu.alpha = 1;
            Time.timeScale = 0;
        }
        else if(Time.timeScale == 0)
        {
            pauseMenu.interactable = false;
            pauseMenu.alpha = 0;
            Time.timeScale = 1;
        }
    }
    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;    }
    public void OnCollisionEnter(Collision collision)
    {
        isAlive = false;
        //shake cam
        CameraEffects.Instance.ShakeCam(deathCameraShakeAmount, deathCameraShakeLength);       

        //explode
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        }

        //destroy all remaining missiles
        if (MissileManager.Instance != null)
        {
            MissileManager.Instance.DestroyAllMissiles();
        }
        
    }
}
