using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WizardCharController : CharacterController
{
    // Start is called before the first frame update

    
    void Start()
    {
        base.Start();
        UpdateState(character_state);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(horizontalInput, 0, 0.0f); // get input for horizontal movement

        if (char_body.velocity.y < 0.02 && char_body.velocity.y > -0.02)
        {
            character_state = State.Idle;
            UpdateState(character_state);
        }


        if (horizontalInput != 0.0f)
        {
            character_state = State.Run;
            
            UpdateState(character_state);
            
            MoveHorizontal(move); // although animation still fall or jump character can move on air like other platformers

        }
    }

    
    void UpdateState(State state) // changing animation // clearing all animation before change
    {
        clearAnim();
        switch (state)
        {
            case State.Idle:
                char_animator.SetBool("idle", true);
                break;
            case State.Run:
                char_animator.SetBool("run", true);
                break;
            case State.Jump:
                char_animator.SetBool("jump", true);
                break;
            case State.Dash:
                char_animator.SetBool("dash", true);
                break;
            case State.Melee:
                char_animator.SetBool("melee", true);
                break;
            case State.Attack:
                char_animator.SetBool("attack", true);
                break;
            case State.OnAir:
                char_animator.SetBool("onAir", true);
                break;
        }
    }
    void clearAnim() // clearing animation parameters except onGround
    {
        char_animator.SetBool("idle", false);
        char_animator.SetBool("run", false);
        char_animator.SetBool("jump", false);
        char_animator.SetBool("attack", false);
        char_animator.SetBool("dash", false);
        char_animator.SetBool("melee", false);
        char_animator.SetBool("onAir", false);

    }
}
