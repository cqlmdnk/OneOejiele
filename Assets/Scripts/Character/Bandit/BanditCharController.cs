using UnityEngine;
using System.Collections;
using System;

public class BanditCharController : CharacterController
{

    float m_speed = 4.0f;
    float m_jumpForce = 7.5f;



    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool m_attackReverse = false;
    private float m_timerForAttackReverse = 0.0f;
    private bool m_onAttack = false;
    


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

        HandleAttack();


    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !m_onAttack)
        {
            m_onAttack = true;
            StartCoroutine(AttackTimer());
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = (float)Math.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x + 0.5f) * Mathf.Rad2Deg;
            if ((angle > 90.0f || angle < -90.0f))
            {
                m_attackReverse = transform.localScale == new Vector3(1.0f, 1.0f, 1.0f) ? false : true;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            }
            else if (!(angle > 90.0f || angle < -90.0f))
            {
                m_attackReverse = transform.localScale == new Vector3(-1.0f, 1.0f, 1.0f) ? false : true;
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            m_characterState = CharacterState.Attack;
        }
    }

    private void HandleState()
    {
        if (m_characterState != CharacterState.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (m_charBody.velocity.y < 0.02 && m_charBody.velocity.y > -0.02 )
            {
                if(!m_onAttack)
                    m_characterState = CharacterState.Idle;
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
        
        if (horizontalInput != 0.0f)
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
        if (Input.GetKeyDown(KeyCode.W) && m_onGround) // jumping if character on ground or on something concrete
        {

            m_characterState = CharacterState.Jump;
            m_charBody.velocity = new Vector2(0.0f, 10.0f);

        }
    }

    

    public CharacterState GetState()
    {
        return m_characterState;
    }


    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(0.5f);
        m_onAttack = false;
    }
}
