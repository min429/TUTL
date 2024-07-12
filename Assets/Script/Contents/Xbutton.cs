using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xbutton : MonoBehaviour
{
    GameObject web;
    // Start is called before the first frame update
    public void XbutClick()
    {
        web=this.gameObject.transform.parent.gameObject;
        web.SetActive(false);
    }
}
