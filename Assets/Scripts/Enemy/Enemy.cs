using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected float health, damage, damageTimer;

    public GameObject damagePopUp;
    public GameObject coin, arrowSack;
    public Sprite deadBody;
    protected Rigidbody2D player;
    protected Animator animator;
    public ParticleSystem explosion;
    protected bool stopForAttack;
    [SerializeField]
    protected float length;
    protected bool lockedTarget = false, attackCooledDown = true;
    public float attackRate = 1.0f;
    private float nextAttackTime;
    CharacterState enemyState;

    protected virtual void Update()
    {
        if (HandleDeath())
            return;

        HandleNewPath();
        HandleMovement();
    }


    protected void Init()
    {

        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        length = UnityEngine.Random.Range(-2, 2);

    }


    protected void HandleNewPath()
    {
        if (!stopForAttack && length > -0.01 && length < 0.01)
        {
            length = UnityEngine.Random.Range(-1, 2);

        }
    }

    protected void HandleState()
    {
        if (enemyState != CharacterState.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (!attackCooledDown)
            {
                if (!stopForAttack || attackCooledDown)
                {
                    enemyState = CharacterState.Idle;
                }
                else
                {
                    enemyState = CharacterState.Run;
                }
            }


        }
    }




    protected bool HandleDeath()
    {
        if (health < 0.0f)
        {
            if (explosion != null)
                explosion.transform.parent = null;
            animator.SetBool("dead", true);
            Destroy(this.gameObject, 4.0f);
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            return true;
        }
        return false;
    }

    protected GameObject SightCheck()
    {
        /*             * * *    raycasting more than 90 degrees per direction         
                     *    I *
                   *      I
                 *        I
                *         I           
                ---------------------        
         */
        GetComponent<CircleCollider2D>().enabled = false;
        GameObject collider = null;
        List<RaycastHit2D> sighttest = new List<RaycastHit2D>();
        if (length < 0)
        {

            for (int i = 0; i < 12; i++)
            {
                sighttest.Add(Physics2D.Raycast(transform.position, new Vector3(-1.0f + (i / 10.0f), (i / 10.0f), 0.0f)));
            }

        }

        else
        {

            for (int i = 0; i < 12; i++)
            {
                sighttest.Add(Physics2D.Raycast(transform.position, new Vector3((i / 10.0f), 1.0f - (i / 10.0f), 0.0f)));
            }
        }
        foreach (RaycastHit2D raycast in sighttest)
        {
            if (raycast.collider != null)
            {
                if (raycast.collider.tag == "Player" && raycast.distance < 6.0f)
                {

                    collider = raycast.collider.gameObject;
                }
                else if (raycast.collider.tag == "NPC" && raycast.distance < 10.0f)
                {
                    collider = raycast.collider.gameObject;
                }

            }
        }
        GetComponent<CircleCollider2D>().enabled = true;
        return collider;

    }

    protected void DetermineDirection(float moveX)
    {
        if (moveX < 0)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            foreach (Transform child in transform) // child class a alınacak
            {
                child.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            foreach (Transform child in transform)
            {
                child.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }

    protected void HandleMovement()
    {
        GameObject seenObject = SightCheck();

        lockedTarget = seenObject == null ? false : true;

        if (!stopForAttack)
        {
            if (!lockedTarget)
            {
                Vector3 move = new Vector3(((0.1f) * Math.Sign(length) + length / 30), 0, 0.0f);
                transform.position = transform.position + 5 * move * Time.deltaTime;
                length -= move.x * Time.deltaTime;
                DetermineDirection(move.x);
            }
            else
            {
                Vector3 move = new Vector3((0.1f) * Math.Sign(seenObject.transform.position.x - transform.position.x) + seenObject.transform.position.x - transform.position.x, 0, 0);
                transform.position = transform.position + 4 * move.normalized * Time.deltaTime;
                DetermineDirection(move.x);
                length = move.x > 0 ? 1.0f : -1.0f;


            }
        }
    }
    public void TakeDamage(float amount)
    {
        Debug.Log("Girdim");
        animator.SetTrigger("takingDamage");
        health -= amount;
        damage = amount;
        Vector3 popUpPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        GameObject _popUp = Instantiate(damagePopUp, popUpPos, Quaternion.identity);
        _popUp.GetComponent<TextMesh>().text = damage.ToString("#.00");
        _popUp.transform.SetParent(this.gameObject.transform);
        _popUp.SetActive(true);
        animator.ResetTrigger("takingDamage");
    }


    protected void OnBeforeDestroy() // creating dead body
    {
        if (health > 0.0f) // deleted by ending execution not killed by player
            return;

        GameObject dead_zombie = new GameObject("dead_zombie");
        SpriteRenderer renderer = dead_zombie.AddComponent<SpriteRenderer>();

        renderer.sprite = deadBody;
        renderer.sortingOrder = 109;

        dead_zombie.transform.position = this.gameObject.transform.position;
        dead_zombie.transform.rotation = this.gameObject.transform.rotation;
        dead_zombie.gameObject.SetActive(true);
        dead_zombie.transform.SetParent(GameObject.Find("Container").transform);

        dead_zombie.transform.localScale = this.transform.localScale;
        renderer.material = GetComponent<SpriteRenderer>().material;
        //DROP SYSTEM WILL BE WRITTEN HERE FURTHER
        int coin_count = (int)UnityEngine.Random.Range(2.0f, 7.0f);

        for (int i = 0; i < coin_count; i++)
        {
            if (i % 2 == 0)
            {
                Instantiate(coin, new Vector3(transform.position.x + i * 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(coin, new Vector3(transform.position.x - i * 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
            }
        }
        Instantiate(arrowSack, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);


    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        Rigidbody2D throwableObjectBody = collidedObject.GetComponent<Rigidbody2D>();
        if (collidedObject.tag.Equals("Throwable"))
        {
            animator.SetBool("takingDamage", true);
            ThrowableController throwableController = collidedObject.GetComponent<ThrowableController>();
            damage = throwableController.GetDamage();
            TakeDamage(damage);
        }

    }
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("NPC"))
        {
            stopForAttack = false;
            animator.SetBool("attack", false);
        }
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("NPC"))
        {
            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(MeleeAttack(collision.gameObject));
            }

        }
    }

    IEnumerator MeleeAttack(GameObject gameObject)
    {
        attackCooledDown = false;
        length = 0;
        stopForAttack = true;
        lockedTarget = false;
        animator.SetBool("attack", true);
        nextAttackTime = Time.time + 1.0f / attackRate;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<CharacterController>().TakeDamage(2f);
        
        attackCooledDown = true;
        yield break;
    }









}
