using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Base;
    public Transform cameraPosition;
    public float CamSpeed = 7.5f;
    public float sensitivity = 5f;
    public float maxYaw = 0;
    public float minYaw = -90;
    public bool invert = false;

    public bool SetCameraToDefaultTransform = true;
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
        camDistance = Vector3.Distance(Base.transform.GetChild(GameManager.instance.selectedCarIndex).position, transform.position);
        ZoomLevel = camDistance;

        yaw = cameraPosition.transform.rotation.eulerAngles.x;
        pitch = cameraPosition.transform.rotation.eulerAngles.y;
        roll = cameraPosition.transform.rotation.eulerAngles.z;

        baseRotX = Base.transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        if (SetCameraToDefaultTransform)
        {
            CameraRotation();
            CameraZooming();
        }
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
}
