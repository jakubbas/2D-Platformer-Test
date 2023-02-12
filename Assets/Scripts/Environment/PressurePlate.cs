using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Start is called before the first frame update
    private bool activated = false;
    private Animation animations;

    //Gameobject list.
    private List<GameObject> objectsTouchingPlate = new List<GameObject>();

    private void Start()
    {
        animations = GetComponent<Animation>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.GetComponent<TriggersPlate>() != null)
        {
            if (objectsTouchingPlate.Count == 0)
            {
                PlateDown();
            }
            objectsTouchingPlate.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<TriggersPlate>() != null)
        {
            objectsTouchingPlate.Remove(other.gameObject);
            if (objectsTouchingPlate.Count == 0)
            {
                PlateUp();
            }
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
