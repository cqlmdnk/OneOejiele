using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum State
{
    Idle,
    Run,
    Melee,
    Dash,
    Jump
}
public class Controller : MonoBehaviour
{
    public GameObject cam;
    public Rigidbody2D archer;
    public Animator animator;
    State archer_state;
    bool facingRight;
    float dash_time1;
    float dash_time2;
    bool isTap = false;
    // Start is called before the first frame update
    void Start()
    {
        archer_state = State.Idle;
        animator.SetBool("idle", true);
        animator.SetBool("run", false);
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(horizontalInput, 0, 0.0f);
        if ((cam.transform.position.x - archer.transform.position.x > 4.0f || cam.transform.position.x - archer.transform.position.x < -1.0f)) // camera movement if player on edge of defined rectangle
        {
            cam.transform.position = new Vector3(cam.transform.position.x + 5 * (-cam.transform.position.x+ archer.transform.position.x)/4 * Time.deltaTime, cam.transform.position.y, cam.transform.position.z);
            
        }
        if (horizontalInput != 0.0f)
        {
            archer_state = State.Run;
            if (facingRight && horizontalInput < 0.0f)
            {
                facingRight = false;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if(!facingRight && horizontalInput > 0.0f)
            {
                facingRight = true;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            
            archer.transform.position = archer.transform.position + 5 * move * Time.deltaTime;
            animator.SetBool("idle", false);
            animator.SetBool("run", true);
           
        }
        else if(archer_state != State.Idle && archer_state != State.Melee && archer_state != State.Jump)
        {

            animator.SetBool("idle", true);
            animator.SetBool("run", false);
            archer_state = State.Idle;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            archer_state = State.Melee;
            animator.SetBool("idle", false);
            animator.SetBool("run", false);
            animator.SetBool("melee", true);
        }
        else
        {
            animator.SetBool("melee", false);
        }



        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (isTap == true)
            {
                dash_time1 = Time.time;
                isTap = false;

                if (dash_time1 - dash_time2 < 0.4f)
                {
                    animator.SetBool("dash", true);
                    archer_state = State.Dash;
                    archer.velocity = new Vector2(5.0f, 0.0f);

                }
            }

            else
            {
                dash_time2 = Time.time;
                isTap = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetBool("idle", false);
            animator.SetBool("run", false);
            animator.SetBool("jump", true);
            archer_state = State.Jump;
            archer.velocity = new Vector2(0.0f, 10.0f);
        }
        else if( archer_state != State.Run)
        {
            animator.SetBool("dash", false);
            animator.SetBool("jump", false);
            animator.SetBool("idle", true);
            archer_state = State.Idle;
        }
        else
        {
            animator.SetBool("dash", false);
            animator.SetBool("jump", false);
            animator.SetBool("idle", false);
            archer_state = State.Run;

        }

        Debug.Log(cam.transform.position.x - archer.transform.position.x);
    }
}
