using Assets.ArcherChar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;


public class ArcherCharController : CharacterController
{
    
    
    
    public  GameObject  arrow;
    public  int         arrow_count = 30;
    private bool        drawRight = true;
    private float       drawingTime = 1.5f;
    private bool        drawedToOpposite;
    private float       dash_time1;
    private float       dash_time2;
    private bool        isTap = false;
    private bool        dashDir = false;




    void Start()
    {
        base.Start();
        
        

    }


    void FixedUpdate()
    {
        HandleMouseMovement();
        HandleState();
        HandleMovement();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        HandleReleaseArrow();
        HandleDrawArrow();

        
        
        HandleMeleeAttack();
        HandleDashMechanic();
        
        HandleDamageAssesment();
    }

    private void HandleDamageAssesment()
    {
        m_damageAssesTime -= Time.deltaTime;
        if (m_damageAssesTime <= 0)
        {
            assesDamage();
        }
    }

    private void HandleMeleeAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_characterState != CharacterState.Dash) // melee attack will be expanded
        {
            m_characterState = CharacterState.Melee;
        }
    }

    private void HandleDrawArrow()
    {
        if (Input.GetMouseButtonDown(0) && !m_attackCooledDown&& arrow_count > 0) // drawing starts from here, until drawing animation ends there will be no arrow
        {
            Debug.Log("Oku aldım");
            m_characterState = CharacterState.Attack;
            DetermineDrawingDirection();

            m_attackCooledDown= true;
        }
    }
    private void DetermineDrawingDirection()
    {
        if (mousePos.x > transform.position.x)
        {
            drawRight = true;

        }
        else
        {
            drawRight = false;
        }
    }

    private void HandleDashMechanic()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) // double tap dash control
        {
            if (isTap == true && dashDir == m_facingRight)
            {
                dash_time1 = Time.time;
                isTap = false;

                if (dash_time1 - dash_time2 < 0.2f)
                {
                    m_characterState = CharacterState.Dash;
                    if (m_facingRight)
                        m_charBody.velocity = new Vector2(5.0f, 0.0f); // velocity gives deeling like quick spin like on dash
                    else
                        m_charBody.velocity = new Vector2(-5.0f, 0.0f);
                }
            }
            else
            {
                dash_time2 = Time.time;
                dashDir = m_facingRight;
                isTap = true;
            }
        }
    }

    private void HandleState()
    {
        if (m_characterState != CharacterState.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (m_charBody.velocity.y < 0.02 && m_charBody.velocity.y > -0.02)
            {
               
            }
            else
            {
                m_characterState = CharacterState.OnAir;
            }

        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(horizontalInput, 0, 0.0f); // get input for horizontal movement


        if ((horizontalInput > 0 && m_facingRight) || (horizontalInput < 0 && !m_facingRight) || !m_attackCooledDown) // if direction is opposite do not move until drwaing animation ends
        {
            if (m_attackCooledDown)
            {
                m_characterState = CharacterState.Idle;
                //arrow is drawed same direction of facing
            }
            if (!m_attackCooledDown&& horizontalInput != 0.0f)
            {
                
                if (m_onGround) // if character is on ground animation can change
                {
                    m_characterState = CharacterState.Run;
                }
                MoveHorizontal(move); // although animation still fall or jump character can move on air like other platformers

            }
            else
            {
                m_characterState = CharacterState.Idle;
            }
        }
        else
        {
            drawedToOpposite = true;
        }

        if (Input.GetKeyDown(KeyCode.W) && m_onGround) // jumping if character on ground or on something concrete
        {

            m_characterState = CharacterState.Jump;
            m_charBody.velocity = new Vector2(0.0f, 10.0f);

        }
    }

    private void HandleReleaseArrow()
    {
        if (!m_charAnimator.GetCurrentAnimatorStateInfo(0).IsName("character_draw") && m_attackCooledDown&& !Input.GetMouseButton(0)) // piece that arrow instantiated
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
            m_charAnimator.enabled = true;
            m_attackCooledDown= false;
        }
        else if (m_attackCooledDown&& Input.GetMouseButton(0))
        {

            m_characterState = CharacterState.Idle;
            Debug.Log("Yayı geriyorum");
            drawingTime += Time.deltaTime;
            //Debug.Log(drawingTime);
        }
    }

    
    public CharacterState GetState()
    {
        return m_characterState;
    }


    
    
   
   

    
}
