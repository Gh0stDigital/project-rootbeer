using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    public Transform playerTargetPoint;

    public LayerMask Obstacle;
    public LayerMask Dirt;

    public bool obstaclePresent = false;

    public bool isAimless = true;
    public bool isChasing;

    public bool moveRight = true;

    public ParticleSystem digEff;

 
    void Start()
    {
        digEff.Pause();
    }

   void Update()
    {
        if (isAimless)
        {
            if (Physics2D.OverlapCircle(transform.position, 0.1f, Obstacle) || Physics2D.OverlapCircle(transform.position, 0.1f, Dirt))
            {
                moveRight = !moveRight;
            }

            if (moveRight)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
                GetComponent<SpriteRenderer>().flipX= false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
                GetComponent<SpriteRenderer>().flipX = true;

            }
           
        }

        if (isChasing)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTargetPoint.position, moveSpeed * Time.deltaTime);

            if (Physics2D.OverlapCircle(transform.position, 0.1f, Obstacle) || Physics2D.OverlapCircle(transform.position, 0.1f, Dirt))
            {
                moveSpeed = 1f;
                digEff.Play();
            }
            else
            {
                digEff.Pause();
                moveSpeed = 1.7f;
            }
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

