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
    protected   GameObject      healthBar;
    protected   float           health = 100;
    protected   bool            facingRight;
    protected   GameObject      character;
    protected   Rigidbody2D     charBody;
    protected   Animator        charAnimator;
    protected   bool            onGround;
    [SerializeField]
    protected   CharacterState  characterState;
    protected   float           damage = 0;
    protected   float           damageAssesTime = 0.5f;
    protected   bool            damageTaken = false;
    [SerializeField]
    protected   bool            attackCooledDown;
    protected   Vector3         mousePos;
    protected void Start()
    {
        characterState = CharacterState.Idle;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        charBody = GetComponent<Rigidbody2D>();
        charAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
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

        if (horizontalInput != 0.0f && characterState != CharacterState.Attack)
        {
            if (onGround) // if character is on ground animation can change
            {
                characterState = CharacterState.Run;
            }
            Vector3 move = new Vector3(horizontalInput, 0, 0.0f);
            MoveHorizontal(move); // although animation still fall or jump character can move on air like other platformers

        }
        
        if (Input.GetKeyDown(KeyCode.W) && onGround) // jumping if character on ground or on something concrete
        {

            characterState = CharacterState.Jump;
            charBody.velocity = new Vector2(0.0f, 10.0f);

        }
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
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            facingRight = false;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

    }

    protected void DealDamage() // tried to do damages discrete like other platformers
    {
        if (health < 0)
        {

        }
        else
        {
            healthBar.transform.localScale -= new Vector3(damage, 0.0f, 0.0f);

            health -= damage * 100 / 55;
            damageAssesTime = 0.5f;
            damage = 0;
            transform.GetChild(0).gameObject.SetActive(false);

        }
    }

    protected void HandleMouseMovement()
    {
        if (attackCooledDown || (characterState == CharacterState.Run))
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

    protected void HandleState()
    {
        DealDamage();
        if (characterState != CharacterState.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (charBody.velocity.y > 0.02)
            {
                characterState = CharacterState.Jump;
            }
            else if (charBody.velocity.y < -0.02)
            {
                characterState = CharacterState.Fall;
            }
            if (!attackCooledDown)
            {
                if (Input.GetAxis("Horizontal") == 0)
                {
                    characterState = CharacterState.Idle;
                }
                else
                {
                    characterState = CharacterState.Run;
                }
            }
            

        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Anam bişiler batladı");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Enemy"))
        {

            if (col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {
                if (!damageTaken)
                {
                    damage += 1.0f;
                    transform.GetChild(0).gameObject.SetActive(true);
                    Debug.Log("Damage added. Total_damage: " + damage.ToString());
                    damageTaken = true;
                    StartCoroutine(DamageTakeTimer());
                }
                

            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag.Equals("Ground"))// hit ground
        {
            onGround = true;
            charAnimator.SetBool("onGround", true);
            charAnimator.SetBool("onAir", false);
        }



    }
    void OnCollisionExit2D(Collision2D col) // exit from ground
    {

        if (col.gameObject.tag.Equals("Ground"))
        {
            onGround = false;
            charAnimator.SetBool("onGround", false);
            charAnimator.SetBool("onAir", true);

        }
    }
    public CharacterState GetState()
    {
        return characterState;
    }
    IEnumerator DamageTakeTimer()
    {
        yield return new WaitForSeconds(0.2f);
        damageTaken = false;
    }
}
