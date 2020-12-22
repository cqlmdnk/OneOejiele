using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WizardCharController : CharacterController
{
    // Start is called before the first frame update
    
    public GameObject MagicSpell;
    private float Attack1Time = 0.7f;
    void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }
    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !attackCooledDown)
        {
            if (mousePos.x > transform.position.x)
                FaceMe(true);
            else
                FaceMe(false);
            characterState = CharacterState.Attack;
            attackCooledDown = true;
            StartCoroutine(Attack1Timer());
        }
        else if (Input.GetMouseButtonDown(1) && !attackCooledDown)
        { // will be initiate the AttackTimer2 in future
            characterState = CharacterState.Dash;
            attackCooledDown = true;
            StartCoroutine(Attack1Timer());
        }
    }
    private void HandleCastSpell(float angle, Vector3 SpellPosition)
    {
        GameObject _MagicSpell = Instantiate(MagicSpell, SpellPosition, Quaternion.AngleAxis(angle, Vector3.forward));
        _MagicSpell.GetComponent<MagicSpellController>().SetDamage(25.0f);
        _MagicSpell.SetActive(true);
        Rigidbody2D _MagicSpellBody = _MagicSpell.GetComponent<Rigidbody2D>();
        Vector3 veloctiy3d = Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3(8.0f, 0, 0);
        _MagicSpellBody.velocity = (new Vector2(veloctiy3d.x, veloctiy3d.y));
    }
    IEnumerator Attack1Timer()
    {
        Vector3 SpellPosition;
        ///BUGGGGG
        if (facingRight)
        {
            SpellPosition = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        }
        else
        {
            SpellPosition = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
        }
        float angle = (float)Math.Atan2(mousePos.y - SpellPosition.y, mousePos.x - SpellPosition.x) * Mathf.Rad2Deg;
        yield return new WaitForSeconds(Attack1Time);
        HandleCastSpell(angle, SpellPosition);
        attackCooledDown = false;
        yield break;
    }

}

