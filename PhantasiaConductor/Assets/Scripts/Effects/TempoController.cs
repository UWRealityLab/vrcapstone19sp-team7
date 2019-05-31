using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoController : MonoBehaviour
{
    public AudioSource normal;
    public float changeDuration = 0.3f;
    public float fastPitch = 1.2f;
    public float slowPitch = 0.8f;

    private void Start()
    {
    }

    private IEnumerator shiftTempo(float initial, float final)
    {
        float counter = 0;
        while (normal.pitch != final && counter < changeDuration)
        {
            counter += Time.deltaTime;

            normal.pitch += Time.deltaTime * (final - initial) / changeDuration;
            yield return null; ;
        }
    }
}
