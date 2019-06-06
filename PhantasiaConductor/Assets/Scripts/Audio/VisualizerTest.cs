using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VisualizerTest : MonoBehaviour
{
    private AudioSource source;
    public float amplitudeScale;

    private int sampleLength = 512;

    private float[] samples;

    private GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        samples = new float[sampleLength];
        objects = new GameObject[sampleLength];

        for (int i = 0; i < sampleLength; i++)
        {
            objects[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            objects[i].transform.parent = transform;
            objects[i].transform.localPosition = Vector3.left * i;
        }

    }

    // Update is called once per frame
    void Update()
    {
        source.GetOutputData(samples, 1);
        for (int i = 0; i < samples.Length; i++)
        {
            Vector3 curr = objects[i].transform.localPosition;
            objects[i].transform.localPosition = new Vector3(curr.x, amplitudeScale * samples[i], 0);
        }


    }
}
