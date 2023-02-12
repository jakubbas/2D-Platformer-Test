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
    private float earlyInputForgivenessTimer;

    [SerializeField, Range(0f, 100f)] private float jumpCutMultiplier = 2f;

    private bool isRunning = false;
    //private bool tempDisable = false;

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

        earlyInputForgivenessTimer = earlyInputForgiveness;
    }

    
    void Update()
    {    
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
    }

    private void JumpAction()
    {
        //If the player is grounded or has air jumps left.
        if ((isGrounded || jumpPhase < maxAirJumps))
        {
            //Reset Coroutine.
            StopCoroutine(InputForgivenessTimer());
            //StopAllCoroutines();
            isRunning = false;
            earlyInputForgivenessTimer = earlyInputForgiveness;

            //Increase jump count.
            jumpPhase += 1;

            AddJumpVelocity();
        }
        //Saving jump input if the player is about to land. Early Input Forgiveness.
        else if (!isGrounded && !isRunning)
        {
            StartCoroutine(InputForgivenessTimer());
        }
    }

    private IEnumerator InputForgivenessTimer()
    {
        isRunning = true;
        while (earlyInputForgivenessTimer > 0f)
        {
            yield return new WaitForSeconds(0.05f);
            earlyInputForgivenessTimer -= 0.05f;
            JumpAction();
        }
        isRunning = false;
        yield break;
    }

    private void AddJumpVelocity()
    {
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
        rb.velocity = velocity;

    }

    //private IEnumerator InputForgivenessTimer()
    //{
    //    isRunning = true;
    //    float tempTimer = earlyInputForgiveness;
        
    //    while (tempTimer > 0f)
    //    {
    //        tempDisable = true;
    //        //Debug.Log(tempTimer);
    //        yield return new WaitForSeconds(0.05f);
    //        tempTimer -= 0.05f;
    //        if (isGrounded)
    //        {
    //            //Increase jump count.
    //            jumpPhase += 1;
    //            //Formula for jump height, taking in the gravity and set jump height values.

    //            AddJumpVelocity();

    //            isRunning = false;
    //            tempDisable = false;
    //            yield break;
    //        }

    //        //if (!isGrounded)
    //        //{
    //        //    tempDisable = false;
    //        //}

    //    }

    //    tempDisable = false;
    //    isRunning = false;
    //    yield break;
    //}
}
