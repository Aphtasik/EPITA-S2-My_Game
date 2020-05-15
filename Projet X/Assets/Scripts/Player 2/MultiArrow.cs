﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiArrow : MonoBehaviour
{
	private BoxCollider2D bc;
    // Start is called before the first frame update
    void Awake()
    {
		bc = GetComponent<BoxCollider2D>();
    }

	private void OnTriggerEnter(Collider col)
	{
		if (col.tag == "ennemi") {
			Debug.Log("P2 arrow hit " + col.name);
			GameObject target = col.GetComponent<GameObject>();
			//ajouter qqchose qui permet de faire les dégâts
		}
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}