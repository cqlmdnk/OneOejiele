using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WizardCharController : CharacterController
{
    // Start is called before the first frame update
    
    public GameObject MagicSpell;
    private float Attack1Time = 0.7f;
    void Start()
    {
        base.Start();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        HandleMouseMovement();
        HandleMovement();
        HandleState();
        Debug.Log(m_characterState);

        HandleAttack();

    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !m_attackCooledDown)
        {
            m_characterState = CharacterState.Attack;

            m_attackCooledDown = true;
            StartCoroutine(Attack1Timer());
        }
        else if (Input.GetMouseButtonDown(1) && !m_attackCooledDown)
        {
            m_characterState = CharacterState.Dash;
            m_attackCooledDown = true;

            StartCoroutine(Attack1Timer());
            

        }
    }

    private void HandleCastSpell(float angle, Vector3 SpellPosition)
    {
        
        
        GameObject _MagicSpell = Instantiate(MagicSpell, SpellPosition, Quaternion.AngleAxis(angle, Vector3.forward));
        _MagicSpell.SetActive(true);
        Rigidbody2D _MagicSpellBody = _MagicSpell.GetComponent<Rigidbody2D>();
        Vector3 veloctiy3d = Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3(8.0f, 0, 0);
        _MagicSpellBody.velocity = (new Vector2(veloctiy3d.x, veloctiy3d.y));
    }

    private void HandleMovement()
    {
        if (m_attackCooledDown)
            return;
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(horizontalInput, 0, 0.0f); // get input for horizontal movement
        MoveHorizontal(move);
        if (horizontalInput != 0.0f)
        {
            m_characterState = CharacterState.Run;
        }
        else if (m_charBody.velocity.y < 0.02 && m_charBody.velocity.y > -0.02)
        {
            m_characterState = CharacterState.Idle;

        }
        if (Input.GetKeyDown(KeyCode.W) && m_onGround) // jumping if character on ground or on something concrete
        {

            m_characterState = CharacterState.Jump;
            m_charBody.velocity = new Vector2(0.0f, 10.0f);

        }
    }

    

    private void HandleState()
    {
        if (m_characterState != CharacterState.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (m_charBody.velocity.y > 0.02 )
            {
                m_characterState = CharacterState.Jump;
            }
            else if( m_charBody.velocity.y < -0.02)
            {
                m_characterState = CharacterState.Fall;
            }
            else if(Input.GetAxis("Horizontal") == 0 && m_attackCooledDown)
            {
                m_characterState = CharacterState.Idle;
            }

        }
    }

    IEnumerator Attack1Timer()
    {
        Vector3 SpellPosition;
        if (m_facingRight)
        {
            SpellPosition = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        }
        else
        {
            SpellPosition = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
        }
        float angle = (float)Math.Atan2(mousePos.y - SpellPosition.y, mousePos.x - SpellPosition.x) * Mathf.Rad2Deg;
        yield return new WaitForSeconds(Attack1Time);
        HandleCastSpell(angle, SpellPosition);
        m_attackCooledDown = false;

    }



    public CharacterState GetState()
    {
        return m_characterState;
    }
    




}

