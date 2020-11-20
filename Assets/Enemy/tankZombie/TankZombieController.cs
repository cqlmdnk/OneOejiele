using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankZombieController : Enemy
{
    // Start is called before the first frame update
    
    bool isParticlesAnimating = false;
    public ParticleSystem explosion, throwUp;
    float particleTimer = -0.1f;
    void Start()
    {
        Init();
        explosion.Stop();
        health = 200;
        length = UnityEngine.Random.Range(-1, 1);
        if (length < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;
       
    }

    // Update is called once per frame
    void Update()
    {   
        if(health < 0.0f)
        {
            
            explosion.transform.parent = null;
            animator.SetBool("isDying", true);
            Destroy(this.gameObject, 3.0f);
            
        }
        
        if(UnityEngine.Random.Range(0, 100) < 1.0f   &&  SightCheck()  && particleTimer <0)
        {
            throwUp.Play();
            isParticlesAnimating = true;
            particleTimer = 3.0f;
        }
        else if(isParticlesAnimating)
        {
            particleTimer -= Time.deltaTime;
        }

        if (length > -0.01 && length < 0.001)
        {
            length = UnityEngine.Random.Range(-5, 5);
            if (length < 0)
            {

                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            else
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);

            }
        }
        Vector3 move = new Vector3(((0.01f) * Math.Sign(length) + length / 100), 0, 0.0f);
        transform.position = transform.position + 3 * move * Time.deltaTime;
        length -= move.x * Time.deltaTime;

    }

    void OnCollisionEnter2D(Collision2D col)
    {

        base.OnCollisionEnter2D(col);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            stopForAttack = false;
            animator.SetBool("attack", false);
        }

    }

    void OnDestroy() // creating dead body
    {
        explosion.Play();
        Destroy(explosion.gameObject, 3.0f);
        base.deleteChildrenPhysics();

        base.OnBeforeDestroy();
    }

    
}
