using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxHealth;
    private float healthBarScale;
    public Transform healthBar;
    [SerializeField]
    protected float health;
    protected float damage = 0;
    protected bool damageTaken = false;
    void Awake()
    {
        if (healthBar == null)
            healthBar = GameObject.FindGameObjectWithTag("HealthBarPlayer").transform;
        
        health = maxHealth;
        healthBarScale = healthBar.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amount) // tried to do damages discrete like other platformers
    {
        if (health < 0)
        {
            Debug.Log("karakter öldü");
            //die();
        }
        else
        {
            healthBar.localScale -= new Vector3(amount * healthBarScale / maxHealth, 0.0f, 0.0f);
            health -= amount;
        }
    }
    public void AddHealth(float addedHealth)
    {
        if (health + addedHealth <= 100)
        {
            health += addedHealth;
            healthBar.localScale += new Vector3(addedHealth * 55 / 100, 0.0f, 0.0f);

        }
        else
        {
            health = 100.0f;
            healthBar.localScale = new Vector3(55, 10.5f, 1.0f);
        }

    }
    public float GetHealth()
    {
        return health;
    }
}
