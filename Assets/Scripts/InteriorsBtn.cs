using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteriorsBtn : MonoBehaviour
{
    public Configurator config;
    public Color tint;
    GameObject btn;

    void Start()
    {
        btn = this.gameObject;
        GetComponent<Button>().onClick.AddListener(OnBtnClick);
    }

    public void OnBtnClick()
    {
        config.SetInteriorColor(btn, tint);
    }
}
