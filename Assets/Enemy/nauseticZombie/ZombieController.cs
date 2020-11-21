using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZombieController : Enemy
{
    // Start is called before the first frame update
    
    public float zombieRecoverTime;
   
    void Start()
    {
        Init();
        health = 100;
        Physics2D.queriesStartInColliders = false;
        length = UnityEngine.Random.Range(-3, 3);
        if (length < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;
        damageTimer = zombieRecoverTime;


    }

    // Update is called once per frame
    void Update()
    {

        if(health <= 0)
        {
            animator.SetBool("dead", true);
            DestroyImmediate(this.gameObject.GetComponent<Rigidbody2D>());
            DestroyImmediate(this.gameObject.GetComponent<BoxCollider2D>());
            base.deleteChildrenPhysics();
            Destroy(this.gameObject,5.0f);
        }
        else
        {
            if (length > -0.01 && length < 0.001)
            {
                length = UnityEngine.Random.Range(-5, 5);
                if (length < 0)
                {

                    GetComponent<SpriteRenderer>().flipX = true;
                }

                else
                {

                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {
                float harmonicForce = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % (animator.GetCurrentAnimatorStateInfo(0).length * 2);
                transform.position = new Vector3(transform.position.x + (harmonicForce / 40.0f) - 0.025f, transform.position.y, transform.position.z);
            }


            if (damage != 0)
            {
                damageTimer -= Time.deltaTime;
                if (damageTimer < 0)
                {
                    damageEnd();
                    damageTimer = 0.717f;
                }
            }



            Vector3 start = transform.position;
            Vector3 direction = player.transform.position - transform.position;

            float aggro = 0.0f;

            //ray casting 90 degrees in 10 segments with respect to facing
            if (SightCheck())
            {
                aggro = 0.4f;
            }

            if (!stopForAttack)
            {
                Vector3 move = new Vector3(((0.1f + aggro) * Math.Sign(length) + length / 30), 0, 0.0f);
                transform.position = transform.position + 5 * move * Time.deltaTime;
                length -= move.x * Time.deltaTime;
            }
        }

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {

            stopForAttack = true;
            animator.SetBool("attack", true);
            
        }
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
    void damageEnd()
    {
        stopForAttack = false;
        animator.SetBool("takingDamage", false);
        
        
        damage = 0;
    }
    void OnDestroy() // creating dead body
    {
        
        base.OnBeforeDestroy();
    }

}
