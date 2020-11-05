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
    OnAir,
    Fall,
    Attack,
    AimUp
}
public class Controller : MonoBehaviour
{
    public Transform healthBar;
    public ParticleSystem dust;
    public Rigidbody2D archer;
    public Animator animator;
    public GameObject arrow;
    State archer_state;
    public bool facingRight;
    bool draw = false, drawRight = true;
    float dash_time1;
    float dash_time2;
    bool isTap = false;
    bool dashDir = false;
    float damage = 0;
    float damage_asses_time = 1.0f;
    bool onGround;
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
        damage_asses_time -= Time.deltaTime;
        if(damage_asses_time <= 0)
        {
            assesDamage();
        }

        /* Run and Shoot animation will be added*/
        //clearAnim();

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("character_draw") && draw)
        {
            Vector3 transPos = transform.position;
            if (drawRight)
            {
                faceMe(drawRight);
                transPos.x++;
                GameObject _arrow = Instantiate(arrow, transPos, Quaternion.identity);
                Rigidbody2D arrow_body = _arrow.GetComponent<Rigidbody2D>();
                arrow_body.velocity = new Vector2(20.0f, 0.0f);
            }
            else
            {
                faceMe(drawRight);
                transPos.x--;
                GameObject _arrow = Instantiate(arrow, transPos, Quaternion.identity);
                Rigidbody2D arrow_body = _arrow.GetComponent<Rigidbody2D>();
                arrow_body.velocity = new Vector2(-20.0f, 0.0f);
                arrow_body.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            draw = false;
        }

        if (archer_state != State.Idle )
        {
            if(archer.velocity.y <0.02 && archer.velocity.y > -0.02)
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

        Vector3 move = new Vector3(horizontalInput, 0, 0.0f);
        


        if(draw && !(facingRight == drawRight))
        {
            Debug.Log("Tersime ok attım");
            
        }
        else if ( (horizontalInput > 0 && facingRight) || (horizontalInput < 0 && !facingRight) || !draw)
        {
            if (draw)
            {
                Debug.Log("Önüme ok attım");
            }
            if (horizontalInput != 0.0f)
            {
                archer_state = State.Run;
                if (onGround)
                {
                    UpdateState(archer_state);
                }

                MoveHorizontal(move);
                if (archer.velocity.y < 0.05f && archer.velocity.y > -0.05f)
                    dust.Play();


            }
        }
        


        if (Input.GetKeyDown(KeyCode.W) && onGround)
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
                    if (facingRight)
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
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !draw)
        {
            archer_state = State.Attack;
            UpdateState(archer_state);
            faceMe(false);
            drawRight = false;
            draw = true;

            // Avoid any reload.




        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !draw)
        {
            archer_state = State.Attack;
            UpdateState(archer_state);
            faceMe(true);

            drawRight = true;
            draw = true;
            // Avoid any reload.



        }




    }

    void MoveHorizontal(Vector3 move)
    {

        
            if (facingRight && move[0] < 0.0f)
            {
                faceMe(false);


            }
            else if (!facingRight && move[0] > 0.0f)
            {
                faceMe(true);
            }

        float vel = 5;
        if (draw)
        {
            vel = 2;
        }
        archer.transform.position = archer.transform.position + vel * move * Time.deltaTime;
        
    }

    void UpdateState(State state)
    {
        
        switch (state)
        {
            case State.Idle:
                clearAnim();
                animator.SetBool("idle", true);
                break;
            case State.Run:
                clearAnim();
                animator.SetBool("run", true);
                break;
            case State.Jump:
                clearAnim();
                animator.SetBool("jump", true);
                break;
            case State.Dash:
                clearAnim();
                animator.SetBool("dash", true);
                break;
            case State.Melee:
                clearAnim();
                animator.SetBool("melee", true);
                break;
            case State.Attack:
                clearAnim();
                animator.SetBool("attack", true);
                break;
            case State.OnAir:
                clearAnim();
                animator.SetBool("onAir", true);
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
        animator.SetBool("onAir", false);

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.tag.Equals("Ground"))
        {
            onGround = true;
            animator.SetBool("onGround", true);
            animator.SetBool("onAir", false);
        }

    }
    void OnCollisionExit2D(Collision2D col)
    {
      
        if (col.gameObject.tag.Equals("Ground"))
        {
            onGround = false;
            animator.SetBool("onGround", false);
            animator.SetBool("onAir", true);
            
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Enemy"))
        {
            if (col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {
                if(col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime % col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length<0.1)
                {
                    damage += 0.5f;
                }
                
            }
        }
    }
    void assesDamage()
    {
        healthBar.localScale -= new Vector3(damage, 0.0f, 0.0f);
        damage_asses_time = 1.0f;
        damage = 0;
    }


    void faceMe(bool right)
    {
        if (right)
        {
            facingRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else {
            facingRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
       
    }
}
