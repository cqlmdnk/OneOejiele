using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerControl : MonoBehaviour
{
    // Start is called before the first frame update
    private int  removedChildCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.childCount != removedChildCount)
        {
            removedChildCount = 0;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject, 10.0f);
                removedChildCount++;
            }
        }
       
        

    }
}
