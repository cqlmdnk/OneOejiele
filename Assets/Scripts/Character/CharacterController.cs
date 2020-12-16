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
    public      GameObject      m_healthBar;
    protected   float           m_health = 100;
    protected   bool            m_facingRight;
    protected   GameObject      m_character;
    protected   Rigidbody2D     m_charBody;
    protected   Animator        m_charAnimator;
    protected   bool            m_onGround;
    protected   CharacterState  m_characterState;
    protected   float           m_damage = 0;
    protected   float           m_damageAssesTime = 0.5f;
    protected void Start()
    {
        m_characterState = CharacterState.Idle;

        m_charBody = GetComponent<Rigidbody2D>();
        m_charAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected void Update()
    {
        
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
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            m_facingRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
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
            Debug.Log("Damage assessed : " + m_health.ToString());
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

    void OnParticleCollision(GameObject other)
    {

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Enemy"))
        {

            if (col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {

                m_damage += 1.0f;
                transform.GetChild(0).gameObject.SetActive(true);
                Debug.Log("Damage added. Total_damage: " + m_damage.ToString());

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
}
