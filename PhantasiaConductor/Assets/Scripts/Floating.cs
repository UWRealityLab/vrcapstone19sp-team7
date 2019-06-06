using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour
{
    // Effect to float up/down with spinning

    public float degPerSecond = 10.0f;
    public float amp = 0.5f;
    public float freq = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 temp = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        temp = posOffset;
        temp.y += Mathf.Sin(Time.fixedTime * Mathf.PI * freq) * amp;

        transform.position = temp;

        if(FindObjectOfType<FantasiaController>().tracking)
        {
            amp = 2f;
            degPerSecond = 15.0f;
            freq = 0.25f;
        }
        else
        {
            amp = 0.5f;
            degPerSecond = 10.0f;
            freq = 0.5f;
        }

    }
}
