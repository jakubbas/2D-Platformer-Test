using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float jumpHeight = 2f;
    [SerializeField, Range(0f, 5f)] private int maxAirJumps = 0;
    //Downward force when falling.
    [SerializeField, Range(0f, 100f)] private float downForce = 3f;
    //Upward force when jumping.
    [SerializeField, Range(0f, 100f)] private float upForce = 2f;

    [SerializeField, Range(0f, 0.5f)] private float earlyInputForgiveness = 0.1f;

    private Rigidbody2D rb;
    private Ground ground;
    private Vector2 velocity;

    private int jumpPhase;
    private float defaultGravityScale;

    private bool desiredJump;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        //Default gravity when on the ground.
        defaultGravityScale = 1f;
    }

    
    void Update()
    {
        //Check if the player wants to jump.
        desiredJump |= input.RetrieveJumpInput();

    }

    private void FixedUpdate()
    {
        isGrounded = ground.GetIsGrounded();       
        velocity = rb.velocity;

        if (isGrounded)
        {
            jumpPhase = 0;
        }

        if (desiredJump)
        {
            desiredJump = false;
            JumpAction();
        }

        if (rb.velocity.y > 0f)
        {
            rb.gravityScale = upForce;
        }

        else if (rb.velocity.y < 0f)
        {

            rb.gravityScale = downForce;
        }

        else if (rb.velocity.y == 0f)
        {
            rb.gravityScale = defaultGravityScale;
        }

        rb.velocity = velocity;
        Debug.Log(rb.velocity);
    }

    private void JumpAction()
    {
        //If the player is grounded or has air jumps left.
        if (isGrounded || jumpPhase < maxAirJumps)
        {
            //Increase jump count.
            jumpPhase += 1;

            //Formula for jump height, taking in the gravity and set jump height values.
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);

            //Check if the jump speed never goes negative due to the downward gravity.
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            //If the velocity is downward, clear the momentum. (Fixes double jump having less power while falling.)
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.velocity.y);
            }
            velocity.y += jumpSpeed;
        }
        //Saving jump input if the player is about to land. Early Input Forgiveness.
        else if (!isGrounded)
        {
            StartCoroutine(InputForgivenessTimer());
        }
    }

    private IEnumerator InputForgivenessTimer()
    {
        float tempTimer = earlyInputForgiveness;
        
        while (tempTimer > 0f)
        {
            //Debug.Log(tempTimer);
            yield return new WaitForSeconds(0.05f);
            tempTimer -= 0.05f;
            if (isGrounded)
            {
                //Increase jump count.
                jumpPhase += 1;

                //Formula for jump height, taking in the gravity and set jump height values.
                float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
                velocity.y += jumpSpeed;
                rb.velocity = velocity;
                //JumpAction();
                yield break;
            }
        }
        yield break;
    }
}
