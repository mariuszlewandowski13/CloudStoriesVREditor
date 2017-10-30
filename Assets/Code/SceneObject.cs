using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SceneObject   {
    public int ID;
    public string type;
    public string type2;
    public Vector3 lastSavedPosition;
    public Vector3 lastSavedRotation;

    public SceneObject(int id, string typ, string typ2, Vector3 pos, Vector3 rot)
    {
        ID = id;
        type = typ;
        type2 = typ2;
        lastSavedPosition = pos;
        lastSavedRotation = rot;
    }
	
	
}
