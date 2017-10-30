﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
    #region Public Events & Delegates
    public GameObject rotationCenterPrefab;


    public delegate void Interaction(GameObject gameObject);
    public event Interaction ControllerMove;
    public event Interaction TriggerUp;
    public event Interaction TriggerDown;

    public static event Interaction TriggerDownGlobal;

    public event Interaction GripDown;

    public event Interaction GripUp;

    public GameObject rotationCenter;

    public GameObject pickup = null;

    public List<GameObject> collidingGameObjects;
    public List<GameObject> realCollidingGameObjects;

    #endregion

    #region Private Properties

    //private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    //public Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    //  public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    //  public SteamVR_TrackedObject trackedObj;
    public GameObject selected;

    private object selectedLock = new object();
    private object pickupLock = new object();


   public bool triggerDown;
   public bool triggerUp;

    bool previousStatePressed = false;
    bool actualStatePressed = false;

    

    #endregion

    void Start()
    {
        rotationCenter = Instantiate(rotationCenterPrefab);
        //   trackedObj = transform.parent.parent.GetComponent<SteamVR_TrackedObject>();
        collidingGameObjects = new List<GameObject>();
        realCollidingGameObjects = new List<GameObject>();
    }

    void Update()
    {
        lock(pickupLock)
        {
        triggerDown = (!previousStatePressed && actualStatePressed);
        triggerUp = (previousStatePressed && !actualStatePressed);
            Debug.Log("TriggerDown :" + triggerDown.ToString() + " Pickup: " + pickup);
        

            if (triggerDown && TriggerDown != null)
            {
                TriggerDown(gameObject);
            }

            if (triggerDown && TriggerDownGlobal != null)
            {
                TriggerDownGlobal(gameObject);
            }

            if (triggerUp && TriggerUp != null)
            {
                TriggerUp(gameObject);
            }
           

            if (ControllerMove != null)
            {
                ControllerMove(gameObject);
            }
            if (triggerDown && pickup == null)
            {
                DeleteSelection();
            }

        }
      //  //CheckCollidingObjects();
       // realCollidingGameObjects.Clear();

      

    }

    private void LateUpdate()
    {
        triggerDown = false;
        triggerUp = false;
    }

    public void SetTriggerState(bool actualState)
    {
        lock (pickupLock)
        {
            previousStatePressed = actualStatePressed;
            actualStatePressed = actualState;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("col");

                lock (pickupLock)
                {
                    if (pickup != null)
                    {
                        if (collider.gameObject.GetComponent<ObjectInteractionScript>() != null && !pickup.GetComponent<ObjectInteractionScript>().GetIsSelected())
                        {
                            pickup.GetComponent<ObjectInteractionScript>().SetCollision(false, gameObject);
                            collidingGameObjects.Add(pickup);
                            pickup = collider.gameObject;
                            pickup.GetComponent<ObjectInteractionScript>().SetCollision(true, gameObject);
                        }
                    }
                    else
                    {
                        if (collider.gameObject.GetComponent<ObjectInteractionScript>() != null )
                        {
                            pickup = collider.gameObject;
                            pickup.GetComponent<ObjectInteractionScript>().SetCollision(true, gameObject);
                        }
                    }

                }
            
        

    }


    public GameObject GetPickup()
    {
        GameObject result;
        lock (pickupLock)
        {
            result = pickup;
        }
        return result;

    }


    public void SetSelected(GameObject newSelected)
    {
        lock (selectedLock)
        {
            if (selected != null)
            {
                if (selected != newSelected)
                {
                    selected.GetComponent<SelectingObjectsScript>().SetAsNonSelected(gameObject);
                    selected = newSelected;
                    selected.GetComponent<SelectingObjectsScript>().SetAsSelected(gameObject);
                }
            }
            else
            {
                selected = newSelected;
                selected.GetComponent<SelectingObjectsScript>().SetAsSelected(gameObject);
            }
        }
    }


    public void DeleteSelection()
    {
        lock (selectedLock)
        {
            if (selected != null)
            {
                selected.GetComponent<SelectingObjectsScript>().SetAsNonSelected(gameObject);
                selected = null;
            }
        }

    }

    //void OnTriggerStay(Collider collider)
    //{
    //    if ((collidingGameObjects.Contains(collider.gameObject) || pickup == collider.gameObject) && !realCollidingGameObjects.Contains(collider.gameObject))
    //    {
    //        realCollidingGameObjects.Add(collider.gameObject);
    //    }

    //}

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log("col");

        lock (pickupLock)
        {
            if (pickup != null)
            {
                if (pickup == collider.gameObject)
                {
                    pickup.GetComponent<ObjectInteractionScript>().SetCollision(false, gameObject);
                    pickup = null;
                }
            }
        }
    }

    private void CheckCollidingObjects()
    {
        List<GameObject> objToRemove = new List<GameObject>();

        foreach (GameObject obj in collidingGameObjects)
        {
            if (!realCollidingGameObjects.Contains(obj))
            {
                objToRemove.Add(obj);
            }
        }

        foreach (GameObject obj in objToRemove)
        {
            collidingGameObjects.Remove(obj);
        }

        if (pickup != null && !realCollidingGameObjects.Contains(pickup) && !pickup.GetComponent<ObjectInteractionScript>().GetIsSelected())
        {
            pickup.GetComponent<ObjectInteractionScript>().SetCollision(false, gameObject);
            pickup = null;
        }
    }

    private void OnDestroy()
    {
        if (rotationCenter != null)
        {
            Destroy(rotationCenter);
        }
    }


}
