﻿using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class ScreenshotClick : MonoBehaviour, IPointerDownHandler
{
    [Serializable]
    public class ButtonPressEvent : UnityEvent { }

    public ButtonPressEvent OnPress = new ButtonPressEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress.Invoke();
    }
}