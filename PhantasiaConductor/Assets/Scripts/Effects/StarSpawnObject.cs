using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawnObject : MonoBehaviour
{
    public float lifetime = 5f;

    private Vector3 prevPosition;
    private float cumVelocity;
    private int sampleCount;

    private void OnEnable()
    {
        cumVelocity = 0;
        prevPosition = transform.position;
        sampleCount = 0;
    }

    private void FixedUpdate()
    {
        cumVelocity += (transform.position - prevPosition).magnitude / Time.deltaTime;
        prevPosition = transform.position;
        sampleCount++;
    }

    public void Detach()
    {
        /*
        Vector3 newV = transform.forward.normalized;
        if (transform.parent != null)
        {
            newV = transform.parent.transform.forward.normalized;
        }
        newV = newV * (cumVelocity / (1.0f * sampleCount));
        */

        // transform.parent = null;
        // GetComponent<Rigidbody>().velocity = newV;
        Invoke("EndOfLife", lifetime);
    }

    private void EndOfLife()
    {
        Destroy(gameObject);
    }
}
