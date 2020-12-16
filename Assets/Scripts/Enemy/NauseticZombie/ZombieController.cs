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
    private bool lockedTarget = false;

    void Start()
    {
        Init();
        health = 100;
        Physics2D.queriesStartInColliders = false;

        length = UnityEngine.Random.Range(-2, 2);
        if (length < 0)
        {
           transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
           
        damageTimer = zombieRecoverTime;


    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
        {
            animator.SetBool("dead", true);
            DestroyImmediate(this.gameObject.GetComponent<Rigidbody2D>());
            DestroyImmediate(this.gameObject.GetComponent<BoxCollider2D>());

            Destroy(this.gameObject, 5.0f);
        }
        else
        {
            if (length > -0.01 && length < 0.001)
            {
                length = UnityEngine.Random.Range(-5, 5);
                
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {
                //float harmonicForce = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % (animator.GetCurrentAnimatorStateInfo(0).length * 2);
                //transform.position = new Vector3(transform.position.x + (harmonicForce / 40.0f) - 0.025f, transform.position.y, transform.position.z);
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

            /*
             if raycast angle is greater than 80(maybe 70 ) then it should turn other direction while assuming the character is jumped off him.
             
             
             
             */
            

            //ray casting 90 degrees in 10 segments with respect to facing
            GameObject seenObject = SightCheck();
            
            lockedTarget = seenObject == null ? false : true;
           

            if (!stopForAttack && !lockedTarget)
            {
                Vector3 move = new Vector3(((0.1f ) * Math.Sign(length) + length / 30), 0, 0.0f);
                transform.position = transform.position + 5 * move * Time.deltaTime;
                length -= move.x * Time.deltaTime;
                if(move.x < 0)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }
            else if (!stopForAttack && lockedTarget)
            {
                Vector3 move = new Vector3((0.1f ) * Math.Sign(seenObject.transform.position.x - transform.position.x) + seenObject.transform.position.x - transform.position.x, 0, 0);
                transform.position = transform.position + 4 * move.normalized * Time.deltaTime;
                if (move.x < 0)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                length = move.x > 0 ? 1.0f : -1.0f;


            }
        }

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {

            stopForAttack = true;
            lockedTarget = false;
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
