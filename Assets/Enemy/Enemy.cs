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
    public ParticleSystem fluidParticles;
    protected bool stopForAttack;
    protected float length;
   protected void Init()
    {
       
        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();

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


        List<RaycastHit2D> sighttest = new List<RaycastHit2D>();
        if (length < 0)
        {

            for (int i = 0; i < 12; i++)
            {
                sighttest.Add(Physics2D.Raycast(transform.position, new Vector3(-1.0f + (i / 10.0f), (i / 10.0f), 0.0f), 6.0f));
            }

        }

        else
        {

            for (int i = 0; i < 12; i++)
            {
                sighttest.Add(Physics2D.Raycast(transform.position, new Vector3((i / 10.0f), 1.0f - (i / 10.0f), 0.0f), 6.0f));
            }
        }
       



        foreach (RaycastHit2D testc in sighttest)
        {
            if (testc.collider != null)
            {
                if (testc.collider.tag == "Player")
                {

                    return testc.collider.gameObject;
                }

            }
        }
        return null;

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
    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Throwable"))
        {
            stopForAttack = true;
            animator.SetBool("takingDamage", true);
            health -= 25.0f;
            damage = 25;
            Vector3 popUpPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            GameObject _popUp = Instantiate(damagePopUp, popUpPos, Quaternion.identity);
            _popUp.transform.SetParent(this.gameObject.transform);
            _popUp.SetActive(true);
            fluidParticles.gameObject.SetActive(true);
            fluidParticles.Play();

            length = transform.position.x-col.transform.position.x > 0 ? -1.0f : 1.0f; // change direction to damage taken side
        }
    }
   
    





}
