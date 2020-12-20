﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpellController : ThrowableController
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,10.0f);
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
            Destroy(this.gameObject);
    }
}
