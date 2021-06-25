using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public CameraController cameraController;

    public Transform FrontView;
    public Transform RearView;
    public Transform RightWindowView;
    public Transform LeftWindowView;
    public Transform FrontRightWheelView;
    public Transform FrontLeftWheelView;
    public Transform InteriorPassengerView;
    public Transform InteriorDriverView;

    public GameObject defaultViewBtns;
    public GameObject rightWindowViewBtns;
    public GameObject leftWindowViewBtns;
    public GameObject frontViewBtns;
    public GameObject rearViewBtns;
    public GameObject interior_P_ViewBtns;
    public GameObject interior_D_ViewBtns;
    public GameObject FrontRightWheelViewBtns;
    public GameObject FrontLeftWheelViewBtns;

    public GameObject[] body;

    GameObject lastselected;

    void Start()
    {
        lastselected = defaultViewBtns;
    }

    public void SetCameraToDefaultView()
    {
        cameraController.SetDefaultCameraPosition();
        switchBtnSet(defaultViewBtns);
        cameraController.LookAround = false;
    }

    public void SetCameraToFrontView()
    {
        cameraController.SetCustomCameraTransform(FrontView);
        switchBtnSet(frontViewBtns);
        cameraController.LookAround = false;
    }

    public void SetCameraToRearView()
    {
        cameraController.SetCustomCameraTransform(RearView);
        switchBtnSet(rearViewBtns);
        cameraController.LookAround = false;
    }

    public void SetCameraToRightWindow()
    {
        cameraController.SetCustomCameraTransform(RightWindowView);
        switchBtnSet(rightWindowViewBtns);
        cameraController.LookAround = false;
    }


    public void SetCameraToLeftWindow()
    {
        cameraController.SetCustomCameraTransform(LeftWindowView);
        switchBtnSet(leftWindowViewBtns);
        cameraController.LookAround = false;
    }
    public void SetCameraToFrontRightWheel()
    {
        cameraController.SetCustomCameraTransform(FrontRightWheelView);
        switchBtnSet(FrontRightWheelViewBtns);
        cameraController.LookAround = false;
    }

    public void SetCameraToFrontLeftWheel()
    {
        cameraController.SetCustomCameraTransform(FrontLeftWheelView);
        switchBtnSet(FrontLeftWheelViewBtns);
        cameraController.LookAround = false;
    }

    public void SetCameraToInteriorPassenger()
    {
        cameraController.SetCustomCameraTransform(InteriorPassengerView);
        switchBtnSet(interior_P_ViewBtns);
        cameraController.LookAround = true;
    }

    public void SetCameraToInteriorDriver()
    {
        cameraController.SetCustomCameraTransform(InteriorDriverView);
        switchBtnSet(interior_D_ViewBtns);
        cameraController.LookAround = true;
    }

    void switchBtnSet(GameObject btnSet)
    {
        lastselected.SetActive(false);
        btnSet.SetActive(true);
        lastselected = btnSet;
    }
}
