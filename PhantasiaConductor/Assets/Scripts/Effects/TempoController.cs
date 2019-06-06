using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//@COMMENTEDOUT
public class TempoController : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public UnityEvent onStop;
    public UnityEvent onStart;

    public AudioSource song;
    public static float slowDownTime = 0.3f;
    // public float fastPitch = 1.2f;
    // public float slowPitch = 0.8f;
    // public float slowThreshhold = 0.5f;
    // public float fastThreshhold = 1.2f;
    public float stopThreshhold = 0.1f; 
    public float averagingTime = 1f;

    private float cumLeft;
    private float cumRight;
    private Vector3 prevLeft;
    private Vector3 prevRight;
    private float sampleCount;
    private bool stopped;

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
        if (!stopped && average < stopThreshhold)
        {
            onStop.Invoke();
            stopped = true;
        }
        else if (stopped && average > stopThreshhold)
        {
            onStart.Invoke();
            stopped = false;
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

    public void StopPlaying()
    {
        StartCoroutine(shiftTempo(song.pitch, 0));
    }

    public void StartPlaying()
    {
        StartCoroutine(shiftTempo(song.pitch, 1));
    }

    private IEnumerator shiftTempo(float initial, float final)
    {
        float counter = 0;
        while (song.pitch != final && counter < slowDownTime)
        {
            counter += Time.deltaTime;

            song.pitch += Time.deltaTime * (final - initial) / slowDownTime;
            yield return null; ;
        }
        song.pitch = final;
    }
}
