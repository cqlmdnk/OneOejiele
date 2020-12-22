using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator charAnimator;
    private BanditCharController BanditCharController;
    private CharacterState CharacterState;
    void Start()
    {
        BanditCharController = GetComponent<BanditCharController>();
        charAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterState = BanditCharController.GetState();
        UpdateState();
    }

    void UpdateState() // changing animation // clearing all animation before change
    {
        clearAnim();
        switch (CharacterState)
        {
            case CharacterState.Idle:
                charAnimator.SetBool("onGround", true);
                charAnimator.SetBool("idle", true);
                break;
            case CharacterState.Run:
                charAnimator.SetBool("onGround", true);
                charAnimator.SetBool("run", true);
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
        charAnimator.SetBool("onAir", false);

    }
}
