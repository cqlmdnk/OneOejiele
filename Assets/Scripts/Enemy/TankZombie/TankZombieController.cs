using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankZombieController : EnemyController
{
    // Start is called before the first frame update
    
    bool isParticlesAnimating = false;
    public ParticleSystem throwUp;
    float particleTimer = -0.1f;
    protected override void Awake()
    {
        base.Awake();
        explosion.Stop();
        health = 200;
        
            
       
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        HandleThrowUp();
        

    }

   

    

    private void HandleThrowUp()
    {
        if (UnityEngine.Random.Range(0, 100) < 1.0f && SightCheck() && particleTimer < 0)
        {
            throwUp.Play();
            isParticlesAnimating = true;
            particleTimer = 3.0f;
        }
        else if (isParticlesAnimating)
        {
            particleTimer -= Time.deltaTime;
        }
    }


    protected override void OnCollisionEnter2D(Collision2D col)
    {

        base.OnCollisionEnter2D(col);
    }

 

    void OnDestroy() // creating dead body
    {
        explosion.Play();
        Destroy(explosion.gameObject, 3.0f);

        base.OnBeforeDestroy();
    }

  



}
