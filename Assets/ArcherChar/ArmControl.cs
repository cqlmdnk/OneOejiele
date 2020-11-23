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
    void Update()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = (float)Math.Atan2(mousePos.y- transform.position.y, mousePos.x- transform.position.x+0.5f)* Mathf.Rad2Deg ;
        
        if((angle > 90.0f || angle <-90.0f) && !movedLeft)
        {
            transform.position = new Vector3(transform.position.x-0.15f, transform.position.y, transform.position.z);
            movedLeft = true;
        }
        else if (!(angle > 90.0f || angle < -90.0f) && movedLeft)
        {
            transform.position = new Vector3(transform.position.x +0.15f, transform.position.y, transform.position.z);
            movedLeft = false;
        }
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (transform.parent.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("character_run"))
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 109;
            this.GetComponentInChildren<SpriteRenderer> ().sortingOrder = 109;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 115;
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 120;
        }
    }
}
