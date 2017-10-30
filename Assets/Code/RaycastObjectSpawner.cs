using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObjectSpawner : RaycastBase {

    private GameObject enviroment;

   

    private ControllerScript controller;

    private GameObject spawnedObject;

    private bool spawning;

   

    

    private RaycastHit hit;

    void Start()
    {
        controller = GetComponent<ControllerScript>();
        //trackedObj = transform.parent.GetComponent<SteamVR_TrackedObject>();
        
        enviroment = GameObject.Find("SCENE");
    }

    public void StartSpawning(GameObject objectToSpawn, Vector3 pos)
    {

        spawning = true;
        spawnedObject = Instantiate(objectToSpawn, pos, new Quaternion());
        spawnedObject = enviroment.GetComponent<EnviromentMAnager>().SpawnObject(objectToSpawn, pos, new Quaternion());

        GetComponent<ControllerRaycastScript>().isActive = false;
    }

    private void Update()
    {
        if (spawning)
        {

            spawnedObject.transform.position = new Vector3(1000f, 1000f, 1000f);

            Ray ray = new Ray(transform.position, transform.forward);

            Physics.Raycast(ray, out hit, 3);
            if (hit.transform != null)
            {
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = transform.forward * 3 + transform.position;
            }
            CursorOn();
            spawnedObject.transform.position = hitPoint;

            if (controller.triggerUp)
            {
                StopSpawning();
            }
        }
    }


    public void StopSpawning()
    {
        spawning = false;
        GetComponent<ControllerRaycastScript>().isActive = true;
        CursorOff();
        spawnedObject.AddComponent<ObjectInteractionScript>();
        spawnedObject.AddComponent<ImageMoveScript>();
        spawnedObject.AddComponent<SelectingObjectsScript>();
    }

   

}
