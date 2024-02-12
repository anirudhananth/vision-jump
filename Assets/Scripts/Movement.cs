using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    Vector3 movementVector;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        Jump();
    }

    private void FixedUpdate()
    {
        Vector3 movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        // rb.MovePosition(rb.position + movementVector * speed * Time.fixedDeltaTime);
        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, 0);
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
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            rb.velocity = new Vector3(rb.velocity.x, jumpingPower, 0);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0);
        }
    }

    private bool IsGrounded() {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
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
}
