using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer helment;

    public float moveSpeed = 5f;
    public float digSpeed = 3f;
    public Transform movePoint;

    public LayerMask Obstacle, Dirt;

    public bool obsticlePresent;

    //Velocity Check
    public Vector3 startPos, velocity;

    public enum PlayerState
    {
        Idle,
        Moving,
        Digging,
        Mining,
        Hurt,
        Death,
    };

    [SerializeField]
    private PlayerState _playerState;

    [SerializeField]
    private AudioSource _walkSFX, _digSFX;

    void Awake()
    {
        startPos = transform.position;
    }

    void Start()
    {
        movePoint.parent = this.transform.parent;
    }

   void Update()
    {

        MovePlayer();
        PlayerMoveDirection();

        switch (_playerState)
        {
            case PlayerState.Idle:
                GetComponent<Animator>().Play("Base Layer.Player_Idle", 0);
                StopAllSFX();
                break;

            case PlayerState.Moving:
                GetComponent<Animator>().Play("Base Layer.Player_Move", 0);
                moveSpeed = 5f;
                if (!_walkSFX.isPlaying)
                {
                    _walkSFX.Play();
                }
                break;

            case PlayerState.Digging:
                /*moveSpeed = 3f;
                if (!_walkSFX.isPlaying)
                {
                    _walkSFX.Play();
                }*/
                break;

            default:
                break;

        }
    }

    public void MovePlayer()
    {
        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, Obstacle)
            || !Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, Obstacle))
        {
            
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            //Get Velocity
            velocity = ((transform.position - startPos) / Time.deltaTime).normalized;
            startPos = transform.position;

            if (obsticlePresent) return;

            if(transform.position == movePoint.position)
            {
                _playerState = PlayerState.Idle;
            }
            else
            {
                _playerState = PlayerState.Moving;
            }
            
        }
        else
        {
            _playerState = PlayerState.Idle;
        }

/*        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, Dirt)
            || !Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, Dirt))
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, digSpeed * Time.deltaTime);

        }*/

    }

    public void PlayerMoveDirection()
    {
        Vector3 previousPos = movePoint.position;

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);

                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    helment.flipX = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    helment.flipX = false;
                }
                
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }

            if (Physics2D.OverlapCircle(movePoint.position, .2f, Dirt))
            {
                FindObjectOfType<TilePallet_Behavior>().DigTile(Vector3Int.RoundToInt(movePoint.localPosition));
                _digSFX.Play();
            }

            if (Physics2D.OverlapCircle(movePoint.position, .2f, Obstacle))
            {
                movePoint.position = previousPos;
            }
        }
    }

    public void StopAllSFX()
    {
        foreach (AudioSource sfx in GetComponentsInChildren<AudioSource>())
        {
            sfx.Stop();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Root"))
        {
            collision.gameObject.GetComponent<Root_Object>().HitRoot();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {

    }

}



