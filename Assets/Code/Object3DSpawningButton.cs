using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointableGUIButton))]
public class Object3DSpawningButton : MonoBehaviour, IClickable {

    public GameObject objectToSpawn;

    public bool click;

    public void Clicked(Vector3 pos, GameObject clickingObject)
    {
        clickingObject.GetComponent<RaycastObjectSpawner>().StartSpawning(objectToSpawn, pos);

    }


    private void Update()
    {
        if (click)
        {
            click = false;
            Clicked(new Vector3(), gameObject);
        }
    }
}
