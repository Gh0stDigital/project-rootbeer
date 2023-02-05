using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Transform playerTargetPoint;

    public LayerMask Obstacle;

    public bool obstaclePresent = false;

    public bool isAimless = true;
    public bool isChasing;

    public bool moveRight = true;
 
    void Start()
    {

    }

   void Update()
    {
        if (isAimless)
        {
            if (Physics2D.OverlapCircle(transform.position, 0.1f, Obstacle))
            {
                moveRight = !moveRight;
            }

            if (moveRight)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
            }
           
        }

        if (isChasing)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTargetPoint.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isAimless = false;
            isChasing = true;


            Debug.Log("player detected");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            isAimless = true;
            isChasing = false;


            Debug.Log("player no longer detected");
        }
    }
}

