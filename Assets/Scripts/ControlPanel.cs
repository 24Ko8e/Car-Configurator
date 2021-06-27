using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

public class ControlPanel : MonoBehaviour
{
    public CameraController cameraController;

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

        cam.targetTexture = new RenderTexture(2 * Screen.width, 2 * Screen.height, 24);
        Texture2D screenshot = new Texture2D(2 * Screen.width, 2 * Screen.height, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = cam.targetTexture;
        screenshot.ReadPixels(new Rect(0, 0, 2 * Screen.width, 2 * Screen.height), 0, 0);

        byte[] bytes = screenshot.EncodeToJPG(100);
        System.IO.File.WriteAllBytes(directory + "/Car-" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".jpg", bytes);
        
        cam.targetTexture = null;

        if (!hotspotsAlreadyDisabled)
            GameManager.instance.cars[GameManager.instance.selectedCarIndex].EnableHotspots();

#elif UNITY_WEBGL
        
        StartCoroutine(UploadJPG());        

#endif
    }

    IEnumerator UploadJPG()
    {
        yield return new WaitForEndOfFrame();

        bool hotspotsAlreadyDisabled = !GameManager.instance.cars[GameManager.instance.selectedCarIndex].hotspots.activeInHierarchy;

        if (!hotspotsAlreadyDisabled)
            GameManager.instance.cars[GameManager.instance.selectedCarIndex].DisableHotspots();

        Camera cam = Camera.main;

        string directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures) + "/Car Configurator";

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        cam.targetTexture = new RenderTexture(2* Screen.width, 2 * Screen.height, 24);
        Texture2D screenshot = new Texture2D(2 * Screen.width, 2 * Screen.height, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = cam.targetTexture;
        screenshot.ReadPixels(new Rect(0, 0, 2 * Screen.width, 2 * Screen.height), 0, 0);

        byte[] bytes = screenshot.EncodeToJPG(100);

        string encodedText = System.Convert.ToBase64String(bytes);

        var image_url = "data:image/jpg;base64," + encodedText;

        cam.targetTexture = null;

        if (!hotspotsAlreadyDisabled)
            GameManager.instance.cars[GameManager.instance.selectedCarIndex].EnableHotspots();

#if !UNITY_EDITOR
        openWindow(image_url);
#endif

    }

    [DllImport("__Internal")]
    static extern void openWindow(string url);
}
