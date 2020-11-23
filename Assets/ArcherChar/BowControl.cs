using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.transform.parent.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("character_run"))
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 109;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 120;
        }
    }
}
