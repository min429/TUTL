using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField inputEmail;       // 유저 아이디 입력 필드
    public TMP_InputField inputPwd;      // 유저 비밀번호 입력 필드
    public Button loginButton;      // 로그인 버튼
    public Button signupButotn;      // 회원가입 버튼
    public TextMeshProUGUI errorMessage;       // 에러 메시지 텍스트

    private void Start()
    {
        // 버튼 클릭 리스너 등록
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        signupButotn.onClick.AddListener(OnSignUpButtonClicked);

        // 에러 메시지 숨기기
        errorMessage.gameObject.SetActive(false);

        Debug.Log($"현재 RefreshToken: {PlayerPrefs.GetString("refreshToken")}");

        // 자동 로그인 시도
        TryAutoLogin();
    }

    private void OnLoginButtonClicked()
    {
        string email = inputEmail.text;    // 입력된 유저 아이디
        string pwd = inputPwd.text;  // 입력된 비밀번호

        StartCoroutine(LoginCoroutine(email, pwd)); // 로그인 코루틴 시작
    }

    private void OnSignUpButtonClicked()
    {
        SceneManager.LoadScene("Signup");
    }

    private IEnumerator LoginCoroutine(string email, string pwd)
    {
        string url = Config.baseUrl + "users/login";  // 로그인 API 엔드포인트 URL
        LoginRequest loginRequest = new LoginRequest(email, pwd);  // 로그인 요청 객체 생성
        string jsonRequestBody = JsonUtility.ToJson(loginRequest);  // 요청 객체를 JSON 문자열로 변환

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json"); // 요청 헤더 설정

            yield return www.SendWebRequest(); // 요청 보내고 응답 기다림

            // 서버 응답 상태 코드 확인
            if (www.responseCode == 200) // 200 OK 상태 코드일 때
            {
                string userDataJson = www.downloadHandler.text;  // 서버에서 받은 유저 데이터 JSON
                LogInResponseDto userData = JsonUtility.FromJson<LogInResponseDto>(userDataJson); // JSON을 객체로 변환

                // 로그인 성공, 유저 데이터를 싱글톤으로 관리된 User 클래스에 할당
                User.Instance.id = userData.userId;
                User.Instance.username = userData.userName;
                User.Instance.accessToken = userData.accessToken;
                User.Instance.refreshToken = userData.refreshToken;

                // 토큰 저장
                PlayerPrefs.SetString("accessToken", userData.accessToken);
                PlayerPrefs.SetString("refreshToken", userData.refreshToken);
                PlayerPrefs.Save();

                Debug.Log($"새로운 RefreshToken 저장됨: {PlayerPrefs.GetString("refreshToken")}");

                Debug.Log($"Logged in as {User.Instance.username}");

                // 다음 씬으로 이동 등 추가 로직 처리
                SceneManager.LoadScene("Jeju");
            }
            else
            {
                Debug.LogError($"Login failed with response code: {www.responseCode}");
                ShowErrorMessage("다시 시도해주세요");
            }
        }
    }


    private void TryAutoLogin()
    {
        if (PlayerPrefs.HasKey("refreshToken"))
        {
            string storedRefreshToken = PlayerPrefs.GetString("refreshToken");
            StartCoroutine(RefreshTokenCoroutine(storedRefreshToken));
        }
    }

    private IEnumerator RefreshTokenCoroutine(string refreshToken)
    {
        string url = Config.baseUrl + "tokens/create";  // 토큰 갱신 API 엔드포인트 URL
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(new byte[0]);
            www.downloadHandler = new DownloadHandlerBuffer();

            www.SetRequestHeader("Authorization", refreshToken); // 헤더에 리프레시 토큰 추가
            www.SetRequestHeader("Content-Type", "application/json"); // 요청 헤더 설정

            yield return www.SendWebRequest(); // 요청 보내고 응답 기다림

            if (www.responseCode == 201)
            {
                string userDataJson = www.downloadHandler.text;  // 서버에서 받은 유저 데이터 JSON
                LogInResponseDto userData = JsonUtility.FromJson<LogInResponseDto>(userDataJson); // JSON을 객체로 변환

                // 새로운 토큰 저장
                PlayerPrefs.SetString("accessToken", userData.accessToken);
                PlayerPrefs.SetString("refreshToken", userData.refreshToken);
                PlayerPrefs.Save();

                // 유저 정보를 갱신
                User.Instance.id = userData.userId;
                User.Instance.username = userData.userName;
                User.Instance.accessToken = userData.accessToken;
                User.Instance.refreshToken = userData.refreshToken;

                Debug.Log($"Token refreshed. Logged in as {User.Instance.username}");
                // 자동 로그인 후 다음 씬으로 이동 등 추가 로직 처리
                SceneManager.LoadScene("Jeju");
            }
            else
            {
                Debug.Log("Failed to refresh token. User needs to login again.");

                // 로그인 요청
                ShowErrorMessage("자동 로그인 실패");
            }
        }
    }

    private IEnumerator FadeOutText(TextMeshProUGUI text, float fadeDuration)
    {
        Color originalColor = text.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        text.gameObject.SetActive(false);
    }

    private void ShowErrorMessage(string message)
    {
        errorMessage.text = message;
        errorMessage.color = new Color(errorMessage.color.r, errorMessage.color.g, errorMessage.color.b, 1f); // Alpha 값을 1로 설정
        errorMessage.gameObject.SetActive(true);

        StartCoroutine(FadeOutText(errorMessage, 2f)); // 2초 동안 서서히 사라짐
    }
}

[System.Serializable]
public class LoginRequest
{
    public string email;    // 로그인 요청 아이디
    public string password;   // 로그인 요청 비밀번호

    public LoginRequest(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}

[System.Serializable]
public class LogInResponseDto
{
    public long userId;          // 유저 아이디
    public string userName;      // 유저 이름
    public string accessToken;   // 접근 토큰
    public string refreshToken;  // 리프레시 토큰
}
