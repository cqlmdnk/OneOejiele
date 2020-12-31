using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ActorController
{
    // Start is called before the first frame update
    protected float health, damage, damageTimer;

    public GameObject damagePopUp;
    public GameObject coin, arrowSack;
    public Sprite deadBody;
    public ParticleSystem explosion;
    protected bool stopForAttack;
    [SerializeField]
    protected float length;
    protected bool lockedTarget = false, attackCooledDown = true;
    public float attackRate = 1.0f;
    private float nextAttackTime;
    protected float distanceToEnemy;

    protected override void Update()
    {
        if (HandleDeath())
            return;

        HandleNewPath();
        HandleMovement();
    }


    protected override void Awake()
    {

        base.Awake();
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
        if (characterState != CharacterState.Idle) // if state is not idle always turn idle except falling and jumping
        {
            if (!attackCooledDown)
            {
                if (!stopForAttack || attackCooledDown)
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




    protected bool HandleDeath()
    {
        if (health < 0.0f)
        {
            if (explosion != null)
                explosion.transform.parent = null;
            charAnimator.SetBool("dead", true);
            Destroy(this.gameObject, 4.0f);
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            return true;
        }
        return false;
    }

    protected GameObject SightCheck() // check for Player NPC and King
    {
        if (GetComponent<CircleCollider2D>() != null)
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
                if (raycast.collider.tag == "Player" && raycast.distance < 4.0f)
                {

                    collider = raycast.collider.gameObject;
                    distanceToEnemy = raycast.distance;
                }
                else if (raycast.collider.tag == "WallsTowers" && raycast.distance < 10.0f)
                {
                    collider = raycast.collider.gameObject;
                    distanceToEnemy = raycast.distance;
                }
                else if (raycast.collider.tag == "King" && raycast.distance < 12.0f)
                {
                    collider = raycast.collider.gameObject;
                    distanceToEnemy = raycast.distance;
                }

            }
        }
        if (GetComponent<CircleCollider2D>() != null)
            GetComponent<CircleCollider2D>().enabled = true;
        return collider;
    }

    protected void DetermineDirection(float moveX)
    {
        if (moveX < 0)
        {
            FaceMe(false);
        }
        else
        {
            FaceMe(true);
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

        charAnimator.SetTrigger("takingDamage");
        health -= amount;
        damage = amount;
        Vector3 popUpPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        GameObject _popUp = Instantiate(damagePopUp, popUpPos, Quaternion.identity);
        _popUp.GetComponent<TextMesh>().text = damage.ToString("#.00");
        _popUp.transform.SetParent(this.gameObject.transform);
        _popUp.SetActive(true);
        charAnimator.ResetTrigger("takingDamage");
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
            charAnimator.SetBool("takingDamage", true);
            ThrowableController throwableController = collidedObject.GetComponent<ThrowableController>();
            damage = throwableController.GetDamage();
            TurnToAttacker(collidedObject);
            TakeDamage(damage);
        }
        

    }

    private void TurnToAttacker(GameObject collidedObject)
    {
        if (collidedObject.transform.position.x < transform.position.x)
        {
            length = length < 0 ? length : -length;
        }
        else
        {
            length = length < 0 ? -length : length;
        }
    }
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("NPC") || collision.gameObject.tag.Equals("King") || collision.gameObject.tag.Equals("WallsTowers"))
        {
            stopForAttack = false;
            charAnimator.SetBool("attack", false);
        }
        
    }
    protected void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("NPC") || collision.gameObject.tag.Equals("King") || collision.gameObject.tag.Equals("WallsTowers"))
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
        charAnimator.SetBool("attack", true);
        nextAttackTime = Time.time + 1.0f / attackRate;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<HealthController>().TakeDamage(2f);
        
        attackCooledDown = true;
        yield break;
    }









}
