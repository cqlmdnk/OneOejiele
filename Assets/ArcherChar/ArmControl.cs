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
