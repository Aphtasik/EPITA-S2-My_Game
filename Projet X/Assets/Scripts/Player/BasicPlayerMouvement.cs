﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class BasicPlayerMouvement : MonoBehaviour
{
    // Start is called before the first frame update

    
    public Rigidbody2D rb;
    Vector2 mouvement;
    float moveSpeed = 10f;
    [SerializeField] float DashForce = 10f;
    

    // Update is called once per frame
    void Update()
    {
        Mouvement();
        //Dash();
    }

   /* void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.forward * DashForce);
        }
    }*/
        
    private void Mouvement()
    {
        mouvement.x = Input.GetAxisRaw("Horizontal");
        mouvement.y = Input.GetAxisRaw("Vertical");
         
    }


}