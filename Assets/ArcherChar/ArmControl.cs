using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = (float)Math.Atan2(mousePos.y- transform.position.y, mousePos.x- transform.position.x+0.5f)* Mathf.Rad2Deg ;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
    }
}
