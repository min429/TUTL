using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun
{
    public TMP_InputField chatInputField;  // 채팅 입력 필드
    public GameObject chatMessagePrefab;  // 채팅 메시지 프리팹
    public Transform contentTransform;  // 채팅 메시지가 들어갈 Content Transform
    public ScrollRect scrollRect;  // 스크롤이 적용될 Scroll Rect
    private PhotonView pv;  // PhotonView 컴포넌트
    public static bool isChatting;  // 채팅 중인지 여부를 나타내는 플래그

    void Start()
    {
        // PhotonView 초기화
        pv = GetComponent<PhotonView>();

        // 필드들이 null인지 확인하는 디버그 로그 추가
        if (chatInputField == null)
        {
            Debug.LogError("Chat Input Field is not assigned.");
        }
        if (chatMessagePrefab == null)
        {
            Debug.LogError("Chat Message Prefab is not assigned.");
        }
        if (contentTransform == null)
        {
            Debug.LogError("Content Transform is not assigned.");
        }
        if (scrollRect == null)
        {
            Debug.LogError("Scroll Rect is not assigned.");
        }
        if (pv == null)
        {
            Debug.LogError("PhotonView is not assigned.");
        }
    }

    void Update()
    {
        // 엔터 키로 메시지 전송
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnSendButtonClicked();
            chatInputField.ActivateInputField();  // 엔터 키를 누른 후 다시 포커스를 유지
        }

        // 채팅 입력 필드에 포커스가 있을 때 isChatting 플래그 설정
        isChatting = chatInputField.isFocused;
    }

    // 전송 버튼 클릭 시 호출되는 메서드
    public void OnSendButtonClicked()
    {
        if (chatInputField == null || string.IsNullOrEmpty(chatInputField.text))
        {
            return;
        }

        string message = chatInputField.text;

        if (!string.IsNullOrEmpty(message) && PhotonNetwork.InRoom)
        {
            pv.RPC("ReceiveMessage", RpcTarget.All, PhotonNetwork.NickName, message);  // 모든 플레이어에게 메시지 전송
            chatInputField.text = string.Empty;  // 입력 필드 초기화
        }
        else if (!PhotonNetwork.InRoom)
        {
            Debug.LogWarning("You must be in a room to send messages.");
        }
    }

    // RPC로 호출되어 모든 플레이어에게 메시지를 받고 처리하는 메서드
    [PunRPC]
    void ReceiveMessage(string sender, string message)
    {
        if (chatMessagePrefab == null || contentTransform == null)
        {
            return;
        }

        // 포맷된 메시지 생성
        string formattedMessage = $"<b>{sender}:</b> {message}";

        // 채팅 메시지 프리팹을 이용해 새로운 메시지 생성
        GameObject newMessage = Instantiate(chatMessagePrefab, contentTransform);

        // 생성된 메시지의 TextMeshProUGUI 컴포넌트를 가져와 포맷된 메시지 설정
        TextMeshProUGUI textComponent = newMessage.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = formattedMessage;
        }

        // 캔버스 강제 업데이트
        Canvas.ForceUpdateCanvases();

        // 스크롤 뷰를 최신 메시지가 보이도록 아래로 이동시킴
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
