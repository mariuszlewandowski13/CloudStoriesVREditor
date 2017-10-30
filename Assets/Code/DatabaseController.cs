using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseController : MonoBehaviour {

    public delegate void ResultMethod(ProjectObject project);
    public delegate void ResultMethod2(SceneObject obj);
    public delegate void ResultMethod3(int number);
    public delegate void ResultMethod4();


    private string message;

    public void LoadUserLayouts(ResultMethod4 method)
    {
        WWWForm form = new WWWForm();
        form.AddField("owner", ApplicationStaticData.actualProject.owner);
        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "LoadUserLayouts.php", form);
        StartCoroutine(request(w, method));
    }

    IEnumerator request(WWW w, ResultMethod4 method)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else
        {
            message = "ERROR: " + w.error + "\n";
        }
        Debug.Log(message);

        string[] msg = null;
        msg = message.Split(new string[] { "#####" }, StringSplitOptions.None);
        if (msg.Length > 0)
        {
            int ID;
            for (int i = 0; i < msg.Length; i ++)
            {
                if (Int32.TryParse(msg[i], out ID))
                {
                    ApplicationStaticData.layoutsNumber.Add(ID);
                }
                
            }

            method();
        }
    }

    public void CreateProject(ResultMethod method)
    {
        WWWForm form = new WWWForm();
        form.AddField("Owner", ApplicationStaticData.owner);
 
        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "CreateProject.php", form);
        StartCoroutine(request(w, method));
    }

    public void SaveLayout(ResultMethod3 method)
    {
        WWWForm form = new WWWForm();
        form.AddField("Owner", ApplicationStaticData.owner);

        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "CreateLayout.php", form);
        StartCoroutine(request(w, method));
    }

    public void SaveLayoutObject(string objectName, int number, Vector3 pos, Vector3 rot, int layID)
    {
        WWWForm form = new WWWForm();
        form.AddField("objectName", objectName);
        form.AddField("objectNumber", number);
        form.AddField("posX", pos.x.ToString());
        form.AddField("posY", pos.y.ToString());
        form.AddField("posZ", pos.z.ToString());

        form.AddField("rotX", rot.x.ToString());
        form.AddField("rotY", rot.y.ToString());
        form.AddField("rotZ", rot.z.ToString());

        form.AddField("layoutID", layID);


        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "AddLayoutObject.php", form);
        StartCoroutine(request(w));
    }


    public void SaveObject(string objectName, int number, Vector3 pos, Vector3 rot, ResultMethod2 meth)
    {
        Debug.Log("Saving");
        WWWForm form = new WWWForm();
        form.AddField("objectName", objectName);
        form.AddField("objectNumber", number);
        form.AddField("posX", pos.x.ToString());
        form.AddField("posY", pos.y.ToString());
        form.AddField("posZ", pos.z.ToString());

        form.AddField("rotX", rot.x.ToString());
        form.AddField("rotY", rot.y.ToString());
        form.AddField("rotZ", rot.z.ToString());

        form.AddField("projectID", ApplicationStaticData.actualProject.id);


        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "AddObject.php", form);
        StartCoroutine(request(w, meth, pos, rot));
    }

    public void UpdateObject( Vector3 pos, Vector3 rot, int ID)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", ID);

        form.AddField("posX", pos.x.ToString());
        form.AddField("posY", pos.y.ToString());
        form.AddField("posZ", pos.z.ToString());

        form.AddField("rotX", rot.x.ToString());
        form.AddField("rotY", rot.y.ToString());
        form.AddField("rotZ", rot.z.ToString());

        form.AddField("projectID", ApplicationStaticData.actualProject.id);


        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "UpdateObject.php", form);
        StartCoroutine(request(w));
    }

    public void DeleteObject(int ID)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", ID);
        form.AddField("projectID", ApplicationStaticData.actualProject.id);
        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "DeleteObject.php", form);
        StartCoroutine(request(w));
    }

    //public void SaveLayout(int number)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("layoutNumber", number);
    //    WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "ChangeLayout.php", form);
    //    StartCoroutine(request(w));
    //}

    IEnumerator request(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else
        {
            message = "ERROR: " + w.error + "\n";
        }
        Debug.Log(message);
    }

    IEnumerator request(WWW w, ResultMethod2 method, Vector3 pos, Vector3 rot)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else
        {
            message = "ERROR: " + w.error + "\n";
        }
        Debug.Log(message);

        string[] msg = null;
        msg = message.Split(new string[] { "#####" }, StringSplitOptions.None);
        if (msg.Length > 0)
        {
            int ID = Int32.Parse(msg[0]);
            string typ1 = msg[1];
            string typ2 = msg[2];
            SceneObject obj = new SceneObject(ID,typ1, typ2, pos, rot);
            method(obj);
        }
    }

    IEnumerator request(WWW w, ResultMethod method)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else
        {
            message = "ERROR: " + w.error + "\n";
        }
        Debug.Log(message);

        string[] msg = null;
        msg = message.Split(new string[] { "#####" }, StringSplitOptions.None);
        if (msg.Length > 0)
        {
            int ID = Int32.Parse(msg[0]);
            string auth = msg[1];
            string onwer = msg[2];
            string name = msg[3];
            ProjectObject proj = new ProjectObject(ID, auth, onwer, name);
            method(proj);
        }
    }

    IEnumerator request(WWW w, ResultMethod3 method)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else
        {
            message = "ERROR: " + w.error + "\n";
        }
        Debug.Log(message);

        int ID;

        if (Int32.TryParse(message, out ID))
        {
            method(ID);
        }
            
    }



}
