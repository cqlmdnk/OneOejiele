using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class WizardAnimationController : MonoBehaviour
{

    WizardCharController WizardCharController;
    CharacterState CharacterState;
    Animator m_charAnimator;
    // Start is called before the first frame update
    private void Start()
    {
        m_charAnimator = GetComponent<Animator>();
        WizardCharController = GetComponent<WizardCharController>();
        UpdateState();
    }
    private void Update()
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
                clearAnim();
                m_charAnimator.SetBool("idle", true);
                break;
            case CharacterState.Run:
                m_charAnimator.SetBool("run", true);
                break;
            case CharacterState.Jump:
                m_charAnimator.SetBool("jump", true);
                break;
            case CharacterState.Dash:
                m_charAnimator.SetBool("attack2", true);
                break;
            case CharacterState.Melee:
                m_charAnimator.SetBool("melee", true);
                break;
            case CharacterState.Attack:
                m_charAnimator.SetBool("attack1", true);
                break;
            case CharacterState.Fall:
                m_charAnimator.SetBool("fall", true);
                break;
            default:
                m_charAnimator.SetBool("idle", true);
                break;

        }
    }
    void clearAnim() // clearing animation parameters except onGround
    {
        m_charAnimator.SetBool("idle", false);
        m_charAnimator.SetBool("run", false);
        m_charAnimator.SetBool("jump", false);
        m_charAnimator.SetBool("attack1", false);
        m_charAnimator.SetBool("attack2", false);
        m_charAnimator.SetBool("dash", false);
        m_charAnimator.SetBool("fall", false);
        m_charAnimator.SetBool("onAir", false);

    }

    
    
}
