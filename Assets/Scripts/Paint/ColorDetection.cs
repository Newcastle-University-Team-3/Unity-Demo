using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDetection : MonoBehaviour
{
    public Transform _colorDetectionPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        colorDetect();
    }

    public void colorDetect()
    {

        RaycastHit hitInfo;

        if (Physics.Raycast(_colorDetectionPoint.position,Vector3.down, out hitInfo, 0.20f))
        {
            //Debug.Log("DO HIT");
            //Debug.Log(hitInfo.transform.tag);
        }
        Debug.Log(transform.position);
        Debug.Log(transform.name);
    }

}
