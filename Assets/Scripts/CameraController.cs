using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Base;
    public Transform defaultPosition;
    public float CamSpeed = 7.5f;
    public float sensitivity = 5f;
    public float maxYaw = 0;
    public float minYaw = -90;
    public bool invert = false;

    Vector3 camInitialPos;
    float currYaw;
    float yaw;
    float pitch;
    float roll;

    void Start()
    {
        camInitialPos = defaultPosition.transform.position;
        yaw = defaultPosition.transform.rotation.eulerAngles.x;
        pitch = defaultPosition.transform.rotation.eulerAngles.y;
        roll = defaultPosition.transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse Y") * sensitivity * (invert ? 1f : -1f);
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        }

        currYaw = Mathf.Lerp(currYaw, yaw, CamSpeed * Time.deltaTime);
        transform.position = Base.transform.GetChild(GameManager.instance.selectedCarIndex).position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(currYaw, pitch, roll), CamSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, -Vector3.Distance(Base.transform.GetChild(GameManager.instance.selectedCarIndex).position, camInitialPos)));
    }
}
