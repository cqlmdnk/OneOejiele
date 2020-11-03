using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update
    float length;
    public Rigidbody2D player;
    public Animator animator;
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        length = UnityEngine.Random.Range(-2, 2);
        if (length < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (length > -0.01 && length < 0.001)
        {
            length = UnityEngine.Random.Range(-5, 5);
            if (length < 0)
            {
                
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            else { 
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                
            }
        }
       
       




       


        Vector3 start = transform.position;
        Vector3 direction = player.transform.position - transform.position;

        

        //draw ray in editor
        RaycastHit2D sighttest;
        if (length < 0)
        {
            sighttest = Physics2D.Raycast(transform.position, new Vector3(-1.0f, 0.0f, 0.0f), 6.0f);

        }

        else
        {

            sighttest = Physics2D.Raycast(transform.position, new Vector3(1.0f, 0.0f, 0.0f), 6.0f);
        }
        float aggro = 0.0f;

        if (sighttest.collider != null)
        {
            if (sighttest.collider.tag == "Player")
            {
                Debug.Log("Düşman görüldü");
                animator.SetBool("attack", true);
                aggro = 0.4f;
            }
            else
            {
                //Debug.Log("Düşman görünmüyor");
                animator.SetBool("attack", false);
            }
        }
        else
        {
            //Debug.Log("Kimse görünmüyor");
        }
        Vector3 move = new Vector3(((0.1f + aggro) * Math.Sign(length)  + length / 30), 0, 0.0f);
        transform.position = transform.position + 5 * move * Time.deltaTime ;
        length -= move.x * Time.deltaTime;
    }


}
