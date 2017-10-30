using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRaycastScript : RaycastBase {

    private bool isPointing;



    private RaycastHit hit;

    private Transform actualPointing;

    private ControllerScript controller;

    public bool isActive = true;

    void Start () {
        controller = GetComponent<ControllerScript>();
    }

	void Update () {
        if (isActive)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            Physics.Raycast(ray, out hit, 5);
            if (hit.transform != null && hit.transform.GetComponent<IClickable>() != null && (hit.transform.tag == "Btn" || hit.transform.tag == "SceneObject"))
            {
                isPointing = true;
                hitPoint = hit.point;
                CursorOn();

                if ( hit.transform != actualPointing)
                {
                    if (actualPointing != null && actualPointing.GetComponent<IRaycastPointable>() != null)
                    {
                        actualPointing.GetComponent<IRaycastPointable>().Highlight(false);
                    }

                    actualPointing = hit.transform;
                    if (actualPointing.GetComponent<IRaycastPointable>() != null)
                    {
                        actualPointing.GetComponent<IRaycastPointable>().Highlight(true);
                    }
                }
                bool pressedDown = controller.triggerDown;

                if (pressedDown)
                {
                    actualPointing.GetComponent<IClickable>().Clicked(hitPoint, gameObject);
                }
            }
            else if (isPointing)
            {
                CursorOff();
                isPointing = false;

                if (actualPointing != null)
                {
                    if (actualPointing.GetComponent<IRaycastPointable>() != null)
                    {
                        actualPointing.GetComponent<IRaycastPointable>().Highlight(false);
                    }

                    actualPointing = null;
                }
            }
        }

    }

}
