using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBase : MonoBehaviour {

    private LineRenderer lineRenderer;

    private bool active;

    protected Vector3 hitPoint;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
