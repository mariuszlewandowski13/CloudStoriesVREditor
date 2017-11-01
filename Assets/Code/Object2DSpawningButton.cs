using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Object2DSpawningButton : Object3DSpawningButton {

    public Texture2D tex;

    public string path;

    private float ratio;

    public override void Clicked(Vector3 pos, GameObject clickingObject)
    {
        clickingObject.GetComponent<RaycastObjectSpawner>().StartSpawning(objectToSpawn, pos,transform.lossyScale, transform.rotation.eulerAngles,  type, type2, path, true, "PNG");

    }

    public void SetObjectTypes(string path, string name, float ratio)
    {
        this.type2 = name;
        this.path = path;
        this.ratio = ratio;
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
            ChangeSizeByRatio();
        }
    }

    private void ChangeSizeByRatio()
    {
        if (ratio != 0.0f)
        {
            

            Vector3 scale = transform.localScale;
            if (ratio < 1.0f)
            {
                scale.z *= ratio;
            }
            else {
                scale.x *= (1/ratio);
            }
            
            transform.localScale = scale;
        }
    }
}
