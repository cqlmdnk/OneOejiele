using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator m_charAnimator;
    private BanditCharController BanditCharController;
    private CharacterState CharacterState;
    void Start()
    {
        BanditCharController = GetComponent<BanditCharController>();
        m_charAnimator = GetComponent<Animator>();
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
                m_charAnimator.SetBool("idle", true);
                break;
            case CharacterState.Run:
                m_charAnimator.SetBool("run", true);
                break;
            case CharacterState.Jump:
                m_charAnimator.SetBool("jump", true);
                break;
            case CharacterState.Dash:
                m_charAnimator.SetBool("dash", true);
                break;
            case CharacterState.Melee:
                m_charAnimator.SetBool("melee", true);
                break;
            case CharacterState.Attack:
                m_charAnimator.SetBool("attack", true);
                break;
            case CharacterState.OnAir:
                m_charAnimator.SetBool("onAir", true);

                break;
        }
    }
    void clearAnim() // clearing animation parameters except onGround
    {
        m_charAnimator.SetBool("idle", false);
        m_charAnimator.SetBool("run", false);
        m_charAnimator.SetBool("jump", false);
        m_charAnimator.SetBool("attack", false);
        m_charAnimator.SetBool("dash", false);
        m_charAnimator.SetBool("melee", false);
        m_charAnimator.SetBool("onAir", false);

    }
}
