using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;  // Player 프리팹을 GameObject로 변경

    // Start is called before the first frame update
    void Start()
    {
        if (User.Instance != null && !string.IsNullOrEmpty(User.Instance.username))
        {
            PhotonNetwork.NickName = User.Instance.username; // 유저 이름으로 플레이어 이름 설정
        }
        else
        {
            PhotonNetwork.NickName = "Guest"; // 기본값 설정
        }

        // Photon 서버 연결
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // 룸 입장(서버 연결됐다는 뜻)
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room successfully");

        // PhotonNetwork.Instantiate는 Resources 폴더 내 프리팹 이름을 사용해야 함
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }
}
