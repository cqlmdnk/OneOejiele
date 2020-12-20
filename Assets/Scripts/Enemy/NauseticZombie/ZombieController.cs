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

        if (HandleDeath())
            return;



        HandleNewPath();
        /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
        {
            //float harmonicForce = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % (animator.GetCurrentAnimatorStateInfo(0).length * 2);
            //transform.position = new Vector3(transform.position.x + (harmonicForce / 40.0f) - 0.025f, transform.position.y, transform.position.z);
        }
        */

        HandleDamageAssesment();
        HandleMovement();

    }

    private void HandleMovement()
    {
        GameObject seenObject = SightCheck();

        lockedTarget = seenObject == null ? false : true;

        if (!stopForAttack)
        {
            if (!lockedTarget)
            {
                Vector3 move = new Vector3(((0.1f) * Math.Sign(length) + length / 30), 0, 0.0f);
                transform.position = transform.position + 5 * move * Time.deltaTime;
                length -= move.x * Time.deltaTime;
                DetermineDirection(move.x);
            }
            else
            {
                Vector3 move = new Vector3((0.1f) * Math.Sign(seenObject.transform.position.x - transform.position.x) + seenObject.transform.position.x - transform.position.x, 0, 0);
                transform.position = transform.position + 4 * move.normalized * Time.deltaTime;
                DetermineDirection(move.x);
                length = move.x > 0 ? 1.0f : -1.0f;


            }
        }
    }

    private void DetermineDirection(float moveX)
    {
        if (moveX < 0)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            if (this.transform.childCount > 1)
            {
                transform.FindChild("DamagePopUp(Clone)").transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (this.transform.childCount > 1)
            {
                transform.FindChild("DamagePopUp(Clone)").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("EnemyTarget"))
        {

            stopForAttack = true;
            lockedTarget = false;
            animator.SetBool("attack", true);

        }
        base.OnCollisionEnter2D(col);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("EnemyTarget"))
        {
            stopForAttack = false;
            animator.SetBool("attack", false);
        }

    }
    
    void OnDestroy() // creating dead body
    {

        base.OnBeforeDestroy();
    }



}
