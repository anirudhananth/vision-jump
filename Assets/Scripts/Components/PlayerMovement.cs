using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public AudioClip hitAudio;
    public AudioClip jumpAudio;

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 20f;
    private bool isFacingRight = true;
    private bool canMove = true;
    private bool isGrounded = true;
    Vector3 movementVector;
    private int health = 4;

    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!Game.obj.GameHasStarted || Game.obj.GameIsPaused) return;
        isGrounded = IsGrounded();
        horizontal = Input.GetAxis("Horizontal");
        animator.SetBool("IsMoving", canMove && horizontal > 0.1f);
        animator.SetBool("IsGrounded", isGrounded);
        Jump();
    }

    private void FixedUpdate()
    {
        if (!Game.obj.GameHasStarted || Game.obj.GameIsPaused) return;
        if (!canMove) return;
        Vector3 movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        // rb.MovePosition(rb.position + movementVector * speed * Time.fixedDeltaTime);
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if (horizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if(!canMove) return;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
            audioSource.clip = jumpAudio;
            audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(gameObject.name == "Player" && col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<DeathHandler>().Die();
            if(health-- == 0) {
                Debug.Log("Game Over.");
                rb.velocity = Vector3.zero;
                canMove = false;
                animator.SetTrigger("Die");
                GetComponent<DeathHandler>().Die();
            }
            else
            {
                animator.SetTrigger("Hit");
                audioSource.clip = hitAudio;
                audioSource.Play();
            }
        }
    }
}
