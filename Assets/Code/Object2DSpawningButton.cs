using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Object2DSpawningButton : Object3DSpawningButton {

    public Texture2D tex;

    public string path;

    public override void Clicked(Vector3 pos, GameObject clickingObject)
    {
        clickingObject.GetComponent<RaycastObjectSpawner>().StartSpawning(objectToSpawn, pos, type, type2, path, true, "PNG");

    }

    public void SetObjectTypes(string path, string name)
    {
        this.type2 = name;
        this.path = path;
        LoadTexture();
    }

    public void LoadTexture()
    {
        if (type2 != "")
        {
            byte [] bytes = bytes = File.ReadAllBytes(path + type2);
            tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }
}
