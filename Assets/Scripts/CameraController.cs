using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Base;
    public Transform defaultCameraTransform;
    
    Transform targetCameraPosition;
    
    public float CamSpeed = 7.5f;
    public float sensitivity = 5f;
    public float maxYaw = 0;
    public float minYaw = -90;
    public bool invert = false;
    public bool CameraControls = true;

    public bool CameraSetToDefaultTransform = true;
    public bool LookAround = false;
    float currYaw;
    float yaw;
    float pitch;
    float roll;

    float interiorLookAroundCurrYaw;
    float interiorLookAroundYaw;
    float interiorLookAroundPitch;

    float camDistance;
    float ZoomLevel;
    public float camZoomSpeed;
    public float zoomSensitivity;
    public float MaxZoom;
    public float minZoom;

    float baseRotX;

    void Start()
    {
        camDistance = Vector3.Distance(Base.transform.GetChild(GameManager.instance.selectedCarIndex).position, defaultCameraTransform.transform.position);
        ZoomLevel = camDistance;

        yaw = defaultCameraTransform.transform.rotation.eulerAngles.x;
        pitch = defaultCameraTransform.transform.rotation.eulerAngles.y;
        roll = defaultCameraTransform.transform.rotation.eulerAngles.z;

        baseRotX = Base.transform.rotation.eulerAngles.y;

        CameraControls = true;
    }

    void Update()
    {
        if (CameraSetToDefaultTransform)
        {
            CameraRotation();
            CameraZooming();
        }
        else
        {
            SetCameraToFocusPoint();
        }
    }

    private void SetCameraToFocusPoint()
    {


        if (LookAround)
        {
            transform.position = Vector3.Lerp(transform.position, targetCameraPosition.position, CamSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetCameraPosition.rotation.eulerAngles.x + interiorLookAroundCurrYaw,
                                    targetCameraPosition.rotation.eulerAngles.y + interiorLookAroundPitch, targetCameraPosition.rotation.eulerAngles.z), CamSpeed * Time.deltaTime);

            if (Input.GetMouseButton(0) && CameraControls)
            {
                interiorLookAroundYaw += Input.GetAxis("Mouse Y") * sensitivity * (invert ? -1f : 1f);
                interiorLookAroundYaw = Mathf.Clamp(interiorLookAroundYaw, -90, 90);
                interiorLookAroundPitch -= Input.GetAxis("Mouse X") * sensitivity;

            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                interiorLookAroundYaw += Input.GetAxis("Vertical") * sensitivity * (invert ? -1f : 1f);
                interiorLookAroundYaw = Mathf.Clamp(interiorLookAroundYaw, -90, 90);
                interiorLookAroundPitch -= Input.GetAxis("Horizontal") * sensitivity * 0.5f;
            }
            //interiorLookAroundCurrYaw = Mathf.Lerp(interiorLookAroundCurrYaw, interiorLookAroundYaw, CamSpeed * Time.deltaTime);
            interiorLookAroundCurrYaw = interiorLookAroundYaw;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetCameraPosition.position, CamSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetCameraPosition.rotation, CamSpeed * Time.deltaTime);
        }
    }

    private void CameraRotation()
    {

        if (Input.GetMouseButton(0) && CameraControls)
        {
            yaw += Input.GetAxis("Mouse Y") * sensitivity * (invert ? 1f : -1f);
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

            baseRotX -= Input.GetAxis("Mouse X") * sensitivity;
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            yaw += Input.GetAxis("Vertical") * sensitivity * (invert ? 1f : -1f);
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

            baseRotX -= Input.GetAxis("Horizontal") * sensitivity * 0.5f;
        }

        currYaw = Mathf.Lerp(currYaw, yaw, CamSpeed * Time.deltaTime);

        defaultCameraTransform.transform.position = Base.transform.GetChild(GameManager.instance.selectedCarIndex).position;
        defaultCameraTransform.rotation = Quaternion.Euler(currYaw, pitch, roll);
        defaultCameraTransform.transform.Translate(new Vector3(0, 0, -camDistance));

        transform.position = Vector3.Lerp(transform.position, defaultCameraTransform.position, CamSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, defaultCameraTransform.rotation, CamSpeed * Time.deltaTime);

        Base.transform.rotation = Quaternion.Slerp(Base.transform.rotation, Quaternion.Euler(0, baseRotX, 0), CamSpeed * Time.deltaTime);
    }

    private void CameraZooming()
    {
        if (CameraControls)
        {
            ZoomLevel += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        }

        if (ZoomLevel < minZoom)
        {
            ZoomLevel = Mathf.Lerp(ZoomLevel, minZoom, 50f * Time.deltaTime);
        }
        if (ZoomLevel > MaxZoom)
        {
            ZoomLevel = Mathf.Lerp(ZoomLevel, MaxZoom, 50f * Time.deltaTime);
        }
        //camDistance = Mathf.Lerp(camDistance, ZoomLevel, camZoomSpeed * Time.deltaTime);
        camDistance = ZoomLevel;
    }

    public void ZoomIn()
    {
        ZoomLevel -= zoomSensitivity;
    }

    public void ZoomOut()
    {
        ZoomLevel += zoomSensitivity;
    }

    public void UpCameraRotation()
    {
        if (CameraSetToDefaultTransform)
        {
            yaw += sensitivity * (invert ? 1f : -1f);
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        }
        else
        {
            interiorLookAroundYaw += sensitivity * (invert ? -1f : 1f);
            interiorLookAroundYaw = Mathf.Clamp(interiorLookAroundYaw, -90, 90);
        }
    }

    public void DownCameraRotation()
    {
        if (CameraSetToDefaultTransform)
        {
            yaw -= sensitivity * (invert ? 1f : -1f);
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        }
        else
        {
            interiorLookAroundYaw -= sensitivity * (invert ? -1f : 1f);
            interiorLookAroundYaw = Mathf.Clamp(interiorLookAroundYaw, -90, 90);
        }
    }

    public void LeftCameraRotation()
    {
        if (CameraSetToDefaultTransform)
        {
            baseRotX += sensitivity;
        }
        else
        {
            interiorLookAroundPitch += sensitivity;
        }
    }

    public void RightCameraRotation()
    {
        if (CameraSetToDefaultTransform)
        {
            baseRotX -= sensitivity;
        }
        else
        {
            interiorLookAroundPitch -= sensitivity;
        }
    }

    public void SetCustomCameraTransform(Transform target)
    {
        CameraSetToDefaultTransform = false;
        targetCameraPosition = target;
        ResetInteriorCameraRotation();
    }

    public void SetDefaultCameraPosition()
    {
        CameraSetToDefaultTransform = true;
        targetCameraPosition = defaultCameraTransform;
        ResetInteriorCameraRotation();
    }

    void ResetInteriorCameraRotation()
    {
        interiorLookAroundYaw = 0;
        interiorLookAroundPitch = 0;
    }
}
