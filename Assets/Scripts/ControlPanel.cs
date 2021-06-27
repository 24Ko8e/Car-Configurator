using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ControlPanel : MonoBehaviour
{
    public CameraController cameraController;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ToggleHotspots(GameObject btn)
    {
        GameManager.instance.cars[GameManager.instance.selectedCarIndex].ToggleHotspots();

        btn.transform.GetChild(0).gameObject.SetActive(!btn.transform.GetChild(0).gameObject.activeInHierarchy);
        btn.transform.GetChild(1).gameObject.SetActive(!btn.transform.GetChild(1).gameObject.activeInHierarchy);
    }

    public void EnableCameraController()
    {
        cameraController.CameraControls = true;
    }

    public void DisableCameraController()
    {
        cameraController.CameraControls = false;
    }

    public void TakeScreenshot()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        bool hotspotsAlreadyDisabled = !GameManager.instance.cars[GameManager.instance.selectedCarIndex].hotspots.activeInHierarchy;
        
        if(!hotspotsAlreadyDisabled)
            GameManager.instance.cars[GameManager.instance.selectedCarIndex].DisableHotspots();

        Camera cam = Camera.main;

        string directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures) + "/Car Configurator";

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = cam.targetTexture;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        byte[] bytes = screenshot.EncodeToJPG();
        System.IO.File.WriteAllBytes(directory + "/Car-" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".jpg", bytes);
        
        cam.targetTexture = null;

        if (!hotspotsAlreadyDisabled)
            GameManager.instance.cars[GameManager.instance.selectedCarIndex].EnableHotspots();

#elif UNITY_WEBGL

#endif
    }
}
