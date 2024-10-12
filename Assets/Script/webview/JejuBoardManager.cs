using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JejuBoardManager : MonoBehaviour
{
    private WebViewObject webViewObject;

    void Start()
    {
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
}
