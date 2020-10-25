using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    // Start is called before the first frame update
    float particleTime = 5.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Renderer>().isVisible &&(gameObject.name.Contains("(Clone)")))
        {
            particleTime -= Time.deltaTime;
            if (particleTime < 0)
            {
                Destroy(this.gameObject);
            }
            
        }
        
    }
}
