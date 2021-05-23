using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShipPhysics))]
[RequireComponent(typeof(ShipInput))]
public class Ship : MonoBehaviour
{
    private ShipInput input;
    private ShipPhysics physics;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<ShipInput>();
        physics = GetComponent<ShipPhysics>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        physics.SetPhysicsInput(new Vector3(input.strafe, 0, input.throttle), new Vector3(input.pitch, input.yaw, input.roll));


    }
}
