using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankZombieController : Enemy
{
    // Start is called before the first frame update
    
    bool isParticlesAnimating = false;
   
    float particleTimer = -0.1f;
    void Start()
    {
        Init();

        health = 400;
        length = UnityEngine.Random.Range(-1, 1);
        if (length < 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 180, 0);
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if(UnityEngine.Random.Range(0, 100) < 1.0f  && particleTimer <0)
        {
            GetComponent<ParticleSystem>().Play();
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
       
        if (col.gameObject.tag.Equals("Throwable"))
        {
         
            health -= 25.0f;
            damage = 25;
            Vector3 popUpPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            GameObject _popUp = Instantiate(damagePopUp, popUpPos, Quaternion.identity);
            _popUp.transform.SetParent(this.gameObject.transform);
            _popUp.SetActive(true);
            fluidParticles.gameObject.SetActive(true);
            fluidParticles.Play();
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            stopForAttack = false;
            animator.SetBool("attack", false);
        }

    }
}
