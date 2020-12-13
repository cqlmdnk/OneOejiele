using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterController : MonoBehaviour
{
    protected enum State
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
    // Start is called before the first frame update
    public GameObject healthBar;
    protected float health = 100;
    protected bool facingRight;
    protected GameObject character;
    protected  Rigidbody2D char_body;
    protected  Animator char_animator;
    protected bool onGround;
    protected State character_state;
    protected float m_damage = 0;
    protected float m_damageAssesTime = 0.5f;
    protected void Start()
    {
        character_state = State.Idle;
        
        char_body = GetComponent<Rigidbody2D>();
        char_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }
    protected void MoveHorizontal(Vector3 move)
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
        this.transform.position = this.transform.position + vel * move * Time.deltaTime;

    }
    protected void faceMe(bool right) // changing local facing
    {
        if (right)
        {
            facingRight = true;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            facingRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }

    }

    protected void assesDamage() // tried to do damages discrete like other platformers
    {
        if (health < 0)
        {

        }
        else
        {
            healthBar.transform.localScale -= new Vector3(m_damage, 0.0f, 0.0f);

            health -= m_damage * 100 / 55;
            m_damageAssesTime = 0.5f;
            m_damage = 0;
            transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("Damage assessed : " + health.ToString());
        }
    }

    public void addHealth(float addedHealth)
    {
        if (health + addedHealth <= 100)
        {
            health += addedHealth;
            healthBar.transform.localScale += new Vector3(addedHealth * 55 / 100, 0.0f, 0.0f);

        }
        else
        {
            health = 100.0f;
            healthBar.transform.localScale = new Vector3(55, 10.5f, 1.0f);
        }

    }
}
