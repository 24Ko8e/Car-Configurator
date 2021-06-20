using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public CameraController cameraController;

    public Transform FrontView;
    public Transform RearView;
    public Transform RightWindow;
    public Transform LeftWindow;
    public Transform FrontRightWheel;
    public Transform FrontLeftWheel;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetCameraToFrontView()
    {
        cameraController.SetCustomCameraTransform(FrontView);
    }

    public void SetCameraToRearView()
    {
        cameraController.SetCustomCameraTransform(RearView);
    }

    public void SetCameraToRightWindow()
    {
        cameraController.SetCustomCameraTransform(RightWindow);
    }


    public void SetCameraToLeftWindow()
    {
        cameraController.SetCustomCameraTransform(LeftWindow);
    }
    public void SetCameraToFrontRightWheel()
    {
        cameraController.SetCustomCameraTransform(FrontRightWheel);
    }

    public void SetCameraToFrontLeftWheel()
    {
        cameraController.SetCustomCameraTransform(FrontLeftWheel);
    }
}
