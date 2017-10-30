using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRaycastScript : MonoBehaviour {

    //public Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    //public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    //private SteamVR_TrackedObject trackedObj;

    private bool active;
    private bool isPointing;

    private Vector3 hitPoint;

    private RaycastHit hit;

    private Transform actualPointing;

    private LineRenderer lineRenderer;

    private ControllerScript controller;

    public bool isActive = true;

    void Start () {
        controller = GetComponent<ControllerScript>();
        //trackedObj = transform.parent.GetComponent<SteamVR_TrackedObject>();
        lineRenderer = GetComponent<LineRenderer>();
    }

	void Update () {
        if (isActive)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            Physics.Raycast(ray, out hit, 5);
            if (hit.transform != null && hit.transform.GetComponent<IClickable>() != null && hit.transform.tag == "Btn")
            {
                isPointing = true;
                hitPoint = hit.point;
                CursorOn();

                if (hit.transform != actualPointing)
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

    protected void CursorOn()
    {
        active = true;
        Vector3[] points = new Vector3[] { transform.position, hitPoint };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(points);
    }

    protected void CursorOff()
    {
        if (active)
        {
            active = false;
            lineRenderer.positionCount = 0;
        }

    }
}
