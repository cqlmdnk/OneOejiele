using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    ArcherCharController ArcherCharController;
    CharacterState CharacterState;
    Animator charAnimator;
    void Start()
    {
        charAnimator = GetComponent<Animator>();
        ArcherCharController = GetComponent<ArcherCharController>();
        UpdateState();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CharacterState = ArcherCharController.GetState();
        UpdateState();

    }


    void UpdateState() // changing animation // clearing all animation before change
    {
        clearAnim();
        switch (CharacterState)
        {
            case CharacterState.Idle:
                charAnimator.SetBool("idle", true);
                charAnimator.SetBool("onGround", true);
                break;
            case CharacterState.Run:
                charAnimator.SetBool("run", true);
                charAnimator.SetBool("onGround", true);
                break;
            case CharacterState.Jump:
                charAnimator.SetBool("jump", true);
                break;
            case CharacterState.Dash:
                charAnimator.SetBool("dash", true);
                break;
            case CharacterState.Melee:
                charAnimator.SetBool("melee", true);
                break;
            case CharacterState.Attack:
                charAnimator.SetTrigger("attack");
                break;
            case CharacterState.Fall:
                charAnimator.SetBool("onGround", false);
                break;
        }
    }
    void clearAnim() // clearing animation parameters except onGround
    {
        charAnimator.SetBool("idle", false);
        charAnimator.SetBool("run", false);
        charAnimator.SetBool("jump", false);
        charAnimator.ResetTrigger("attack");
        charAnimator.SetBool("dash", false);
        charAnimator.SetBool("melee", false);

    }
}
