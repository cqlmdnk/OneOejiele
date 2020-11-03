using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    // Start is called before the first frame update
    float particleTime = 5.0f;
    public ParticleSystem particles;
    void Start()
    {
        if ( !gameObject.name.Contains("(Clone)"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

       }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Renderer>().isVisible &&(gameObject.name.Contains("(Clone)")))
        {
            particleTime -= Time.deltaTime;
            if (particleTime < 0)
            {
                DetachParticles();


                Destroy(this.gameObject);
            }
            
        }
       
            
        
        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Enemy"))
        {

            DetachParticles();
            Destroy(this.gameObject);
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
