using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneObjectInfo))]
public class ObjectDatabaseUpdater : MonoBehaviour {

    public DatabaseController database;

    public SceneObjectInfo sceneObjInfo;

    public bool creatingObject;

    public int objectNumber;

	void Start () {
        sceneObjInfo = GetComponent<SceneObjectInfo>();
        if (GameObject.Find("LoadScene") != null)
        {
            creatingObject = true;
            database = GameObject.Find("LoadScene").GetComponent<DatabaseController>();
            database.SaveObject("3DObject", objectNumber, transform.position, transform.rotation.eulerAngles, SetSceneObject);
        }
        

    }
	
	// Update is called once per frame
	void Update () {
        if (!creatingObject && sceneObjInfo.obj != null && database != null)
        {
            if (transform.position.x != sceneObjInfo.obj.lastSavedPosition.x || transform.position.y != sceneObjInfo.obj.lastSavedPosition.y || transform.position.z != sceneObjInfo.obj.lastSavedPosition.z || transform.rotation.eulerAngles.x != sceneObjInfo.obj.lastSavedRotation.x || transform.rotation.eulerAngles.y != sceneObjInfo.obj.lastSavedRotation.y || transform.rotation.eulerAngles.z != sceneObjInfo.obj.lastSavedRotation.z)
            {
                UpdateObjectPosRot();
            }
        }
        else if (!creatingObject && sceneObjInfo.obj == null)
        {
            if (GameObject.Find("LoadScene") != null)
            {
                creatingObject = true;
                database = GameObject.Find("LoadScene").GetComponent<DatabaseController>();
                database.SaveObject("3DObject", objectNumber, transform.position, transform.rotation.eulerAngles, SetSceneObject);
            }
        }
	}

    public void SetSceneObject(SceneObject newSceneObj)
    {
        sceneObjInfo.obj = newSceneObj;
        creatingObject = false;
    }

    public void UpdateObjectPosRot()
    {
        if (database != null)
        {
            sceneObjInfo.obj.lastSavedPosition = transform.position;
            sceneObjInfo.obj.lastSavedRotation = transform.rotation.eulerAngles;

            database.UpdateObject(sceneObjInfo.obj.lastSavedPosition, sceneObjInfo.obj.lastSavedRotation, sceneObjInfo.obj.ID);
        }
    }


}
