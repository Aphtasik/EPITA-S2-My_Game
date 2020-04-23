﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//AUTHOR: Léo FERRETTI
public class Arrow : MonoBehaviour
{

    public float speed = 10f;
    public int damage = 40;
    
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
       Debug.Log(hit.name);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
