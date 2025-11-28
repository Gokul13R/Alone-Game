using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class LookInput : MonoBehaviour
{
    [Header("References")]
    public Transform CamTransform; // Object to rotate up/down (usually the camera or its pivot)
    public Transform PlayerTransform;   // Object to rotate left/right (usually the player body)

    [Header("Settings")]
    public float lookSpeed = 0.2f;   // Sensitivity of the look control
    public float minPitch = -90f;    // Minimum vertical look angle
    public float maxPitch = 90f;     // Maximum vertical look angle
  
    private float currentPitch = 0f; // Tracks current vertical angle
    private Dictionary<int, Vector2> lastTouchPositions = new Dictionary<int, Vector2>(); // Stores last touch positions by finger ID

    [SerializeField] private float PCAimAssistSensity; // for mobile 100 
    [SerializeField] private float MobileAimAssistSensity; // for mobile 100 

    void OnEnable()
    {
        // Enable Enhanced Touch Input System
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        // Disable Enhanced Touch when not needed
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
      
       TouchScreen();
        
    }

    private void TouchScreen()
    {
        // Calculate the halfway point of the screen (x-axis)
        float screenMidX = Screen.width * 0.5f;

        // Loop through all current active touches
        foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            int id = touch.finger.index; // Unique ID per finger
            Vector2 currentPos = touch.screenPosition;

            // Only process touches on the right side of the screen
            if (currentPos.x >= screenMidX)
            {
                // If touch is moving
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    // Get the previous position to calculate movement delta
                    if (lastTouchPositions.TryGetValue(id, out Vector2 lastPos))
                    {
                        // Calculate how far the touch has moved since last frame
                        Vector2 delta = (currentPos - lastPos) * lookSpeed * Time.deltaTime;

                        // Rotate player horizontally (yaw) using delta.x
                        PlayerTransform.Rotate(Vector3.up, delta.x, Space.World);

                        // Rotate camera vertically (pitch) using delta.y
                        currentPitch = Mathf.Clamp(currentPitch - delta.y, minPitch, maxPitch);

                        CamTransform.localEulerAngles = Vector3.right * currentPitch;
                    }
                    // Update the last position for this finger
                    lastTouchPositions[id] = currentPos;
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    // Store the initial touch position when touch starts
                    lastTouchPositions[id] = currentPos;
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                         touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    // Remove touch tracking when the finger lifts off or is canceled
                    if (lastTouchPositions.ContainsKey(id))
                        lastTouchPositions.Remove(id);
                }
            }
        }

    }









}
