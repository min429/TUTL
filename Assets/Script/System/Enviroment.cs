using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    public Material dayMat;
    public Material nightMat;
    public GameObject day;
    public GameObject night;
    // Start is called before the first frame update
    public Color dayfog;
    public Color nightfog;
    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 1f);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(5, 5, 80, 20), "Day"))
        {
            RenderSettings.skybox = dayMat;
            day.SetActive(true);
            night.SetActive(false);  
        }
        if (GUI.Button(new Rect(5, 35, 80, 20), "Night"))
        {
            RenderSettings.skybox = nightMat;
            day.SetActive(false);
            night.SetActive(true);
        }
    }

}
