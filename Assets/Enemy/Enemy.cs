using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected float health, damage, damageTimer;
    protected GameObject damagePopUp;
    public Sprite deadBody;
    protected Rigidbody2D player;
    protected Animator animator;
    public ParticleSystem fluidParticles;
    protected bool stopForAttack;
    protected float length;
   protected void Init()
    {
        damagePopUp = GameObject.Find("/DamagePopUp");
        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();

    }

    protected bool SightCheck()
    {
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
       



        foreach (RaycastHit2D testc in sighttest)
        {
            if (testc.collider != null)
            {
                if (testc.collider.tag == "Player")
                {

                    return true;
                }

            }
        }
        return false;

    }


    protected void OnBeforeDestroy() // creating dead body
    {

        GameObject dead_zombie = new GameObject("dead_zombie");
        SpriteRenderer renderer = dead_zombie.AddComponent<SpriteRenderer>();
        renderer.sprite = deadBody;
        renderer.sortingOrder = 109;
        dead_zombie.transform.position = this.gameObject.transform.position;
        dead_zombie.transform.rotation = this.gameObject.transform.rotation;
        dead_zombie.gameObject.SetActive(true);
        dead_zombie.transform.SetParent(GameObject.Find("Container").transform);
    }



   
    // Update is called once per frame

}
