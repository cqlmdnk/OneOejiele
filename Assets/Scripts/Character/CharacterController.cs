using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterController : ActorController
{
    
    // Start is called before the first frame update
    
    
    [SerializeField]
    private     LayerMask        groundLayerMask;
    protected   bool             attackCooledDown = true;
    protected   Vector3          mousePos;
    

    protected override void Awake()
    {
        base.Awake();
       
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

        if (horizontalInput != 0.0f && attackCooledDown)
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
    

    

    protected void HandleMouseMovement()
    {
        if (!attackCooledDown || (characterState == CharacterState.Run))
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
            
            else if (attackCooledDown )
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
            healthController.TakeDamage(0.1f);
        else if (other.tag.Equals("Explosion"))
            healthController.TakeDamage(0.2f);
    }



    protected bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeight, groundLayerMask);
        
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
