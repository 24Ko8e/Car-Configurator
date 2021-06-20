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

    public bool CameraSetToDefaultTransform = true;
    float currYaw;
    float yaw;
    float pitch;
    float roll;

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
        transform.position = Vector3.Lerp(transform.position, targetCameraPosition.position, CamSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetCameraPosition.rotation, CamSpeed * Time.deltaTime);
    }

    private void CameraZooming()
    {
        ZoomLevel += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        if (ZoomLevel < minZoom)
        {
            ZoomLevel = Mathf.Lerp(ZoomLevel, minZoom, 50f * Time.deltaTime);
        }
        if (ZoomLevel > MaxZoom)
        {
            ZoomLevel = Mathf.Lerp(ZoomLevel, MaxZoom, 50f * Time.deltaTime);
        }
        camDistance = Mathf.Lerp(camDistance, ZoomLevel, camZoomSpeed * Time.deltaTime);
    }

    private void CameraRotation()
    {
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse Y") * sensitivity * (invert ? 1f : -1f);
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

            baseRotX -= Input.GetAxis("Mouse X") * sensitivity;
        }

        currYaw = Mathf.Lerp(currYaw, yaw, CamSpeed * Time.deltaTime);
        transform.position = Base.transform.GetChild(GameManager.instance.selectedCarIndex).position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(currYaw, pitch, roll), CamSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, -camDistance));

        Base.transform.rotation = Quaternion.Slerp(Base.transform.rotation, Quaternion.Euler(0, baseRotX, 0), CamSpeed * Time.deltaTime);
    }

    public void SetCustomCameraTransform(Transform target)
    {
        CameraSetToDefaultTransform = false;
        targetCameraPosition = target;
    }

    public void SetDefaultCameraPosition()
    {
        CameraSetToDefaultTransform = true;
        targetCameraPosition = defaultCameraTransform;
    }
}
