using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]

public class PlayerData : ScriptableObject
{
    public InputController inputController;

    [Header("Jump Variables")]

    [SerializeField, Range(0f, 100f)] public float jumpForce;
    [SerializeField, Range(0f, 100f)] public float jumpCutMultiplier;
    [SerializeField, Range(0f, 0.5f)] public float earlyInputForgiveness;
    [SerializeField, Range(0f, 0.15f)] public float coyoteJumpTimer;

    [Space(20)]


    [Header("Movement Variables")]

    [SerializeField, Range(0f, 100f)] public float maxSpeed;
    [SerializeField, Range(0f, 100f)] public float maxAcceleration;
    [SerializeField, Range(0f, 100f)] public float maxAirAcceleration;




}