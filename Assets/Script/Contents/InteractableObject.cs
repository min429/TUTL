using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    public GameObject InteractUI;
    public GameObject self;
    public TextMeshProUGUI textMeshPro;
    public GameObject website;
    private bool isPlayerInRange = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }
    private void Start()
    {
        self = this.gameObject;
        textMeshPro = InteractUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Uiactive();
            Debug.Log("Player is in range");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Uideactive();
            Debug.Log("Player is out of range");
        }
    }

    public void Interact()
    {
        if (isPlayerInRange)
        {
            // 여기에 원하는 상호작용 동작을 추가합니다. 예를 들어 UI 띄우기
            website.SetActive(true);
            InteractUI.SetActive(false);    
            Debug.Log("Interacted with the object");
        }
    }

    void Uiactive()
    {
        InteractUI.SetActive(true);
        textMeshPro.text = this.self.name.ToString() + ": Interact with Key 'F'";
    }

    void Uideactive()
    {
        InteractUI.SetActive(false);
    }
}
