using System.Collections;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController
{
    [DefaultExecutionOrder(-100)]
    public class MouseWorldPosition : MonoBehaviour
    {
        public static Vector3 pos;
        Camera cam;
        float zOffset;

        void Start()
        {
            cam = Camera.main;
            zOffset = -cam.transform.position.z;
        }

        void Update()
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = zOffset;
            pos = cam.ScreenToWorldPoint(mouse);
        }
    }
}