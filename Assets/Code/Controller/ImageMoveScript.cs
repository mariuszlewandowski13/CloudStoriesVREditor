#region Usings
using UnityEngine;
#endregion


public class ImageMoveScript : MonoBehaviour
{
    #region Private Properties

    private bool toDestroy = false;
    private GameObject rotationParent;

    private Transform prevParent;

    private Vector3 controllerFirstPosition;
    private Vector3 controllerSecondPosition;

    private GameObject actualController;

    public bool active = false;

    #endregion
    #region Public Properties

    public float treshold = 0.05f;
    public bool canMove;

    #endregion


    #region Methods
    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        canMove = true;
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
                gameObj.GetComponent<ControllerScript>().TriggerDown += OnTriggerDown;
                active = true;
                actualController = gameObj;
            
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControllerScript>().TriggerDown -= OnTriggerDown;
            active = false;
        }
    }

    private void OnControllerMove(GameObject controller)
    {
        controllerSecondPosition = controllerFirstPosition;
        controllerFirstPosition = controller.transform.position;

        rotationParent.transform.position = controllerFirstPosition;
        rotationParent.transform.rotation = controller.transform.rotation;
                gameObject.transform.parent = rotationParent.transform;

    }

    private void OnTriggerDown(GameObject controller)
    {

           
            controller.GetComponent<ControllerScript>().TriggerUp += OnTriggerUp;
            
            GetComponent<ObjectInteractionScript>().SetIsSelected(true);

            controller.GetComponent<ControllerScript>().ControllerMove += OnControllerMove;
            rotationParent = controller.GetComponent<ControllerScript>().rotationCenter;
            controllerFirstPosition = controllerSecondPosition = controller.transform.position;

            rotationParent.transform.position = controllerFirstPosition;
            rotationParent.transform.rotation = controller.transform.rotation;
            prevParent = gameObject.transform.parent;

                gameObject.transform.parent = rotationParent.transform;
 

            if (GetComponent<Rigidbody>() != null) Destroy(gameObject.GetComponent<Rigidbody>());
            toDestroy = false;
        
    }
    private void OnTriggerUp(GameObject controller)
    {
        controller.GetComponent<ControllerScript>().TriggerUp -= OnTriggerUp;
        controller.GetComponent<ControllerScript>().ControllerMove -= OnControllerMove;
        GetComponent<ObjectInteractionScript>().SetIsSelected(false);
        gameObject.transform.parent = prevParent;
        CheckAndSetToDestroy();
        
    }

    private void CheckAndSetToDestroy()
    {
        float distance = Vector3.Distance(controllerFirstPosition, controllerSecondPosition);
        if (distance > treshold)
        {
            toDestroy = true;
            Rigidbody rigidbod = gameObject.AddComponent<Rigidbody>();
            rigidbod.AddForce((controllerFirstPosition - controllerSecondPosition) * 8000.0f, ForceMode.Force);
            rigidbod.mass = 0.01f;
            rigidbod.useGravity = true;
            rigidbod.detectCollisions = false;
            rigidbod.interpolation = RigidbodyInterpolation.Interpolate;
            DestroyImmediate(GetComponent<BoxCollider>());
            Transform scaleHandler = transform.Find("ScaleHandler");

            ControllerCollision(actualController, false);
            if (scaleHandler != null)
            {
                DestroyObjectAndChildrenColliders(scaleHandler);
            }
            Destroy(gameObject, 5.0f);
        }
        
    }

    private void OnDestroy()
    {    
        GetComponent<ObjectInteractionScript>().ControllerCollision -= ControllerCollision;
    }

    private void DestroyObjectAndChildrenColliders(Transform gameObject)
    {
        foreach (Transform child in gameObject)
        {
            if (child.GetComponent<Collider>() != null) child.GetComponent<Collider>().enabled = false;
        }
    }

    #endregion
}
