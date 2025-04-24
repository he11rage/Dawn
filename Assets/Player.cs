using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    private float _movement;
    public float moveSpeed = 2f;
    public float jumpHeight = 3.5f;
    private bool _facingRight = true;
    public Rigidbody2D rb;
    //private bool _isGrounded;
    private bool _jumpRequested;
    public LayerMask groundLayer;
    public float checkGroundDistance = 0.35f;
    

    void Start()
    {
    }

    void Update()
    {
        _movement = Input.GetAxis("Horizontal");

        if (_movement < 0f && _facingRight)
        {
            transform.eulerAngles += new Vector3(0f, 180f, 0f);
            _facingRight = !_facingRight;
        }
        else if (_movement > 0f && !_facingRight)
        {
            transform.eulerAngles -= new Vector3(0f, 180f, 0f);
            _facingRight = !_facingRight;
        }

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            _jumpRequested = true;
        }

        if (Math.Abs(_movement) > .1f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetMouseButtonDown(0) && Math.Abs(_movement) == 0)
        {
            animator.SetTrigger("Attack");
        } 
        else if (Input.GetMouseButtonDown(0) && Math.Abs(_movement) > 0)
        {
            animator.SetTrigger("Running_Attack");
        }
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.right * (_movement * Time.fixedDeltaTime * moveSpeed);

        if (_jumpRequested)
        {
            animator.SetTrigger("Jump");
            Jump();
            _jumpRequested = false;
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, checkGroundDistance, groundLayer);
        return hit.collider != null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down * checkGroundDistance, Color.yellow); 
    }
}