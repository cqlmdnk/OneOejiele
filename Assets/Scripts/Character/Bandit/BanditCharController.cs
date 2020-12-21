using UnityEngine;
using System.Collections;
using System;

public class BanditCharController : CharacterController
{
    public float attackRange = 0.4f;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    void Start()
    {
        base.Start();

    }
    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            StartCoroutine(AttackTimer());
           
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = (float)Math.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x + 0.5f) * Mathf.Rad2Deg;
            characterState = CharacterState.Attack;
        }
    }
    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(0.5f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(25.0f);
        }

        
        yield break;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
