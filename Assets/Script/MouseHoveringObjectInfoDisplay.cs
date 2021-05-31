
using System.Collections;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController
{
    public class MouseHoveringObjectInfoDisplay : MonoBehaviour
    {
        UIManager uiM;
        Camera cam;
        float zOffset;

        void Start()
        {
            uiM = UIManager.Instance;
            cam = Camera.main;
            zOffset = -cam.transform.position.z;
        }

        void Update()
        {
            //Debug.DrawLine(cam.transform.position, MouseWorldPosition.pos, Color.cyan);
            Vector2 dir = MouseWorldPosition.pos - cam.transform.position;

            //Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
            RaycastHit2D hit = Physics2D.Raycast(cam.transform.position, dir, 100f);

            if (hit.collider != null)
            {
                uiM.SetHovingInfoText(hit.transform.gameObject.name);
            }
            else
            {
                uiM.SetHovingInfoText("");
            }
        }

        void OrthoCameraRayCheck  ()
        {
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);

            //if (hit.collider != null)
            //{
            //    uiM.SetHovingInfoText(hit.transform.gameObject.name);
            //}
            //else
            //{
            //    uiM.SetHovingInfoText("");
            //}
        }
    }
}
