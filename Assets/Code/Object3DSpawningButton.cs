using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointableGUIButton))]
public class Object3DSpawningButton : MonoBehaviour, IClickable {

    public GameObject objectToSpawn;

    public string type;
    public string type2;

    public bool click;

    public virtual void Clicked(Vector3 pos, GameObject clickingObject)
    {
        clickingObject.GetComponent<RaycastObjectSpawner>().StartSpawning(objectToSpawn, pos, type, type2);

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
