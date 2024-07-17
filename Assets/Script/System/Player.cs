using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine && !ChatManager.isChatting) // local 플레이어이면서 채팅 중이 아닌 경우
        { 
            float x = Input.GetAxis("Horizontal") * 10f * Time.deltaTime; // 왼쪽/오른쪽 이동 // 속도: 10 // 시간에 따라 가속
            float z = Input.GetAxis("Vertical") * 10f * Time.deltaTime; // 앞/뒤 이동 // 속도: 10 // 시간에 따라 가속
            transform.Translate(x, 0, z); // 오브젝트를 현재의 위치에서 x축(좌우)과 z축(앞뒤)으로 이동시킴 (y축(상하)은 0이라서 고정)
        }
    }
}
