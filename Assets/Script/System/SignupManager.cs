using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField inputEmail;       // 유저 아이디 입력 필드
    public TMP_InputField inputPwd;      // 유저 비밀번호 입력 필드
    public TMP_InputField inputName;      // 유저 비밀번호 입력 필드
    public TextMeshProUGUI errorMessage;       // 에러 메시지 텍스트
    public Button signupButton;      // 회원가입 버튼
    public Button cancelButton;      // 취소 버튼

    private void Start()
    {
        // 버튼 클릭 리스너 등록
        signupButton.onClick.AddListener(OnSignUpButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);

        // 에러 메시지 숨기기
        errorMessage.gameObject.SetActive(false);
    }
    
    // 회원가입 버튼 클릭 이벤트를 위한 메서드
    public void OnSignUpButtonClicked()
    {
        SignUpRequest signUpRequest = new SignUpRequest
        {
            email = inputEmail.text, // 이메일 입력 필드에서 가져온 값
            password = inputPwd.text, // 패스워드 입력 필드에서 가져온 값
            nickname = inputName.text // 닉네임 입력 필드에서 가져온 값
        };

        StartCoroutine(SendSignUpRequest(signUpRequest));
    }

    public void OnCancelButtonClicked()
    {
        SceneManager.LoadScene("Login");
    }

    // SignUpRequest를 JSON으로 변환하고 서버에 전송하는 코루틴
    private IEnumerator SendSignUpRequest(SignUpRequest signUpRequest)
    {
        string json = JsonUtility.ToJson(signUpRequest);
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(Config.baseUrl + "users/signup", "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            Debug.Log(request.downloadHandler.text); // "회원가입 성공" 메시지 출력
            SceneManager.LoadScene("Login");
        }
        else
        {
            ShowErrorMessage("회원가입 실패");
            Debug.LogError("회원가입 실패: " + request.error);
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

// SignUpRequest 클래스 정의
[System.Serializable]
public class SignUpRequest
{
    public string email;
    public string password;
    public string nickname;
}