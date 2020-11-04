﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update
    float length;
    public Rigidbody2D player;
    public Animator animator;
    private bool stopForAttack;
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        length = UnityEngine.Random.Range(-2, 2);
        if (length < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (length > -0.01 && length < 0.001)
        {
            length = UnityEngine.Random.Range(-5, 5);
            if (length < 0)
            {

                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);

            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
        {
            float harmonicForce = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % (animator.GetCurrentAnimatorStateInfo(0).length * 2);
            transform.position = new Vector3(transform.position.x + (harmonicForce / 40.0f) - 0.025f, transform.position.y, transform.position.z);
        }







        Vector3 start = transform.position;
        Vector3 direction = player.transform.position - transform.position;



        //ray casting 90 degrees in 10 segments with respect to facing
        List<RaycastHit2D> sighttest = new List<RaycastHit2D>();
        if (length < 0)
        {

            for (int i = 0; i < 11; i++)
            {
                sighttest.Add(Physics2D.Raycast(transform.position, new Vector3(-1.0f + (i / 10.0f), (i / 10.0f), 0.0f), 6.0f));
            }

        }

        else
        {

            for (int i = 0; i < 10; i++)
            {
                sighttest.Add(Physics2D.Raycast(transform.position, new Vector3((i / 10.0f), 1.0f-(i / 10.0f), 0.0f), 6.0f));
            }
        }
        float aggro = 0.0f;



        foreach(RaycastHit2D testc in sighttest){
            if (testc.collider != null)
            {
                if (testc.collider.tag == "Player")
                {

                    aggro = 0.4f;
                }

            }
        }  
        
        if (!stopForAttack)
        {
            Vector3 move = new Vector3(((0.1f + aggro) * Math.Sign(length) + length / 30), 0, 0.0f);
            transform.position = transform.position + 5 * move * Time.deltaTime;
            length -= move.x * Time.deltaTime;
        }
       
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            stopForAttack = true;
            animator.SetBool("attack", true);
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
