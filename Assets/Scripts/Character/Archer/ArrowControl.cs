using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    // Start is called before the first frame update
    
    public ParticleSystem particles;
    bool hit = false;
    void Start()
    {
        if ( !gameObject.name.Contains("(Clone)"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

       }

    // Update is called once per frame
    void Update()
    {
        // code for direction of sprite changes with its velocity
        if (hit)
            DestroyImmediate(this.gameObject);

        else if ( GetComponent<Rigidbody2D>() != null) /// !!!!
                transform.rotation = Quaternion.Euler(0, 0,((float)Math.Atan2((double)GetComponent<Rigidbody2D>().velocity.y, (double)GetComponent<Rigidbody2D>().velocity.x))  * Mathf.Rad2Deg);


    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!gameObject.name.Contains("(Clone)"))
        {
            return;
        }
        if (col.gameObject.tag.Equals("Enemy")) 
        {
            DetachParticles();
            hit = true;

        }
        else if (col.gameObject.tag.Equals("Ground") && !hit)
        {
            DetachParticles();
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            this.transform.SetParent(GameObject.Find("Container").transform);
        }

    }
   
    
    public void DetachParticles()
    {
        // This splits the particle off so it doesn't get deleted with the parent
        particles.transform.parent = null;

        // this stops the particle from creating more bits
        particles.emissionRate = 0;

        // This finds the particleAnimator associated with the emitter and then
        // sets it to automatically delete itself when it runs out of particles
        
    }
}
