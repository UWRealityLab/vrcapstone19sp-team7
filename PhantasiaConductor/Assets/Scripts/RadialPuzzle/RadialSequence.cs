using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class RadialSequence : MonoBehaviour
{
    public UnityEvent onSuccess;

    public UnityEvent onObjectCaught;

    public UnityEvent onReachedEndFail;



    public BeatInfo[] beatInfos;

    public float[] spawnDegrees;


    public MasterLoop masterLoop;

    public GameObject radialObjectPrefab;

    public float radius = 10f;
    public float spawnHeight = 5f;

    public Transform originTransform;

    // public AudioSourceList audioSourcesList;
    public AudioSource[] audioSources;

    public CaptureType[] captureTypes;

    public float[] angularSpeeds;

    public AudioSource loopSource;
    public bool isLastSequence = false;



    private int beatInfoIndex = 0;
    private int beatIndex = 0;
    private int rIndex = 0;

    private float degOffset = 0;

    // number of objects that fall
    private int totalObjectsToCatch;

    private Dictionary<int, int> objectsCaughtByGroupId = new Dictionary<int, int>();

    private int recentGroupId = 0;
    private bool complete = false;
    private bool fantasiaOn;
    private float delay;

    private float prevTime;

    // Start is called before the first frame update
    void Awake()
    {

        // should be same as length of spawnDegrees array
        totalObjectsToCatch = spawnDegrees.Length;

        transform.position = originTransform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            onSuccess.Invoke();
            complete = true;
        }
    }

    void NextBeat()
    {
        BeatInfo currBeatInfo = beatInfos[beatInfoIndex];
        bool shouldSpawn = currBeatInfo.beats[beatIndex];

        if (shouldSpawn)
        {
            GameObject obj = Instantiate(radialObjectPrefab);
            RadialObject radialObject = obj.GetComponent<RadialObject>();
            radialObject.BindSequence(this);

            if (rIndex < captureTypes.Length)
            {
                radialObject.SetColor(captureTypes[rIndex].GetColor());
                radialObject.SetCaptureType(captureTypes[rIndex]);
            }

            if (rIndex == 0)
            {
                // first object so increment
                recentGroupId++;
                objectsCaughtByGroupId[recentGroupId] = 0;
            }

            radialObject.groupId = recentGroupId;
            if (audioSources != null)
            {
                // radialObject.audioSource = audioSources.Get(audioIndices[rIndex]);
                radialObject.audioSource = audioSources[rIndex];
            }

            obj.transform.parent = transform;

            float deg = spawnDegrees[rIndex] + degOffset;
            float x = Mathf.Cos(deg * Mathf.Deg2Rad) * radius;
            float z = Mathf.Sin(deg * Mathf.Deg2Rad) * radius;
            obj.transform.localPosition = new Vector3(x, spawnHeight, z);
            radialObject.startAngle = deg;
            if (rIndex < angularSpeeds.Length)
            {
                radialObject.angularSpeed = angularSpeeds[rIndex];
            }
            radialObject.centerOfRotation = obj.transform.localPosition;
            radialObject.numObjectsInSeq = totalObjectsToCatch;

            rIndex++;

            bool isLastObject = rIndex >= totalObjectsToCatch;
            radialObject.isLastObject = isLastObject;
        }

        beatIndex++;
        if (beatIndex >= currBeatInfo.beats.Length)
        {
            beatIndex = 0;
            beatInfoIndex++;
        }

        if (beatInfoIndex < beatInfos.Length)
        {
            prevTime = Time.time;
            Invoke("NextBeat", beatInfos[beatInfoIndex].beatTime);
        }
    }

    public void NewLoop()
    {
        if (isLastSequence && complete && !fantasiaOn)
        {
            loopSource.Play();
        }
        if (beatInfoIndex >= beatInfos.Length)
        {
            // all beats spawned, go back
            rIndex = 0;
            beatIndex = 0;
            beatInfoIndex = 0;

            CancelInvoke();
            prevTime = Time.time;
            Invoke("NextBeat", beatInfos[beatInfoIndex].beatTime);
        }
    }
    
    public void ObjectCaught(int groupId)
    {
        objectsCaughtByGroupId[groupId]++;
        int objectsCaught = objectsCaughtByGroupId[groupId];
        onObjectCaught.Invoke();
        // Debug.Log("objects caught" + objectsCaught);
        Baton baton = AgnosticHand.GetRightBaton();
        baton.SetCompletion((float)objectsCaught / totalObjectsToCatch , 1.0f);
        if (objectsCaught == totalObjectsToCatch)
        {
            // this needs to be the last thing it does since we may inactive the sequence
            // Debug.Log("all objects caught" + objectsCaught);
            complete = true;
            onSuccess.Invoke();

        }
    }

    public void LastObjectDestroyed(int groupId)
    {
        objectsCaughtByGroupId.Remove(groupId);
        // Debug.Log("last object destroyed");
        Baton baton = AgnosticHand.GetRightBaton();
        baton.SetCompletion(0.0f, 1.0f);
    }

    // if completed
    public void Unlock()
    {
        complete = true;
        if (!isLastSequence)
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        // allows new loop to start the beat sequence
        beatInfoIndex = beatInfos.Length;
        transform.position = originTransform.position;
    }

    public void FantasiaOn()
    {
        fantasiaOn = true;
    }

    public void PauseFantasia()
    {
        delay = beatInfos[beatInfoIndex % beatInfos.Length].beatTime - (Time.time - prevTime);
        if (delay < 0) { delay = 0; }
        CancelInvoke();
    }

    public void ResumeFantasia()
    {
        Invoke("NextBeat", delay);
    }

    float loopTime
    {
        get
        {
            return MasterLoop.loopTime;
        }
    }




}

public enum CaptureType
{
    BOTH,
    RIGHT,
    LEFT
}

// extension methods
static class CaptureTypeMethods
{
    public static Color GetColor(this CaptureType ct)
    {
        switch (ct)
        {
            case CaptureType.RIGHT:
                return Color.red;
            case CaptureType.LEFT:
                return Color.blue;
            case CaptureType.BOTH:
                return Color.white;
            default:
                return Color.white;
        }

    }
}
