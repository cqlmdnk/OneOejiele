using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Text coins;
    public int coinsack = 0;
    State archer_state;
    
    bool facingRight;
    bool draw = false, drawRight = true;
    float drawingTime = 1.5f;
    bool drawedToOpposite;

    float dash_time1;
    float dash_time2;
    bool isTap = false;
    bool dashDir = false;

    float damage = 0;
    float damage_asses_time = 1.0f;

    bool onGround;

    void Start()
    {
        archer_state = State.Idle;
        UpdateState(archer_state);
        facingRight = true;
    }


    void Update()
    {
        coins.text = coinsack.ToString();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
        {
            faceMe(true);

        }
        else
        {
            faceMe(false);
        }
        damage_asses_time -= Time.deltaTime;
        if (damage_asses_time <= 0)
        {
            assesDamage();
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("character_draw") && draw && !Input.GetMouseButton(0) ) // piece that arrow instantiated
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
            arrow_body.velocity = (new Vector2( veloctiy3d.x , veloctiy3d.y));


            drawingTime = 1.5f;

            animator.enabled = true;
            draw = false;
        }
        else if(draw && Input.GetMouseButton(0))
        {
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("character_draw"))
                animator.enabled = false;
            //Debug.Log("Yayı geriyorum");
            drawingTime += Time.deltaTime;
            //Debug.Log(drawingTime);
        }

        if (archer_state != State.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (archer.velocity.y < 0.02 && archer.velocity.y > -0.02)
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
                if (archer.velocity.y < 0.05f && archer.velocity.y > -0.05f)
                    dust.Play(); // it is just for dust particles from character's feet
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
            archer.velocity = new Vector2(0.0f, 10.0f);
            dust.Stop(); //no dust on air
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
                    animator.SetBool("dash", true);
                    archer_state = State.Dash;
                    if (facingRight)
                        archer.velocity = new Vector2(5.0f, 0.0f); // velocity gives deeling like quick spin like on dash
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
        if (Input.GetMouseButtonDown(0) && !draw) // drawing starts from here, until drawing animation ends there will be no arrow
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
        if (draw) // while drawing and moving same direction velocity of character is stepped down
        {
            vel = 2;
        }
        archer.transform.position = archer.transform.position + vel * move * Time.deltaTime;

    }

    void UpdateState(State state) // changing animation // clearing all animation before change
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
    void clearAnim() // clearing animation parameters except onGround
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

        if (col.gameObject.tag.Equals("Ground"))// hit ground
        {
            onGround = true;
            animator.SetBool("onGround", true);
            animator.SetBool("onAir", false);
        }
        else if (col.gameObject.tag.Equals("Collectable"))// hit ground
        {
            if(col.gameObject.name.Contains("Coin") ){
                Destroy(col.gameObject);
                coinsack++;

            }
        }

    }
    void OnCollisionExit2D(Collision2D col) // exit from ground
    {

        if (col.gameObject.tag.Equals("Ground"))
        {
            onGround = false;
            animator.SetBool("onGround", false);
            animator.SetBool("onAir", true);

        }
    }

    void OnCollisionStay2D(Collision2D col) // for damage taking syncing with enemy animation
    {
        if (col.gameObject.tag.Equals("Enemy"))
        {
            if (col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {
                if (col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime % col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length < 0.1)
                {
                    damage += 0.5f;
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }
    void assesDamage() // tried to do damages discrete like other platformers
    {
        healthBar.localScale -= new Vector3(damage, 0.0f, 0.0f);
        damage_asses_time = 1.0f;
        damage = 0;
        transform.GetChild(0).gameObject.SetActive(false);
    }


    void faceMe(bool right) // changing local facing
    {
        if (right)
        {
            facingRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            facingRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
    }
}
