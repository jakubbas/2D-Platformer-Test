using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedJump : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float jumpForce = 2f;
    [SerializeField, Range(0f, 100f)] private float jumpCutMultiplier = 2f;




    private Ground ground;
    private bool desiredJump;
    private bool isGrounded;

    private float lastPressedJumpTimer;
    private float lastOnGroundTimer;

    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
    }


    // Update is called once per frame
    void Update()
    {
        desiredJump |= input.RetrieveJumpInput();
    }


    private void Jump()
    {
        lastPressedJumpTimer = 0f;
        lastOnGroundTimer = 0f;

        float force = jumpForce;
        if (rb.velocity.y > 0)
        {
            force -= rb.velocity.y;
        }

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void JumpCut()
    {
        rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
    }
}
