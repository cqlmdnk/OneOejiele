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

        

        damageTimer = zombieRecoverTime;


    }

    // Update is called once per frame
    protected override void Update()
    {


        base.Update();
        /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
        {
            //float harmonicForce = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % (animator.GetCurrentAnimatorStateInfo(0).length * 2);
            //transform.position = new Vector3(transform.position.x + (harmonicForce / 40.0f) - 0.025f, transform.position.y, transform.position.z);
        }
        */



    }
    void OnDestroy() // creating dead body
    {

        base.OnBeforeDestroy();
    }



}
