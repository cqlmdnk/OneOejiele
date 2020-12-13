using Assets.ArcherChar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;


public class ArcherController : CharacterController
{
    
    
    
    public GameObject arrow;

    public int arrow_count = 30;
    State archer_state;

  
    bool draw = false, drawRight = true;
    float drawingTime = 1.5f;
    bool drawedToOpposite;

    float dash_time1;
    float dash_time2;
    bool isTap = false;
    bool dashDir = false;

    


    void Start()
    {
        base.Start();
        UpdateState(archer_state);
        

    }


    void FixedUpdate()
    {
        


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
        {
            faceMe(true);

        }
        else
        {
            faceMe(false);
        }
        m_damageAssesTime -= Time.deltaTime;
        if (m_damageAssesTime <= 0)
        {
            assesDamage();
        }
        if (!char_animator.GetCurrentAnimatorStateInfo(0).IsName("character_draw") && draw && !Input.GetMouseButton(0)) // piece that arrow instantiated
        {
            Vector3 transPos = GameObject.Find("Archer_bow").transform.position;
            //Debug.Log("Bıraktım");
            float angle = (float)Math.Atan2(mousePos.y - transPos.y, mousePos.x - transPos.x) * Mathf.Rad2Deg;

            float arrowDrop;
            if (drawedToOpposite)
            {
                arrowDrop = -0.01f + UnityEngine.Random.Range(-0.5f, 0.5f);
                drawedToOpposite = false;
            }
            else
                arrowDrop = -0.01f;


            faceMe(drawRight);

            GameObject _arrow = Instantiate(arrow, transPos, Quaternion.AngleAxis(angle, Vector3.forward));
            _arrow.SetActive(true);
            Rigidbody2D arrow_body = _arrow.GetComponent<Rigidbody2D>();
            Vector3 veloctiy3d = Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3((float)Math.Log((double)drawingTime, 2.0) * 20, arrowDrop, 0);
            arrow_body.velocity = (new Vector2(veloctiy3d.x, veloctiy3d.y));

            arrow_count--;
            drawingTime = 1.5f;

            char_animator.enabled = true;
            draw = false;
        }
        else if (draw && Input.GetMouseButton(0))
        {
            if (!char_animator.GetCurrentAnimatorStateInfo(0).IsName("character_draw"))
                char_animator.enabled = false;
            //Debug.Log("Yayı geriyorum");
            drawingTime += Time.deltaTime;
            //Debug.Log(drawingTime);
        }

        if (archer_state != State.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (char_body.velocity.y < 0.02 && char_body.velocity.y > -0.02)
            {
                archer_state = State.Idle;
                UpdateState(archer_state);
            }
            else
            {
                archer_state = State.OnAir;
                UpdateState(archer_state);
            }

        }

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(horizontalInput, 0, 0.0f); // get input for horizontal movement


        if ((horizontalInput > 0 && facingRight) || (horizontalInput < 0 && !facingRight) || !draw) // if direction is opposite do not move until drwaing animation ends
        {
            if (draw)
            {

                //arrow is drawed same direction of facing
            }
            if (horizontalInput != 0.0f)
            {
                archer_state = State.Run;
                if (onGround) // if character is on ground animation can change
                {
                    UpdateState(archer_state);
                }
                MoveHorizontal(move); // although animation still fall or jump character can move on air like other platformers

            }
        }
        else
        {
            drawedToOpposite = true;
        }


        if (Input.GetKeyDown(KeyCode.W) && onGround) // jumping if character on ground or on something concrete
        {

            archer_state = State.Jump;
            UpdateState(archer_state);
            char_body.velocity = new Vector2(0.0f, 10.0f);

        }

        if (Input.GetKeyDown(KeyCode.Space) && archer_state != State.Dash) // melee attack will be expanded
        {
            archer_state = State.Melee;
            UpdateState(archer_state);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) // double tap dash control
        {
            if (isTap == true && dashDir == facingRight)
            {
                dash_time1 = Time.time;
                isTap = false;

                if (dash_time1 - dash_time2 < 0.4f)
                {
                    char_animator.SetBool("dash", true);
                    archer_state = State.Dash;
                    if (facingRight)
                        char_body.velocity = new Vector2(5.0f, 0.0f); // velocity gives deeling like quick spin like on dash
                    else
                        char_body.velocity = new Vector2(-5.0f, 0.0f);
                }
            }
            else
            {
                dash_time2 = Time.time;
                dashDir = facingRight;
                isTap = true;
            }
        }
        if (Input.GetMouseButtonDown(0) && !draw && arrow_count > 0) // drawing starts from here, until drawing animation ends there will be no arrow
        {
            //Debug.Log("Oku aldım");
            archer_state = State.Attack;
            UpdateState(archer_state);
            faceMe(true);

            if (mousePos.x > transform.position.x)
            {
                drawRight = true;

            }
            else
            {
                drawRight = false;
            }

            draw = true;
        }

    }

   

    void UpdateState(State state) // changing animation // clearing all animation before change
    {
        clearAnim();
        switch (state)
        {
            case State.Idle:
                char_animator.SetBool("idle", true);
                break;
            case State.Run:
                char_animator.SetBool("run", true);
                break;
            case State.Jump:
                char_animator.SetBool("jump", true);
                break;
            case State.Dash:
                char_animator.SetBool("dash", true);
                break;
            case State.Melee:
                char_animator.SetBool("melee", true);
                break;
            case State.Attack:
                char_animator.SetBool("attack", true);
                break;
            case State.OnAir:
                char_animator.SetBool("onAir", true);
                break;
        }
    }
    void clearAnim() // clearing animation parameters except onGround
    {
        char_animator.SetBool("idle", false);
        char_animator.SetBool("run", false);
        char_animator.SetBool("jump", false);
        char_animator.SetBool("attack", false);
        char_animator.SetBool("dash", false);
        char_animator.SetBool("melee", false);
        char_animator.SetBool("onAir", false);

    }
    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag.Equals("Ground"))// hit ground
        {
            onGround = true;
            char_animator.SetBool("onGround", true);
            char_animator.SetBool("onAir", false);
        }



    }
    void OnCollisionExit2D(Collision2D col) // exit from ground
    {

        if (col.gameObject.tag.Equals("Ground"))
        {
            onGround = false;
            char_animator.SetBool("onGround", false);
            char_animator.SetBool("onAir", true);

        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Enemy Triggered Collider");
            if (col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {

               m_damage+= 1.0f;
                transform.GetChild(0).gameObject.SetActive(true);
                Debug.Log("Damage added. Total_damage: " + m_damage.ToString());
                
            }
        }
    }
   
    



    void OnParticleCollision(GameObject other)
    {
        
    }

    
}
