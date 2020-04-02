using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;    

    public Vector3 zoomAmount;
    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPositon;
    public Vector3 rotateCurrentPositon;


    public UnityEngine.KeyCode forwardKey = KeyCode.Z;
    public UnityEngine.KeyCode altForwardKey = KeyCode.UpArrow;
    public UnityEngine.KeyCode backKey = KeyCode.S;
    public UnityEngine.KeyCode altBackKey = KeyCode.DownArrow;
    public UnityEngine.KeyCode rightKey= KeyCode.D;
    public UnityEngine.KeyCode altRightKey = KeyCode.RightArrow;
    public UnityEngine.KeyCode leftKey = KeyCode.Q;
    public UnityEngine.KeyCode altLeftKey = KeyCode.LeftArrow;
    public UnityEngine.KeyCode fastKey = KeyCode.LeftShift;
    public UnityEngine.KeyCode rightRotationKey = KeyCode.E;
    public UnityEngine.KeyCode leftRotationKey = KeyCode.A;
    public UnityEngine.KeyCode zoomInKey = KeyCode.R;
    public UnityEngine.KeyCode zoomOutKey = KeyCode.F;

    // Start is called before the first frame update
    void Start()
    {
        
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();
        HandleMovementInput();        
    }

    void HandleMouseInput()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray,out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if(Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray,out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;

            }
        }

        if(Input.GetMouseButtonDown(2))
        {
            rotateStartPositon = Input.mousePosition;
        }
        if(Input.GetMouseButton(2))
        {
            rotateCurrentPositon = Input.mousePosition;

            Vector3 difference = rotateStartPositon - rotateCurrentPositon;

            rotateStartPositon = rotateCurrentPositon;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
        
    }

    void HandleMovementInput()
    {
        if(Input.GetKey(fastKey))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if(Input.GetKey(forwardKey) || Input.GetKey(altForwardKey))
        {
            newPosition += (transform.forward * movementSpeed);
        }

        if(Input.GetKey(backKey) || Input.GetKey(altBackKey))
        {
            newPosition += (transform.forward * -movementSpeed);
        }

        if(Input.GetKey(rightKey) || Input.GetKey(altRightKey))
        {
            newPosition += (transform.right * movementSpeed);
        }

        if(Input.GetKey(leftKey) || Input.GetKey(altLeftKey))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if(Input.GetKey(rightRotationKey))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if(Input.GetKey(leftRotationKey))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if(Input.GetKey(zoomInKey)){
            newZoom += zoomAmount;
        }

        if(Input.GetKey(zoomOutKey)){
            newZoom -= zoomAmount;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }




}
