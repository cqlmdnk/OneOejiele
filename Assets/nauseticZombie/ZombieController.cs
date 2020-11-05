using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update
    float length;
    public Sprite deadBody;
    public Rigidbody2D player;
    public Animator animator;
    private bool stopForAttack;
    private float damage;
    private float health  = 100;
    private float damageTimer;
    public float zombieRecoverTime;
    public GameObject damagePopUp;
   
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        length = UnityEngine.Random.Range(-2, 2);
        if (length < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        damageTimer = zombieRecoverTime;


    }

    // Update is called once per frame
    void Update()
    {

        if(health <= 0)
        {
            animator.SetBool("dead", true);
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
           
            Destroy(this.gameObject,5.0f);
        }
        else
        {
            if (length > -0.01 && length < 0.001)
            {
                length = UnityEngine.Random.Range(-5, 5);
                if (length < 0)
                {

                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }

                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);

                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_melee"))
            {
                float harmonicForce = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % (animator.GetCurrentAnimatorStateInfo(0).length * 2);
                transform.position = new Vector3(transform.position.x + (harmonicForce / 40.0f) - 0.025f, transform.position.y, transform.position.z);
            }


            if (damage != 0)
            {
                damageTimer -= Time.deltaTime;
                if (damageTimer < 0)
                {
                    damageEnd();
                    damageTimer = 0.717f;
                }
            }



            Vector3 start = transform.position;
            Vector3 direction = player.transform.position - transform.position;



            //ray casting 90 degrees in 10 segments with respect to facing
            List<RaycastHit2D> sighttest = new List<RaycastHit2D>();
            if (length < 0)
            {

                for (int i = 0; i < 11; i++)
                {
                    sighttest.Add(Physics2D.Raycast(transform.position, new Vector3(-1.0f + (i / 10.0f), (i / 10.0f), 0.0f), 6.0f));
                }

            }

            else
            {

                for (int i = 0; i < 10; i++)
                {
                    sighttest.Add(Physics2D.Raycast(transform.position, new Vector3((i / 10.0f), 1.0f - (i / 10.0f), 0.0f), 6.0f));
                }
            }
            float aggro = 0.0f;



            foreach (RaycastHit2D testc in sighttest)
            {
                if (testc.collider != null)
                {
                    if (testc.collider.tag == "Player")
                    {

                        aggro = 0.4f;
                    }

                }
            }

            if (!stopForAttack)
            {
                Vector3 move = new Vector3(((0.1f + aggro) * Math.Sign(length) + length / 30), 0, 0.0f);
                transform.position = transform.position + 5 * move * Time.deltaTime;
                length -= move.x * Time.deltaTime;
            }
        }
        
       
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {

            stopForAttack = true;
            animator.SetBool("attack", true);
        }
        else if (col.gameObject.tag.Equals("Throwable"))
        {
            stopForAttack = true;
            animator.SetBool("takingDamage", true);
            health -= 25.0f;
            damage = 25;
            Vector3 popUpPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            GameObject _popUp = Instantiate(damagePopUp, popUpPos, Quaternion.identity);
            _popUp.transform.SetParent(this.gameObject.transform);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            stopForAttack = false;
            animator.SetBool("attack", false);
        }
        
    }
    void damageEnd()
    {
        stopForAttack = false;
        animator.SetBool("takingDamage", false);
        
        
        damage = 0;
    }
    void OnDestroy() // creating dead body
    {

        GameObject dead_zombie = new GameObject("dead_zombie");
        SpriteRenderer renderer = dead_zombie.AddComponent<SpriteRenderer>();
        renderer.sprite = deadBody;
        renderer.sortingOrder = 109;
        dead_zombie.transform.position = this.gameObject.transform.position;
        dead_zombie.transform.rotation = this.gameObject.transform.rotation;
        dead_zombie.gameObject.SetActive(true);
       
    }

 }
