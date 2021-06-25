using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Configurator : MonoBehaviour
{
    public Scrollbar bodyColorsScrollbar;
    float bodyColorsScrollbarValue;
    public Animator panelAnimator;
    bool panelVisible = true;

    public GameObject lastSelectedBodyColor;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetBodyColor(GameObject button, Color color)
    {
        lastSelectedBodyColor.transform.GetChild(2).gameObject.SetActive(false);
        lastSelectedBodyColor.transform.GetChild(1).gameObject.SetActive(true);

        button.transform.GetChild(2).gameObject.SetActive(true);
        button.transform.GetChild(1).gameObject.SetActive(false);
        
        lastSelectedBodyColor = button;

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
}
