using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    KingController KingController;
    Animator KingAnimator;
    CharacterState  KingState;
    void Start()
    {
        KingController = GetComponent<KingController>();
        KingAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        KingState = KingController.GetState();
        switch (KingState)
        {
            case CharacterState.Idle:
                KingAnimator.SetBool("idle", true);
                KingAnimator.SetBool("run", false);
                break;
            case CharacterState.Run:
                KingAnimator.SetBool("idle", false);
                KingAnimator.SetBool("run", true);
                break;
            
        }
    }
}
