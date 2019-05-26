using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class RadialSequence : MonoBehaviour
{
    public UnityEvent onSuccess;

    public BeatInfo[] beatInfos;

    public float[] spawnDegrees;

    public MasterLoop masterLoop;

    public GameObject radialObjectPrefab;

    public float radius = 10f;
    public float spawnHeight = 5f;

    private int beatInfoIndex = 0;
    private int beatIndex = 0;
    private int rIndex = 0;

    private float timePerBeat;

    private float degOffset = 90;

    // number of objects that fall
    private int totalObjectsToCatch;
    private int objectsCaught;

    // Start is called before the first frame update
    void Start()
    {
        timePerBeat = loopTime / beatInfos[0].beats.Length;

        totalObjectsToCatch = 0;
        foreach (var b in beatInfos[0].beats)
        {
            if (b)
            {
                totalObjectsToCatch++;
            }
        }

        Debug.Log("total objects " + totalObjectsToCatch);
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

            obj.transform.parent = transform.parent;

            float deg = spawnDegrees[rIndex] + degOffset;
            float x = Mathf.Cos(deg * Mathf.Deg2Rad) * radius;
            float z = Mathf.Sin(deg * Mathf.Deg2Rad) * radius;
            obj.transform.localPosition = new Vector3(x, spawnHeight, z);

            rIndex++;
        }

        beatIndex++;
        if (beatIndex >= currBeatInfo.beats.Length)
        {
            beatIndex = 0;
            beatInfoIndex++;
        }

        if (beatInfoIndex < beatInfos.Length)
        {
            Invoke("NextBeat", timePerBeat);
        }
    }

    public void NewLoop()
    {
        if (beatInfoIndex >= beatInfos.Length)
        {
            // all beats spawned, go back
            objectsCaught = 0;
            rIndex = 0;
            beatIndex = 0;
            beatInfoIndex = 0;

            CancelInvoke();
            Invoke("NextBeat", timePerBeat);
        }
    }

    public void ObjectCaught()
    {
        objectsCaught++;
        Debug.Log(objectsCaught);
        if (objectsCaught == totalObjectsToCatch)
        {
            onSuccess.Invoke();
            Debug.Log("all objects caught");
        }
    }

    void OnEnable()
    {
        // allows new loop to start the beat sequence
        beatInfoIndex = beatInfos.Length;
    }

    float loopTime
    {
        get
        {
            return MasterLoop.loopTime;
        }
    }
}
