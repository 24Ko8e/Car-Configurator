using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBtn : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.LookAt(transform.position - cam.transform.position);
    }
}
