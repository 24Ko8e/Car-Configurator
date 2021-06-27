using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Configurator : MonoBehaviour
{
    public CameraController cameraController;

    public Scrollbar bodyColorsScrollbar;
    public Scrollbar wheelsScrollbar;
    public Scrollbar interiorScrollbar;
    
    float bodyColorsScrollbarValue;
    float wheelsScrollbarValue;
    float interiorScrollbarValue;

    public Animator panelAnimator;
    bool panelVisible = true;

    public GameObject lastSelectedBodyColorBtn;
    public GameObject lastSelectedWheelsBtn;
    public GameObject lastSelectedInteriorsBtn;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetBodyColor(GameObject button, Color color)
    {
        lastSelectedBodyColorBtn.transform.GetChild(2).gameObject.SetActive(false);
        lastSelectedBodyColorBtn.transform.GetChild(1).gameObject.SetActive(true);

        button.transform.GetChild(2).gameObject.SetActive(true);
        button.transform.GetChild(1).gameObject.SetActive(false);
        
        lastSelectedBodyColorBtn = button;

        Car car = GameManager.instance.cars[GameManager.instance.selectedCarIndex];
        for (int i = 0; i < car.body.Length; i++)
        {
            car.body[i].GetComponent<MeshRenderer>().materials[0].SetColor("_BaseColor", color);
        }
    }

    public void RightBtnBodyColor()
    {
        bodyColorsScrollbarValue += 0.2f;
        bodyColorsScrollbarValue = Mathf.Clamp(bodyColorsScrollbarValue, 0, 1);
        bodyColorsScrollbar.value = bodyColorsScrollbarValue;
    }

    public void LeftBtnBodyColor()
    {
        bodyColorsScrollbarValue -= 0.2f;
        bodyColorsScrollbarValue = Mathf.Clamp(bodyColorsScrollbarValue, 0, 1);
        bodyColorsScrollbar.value = bodyColorsScrollbarValue;
    }

    public void SetWheels(GameObject button, int newWheelNumber)
    {
        lastSelectedWheelsBtn.transform.GetChild(2).gameObject.SetActive(false);
        lastSelectedWheelsBtn.transform.GetChild(1).gameObject.SetActive(true);

        button.transform.GetChild(2).gameObject.SetActive(true);
        button.transform.GetChild(1).gameObject.SetActive(false);

        lastSelectedWheelsBtn = button;

        Car car = GameManager.instance.cars[GameManager.instance.selectedCarIndex];

        car.lastSelectedWheel.SetActive(false);
        car.wheels[newWheelNumber - 1].SetActive(true);

        car.lastSelectedWheel = car.wheels[newWheelNumber - 1];
    }

    public void RightBtnWheels()
    {
        wheelsScrollbarValue += 0.2f;
        wheelsScrollbarValue = Mathf.Clamp(wheelsScrollbarValue, 0, 1);
        wheelsScrollbar.value = wheelsScrollbarValue;
    }

    public void LeftBtnWheels()
    {
        wheelsScrollbarValue -= 0.2f;
        wheelsScrollbarValue = Mathf.Clamp(wheelsScrollbarValue, 0, 1);
        wheelsScrollbar.value = wheelsScrollbarValue;
    }

    public void SetInteriorColor(GameObject button, Color color)
    {
        lastSelectedInteriorsBtn.transform.GetChild(2).gameObject.SetActive(false);
        lastSelectedInteriorsBtn.transform.GetChild(1).gameObject.SetActive(true);

        button.transform.GetChild(2).gameObject.SetActive(true);
        button.transform.GetChild(1).gameObject.SetActive(false);

        lastSelectedInteriorsBtn = button;

        Car car = GameManager.instance.cars[GameManager.instance.selectedCarIndex];
        for (int i = 0; i < car.interior.Length; i++)
        {
            car.interior[i].GetComponent<MeshRenderer>().materials[0].SetColor("_BaseColor", color);
        }
    }

    public void RightBtnInterior()
    {
        interiorScrollbarValue += 0.2f;
        interiorScrollbarValue = Mathf.Clamp(interiorScrollbarValue, 0, 1);
        interiorScrollbar.value = interiorScrollbarValue;
    }

    public void LeftBtnInterior()
    {
        interiorScrollbarValue -= 0.2f;
        interiorScrollbarValue = Mathf.Clamp(interiorScrollbarValue, 0, 1);
        interiorScrollbar.value = interiorScrollbarValue;
    }

    public void TogglePanel()
    {
        if (panelVisible)
        {
            panelAnimator.SetTrigger("HideConfig");
            panelVisible = false;
        }
        else
        {
            panelAnimator.SetTrigger("OpenConfig");
            panelVisible = true;
        }
    }

    public void DisableCameraController()
    {
        cameraController.CameraControls = false;
    }

    public void EnableCameraController()
    {
        cameraController.CameraControls = true;
    }
}
