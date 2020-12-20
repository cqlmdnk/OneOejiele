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

    /*Bugs
     -Arrow instantiate position
         */
    void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
        HandleReleaseArrow();
        HandleDrawArrow();
        HandleMeleeAttack();
        HandleDashMechanic();
        
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
            faceMe(drawRight);
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

    private void HandleReleaseArrow()
    {
        if (drawingTime > 2.0f && m_attackCooledDown && !Input.GetMouseButton(0)) // piece that arrow instantiated
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
            _arrow.GetComponent<ArrowController>().SetDamage(arrow_body.velocity.magnitude);

            arrow_count--;
            drawingTime = 1.5f;
            m_charAnimator.enabled = true;
            m_attackCooledDown= false;
        }
        else if (m_attackCooledDown&& Input.GetMouseButton(0))
        {

            if(drawingTime > 2.0f)
            {

                m_charAnimator.enabled = false;
            }
            drawingTime += Time.deltaTime;
            //Debug.Log(drawingTime);
        }
    }

}
