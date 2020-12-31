using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class WizardAnimationController : MonoBehaviour
{

    WizardCharController WizardCharController;
    CharacterState CharacterState;
    Animator charAnimator;
    // Start is called before the first frame update
    private void Start()
    {
        charAnimator = GetComponent<Animator>();
        WizardCharController = GetComponent<WizardCharController>();
        UpdateState();
    }
     void Update()
    {
        CharacterState = WizardCharController.GetState();
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
                charAnimator.SetBool("onGround", false);
                break;
            case CharacterState.Dash:
                charAnimator.SetTrigger("attack2");
                break;
            case CharacterState.Melee:
                charAnimator.SetBool("melee", true);
                break;
            case CharacterState.Attack:
                charAnimator.SetTrigger("attack1");
                break;
            case CharacterState.Fall:
                charAnimator.SetBool("onGround", false);
                break;
            default:
                charAnimator.SetBool("idle", true);
                break;

        }
    }
    void clearAnim() // clearing animation parameters except onGround
    {
        charAnimator.SetBool("idle", false);
        charAnimator.SetBool("run", false);
        charAnimator.SetBool("jump", false);
        charAnimator.ResetTrigger("attack1");
        charAnimator.ResetTrigger("attack2");
        charAnimator.SetBool("dash", false);
        charAnimator.SetBool("fall", false);

    }

    
    
}
