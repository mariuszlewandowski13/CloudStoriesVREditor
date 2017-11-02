using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMoveScript : RaycastBase {


    private ControllerScript controller;

    private GameObject movingObject;
    private Transform movingObejctPreviousParent;

    private bool moving;


    private RaycastHit hit;

    private float raycasterLength = 1.0f;

    private GameObject rotationCenter;

    void Start()
    {
        controller = GetComponent<ControllerScript>();
    }

    public void StartMoving(GameObject objectToMove, Vector3 pos)
    {

        moving = true;
        movingObject = objectToMove;

        raycasterLength = Vector3.Distance(pos, transform.position);


        rotationCenter = new GameObject();
        rotationCenter.transform.position = objectToMove.transform.position;
        rotationCenter.transform.LookAt(gameObject.transform);

        movingObejctPreviousParent = movingObject.transform.parent;

        movingObject.transform.parent = rotationCenter.transform;
        GetComponent<ControllerRaycastScript>().isActive = false;
    }

    private void Update()
    {
        if (moving)
        {

            rotationCenter.transform.position = new Vector3(1000f, 1000f, 1000f);

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
            rotationCenter.transform.position = hitPoint;
            rotationCenter.transform.LookAt(gameObject.transform);

            if (controller.triggerUp)
            {
                StopMoving();
            }
        }
    }


    public void StopMoving()
    {
        moving = false;
        movingObject.transform.parent = movingObejctPreviousParent;

        movingObject = null;
        movingObejctPreviousParent = null;

        Destroy(rotationCenter);

        GetComponent<ControllerRaycastScript>().isActive = true;
        CursorOff();
    }

}
