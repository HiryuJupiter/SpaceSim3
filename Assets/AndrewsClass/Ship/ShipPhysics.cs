using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipPhysics : MonoBehaviour
{
    Rigidbody rb;

    float moveSpeedMod = 100f;
    float rotSpeedMod = 0.2f;
    public Vector3 maxVel = new Vector3(100, 100, 100);  //Lateral, vertical, longitudinal
    public Vector3 maxRot = new Vector3(100, 100, 100); //Pitch, yaw, roll

    Vector3 curVel, curRot;

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(curVel * moveSpeedMod, ForceMode.Force);
        rb.AddRelativeTorque(curRot * rotSpeedMod, ForceMode.Force);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "curVel " + curVel);
        GUI.Label(new Rect(20, 40, 200, 20), "curRot " + curRot);
    }

    public void SetPhysicsInput(Vector3 linearInput, Vector3 angularInput)
    {
        curVel = MultiplyByComponnt(linearInput, maxVel);
        curRot = MultiplyByComponnt(angularInput, maxRot);

    }

    public Vector3 MultiplyByComponnt(Vector3 a, Vector3 b)
    {
        Vector3 ret;
        ret.x = a.x * b.x;
        ret.y = a.y * b.y;
        ret.z = a.z * b.z;

        return ret;
    }
}
