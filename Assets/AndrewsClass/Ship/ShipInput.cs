using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInput : MonoBehaviour
{
    const float DeadZone = 100f;

    [Range(-1, 1)]
    public float pitch;
    [Range(-1, 1)]
    public float yaw;
    [Range(-1, 1)]
    public float roll;
    [Range(-1, 1)]
    public float strafe;
    [Range(0, 1)]
    public float throttle;
    [Range(-1, 1)]
    public float vertical;

    [SerializeField]
    private float throttleAcceleration = 0.5f;
    [SerializeField]
    private float rollAcceleration = 5f;

    float centerX, centerY;


    private void Awake()
    {
        centerX = Screen.width * .5f;
        centerY = Screen.height* .5f;
    }

    private void Update()
    {
        strafe = Input.GetAxis("Horizontal");

        SetDirectionsUsingMouse();
        UpdateMouseWheelThrottle();
        UpdateKeyboardThrottle(KeyCode.W, KeyCode.S);
        UpdateKeyboardRoll(KeyCode.Q, KeyCode.E);
    }


    void SetDirectionsUsingMouse ()
    {
        Vector3 mousePos = Input.mousePosition;
        float xDiff = (mousePos.x - centerX);
        float yDiff = (mousePos.y - centerY);
        float absXDiff = Mathf.Abs(xDiff);
        float absYDiff = Mathf.Abs(yDiff);


        if (absXDiff > DeadZone)
        {
            //yaw = (mousePos.x - centerX) / centerX ;
            //yaw = Mathf.Clamp(yaw, -1, 1);

            yaw = (absXDiff - DeadZone) * Mathf.Sign(xDiff) / centerX;
            yaw = Mathf.Clamp(yaw, -1, 1);
        }

        if (absYDiff > DeadZone)
        {
            pitch = (absYDiff - DeadZone) * Mathf.Sign(yDiff) / centerY;
            pitch = Mathf.Clamp(pitch, -1, 1);
            pitch = -pitch;
        }
    }

    void UpdateKeyboardThrottle(KeyCode increaseKey, KeyCode decreaseKey)
    {
        float target = throttle;
        if (Input.GetKey(increaseKey))
        {
            target = 1;
        }
        else if (Input.GetKey(decreaseKey))
        {
            target = 0;
        }

        throttle = Mathf.MoveTowards(throttle, target, Time.deltaTime * throttleAcceleration);
    }

    void UpdateKeyboardRoll(KeyCode increaseKey, KeyCode decreaseKey)
    {
        float target = 0;
        if (Input.GetKey(increaseKey))
        {
            target = 1;
        }
        else if (Input.GetKey(decreaseKey))
        {
            target = -1;
        }

        roll = Mathf.MoveTowards(roll, target, Time.deltaTime * rollAcceleration);
    }

    private void UpdateMouseWheelThrottle ()
    {
        throttle += Input.GetAxis("Mouse ScrollWheel");
        throttle = Mathf.Clamp01(throttle);
    }
}
