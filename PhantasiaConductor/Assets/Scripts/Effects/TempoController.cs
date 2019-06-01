using System.Collections;
using UnityEngine;

//@COMMENTEDOUT
public class TempoController : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public AudioSource normal;
    public float changeDuration = 0.3f;
    public float fastPitch = 1.2f;
    public float slowPitch = 0.8f;
    public float slowThreshhold = 0.5f;
    public float fastThreshhold = 1.2f;
    public float stopThreshhold = 0.1f; 
    public float averagingTime = 2f;

    private float cumLeft;
    private float cumRight;
    private Vector3 prevLeft;
    private Vector3 prevRight;
    private float sampleCount;
    private bool stopped = false;

    private void OnEnable()
    {
        prevLeft = leftHand.transform.position;
        prevRight = rightHand.transform.position;
        Invoke("TrackVelocity", averagingTime);
    }

    private void TrackVelocity()
    {
        float averageL = cumLeft / sampleCount;
        float averageR = cumRight / sampleCount;

        cumLeft = 0;
        cumRight = 0;
        sampleCount = 0;

        float average = Mathf.Max(averageL, averageR);
        Debug.Log(average);
        if (average < stopThreshhold)
        {
            StartCoroutine(shiftTempo(normal.pitch, 0));
        } else if (average <= fastThreshhold)
        {
            Debug.Log("normal speed");
            StartCoroutine(shiftTempo(normal.pitch, 1));
        } else // average > fastThreshhold
        {
            StartCoroutine(shiftTempo(normal.pitch, fastPitch));
        }
        Invoke("TrackVelocity", averagingTime);
    }

    private void Update()
    {
        float leftVelocity = (prevLeft - leftHand.transform.position).magnitude / Time.deltaTime;
        float rightVelocity = (prevRight - rightHand.transform.position).magnitude / Time.deltaTime;
        prevLeft = leftHand.transform.position;
        prevRight = rightHand.transform.position;

        cumLeft += leftVelocity;
        cumRight += rightVelocity;
        sampleCount++;
    }

    private void switchAudio()
    {
        Debug.Log(normal.clip.length);
        normal.pitch = 1.2f;
        Debug.Log(normal.clip.length);
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
