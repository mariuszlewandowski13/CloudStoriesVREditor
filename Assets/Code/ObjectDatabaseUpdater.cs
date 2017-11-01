using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(SceneObjectInfo))]
public class ObjectDatabaseUpdater : MonoBehaviour {

    public DatabaseController database;

    public SceneObjectInfo sceneObjInfo;

    public bool creatingObject;

    public string objectType3;

    public string objectType2;

    public string objectType;

    public Texture2D tex;

    private byte[] textureBytes;

    private string extension;

    void Start () {
        sceneObjInfo = GetComponent<SceneObjectInfo>();
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

    public void CreateSceneObject()
    {
        if (GameObject.Find("LoadScene") != null)
        {
            creatingObject = true;
            database = GameObject.Find("LoadScene").GetComponent<DatabaseController>();
            database.SaveObject(objectType, objectType2, transform.position, transform.rotation.eulerAngles, transform.lossyScale, SetSceneObject, textureBytes , extension);
        }
    }

    public void SetTypesAndCreate(string type, string type2, string type3, bool loadTexture, string ext)
    {
        objectType = type;
        objectType2 = type2;
        objectType3 = type3;
        if (loadTexture)
        {
            LoadTexture();
            extension = ext;
        }
        CreateSceneObject();

        
    }


    public void LoadTexture()
    {
        if (objectType2 != "")
        {
            textureBytes = File.ReadAllBytes(objectType3 + objectType2);
            tex = new Texture2D(2, 2);
            tex.LoadImage(textureBytes);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }

}
