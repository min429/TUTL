using UnityEngine;
using System.Collections;

public class JejuBoardManager : MonoBehaviour
{
    private WebViewObject webViewObject;

    void Start()
    {
        // WebView 초기화 및 설정
        InitializeWebView();
    }

    // WebView 초기화 및 설정
    private void InitializeWebView()
    {
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init(HandleWebViewMessage);
        webViewObject.SetMargins(0, 0, 0, 0);
        webViewObject.SetVisibility(true);
        webViewObject.LoadURL("http://192.168.0.2:8080/api/boards/thumbnail/SPOT?area=제주도");
    }

    private void HandleWebViewMessage(string msg)
    {
        Debug.Log($"Message from WebView: {msg}");
        if (msg == "openCamera")
        {
            StartCoroutine(CapturePhoto());
        }
        else if (msg == "openGallery")
        {
            OpenGallery();
        }
    }

    // 사진 캡처 및 Base64 인코딩 후 JavaScript로 전송
    private IEnumerator CapturePhoto()
    {
        if (!NativeCamera.IsCameraBusy())
        {
            NativeCamera.Permission permission = NativeCamera.RequestPermission(true);
            if (permission == NativeCamera.Permission.Granted)
            {
                yield return NativeCamera.TakePicture((path) =>
                {
                    if (path != null)
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(path);  // 파일 읽기
                        string base64Image = System.Convert.ToBase64String(imageBytes);  // Base64로 인코딩
                        webViewObject.EvaluateJS($"window.addImageFromUnity('data:image/png;base64,{base64Image}');");
                    }
                });
            }
            else
            {
                Debug.LogWarning("Camera permission not granted.");
            }
        }
        else
        {
            Debug.LogWarning("Camera is busy.");
        }
    }


    // 갤러리 열기 로직 (Native Gallery 사용)
    private void OpenGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);  // 파일 읽기
                string base64Image = System.Convert.ToBase64String(imageBytes);  // Base64로 인코딩
                webViewObject.EvaluateJS($"window.addImageFromUnity('data:image/png;base64,{base64Image}');");
            }
        });
    }
}
