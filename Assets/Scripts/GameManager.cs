using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Car[] cars;
    [HideInInspector]
    public int selectedCarIndex;
    int lastSelectedCarIndex;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ActivateSelectedCar(0);
    }

    void Update()
    {
        
    }

    public void ActivateSelectedCar(int index)
    {
        if (cars[lastSelectedCarIndex] != null)
        {
            cars[lastSelectedCarIndex].gameObject.SetActive(false);
        }

        lastSelectedCarIndex = selectedCarIndex;
        selectedCarIndex = index;

        cars[selectedCarIndex].gameObject.SetActive(true);
    }
}
