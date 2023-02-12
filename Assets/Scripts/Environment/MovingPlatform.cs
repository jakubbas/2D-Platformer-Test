using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)] private float platformSpeed = 2f;
    [SerializeField, Range(0f, 10f)] private float waitTime = 1f;
    private Vector3 startPos;
    private Vector3 targetPos;
    [SerializeField] private GameObject target;
    //private List<GameObject> targets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        targetPos = target.transform.position;
        StartCoroutine(Vector3LerpCoroutine(this.gameObject, targetPos, platformSpeed));
    }

    IEnumerator Vector3LerpCoroutine(GameObject obj, Vector3 target, float speed)
    {
        Vector3 startPosition = obj.transform.position;
        float time = 0f;

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.Lerp(startPosition, target, (time / Vector3.Distance(startPosition, target)) * speed);
            time += Time.deltaTime;
            yield return null;
        }

        if (obj.transform.position == target)
        {
            yield return new WaitForSeconds(waitTime);
            SwitchTargets(obj.transform.position);
            yield break;
        }
    }

    void SwitchTargets(Vector3 currentPos)
    {
        if (startPos == currentPos)
        {
            StartCoroutine(Vector3LerpCoroutine(this.gameObject, targetPos, platformSpeed));
        }

        else if (targetPos == currentPos)
        {
            StartCoroutine(Vector3LerpCoroutine(this.gameObject, startPos, platformSpeed));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SticksToPlatform>() != null)
        {
            other.gameObject.transform.parent = this.transform;
            other.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.GetComponent<SticksToPlatform>() != null)
        {
            other.gameObject.transform.parent = null;
            other.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(gameObject.transform.position, target.transform.position);
    }

}
