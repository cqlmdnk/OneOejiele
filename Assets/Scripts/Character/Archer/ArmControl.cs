using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
    // Start is called before the first frame update
    private bool movedLeft  = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleArmAngle();
        HandleLayers();
    }

    private void HandleArmAngle()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = (float)Math.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x + 0.5f) * Mathf.Rad2Deg;

        if (!(angle > 90.0f || angle < -90.0f) && !movedLeft)
        {
            //transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y, transform.position.z);
            movedLeft = true;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            
            
        }
        else if ((angle > 90.0f || angle < -90.0f) && movedLeft)
        {
            //transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y, transform.position.z);
            movedLeft = false;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            
        }
        angle = (angle > 90.0f || angle < -90.0f) ? -angle : angle;
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
    }
    private void HandleLayers()
    {
        if (transform.parent.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("character_run"))
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 109;
            this.GetComponentInChildren<SpriteRenderer>().sortingOrder = 109;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 115;
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 120;
        }
    }
}
