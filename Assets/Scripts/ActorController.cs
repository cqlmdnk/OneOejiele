using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    // Start is called before the first frame update
    protected GameObject character;
    protected Rigidbody2D charBody;
    protected BoxCollider2D boxCollider2D;
    protected Animator charAnimator;
    protected CharacterState characterState;
    protected HealthController healthController;
    protected bool facingRight;
    protected virtual void Awake()
    {
        characterState = CharacterState.Idle;
        healthController = GetComponent<HealthController>();
        charBody = GetComponent<Rigidbody2D>();
        charAnimator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected void FaceMe(bool right) // changing local facing
    {
        if (right)
        {
            facingRight = true;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            foreach (Transform child in transform)
            {
                child.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            facingRight = false;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            foreach (Transform child in transform)
            {
                child.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }

    }
}
