﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System.IO;

public class PathBeat : MonoBehaviour
{
    public UnityEvent onReachedEnd;

    public enum PathMode
    {
        SPEED_CONSTANT,
        TIMED
    }

    public LineRenderer lineRenderer;

    // time spent at each part of the line
    public List<float> timeMap = new List<float>();

    public GameObject obj;

    public PathMode pathMode = PathMode.SPEED_CONSTANT;

    // units of movement per sec
    public float speed = 5;

    float timeElapsed;
    int index;

    bool beganMovement = false;

    void Start()
    {
        LoadFromFile("Assets/Paths/path.txt");
        Debug.Log(pathLength + " " + pathTime);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            beganMovement = true;
        }

        if (!beganMovement)
        {
            return;
        }
        if (index < vertexCount - 1)
        {
            Vector3 v;
            float t = timeMap[index];

            switch (pathMode)
            {
                case PathMode.TIMED:
                    t = timeMap[index];
                    break;
                case PathMode.SPEED_CONSTANT:
                    t = CalcTimeTraverse(lineRenderer.GetPosition(index), lineRenderer.GetPosition(index + 1));
                    break;

            }

            float completion = timeElapsed / t;

            v = Vector3.Lerp(lineRenderer.GetPosition(index),
                             lineRenderer.GetPosition(index + 1),
                             completion);

            obj.transform.position = v;

            if (timeElapsed > t)
            {
                timeElapsed -= t;
                index++;

                if (index >= vertexCount - 1)
                {
                    onReachedEnd.Invoke();
                }
            }

            timeElapsed += Time.deltaTime;
        }
    }

    public void Reset()
    {
        index = 0;
        obj.transform.position = lineRenderer.GetPosition(index);
    }

    public void Begin()
    {
        beganMovement = true;
    }

    public void Stop()
    {
        beganMovement = false;
    }

    public void AddVertex(Vector3 pos, float t)
    {
        // no line is drawn if only one vertex exists
        if (vertexCount > 0)
        {
            timeMap.Add(t);
        }
        vertexCount += 1;
        lineRenderer.SetPosition(vertexCount - 1, pos);
    }

    public void LoadFromFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        string line;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();

            var tokens = line.Split(',');
            var v = new Vector3(float.Parse(tokens[0]), float.Parse(tokens[1]), float.Parse(tokens[2]));
            AddVertex(v, 1f);
        }

        obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.parent = transform;
        obj.transform.position = lineRenderer.GetPosition(0);
    }

    public void Hello()
    {
        Debug.Log("Hello " + gameObject.name);
    }

    private float CalcTimeTraverse(Vector3 start, Vector3 end)
    {
        var dist = Vector3.Distance(start, end);
        return dist / speed;
    }

    public float pathLength
    {
        get
        {
            float totalDist = 0f;
            Vector3[] ps = positions;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                totalDist += Vector3.Distance(ps[i], ps[i + 1]);
            }

            return totalDist;
        }
    }

    // seconds
    public float pathTime
    {
        get
        {
            switch (pathMode)
            {
                case PathMode.SPEED_CONSTANT:
                    return pathLength / speed;
                case PathMode.TIMED:
                    float s = 0f;
                    foreach (var v in timeMap)
                    {
                        s += v;
                    }
                    return s;
                default:
                    return 0;
            }
        }
    }

    public Vector3[] positions
    {
        get
        {
            int count = lineRenderer.positionCount;
            Vector3[] p = new Vector3[count];

            lineRenderer.GetPositions(p);
            return p;
        }
    }

    private int vertexCount
    {
        get
        {
            return lineRenderer.positionCount;
        }

        set
        {
            lineRenderer.positionCount = value;
        }
    }
}
