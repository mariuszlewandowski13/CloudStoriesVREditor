using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMoveScript : RaycastBase {


    private ControllerScript controller;

    private GameObject movingObject;

    private bool moving;


    private RaycastHit hit;

    private float raycasterLength = 1.0f;

    void Start()
    {
        controller = GetComponent<ControllerScript>();
    }

    public void StartMoving(GameObject objectToMove, Vector3 pos)
    {

        moving = true;
        movingObject = objectToMove;

        raycasterLength = Vector3.Distance(pos, transform.position);

        GetComponent<ControllerRaycastScript>().isActive = false;
    }

    private void Update()
    {
        if (moving)
        {

            movingObject.transform.position = new Vector3(1000f, 1000f, 1000f);

            Ray ray = new Ray(transform.position, transform.forward);

            Physics.Raycast(ray, out hit, raycasterLength);
            if (hit.transform != null)
            {
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = transform.forward * raycasterLength + transform.position;
            }
            CursorOn();
            movingObject.transform.position = hitPoint;

            if (controller.triggerUp)
            {
                StopMoving();
            }
        }
    }


    public void StopMoving()
    {
        moving = false;
        GetComponent<ControllerRaycastScript>().isActive = true;
        CursorOff();
    }

}
