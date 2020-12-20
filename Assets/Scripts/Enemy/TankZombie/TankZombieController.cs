using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankZombieController : Enemy
{
    // Start is called before the first frame update
    
    bool isParticlesAnimating = false;
    public ParticleSystem throwUp;
    float particleTimer = -0.1f;
    void Start()
    {
        Init();
        explosion.Stop();
        health = 200;
        length = UnityEngine.Random.Range(-1, 1);
        if (length < 0)
        {
            transform.Find("ThrowUp").transform.localRotation = Quaternion.Euler(0, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
        }


        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.Find("ThrowUp").transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
            
       
    }

    // Update is called once per frame
    void Update()
    {
        if (HandleDeath())
            return;
        HandleThrowUp();
        HandleNewPath();
        HandleMovement();

    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(((0.1f) * Math.Sign(length) + length / 100), 0, 0.0f);
        DetermineDirection(move.x);
        transform.position = transform.position + 2 * move * Time.deltaTime;
        length -= 2 * move.x * Time.deltaTime;
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

    private void DetermineDirection(float moveX)
    {
        if (moveX < 0)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            
        }
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

        base.OnBeforeDestroy();
    }

  



}
