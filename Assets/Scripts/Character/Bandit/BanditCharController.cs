using UnityEngine;
using System.Collections;
using System;

public class BanditCharController : CharacterController
{
    public float attackRange = 0.4f;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Update()
    {
        HandleAttack();
        base.Update();
        
    }

    private void HandleAttack()
    {
        if (attackCooledDown && Input.GetMouseButtonDown(0))
        {
            attackCooledDown = false;
            StartCoroutine(AttackTimer());
            FaceMeToAttackDirection();
            characterState = CharacterState.Attack;
        }
    }

    private void FaceMeToAttackDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x - transform.position.x < 0.0f)
        {
            FaceMe(false);
        }
        else if (mousePos.x - transform.position.x > 0.0f)
        {
            FaceMe(true);
        }
    }

    IEnumerator AttackTimer()
    {
        characterState = CharacterState.Idle;
        yield return new WaitForSeconds(0.5f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(25.0f);
        }
        
        attackCooledDown = true;
        
        yield break;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
