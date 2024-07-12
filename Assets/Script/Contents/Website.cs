using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Website : MonoBehaviour
{
    public GameObject web;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void butClick()
    {
        web.SetActive(true);
        this.transform.parent.gameObject.SetActive(false);
    }
}
