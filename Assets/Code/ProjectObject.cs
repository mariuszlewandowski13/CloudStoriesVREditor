﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectObject  {

    public int id;
    public string authID;
    public string owner;
    public string name;

    public ProjectObject(int id, string auth, string own, string nam)
    {
        this.id = id;
        authID = auth;
        owner = own;
        name = nam;
    }
}
