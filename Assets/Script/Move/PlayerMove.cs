using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    float haxis;
    float vaxis;

    Vector3 moveVec;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        haxis = Input.GetAxisRaw("Horizontal");
        vaxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(haxis, 0, vaxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;
    }
}
