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

    private Dictionary<int, int> objectsCaughtByGroupId = new Dictionary<int, int>();

    private int recentGroupId = 0;

    // Start is called before the first frame update
    void Awake()
    {
        timePerBeat = loopTime / beatInfos[0].beats.Length;

        // should be same as length of spawnDegrees array
        totalObjectsToCatch = spawnDegrees.Length;
        // Debug.Log("total objects " + totalObjectsToCatch);
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

            if (rIndex == 0)
            {
                // first object so increment
                recentGroupId++;
                objectsCaughtByGroupId[recentGroupId] = 0;
            }
            radialObject.groupId = recentGroupId;

            obj.transform.parent = transform;

            float deg = spawnDegrees[rIndex] + degOffset;
            float x = Mathf.Cos(deg * Mathf.Deg2Rad) * radius;
            float z = Mathf.Sin(deg * Mathf.Deg2Rad) * radius;
            obj.transform.localPosition = new Vector3(x, spawnHeight, z);

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
            Invoke("NextBeat", timePerBeat);
        }
    }

    public void NewLoop()
    {
        if (beatInfoIndex >= beatInfos.Length)
        {
            // all beats spawned, go back
            rIndex = 0;
            beatIndex = 0;
            beatInfoIndex = 0;

            CancelInvoke();
            Invoke("NextBeat", timePerBeat);
        }
    }

    public void ObjectCaught(int groupId)
    {
        objectsCaughtByGroupId[groupId]++;
        int objectsCaught = objectsCaughtByGroupId[groupId];
        Debug.Log(objectsCaught);
        if (objectsCaught == totalObjectsToCatch)
        {
            // this needs to be the last thing it does since we may inactive the sequence
            onSuccess.Invoke();
            Debug.Log("all objects caught");
        }
    }

    public void LastObjectDestroyed(int groupId) {
        objectsCaughtByGroupId.Remove(groupId);
        // Debug.Log("last object destroyed");
    }

    // if completed
    public void Unlock() {
        gameObject.SetActive(false);
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
