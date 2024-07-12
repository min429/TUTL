using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public FixedJoystick joystick;
    public float speed;
    float haxis;
    float vaxis;

    float joyhaxis;
    float joyvaxis;
    public float interactionRange = 10f; // 상호작용 범위
    public LayerMask interactableLayer; // 상호작용할 레이어

    Vector3 moveVec;

    Vector3 moveJoyvec;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        haxis = Input.GetAxisRaw("Horizontal");
        vaxis = Input.GetAxisRaw("Vertical");

        joyhaxis = joystick.Horizontal;
        joyvaxis = joystick.Vertical;

        moveVec = new Vector3(haxis, 0, vaxis).normalized;

        moveJoyvec = new Vector3(joyhaxis, 0, joyvaxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;
        transform.position += moveJoyvec * speed * Time.deltaTime;
    }

    void CheckForInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);

        foreach (Collider collider in hitColliders)
        {
            InteractableObject interactableObject = collider.GetComponent<InteractableObject>();
            if (interactableObject != null)
            {
                interactableObject.Interact();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

}
