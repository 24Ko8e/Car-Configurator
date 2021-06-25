using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyColorBtn : MonoBehaviour
{
    public Configurator config;
    public Color color;
    GameObject btn;

    void Start()
    {
        btn = this.gameObject;
    }

    public void OnBtnClick()
    {
        config.SetBodyColor(btn, color);
    }
}
