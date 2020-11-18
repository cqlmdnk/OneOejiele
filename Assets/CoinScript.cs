using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float bounceRange;
    bool dir = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newpos;
        if (dir)
        {
            bounceRange += 0.005f;
            newpos = new Vector3(transform.position.x, transform.position.y + 0.005f, 0.0f);
        }
        else
        {
            bounceRange -= 0.005f;
            newpos = new Vector3(transform.position.x, transform.position.y - 0.005f, 0.0f);
        }
      
        transform.position = newpos;
        if (bounceRange < -0.2f || bounceRange > 0.2f)
            dir = !dir;
    }
}
