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
    public   Transform      healthBar;
    [SerializeField]
    protected   float           health;
    protected   bool            facingRight;
    protected   GameObject      character;
    protected   Rigidbody2D     charBody;
    protected   BoxCollider2D   boxCollider2D;
    [SerializeField]
    private     LayerMask       platformLayerMask;
    protected   Animator        charAnimator;
    [SerializeField]
    protected   CharacterState  characterState;
    protected   float           damage = 0;
    protected   bool            damageTaken = false;
    [SerializeField]
    protected   bool            attackCooledDown;
    protected   Vector3         mousePos;
    public float maxHealth;
    public float healthBarScale;
    protected virtual void Start()
    {
        characterState = CharacterState.Idle;
        if(healthBar == null)
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").transform;
        charBody = GetComponent<Rigidbody2D>();
        charAnimator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        maxHealth = 100.0f;
        health = maxHealth;
        healthBarScale = healthBar.localScale.x;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HandleMouseMovement();
        HandleMovement();
        HandleState();
        IsGrounded();
    }

    protected void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
         // get input for horizontal movement

        if (horizontalInput != 0.0f && characterState != CharacterState.Attack)
        {
            if (IsGrounded()) // if character is on ground animation can change
            {
                characterState = CharacterState.Run;
            }
            Vector3 move = new Vector3(horizontalInput, 0, 0.0f);
            MoveHorizontal(move); // although animation still fall or jump character can move on air like other platformers

        }
        
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded()) // jumping if character on ground or on something concrete
        {

            characterState = CharacterState.Jump;
            charBody.velocity = new Vector2(0.0f, 10.0f);

        }
    }
    protected void MoveHorizontal(Vector3 move)
    {
        if (facingRight && move[0] < 0.0f)
        {
            FaceMe(false);
        }
        else if (!facingRight && move[0] > 0.0f)
        {
            FaceMe(true);
        }
        float vel = 5;
        this.transform.position = this.transform.position + vel * move * Time.deltaTime;

    }
    protected void FaceMe(bool right) // changing local facing
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

    public void TakeDamage(float amount) // tried to do damages discrete like other platformers
    {
        if (health < 0)
        {
            Debug.Log("karakter öldü"); 
            //die();
        }
        else
        {
            healthBar.localScale -= new Vector3(amount* healthBarScale / maxHealth, 0.0f, 0.0f);
            health -= amount;
        }
    }

    protected void HandleMouseMovement()
    {
        if (attackCooledDown || (characterState == CharacterState.Run))
            return;
        if (mousePos.x > transform.position.x)
        {
            FaceMe(true);

        }
        else
        {
            FaceMe(false);
        }

    }

    public void AddHealth(float addedHealth)
    {
        if (health + addedHealth <= 100)
        {
            health += addedHealth;
            healthBar.localScale += new Vector3(addedHealth * 55 / 100, 0.0f, 0.0f);

        }
        else
        {
            health = 100.0f;
            healthBar.localScale = new Vector3(55, 10.5f, 1.0f);
        }

    }

    protected void HandleState()
    {
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
            
            else if (!attackCooledDown)
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
        if (other.tag.Equals("ThrowUp"))
            TakeDamage(0.1f);
        else if (other.tag.Equals("Explosion"))
            TakeDamage(0.2f);
    }



    protected bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeight, platformLayerMask);
        
        if (raycastHit2D.collider == null)
        {
            characterState = CharacterState.Fall;
        }
        
        return raycastHit2D.collider != null;
    }

    public CharacterState GetState()
    {
        return characterState;
    }
   
}
