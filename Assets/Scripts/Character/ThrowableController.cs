using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    // Start is called before the first frame update
    protected float damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        
        transform.rotation = Quaternion.Euler(0, 0, ((float)Math.Atan2((double)GetComponent<Rigidbody2D>().velocity.y, (double)GetComponent<Rigidbody2D>().velocity.x)) * Mathf.Rad2Deg);

    }
    public float GetDamage()
    {
        return damage;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
