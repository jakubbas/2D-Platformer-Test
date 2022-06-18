using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool isGrounded;
    private float friction;
    [SerializeField, Range(0f, 0.15f)] private float coyoteJumpTimer = 0.1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);

    }
    //Resets values to false/0 when the player exits the collision.
    private void OnCollisionExit2D(Collision2D collision)
    {
        StartCoroutine(CoyoteJumpTimer());
        friction = 0;
    }

    private IEnumerator CoyoteJumpTimer()
    {
        yield return new WaitForSeconds(coyoteJumpTimer);
        isGrounded = false;
        yield return null;
    }

    //Checks if the collision is the floor or not. 1 = Wall, 0 = Floor.
    private void EvaluateCollision(Collision2D other)
    {
        for (int i=0; i < other.contactCount; i++)
        {
            Vector2 normal = other.GetContact(i).normal;
            isGrounded |= normal.y >= 0.9f;
            if (normal.y >= 0.9f)
            {
                StopCoroutine(CoyoteJumpTimer());
            }
        }
    }
    //Retrieves the friction from the collided material.
    private void RetrieveFriction(Collision2D other)
    {
        PhysicsMaterial2D material = other.rigidbody.sharedMaterial;
        friction = 0;
        if (material != null)
        {
            friction = material.friction;
        }
    }
    //Gets the isGrounded bool.
    public bool GetIsGrounded()
    {
        return isGrounded;
    }
    //Gets the friction float.
    public float GetFriction()
    {
        return friction;
    }
}
