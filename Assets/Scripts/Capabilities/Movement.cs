using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    private InputController input = null;
    private float maxSpeed = 4f;
    private float maxAcceleration = 35f;
    private float maxAirAcceleration = 20f;

    private Vector2 direction;
    private Vector2 newVelocity;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private Ground ground;

    private float maxSpeedChange;
    private float acceleration;
    private bool isGrounded;

    // Start is called before the first frame update
    void Awake()
    {
        input = data.inputController;
        maxSpeed = data.maxSpeed;
        maxAcceleration = data.maxAcceleration;
        maxAirAcceleration = data.maxAirAcceleration;


        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = input.RetrieveMoveInput();
        //Retrieve a new velocity multiplied by the friction and the max speed, as long as it doesn't go belong 0.
        newVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);

        //if (groundTag == "MovingPlatform")
        //{
        //    OnMovingObject();
        //}

    }



    private void FixedUpdate()
    {
        isGrounded = ground.GetIsGrounded();
        velocity = rb.velocity;

        //If is grounded is true, use max acceleration. If it isn't true, use max air acceleration.
        acceleration = isGrounded ? maxAcceleration : maxAirAcceleration;

        maxSpeedChange = acceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, newVelocity.x, maxSpeedChange);

        rb.velocity = velocity;


    }
}
