using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Start is called before the first frame update
    private bool activated = false;
    private Animation animations;

    private void Start()
    {
        animations = GetComponent<Animation>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlateDown();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlateUp();
        }
    }

    private void PlateDown()
    {
        if (!activated)
        {
            animations.Play("PressurePlateDown");
            activated = true;
        }

    }

    private void PlateUp()
    {
        if (activated)
        {
            animations.Play("PressurePlateUp");
            activated = false;
        }
    }
}
