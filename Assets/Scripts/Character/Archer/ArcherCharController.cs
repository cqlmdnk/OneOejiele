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
    private float       drawingTime = 0.5f;
    private bool        drawedToOpposite;
    private float       firstTapTime;
    private float       secondTapTime;
    private bool        isTap = false;
    private bool        dashDir = false;

    /*Bugs
     -Arrow instantiate position
         */
    protected override void Awake()
    {
        base.Awake();
        characterState = CharacterState.Idle;
    }

    protected override void Update()
    {
        
        if(characterState == CharacterState.Attack)
            HandleReleaseArrow();
        base.Update();
        HandleDrawArrow();
        HandleMeleeAttack();
        if(characterState == CharacterState.Run || characterState == CharacterState.Idle)
            HandleDashMechanic();
    }
    private void HandleMeleeAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && characterState != CharacterState.Dash) // melee attack will be expanded
        {
            characterState = CharacterState.Melee;
        }
    }

    private void HandleDrawArrow()
    {
        if (Input.GetMouseButtonDown(0) && attackCooledDown && arrow_count > 0) // drawing starts from here, until drawing animation ends there will be no arrow
        {
            characterState = CharacterState.Attack;
            DetermineDrawingDirection();
            FaceMe(drawRight);
            attackCooledDown= false;
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
            if (isTap == true && dashDir == facingRight)
            {
                secondTapTime = Time.time;
                isTap = false;

                if (secondTapTime - firstTapTime < 0.4f)
                {
                    characterState = CharacterState.Dash;
                    if (facingRight)
                        charBody.velocity = new Vector2(5.0f, 0.0f); // velocity gives deeling like quick spin like on dash
                    else
                        charBody.velocity = new Vector2(-5.0f, 0.0f);
                }
            }
            else
            {
                firstTapTime = Time.time;
                dashDir = facingRight;
                isTap = true;
            }
        }
    }

    private void HandleReleaseArrow()
    {
        if (drawingTime >= 0.5f && !Input.GetMouseButton(0)) // piece that arrow instantiated
        {
            Vector3 transPos = GameObject.Find("Archer_bow").transform.position;
            float angle = CalculateAngle(transPos);
            float arrowDrop = DetermineArrowDrop();
            FaceMe(drawRight);
            InstantiateArrow(transPos, angle, arrowDrop);
            attackCooledDown = true;
            arrow_count--;
            drawingTime = 0.5f;
            charAnimator.enabled = true;

        }
        else if (Input.GetMouseButton(0))
        {

            if (drawingTime > 1f)
            {
                Debug.Log("animasyonu durdurdum : " + characterState);
                
                charAnimator.enabled = false;
            }
            drawingTime += Time.deltaTime;
            
        }

        Debug.Log("cooldown " + attackCooledDown);
        

    }

    private float CalculateAngle(Vector3 transPos)
    {
        return (float)Math.Atan2(mousePos.y - transPos.y, mousePos.x - transPos.x) * Mathf.Rad2Deg;
    }

    private float DetermineArrowDrop()
    {
        float arrowDrop;
        if (drawedToOpposite)
        {
            arrowDrop = -0.01f + UnityEngine.Random.Range(-0.5f, 0.5f);
            drawedToOpposite = false;
        }
        else
            arrowDrop = -0.01f;
        return arrowDrop;
    }

    private void InstantiateArrow(Vector3 transPos, float angle, float arrowDrop)
    {
        GameObject _arrow = Instantiate(arrow, transPos, Quaternion.AngleAxis(angle, Vector3.forward));
        _arrow.SetActive(true);
        Rigidbody2D arrow_body = _arrow.GetComponent<Rigidbody2D>();
        Vector3 veloctiy3d = Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3((float)Math.Log((double)drawingTime, 2.0) * 30, arrowDrop, 0);
        arrow_body.velocity = (new Vector2(veloctiy3d.x, veloctiy3d.y));
        _arrow.GetComponent<ThrowableController>().SetDamage(arrow_body.velocity.magnitude);
    }
}
