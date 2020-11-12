using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected float health, damage, damageTimer;
    protected GameObject damagePopUp;
    public Sprite deadBody;
    protected Rigidbody2D player;
    protected Animator animator;
    public ParticleSystem fluidParticles;
    protected bool stopForAttack;
    protected float length;
   protected void Init()
    {
        damagePopUp = GameObject.Find("/DamagePopUp");
        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();

    }


    // Update is called once per frame
   
}
