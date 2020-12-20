using UnityEngine;
using System.Collections;
using System;

public class BanditCharController : CharacterController
{
    private bool m_attackReverse = false;
    void Start()
    {
        base.Start();

    }
    private new void Update()
    {
        base.Update();
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !m_attackCooledDown)
        {
            m_attackCooledDown = true;
            StartCoroutine(AttackTimer());
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = (float)Math.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x + 0.5f) * Mathf.Rad2Deg;
            m_characterState = CharacterState.Attack;
        }
    }
    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(0.5f);
        m_attackCooledDown = false;
    }
}
