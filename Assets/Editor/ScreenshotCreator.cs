using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class ScreenshotCreator : EditorWindow
{
    [MenuItem("Window/Advanced Screenshot Creator")]
    public static void ShowWindow()
    {
        EditorWindow ScreenshotWindow = GetWindow<ScreenshotCreator>("Screenshot");

        ScreenshotWindow.autoRepaintOnSceneChange = true;
        ScreenshotWindow.minSize = new Vector2(320f, 300f);
        ScreenshotWindow.titleContent.image = EditorGUIUtility.IconContent("SceneViewCamera").image;
    }

    string[] view = new string[] { "Scene", "Game" };
    int selectedView = 1;

    // Reference to the camera to use for taking screenshots.
    Camera captureCamera;
    List<Camera> cameraList = new List<Camera>();
    string[] cameraNames;
    int selectedCamera = 0;


    // Capture type
    string[] captureType = new string[] { "2D", "360 Panorama" };
    int selectedCaptureType = 0;

    // Set Resolution and aspect ratio for 2D
    List<Resolutions2D> resolutions2DList = new List<Resolutions2D>();

    // Set Resolution and aspect ratio for panorama
    List<ResolutionsPano> resolutionsPanoList = new List<ResolutionsPano>();

    // Selection of file format
    string[] ext = new string[] { ".JPG", ".PNG", ".EXR", ".TGA" };
    int selectedExt = 0;

    // Quality of image if exporting to jpg
    int imageQuality = 90;

    // Enable or disable transparency
    string[] transparency = new string[] { "RGB", "RGB-A", "Alpha Only" };
    int TransparencySetting = 0;

    // If taking multiple shots, you can enable automatically naming the subsequent shots.
    bool customFilename;

    // Variables for save path
    string directory;
    string filename;
    string path;

    private void OnGUI()
    {
        if (directory == null || directory == "")
            directory = Application.dataPath + "/";

        GUILayout.Space(10);

        GUILayout.BeginHorizontal("box", GUILayout.Height(40));
        GUILayout.BeginVertical();
        GUILayout.Space(5f);
        selectedView = GUILayout.SelectionGrid(selectedView, view, view.Length, GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        UpdateCamerasList();
        if (selectedView == 0)
        {
            EditorGUILayout.LabelField("Scene camera is being used.", GUILayout.MaxWidth(200));
            captureCamera = SceneView.lastActiveSceneView.camera;
        }
        else
        {
            selectedCamera = EditorGUILayout.Popup("Camera", selectedCamera, cameraNames, GUILayout.MaxWidth(300));

            if (cameraList.Count > 1)
            {
                captureCamera = cameraList[selectedCamera];
            }
            else
            {
                captureCamera = cameraList[0];
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        selectedCaptureType = GUILayout.SelectionGrid(selectedCaptureType, captureType, captureType.Length);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        if (selectedCaptureType == 0)
        {
            if (resolutions2DList.Count == 0)
                resolutions2DList.Add(new Resolutions2D());

            for (int i = 0; i < resolutions2DList.Count; i++)
            {
                GUILayout.BeginHorizontal();

                resolutions2DList[i].SetResolution();

                resolutions2DList[i].SelectedRes2D = EditorGUILayout.Popup("Resolution (" + resolutions2DList[i].width + "x" + resolutions2DList[i].height + ")",
                    resolutions2DList[i].SelectedRes2D, resolutions2DList[0].res2D, GUILayout.Width(215));

                if (resolutions2DList[i].customResolution)
                {
                    resolutions2DList[i].width = EditorGUILayout.IntField("", resolutions2DList[i].width, GUILayout.MaxWidth(70));
                    EditorGUILayout.LabelField("x", GUILayout.MaxWidth(10));
                    resolutions2DList[i].height = EditorGUILayout.IntField("", resolutions2DList[i].height, GUILayout.MaxWidth(70));
                }
                else
                {
                    resolutions2DList[i].SelectedAspectRatio2D = EditorGUILayout.Popup(resolutions2DList[i].SelectedAspectRatio2D,
                        resolutions2DList[i].ar2D, GUILayout.Width(55));
                }

                if (i == 0)
                {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus").image, GUILayout.Width(30)))
                    {
                        resolutions2DList.Add(new Resolutions2D());
                    }
                }
                else
                {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus").image, GUILayout.Width(30)))
                    {
                        resolutions2DList.Remove(resolutions2DList[i]);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        else if (selectedCaptureType == 1)
        {
            if (resolutionsPanoList.Count == 0)
                resolutionsPanoList.Add(new ResolutionsPano());

            for (int i = 0; i < resolutionsPanoList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                resolutionsPanoList[i].SetResolution();

                resolutionsPanoList[i].SelectedResPano = EditorGUILayout.Popup("Resolution (" + resolutionsPanoList[i].width + "x" + resolutionsPanoList[i].height + ")",
                    resolutionsPanoList[i].SelectedResPano, resolutionsPanoList[i].resPano, GUILayout.Width(225));

                GUILayout.Space(5);
                EditorGUILayout.LabelField("Stereoscopic", GUILayout.MaxWidth(75));
                resolutionsPanoList[i].stereoscopic = EditorGUILayout.Toggle("", resolutionsPanoList[i].stereoscopic, GUILayout.Width(15));

                GUILayout.Space(10);

                if (i == 0)
                {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus").image, GUILayout.Width(30)))
                    {
                        resolutionsPanoList.Add(new ResolutionsPano());
                    }
                }
                else
                {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus").image, GUILayout.Width(30)))
                    {
                        resolutionsPanoList.Remove(resolutionsPanoList[i]);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("File format", GUILayout.MaxWidth(150));
        selectedExt = GUILayout.SelectionGrid(selectedExt, ext, ext.Length, GUILayout.MinWidth(150), GUILayout.MaxWidth(275));

        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        if (selectedExt == 0)
        {
            GUILayout.BeginHorizontal();
            imageQuality = EditorGUILayout.IntSlider("Image quality", imageQuality, 10, 100, GUILayout.MaxWidth(500));
            GUILayout.EndHorizontal();
        }
        else if (selectedExt == 1 || selectedExt == 3)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Transparency", GUILayout.MaxWidth(150));
            TransparencySetting = GUILayout.SelectionGrid(TransparencySetting, transparency, transparency.Length, GUILayout.MinWidth(150), GUILayout.MaxWidth(275));
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        customFilename = EditorGUILayout.Toggle("Custom file name", customFilename);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        if (!customFilename)
        {
            filename = "Screenshot-" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        }
        else
        {
            filename = EditorGUILayout.TextField("File name", filename, GUILayout.MinWidth(500));
            GUILayout.Space(5);
        }

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Space(75);
        if (GUILayout.Button("Assets/"))
        {
            directory = Application.dataPath;
        }
        if (GUILayout.Button("/Desktop"))
        {
            directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Replace('\\', '/');
        }
        if (GUILayout.Button("/Documents"))
        {
            directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace('\\', '/');
        }
        if (GUILayout.Button("/Pictures"))
        {
            directory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).Replace('\\', '/');
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save folder", GUILayout.MinWidth(75), GUILayout.MaxWidth(145));
        directory = EditorGUILayout.TextField("", directory, GUILayout.MinWidth(145)).Replace('\\', '/');

        if (GUILayout.Button("...", GUILayout.Width(25)))
        {
            directory = EditorUtility.SaveFolderPanel("Screenshot destination folder", directory, "");
        }
        if (GUILayout.Button("Open", GUILayout.Width(50)))
        {
            Application.OpenURL("file://" + directory);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(5);

        if (directory != "" && !directory.EndsWith("/"))
            directory += "/";

        if (GUILayout.Button("-Capture-", GUILayout.Height(30)))
        {
            CheckValidDirectory(directory);

            if (selectedCaptureType == 0)
            {
                for (int i = 0; i < resolutions2DList.Count; i++)
                {
                    if (i == 0)
                    {
                        path = directory + filename + ext[selectedExt];
                    }
                    else
                    {
                        path = directory + filename + "(" + i + ")" + ext[selectedExt];
                    }

                    TakeScreenshot(resolutions2DList[i].width, resolutions2DList[i].height, TransparencySetting);
                    EditorUtility.DisplayProgressBar("Saving...", "(" + (i + 1) + "/" + resolutions2DList.Count + ")", (float)(i + 1) / (float)resolutions2DList.Count);

                    Debug.Log(resolutions2DList[i].width + " , " + resolutions2DList[i].height);
                }
                EditorUtility.ClearProgressBar();
            }
            else if (selectedCaptureType == 1)
            {
                for (int i = 0; i < resolutionsPanoList.Count; i++)
                {
                    if (i == 0)
                    {
                        path = directory + filename + ext[selectedExt];
                    }
                    else
                    {
                        path = directory + filename + "(" + i + ")" + ext[selectedExt];
                    }

                    CapturePanorama(resolutionsPanoList[i].width, resolutionsPanoList[i].height, resolutionsPanoList[i].stereoscopic);
                    EditorUtility.DisplayProgressBar("Saving...", "(" + (i + 1) + "/" + resolutionsPanoList.Count + ")", (float)(i + 1) / (float)resolutionsPanoList.Count);

                    Debug.Log(resolutionsPanoList[i].width + " , " + resolutionsPanoList[i].height);
                }
                EditorUtility.ClearProgressBar();
            }

        }
    }

    void UpdateCamerasList()
    {
        Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
        cameraList.Clear();

        foreach (Camera cam in cameras)
        {
            cameraList.Insert(0, cam);
        }

        cameraNames = new string[cameraList.Count];

        for (int i = 0; i < cameraNames.Length; i++)
        {
            cameraNames[i] = cameraList[i].name;
        }
    }

    void CheckValidDirectory(string d)
    {
        if (!Directory.Exists(d))
        {
            Directory.CreateDirectory(d);
        }
    }

    void TakeScreenshot(int width, int height, int colorFormat)
    {
        captureCamera.targetTexture = new RenderTexture(width, height, 24);
        Texture2D screenshot;

        if (colorFormat == 0)
        {
            screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        }
        else if (colorFormat == 1)
        {
            screenshot = new Texture2D(width, height, TextureFormat.RGBA32, false);
        }
        else
        {
            screenshot = new Texture2D(width, height, TextureFormat.Alpha8, false);
        }

        captureCamera.Render();
        RenderTexture.active = captureCamera.targetTexture;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        CreateFile(screenshot);
    }

    void CapturePanorama(int width, int height, bool stereo)
    {
        RenderTexture cubeMapLeft = new RenderTexture(width, height, 24);
        cubeMapLeft.dimension = UnityEngine.Rendering.TextureDimension.Cube;
        RenderTexture cubeMapRight = new RenderTexture(width, height, 24);
        cubeMapRight.dimension = UnityEngine.Rendering.TextureDimension.Cube;

        RenderTexture equirect = new RenderTexture(width, height, 24);
        equirect.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;

        if (stereo)
        {
            captureCamera.stereoSeparation = 0.065f;

            captureCamera.RenderToCubemap(cubeMapLeft, 63, Camera.MonoOrStereoscopicEye.Left);
            captureCamera.RenderToCubemap(cubeMapRight, 63, Camera.MonoOrStereoscopicEye.Right);

            cubeMapLeft.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Left);
            cubeMapRight.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Right);
        }
        else
        {
            captureCamera.RenderToCubemap(cubeMapLeft);
            cubeMapLeft.ConvertToEquirect(equirect);
        }

        Texture tex = equirect;
        Texture2D tex2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
        captureCamera.Render();
        RenderTexture activeRT = RenderTexture.active;
        RenderTexture renderTexture = new RenderTexture(tex.width, tex.height, 32);
        Graphics.Blit(tex, renderTexture);

        RenderTexture.active = renderTexture;
        tex2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex2D.Apply();
        CreateFile(tex2D);
    }

    private void CreateFile(Texture2D texture)
    {
        byte[] bytes;
        switch (selectedExt)
        {
            case 0:
                bytes = texture.EncodeToJPG(imageQuality);
                break;

            case 1:
                bytes = texture.EncodeToPNG();
                break;

            case 2:
                bytes = texture.EncodeToEXR();
                break;

            case 3:
                bytes = texture.EncodeToTGA();
                break;

            default:
                bytes = texture.EncodeToJPG();
                break;
        }

        System.IO.File.WriteAllBytes(path, bytes);

        AssetDatabase.Refresh();

        captureCamera.targetTexture = null;
    }
}

public class Resolutions2D
{
    public string[] res2D = new string[] { "720p", "1080p", "2K", "4K", "8K", "Custom..." };
    public string[] ar2D = new string[] { "1:1", "4:3", "5:4", "8:5", "16:9", "16:10", "21:9", "32:9" };

    public int width;
    public int height;
    public float aspectRatio { get; private set; }

    public int SelectedRes2D = 1;
    public int SelectedAspectRatio2D = 4;

    public bool customResolution { get; private set; }

    public void SetResolution()
    {
        string[] ratios = ar2D[SelectedAspectRatio2D].Split(':');
        aspectRatio = (float.Parse(ratios[0])) / (float.Parse(ratios[1]));

        switch (SelectedRes2D)
        {
            case 0:
                height = 720;
                width = Mathf.RoundToInt(height * aspectRatio);
                customResolution = false;
                break;

            case 1:
                height = 1080;
                width = Mathf.RoundToInt(height * aspectRatio);
                customResolution = false;
                break;

            case 2:
                width = 2048;
                height = Mathf.RoundToInt(width / aspectRatio);
                customResolution = false;
                break;

            case 3:
                width = 4096;
                height = Mathf.RoundToInt(width / aspectRatio);
                customResolution = false;
                break;

            case 4:
                width = 8192;
                height = Mathf.RoundToInt(width / aspectRatio);
                customResolution = false;
                break;

            case 5:
                customResolution = true;
                break;

            default:
                width = 1280;
                height = 720;
                customResolution = false;
                break;
        }
    }
}


public class ResolutionsPano
{
    public string[] resPano = new string[] { "1K", "2K", "4K", "8K" };

    public int width;
    public int height;

    public int SelectedResPano = 2;
    public bool stereoscopic = false;

    public void SetResolution()
    {
        switch (SelectedResPano)
        {
            case 0:
                width = height = 1024;
                break;

            case 1:
                width = height = 2048;
                break;

            case 2:
                width = height = 4096;
                break;

            case 3:
                width = height = 8192;
                break;

            default:
                width = height = 4096;
                break;
        }
    }
}