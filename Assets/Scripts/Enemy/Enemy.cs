using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected float health, damage, damageTimer;

    public GameObject damagePopUp;
    public GameObject coin , arrowSack;
    public Sprite deadBody;
    protected Rigidbody2D player;
    protected Animator animator;
    public ParticleSystem fluidParticles, explosion;
    protected bool stopForAttack;
    protected float length;
    protected float meleeDamageTakeInterval = 0.9f;
   protected void Init()
    {
       
        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();

    }


    protected void HandleNewPath()
    {
        if (length > -0.01 && length < 0.01)
        {
            length = UnityEngine.Random.Range(-1, 2);

        }
    }


    protected void HandleDamageAssesment()
    {
        if (damage != 0)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer < 0)
            {
                damageEnd();
                damageTimer = 0.717f;
            }
        }
    }

    protected void damageEnd()
    {
        stopForAttack = false;
        damage = 0;
        animator.SetBool("takingDamage", false);


        
    }

    protected bool HandleDeath()
    {
        if (health < 0.0f)
        {
            if(explosion != null)
                explosion.transform.parent = null;
            animator.SetBool("dead", true);
            Destroy(this.gameObject, 4.0f);
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
       



        foreach (RaycastHit2D testc in sighttest)
        {
            if (testc.collider != null)
            {
                if (testc.collider.tag == "Player" && testc.distance < 6.0f)
                {

                    collider =  testc.collider.gameObject;
                }
                else if(testc.collider.tag == "EnemyTarget" && testc.distance < 10.0f)
                {
                    collider = testc.collider.gameObject;
                }

            }
        }
        GetComponent<CircleCollider2D>().enabled = true;
        return collider;

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
        int coin_count = (int)Random.Range(2.0f, 7.0f);

        for (int i = 0; i < coin_count; i++)
        {
            if(i%2 == 0)
            {
                Instantiate(coin, new Vector3(transform.position.x + i* 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(coin, new Vector3(transform.position.x - i * 0.1f, transform.position.y, transform.position.z), Quaternion.identity);

            }


        }

        Instantiate(arrowSack, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);


    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject throwableObject = collision.gameObject;
        Rigidbody2D throwableObjectBody = throwableObject.GetComponent<Rigidbody2D>();
        if (throwableObject.tag.Equals("Throwable"))
        {
            stopForAttack = true;
            animator.SetBool("takingDamage", true);
            ThrowableController throwableController = throwableObject.GetComponent<ThrowableController>();
            damage = throwableController.GetDamage();
            health -= damage;
            
            Vector3 popUpPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            GameObject _popUp = Instantiate(damagePopUp, popUpPos, Quaternion.identity);
            _popUp.GetComponent<TextMesh>().text = damage.ToString("#.00");
            _popUp.transform.SetParent(this.gameObject.transform);
            _popUp.SetActive(true);
            fluidParticles.gameObject.SetActive(true);
            fluidParticles.Play();

            length = transform.position.x- throwableObject.transform.position.x > 0 ? -1.0f : 1.0f; // change direction to damage taken side
        }

        

    }

    void OnTriggerStay2D(Collider2D collision) // timer ekle
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            GameObject throwableObject = collision.gameObject;
            if (collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if(meleeDamageTakeInterval < 0)
                {
                    Debug.Log("Girdim");
                    stopForAttack = true;
                    animator.SetBool("takingDamage", true);
                    health -= 25.0f;
                    damage = 25;
                    Vector3 popUpPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                    GameObject _popUp = Instantiate(damagePopUp, popUpPos, Quaternion.identity);
                    _popUp.GetComponent<TextMesh>().text = damage.ToString("#.00");
                    _popUp.transform.SetParent(this.gameObject.transform);
                    _popUp.SetActive(true);
                    fluidParticles.gameObject.SetActive(true);
                    fluidParticles.Play();
                    meleeDamageTakeInterval = 0.6f;
                }
                else
                {
                    meleeDamageTakeInterval -= Time.deltaTime;

                }
                

                
            }
        }
    }





}
