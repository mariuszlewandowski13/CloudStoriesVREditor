using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnviromentMAnager : MonoBehaviour {

    private GameObject layoutObject;
    private GameObject sceneObjects;

    private void Start()
    {
        layoutObject = transform.Find("Layout").gameObject;
        sceneObjects = transform.Find("SceneObjects").gameObject;
    }

    public GameObject SpawnObject(GameObject objToSpawn, Vector3 position,Vector3 scale,  Quaternion rotation = new Quaternion())
    {
        GameObject newObject = Instantiate(objToSpawn, position, rotation);
        newObject.transform.localScale = scale * 2.0f;
        newObject.transform.parent = sceneObjects.transform;
        return newObject;
    }

    public void LoadLayout(GameObject newLayout, Vector3 pos, int layoutNumber, Quaternion rotation = new Quaternion())
    {
        if (layoutObject.transform.childCount > 0)
        {
            ClearLayout();
        }

        GameObject newLay = Instantiate(newLayout, layoutObject.transform.position, rotation);
        newLay.transform.parent = layoutObject.transform;
        newLay.transform.localPosition = pos;
    }

    private void ClearLayout()
    {
        if (layoutObject != null)
        {
            foreach (Transform child in layoutObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

    }

    public void SaveLayout()
    {
        GameObject.Find("LoadScene").GetComponent<DatabaseController>().SaveLayout(SaveLayoutsElements);
    }

    private void SaveLayoutsElements(int layoutID)
    {
        foreach (Transform child in sceneObjects.transform)
        {
            SceneObject sceneObj = child.GetComponent<SceneObjectInfo>().obj;
            GameObject.Find("LoadScene").GetComponent<DatabaseController>().SaveLayoutObject(sceneObj.type, Int32.Parse(sceneObj.type2), child.position, child.rotation.eulerAngles, child.transform.lossyScale, layoutID);
        }
    }

}
