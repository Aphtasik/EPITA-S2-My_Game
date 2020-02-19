﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions; //positions de départ ou spawn la 1ere salle
    public GameObject[] rooms; //index: 0 = LR, 1 = LRB, 2 = LRT, 3 = LRTB
    public GameObject[] ground; //sol à faire spawn

    public GameObject[] perpective;

    /* direction dans laquelle gen les salles :
     - 1 et 2 -> spawn a droite
     - 2 et 3 -> spawn a gauche
     - 5 -> bouge en bas
        ----> plus de chance que la room spawn sur un coté */
    private int direction;
    
    public float distanceBtwRooms; //distance entre les salles
    
    //temps entre room spawn pour pas creer de pb de chevauchement ou autres
    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;
    
    //def zone de génération des salles
    public float Xmin;
    public float Xmax;
    public float Ymin;

    //bool qui dis qd ne plus générer de salle
    public bool stopGen;

    public LayerMask room;

    private int downCounter;
    private void Start()
    {
        SpawnGround();
        SpawnWalls();
    }

    private void Update()
    {
        if (timeBtwRoom <= 0 && stopGen == false) //quand timeBtwRoom est à 0, appelle Move qui spawn la prochaine room et reset timer
        {
            RoomGeneration();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime; //reduit le timebtwroom
        }
    }
    
    //creer une fonction qui cree un salle de base
    private void SpawnPerspective()
    {

        for (double i = Xmin-4.5 ; i < Xmax+5.5; i++)
        {
            for (double j = Ymin-4.5; j < -Ymin+5.5; j++)
            {
                Vector2 pos = new Vector2((float) i,(float) j);
                transform.position = pos;
                
                int rand = Random.Range(0, ground.Length);
                Instantiate(perpective[rand], transform.position, Quaternion.identity);
            }
        }
    }

    private void SpawnGround()
    {
        for (double i = Xmin-4.5 ; i < Xmax+5.5; i++)
        {
            for (double j = Ymin-4.5; j < -Ymin+5.5; j++)
            {
                Vector2 pos = new Vector2((float) i,(float) j);
                transform.position = pos;
                
                int rand = Random.Range(0, ground.Length);
                Instantiate(ground[rand], transform.position, Quaternion.identity);
            }
        }
    }

    private void SpawnWalls()
    {
        int startPosition = Random.Range(0, startingPositions.Length); //prend un indexe de position de départ parmis toutes les pos possibles
        transform.position = startingPositions[startPosition].position; //set la position du start pos comme base du transform
        Instantiate(rooms[0], transform.position, Quaternion.identity); //instancie la room de spawn

        direction = Random.Range(1, 6); //direction ou sera la prochaine salle
        
        RoomGeneration();
    }
    
    /*il faut creer une fonction qui va génerer les bloc perspective :
      - un bloc perspective est créé sous un mur si le bloc en dessous n'est pas un mur*/
    
    //bouge la prochaine salle générer vers une direction, a x bloc plus loin
    private void RoomGeneration()
    {
        if (direction == 1 || direction == 2) //bouge à droite
        {
            if (transform.position.x < Xmax) //si la prochaine pos est inferieur à Xmax alors c'est ok, on génère random
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x + distanceBtwRooms, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                //si le prochaine direction est gauche, conflit! La gen ferai droite gauche et stackerai 2 salles
                if (direction == 3) //si 3 on bouge droite
                {
                    direction = 2;
                }
                else if (direction == 4) //si 4 on bouge en bas
                {
                    direction = 5;
                }
            }
            else //si la prochaine pos et collé à Xmax on est obligé de descendre pour rester dans les limites
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4)//bouge à gauche
        {
            if (transform.position.x > Xmin) //si la prochaine pos est superieur à Xmin alors c'est ok, on génère random
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - distanceBtwRooms, transform.position.y);
                transform.position = newPos;
                
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);
            }
            else //si la prochaine pos et collé à Xmin on est obligé de descendre pour rester dans les limites
            {
                direction = 5;
            }
        }
        else if (direction == 5)//bouge en bas
        {
            downCounter++; //on descend 1 fois = incrementation
            
            if (transform.position.y > Ymin)//si la prochaine pos est superieur à Ymin alors c'est ok, on génère random
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                
                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {
                    if (downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                    }
                }
                
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - distanceBtwRooms);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
            }
            else //si la prochaine pos et collé à Ymin c'est que l'on a fini de generer
            {
                stopGen = true;
            }
        }
    }
}
