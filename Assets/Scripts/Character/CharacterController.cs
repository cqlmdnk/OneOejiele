using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
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

public class CharacterController : MonoBehaviour
{
    
    // Start is called before the first frame update
    protected   GameObject      m_healthBar;
    protected   float           m_health = 100;
    protected   bool            m_facingRight;
    protected   GameObject      m_character;
    protected   Rigidbody2D     m_charBody;
    protected   Animator        m_charAnimator;
    protected   bool            m_onGround;
    [SerializeField]
    protected   CharacterState  m_characterState;
    protected   float           m_damage = 0;
    protected   float           m_damageAssesTime = 0.5f;
    protected   bool            m_damageTaken = false;
    [SerializeField]
    protected   bool            m_attackCooledDown;
    protected   Vector3         mousePos;
    protected void Start()
    {
        m_characterState = CharacterState.Idle;
        m_healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        m_charBody = GetComponent<Rigidbody2D>();
        m_charAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HandleMouseMovement();
        HandleMovement();
        HandleState();
    }

    protected void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
         // get input for horizontal movement

        if (horizontalInput != 0.0f && m_characterState != CharacterState.Attack)
        {
            if (m_onGround) // if character is on ground animation can change
            {
                m_characterState = CharacterState.Run;
            }
            Vector3 move = new Vector3(horizontalInput, 0, 0.0f);
            MoveHorizontal(move); // although animation still fall or jump character can move on air like other platformers

        }
        
        if (Input.GetKeyDown(KeyCode.W) && m_onGround) // jumping if character on ground or on something concrete
        {

            m_characterState = CharacterState.Jump;
            m_charBody.velocity = new Vector2(0.0f, 10.0f);

        }
    }
    protected void MoveHorizontal(Vector3 move)
    {
        if (m_facingRight && move[0] < 0.0f)
        {
            faceMe(false);
        }
        else if (!m_facingRight && move[0] > 0.0f)
        {
            faceMe(true);
        }
        float vel = 5;
        this.transform.position = this.transform.position + vel * move * Time.deltaTime;

    }
    protected void faceMe(bool right) // changing local facing
    {
        if (right)
        {
            m_facingRight = true;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            m_facingRight = false;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

    }

    protected void assesDamage() // tried to do damages discrete like other platformers
    {
        if (m_health < 0)
        {

        }
        else
        {
            m_healthBar.transform.localScale -= new Vector3(m_damage, 0.0f, 0.0f);

            m_health -= m_damage * 100 / 55;
            m_damageAssesTime = 0.5f;
            m_damage = 0;
            transform.GetChild(0).gameObject.SetActive(false);

        }
    }

    protected void HandleMouseMovement()
    {
        if (m_attackCooledDown || (m_characterState == CharacterState.Run))
            return;
        if (mousePos.x > transform.position.x)
        {
            faceMe(true);

        }
        else
        {
            faceMe(false);
        }

    }

    public void addHealth(float addedHealth)
    {
        if (m_health + addedHealth <= 100)
        {
            m_health += addedHealth;
            m_healthBar.transform.localScale += new Vector3(addedHealth * 55 / 100, 0.0f, 0.0f);

        }
        else
        {
            m_health = 100.0f;
            m_healthBar.transform.localScale = new Vector3(55, 10.5f, 1.0f);
        }

    }

    protected void HandleState()
    {
        assesDamage();
        if (m_characterState != CharacterState.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (m_charBody.velocity.y > 0.02)
            {
                m_characterState = CharacterState.Jump;
            }
            else if (m_charBody.velocity.y < -0.02)
            {
                m_characterState = CharacterState.Fall;
            }
            if (!m_attackCooledDown)
            {
                if (Input.GetAxis("Horizontal") == 0)
                {
                    m_characterState = CharacterState.Idle;
                }
                else
                {
                    m_characterState = CharacterState.Run;
                }
            }
            

        }
    }

    void OnParticleCollision(GameObject other)
    {

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Enemy"))
        {

            if (col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {
                if (!m_damageTaken)
                {
                    m_damage += 1.0f;
                    transform.GetChild(0).gameObject.SetActive(true);
                    Debug.Log("Damage added. Total_damage: " + m_damage.ToString());
                    m_damageTaken = true;
                    StartCoroutine(DamageTakeTimer());
                }
                

            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag.Equals("Ground"))// hit ground
        {
            m_onGround = true;
            m_charAnimator.SetBool("onGround", true);
            m_charAnimator.SetBool("onAir", false);
        }



    }
    void OnCollisionExit2D(Collision2D col) // exit from ground
    {

        if (col.gameObject.tag.Equals("Ground"))
        {
            m_onGround = false;
            m_charAnimator.SetBool("onGround", false);
            m_charAnimator.SetBool("onAir", true);

        }
    }
    public CharacterState GetState()
    {
        return m_characterState;
    }
    IEnumerator DamageTakeTimer()
    {
        yield return new WaitForSeconds(0.2f);
        m_damageTaken = false;
    }
}
