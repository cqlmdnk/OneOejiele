using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum State
{
    Idle,
    Run,
    Melee,
    Dash,
    Jump,
    Fall,
    Attack,
    AimUp
}
public class Controller : MonoBehaviour
{
    public ParticleSystem dust;
    public GameObject cam;
    public Rigidbody2D archer;
    public Animator animator;
    public GameObject arrow;
    State archer_state;
    bool facingRight;
    bool draw = false;
    float dash_time1;
    float dash_time2;
    bool isTap = false;
    bool dashDir = false;
    // Start is called before the first frame update
    void Start()
    {
        archer_state = State.Idle;
        UpdateState(archer_state);
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {


        /* Run and Shoot animation will be added*/
        clearAnim();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("character_draw") && draw)
        {
            if (facingRight)
            {
                GameObject _arrow = Instantiate(arrow, transform.position, Quaternion.identity);
                Rigidbody2D arrow_body = _arrow.GetComponent<Rigidbody2D>();
                arrow_body.velocity = new Vector2(20.0f, 0.0f);
            }
            else
            {
                GameObject _arrow = Instantiate(arrow, transform.position, Quaternion.identity);
                Rigidbody2D arrow_body = _arrow.GetComponent<Rigidbody2D>();
                arrow_body.velocity = new Vector2(-20.0f, 0.0f);
                arrow_body.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            draw = false;
           
        }

        if (archer_state != State.Idle )
        {
            archer_state = State.Idle;
            UpdateState(archer_state);
        }










        float horizontalInput = Input.GetAxis("Horizontal");
        
        Vector3 move = new Vector3(horizontalInput, 0, 0.0f);
        if ((cam.transform.position.x - archer.transform.position.x > 4.0f || cam.transform.position.x - archer.transform.position.x < -1.0f)) // camera movement if player on edge of defined rectangle
        {
            cam.transform.position = new Vector3(cam.transform.position.x + 5 * (-cam.transform.position.x+ archer.transform.position.x)/4 * Time.deltaTime, cam.transform.position.y, cam.transform.position.z);
            
        }

        


        if (horizontalInput != 0.0f)
        {
            archer_state = State.Run;
            UpdateState(archer_state);
            MoveHorizontal(move);
            if(archer.velocity.y <0.05f && archer.velocity.y > -0.05f)
                dust.Play();


        }
        

        if (Input.GetKeyDown(KeyCode.W))
        {
            
            archer_state = State.Jump;
            UpdateState(archer_state);
            archer.velocity = new Vector2(0.0f, 10.0f);
            dust.Stop();
        }


        if (Input.GetKeyDown(KeyCode.Space) && archer_state != State.Dash)
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
                    animator.SetBool("dash", true);
                    archer_state = State.Dash;
                    if(facingRight)
                        archer.velocity = new Vector2(5.0f, 0.0f);
                    else
                        archer.velocity = new Vector2(-5.0f, 0.0f);
                }
            }

            else
            {
                dash_time2 = Time.time;
                dashDir = facingRight;
                isTap = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            archer_state = State.Attack;
            UpdateState(archer_state);
            Vector3 turn = new Vector3(-0.1f, 0.0f, 0.0f);
            MoveHorizontal(turn);
            
            draw = true;

            // Avoid any reload.

            
            
            
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            archer_state = State.Attack;
            UpdateState(archer_state);
            Vector3 turn = new Vector3(0.1f, 0.0f, 0.0f);
            MoveHorizontal(turn);

            
            draw = true;
            // Avoid any reload.

           
            
        }




    }

    void MoveHorizontal(Vector3 move)
    {

        if (facingRight && move[0] < 0.0f)
        {
            facingRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (!facingRight && move[0] > 0.0f)
        {
            facingRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        archer.transform.position = archer.transform.position + 5 * move * Time.deltaTime;
    }

    void UpdateState(State state)
    {
        switch(state){
            case State.Idle:
                animator.SetBool("idle", true);
                animator.SetBool("run", false);
                animator.SetBool("jump", false);
                animator.SetBool("attack", false);
                animator.SetBool("dash", false);
                animator.SetBool("melee", false); 
                break;
            case State.Run:
                animator.SetBool("idle", false);
                animator.SetBool("run", true );
                animator.SetBool("jump", false);
                animator.SetBool("attack", false);
                animator.SetBool("dash", false);
                animator.SetBool("melee", false);
                break;
            case State.Jump:
                animator.SetBool("idle", false);
                animator.SetBool("run", false);
                animator.SetBool("jump", true);
                animator.SetBool("attack", false);
                animator.SetBool("dash", false);
                animator.SetBool("melee", false);
                break;
            case State.Dash:
                animator.SetBool("idle", false);
                animator.SetBool("run", false);
                animator.SetBool("jump", false);
                animator.SetBool("attack", false);
                animator.SetBool("dash", true);
                animator.SetBool("melee", false);
                break;
            case State.Melee:
                animator.SetBool("idle", false);
                animator.SetBool("run", false);
                animator.SetBool("jump", false);
                animator.SetBool("attack", false);
                animator.SetBool("dash", false);
                animator.SetBool("melee", true);
                break;
            case State.Attack:
                animator.SetBool("idle", false);
                animator.SetBool("run", false);
                animator.SetBool("jump", false);
                animator.SetBool("attack", true);
                animator.SetBool("dash", false);
                animator.SetBool("melee", false);
                break;
            case State.AimUp:
                animator.SetBool("idle", false);
                animator.SetBool("run", false);
                animator.SetBool("jump", false);
                animator.SetBool("attack", false);
                animator.SetBool("dash", false);
                animator.SetBool("melee", false);
                break;
        }
    }
    void clearAnim()
    {
        animator.SetBool("idle", false);
        animator.SetBool("run", false);
        animator.SetBool("jump", false);
        animator.SetBool("attack", false);
        animator.SetBool("dash", false);
        animator.SetBool("melee", false);
        
    }
    
}
