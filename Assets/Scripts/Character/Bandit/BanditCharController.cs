using UnityEngine;
using System.Collections;
using System;

public class BanditCharController : CharacterController {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;

    
    
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;
    private bool                m_attackReverse = false;
    private float               m_timerForAttackReverse = 0.0f;
    /*
     if collides with any collider on air. movement will be disabled.
         
         
         */
    // Use this for initialization
    void Start () {
        base.Start();
        
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Check if character just landed on the ground
        

        //Check if character just started falling
       

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0 && !m_attackReverse)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0 && !m_attackReverse)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        if (!m_attackReverse) { 
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 move = new Vector3(horizontalInput, 0, 0.0f);
            this.transform.position = this.transform.position + m_speed * move * Time.deltaTime;
        }
        //Set AirSpeed in animator
        m_charAnimator.SetFloat("AirSpeed", m_charBody.velocity.y);

        // -- Handle Animations --
        //Death

        if (m_attackReverse)
        {
            m_timerForAttackReverse += Time.deltaTime;
            if(m_timerForAttackReverse > 0.6f)
            {
                m_timerForAttackReverse = 0.0f;
                m_attackReverse = false;
            }
        }
        else if (Input.GetKeyDown("e")) {
            if(!m_isDead)
                m_charAnimator.SetTrigger("Death");
            else
                m_charAnimator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_charAnimator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = (float)Math.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x + 0.5f) * Mathf.Rad2Deg;
            if ((angle > 90.0f || angle < -90.0f) )
            {
                m_attackReverse = transform.localScale == new Vector3(1.0f, 1.0f, 1.0f) ? false : true; 
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            }
            else if (!(angle > 90.0f || angle < -90.0f))
            {
                m_attackReverse = transform.localScale == new Vector3(-1.0f, 1.0f, 1.0f) ? false : true;
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            m_charAnimator.SetTrigger("Attack");
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown(KeyCode.W) && m_onGround) {
            m_charAnimator.SetTrigger("Jump");
            m_onGround = false;
            m_charAnimator.SetBool("Grounded", m_onGround);
            m_charBody.velocity = new Vector2(m_charBody.velocity.x, m_jumpForce);
           
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_charAnimator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_charAnimator.SetInteger("AnimState", 1);

        //Idle
        else
            m_charAnimator.SetInteger("AnimState", 0);
    }


    
}
