using UnityEngine;
<<<<<<< Updated upstream
=======
using System.Collections;
>>>>>>> Stashed changes

public class JejuBoardManager : MonoBehaviour
{
    private WebViewObject webViewObject;

    void Start()
    {
<<<<<<< Updated upstream
        Debug.Log("Starting WebView initialization.");

        // WebViewObject 생성
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

        if (webViewObject != null)
        {
            Debug.Log("WebViewObject successfully created.");
        }
        else
        {
            Debug.LogError("WebViewObject creation failed.");
        }

        // WebView 초기화 및 에러 처리 콜백 추가
        webViewObject.Init(
            (msg) => {
                Debug.Log($"Message from WebView: {msg}");
            },
            null,
            (msg) => {
                Debug.Log($"WebView Error: {msg}");
            },
            (msg) => {
                Debug.Log($"HTTP Error: {msg}");
            },
            (msg) => {
                Debug.Log($"SSL Error: {msg}");  // SSL 오류를 처리하는 콜백
            }
        );

        Debug.Log("WebView initialized.");

        // 웹뷰 크기 설정 (왼쪽, 상단, 오른쪽, 하단 여백)
        webViewObject.SetMargins(0, 0, 0, 0);
        webViewObject.SetVisibility(true);

        Debug.Log("Loading URL...");

        // URL 불러오기 (포트 번호 수정된 주소)
        webViewObject.LoadURL("http://192.168.0.2:8080/api/boards/thumbnail/SPOT?area=제주도&user_page=1&admin_page=1");

        Debug.Log("URL load triggered.");
    }
=======
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
>>>>>>> Stashed changes
}
