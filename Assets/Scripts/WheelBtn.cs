using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelBtn : MonoBehaviour
{
    public Configurator config;
    int wheelNumber;
    GameObject btn;

    void Start()
    {
        btn = this.gameObject;
        wheelNumber = int.Parse(this.gameObject.name.Split(' ')[1]);

        GetComponent<Button>().onClick.AddListener(OnBtnClick);
    }

    public void OnBtnClick()
    {
        config.SetWheels(btn, wheelNumber);
    }
}
