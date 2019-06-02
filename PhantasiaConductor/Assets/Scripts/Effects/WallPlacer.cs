using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacer : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public float numWalls = 8;

    public float radius = 50;

    public GameObject wallPrefab;
    public MasterLoop masterLoop;

    bool wasPlaced = false;

    private GameObject[] walls;
    private bool interactive = false;

    private void Awake()
    {
        walls = new GameObject[(int) numWalls];
    }

    void OnEnable()
    {
        if (!wasPlaced)
        {
            float y = transform.localPosition.y;
            float deltaRad = 2 * Mathf.PI / numWalls;
            for (int i = 0; i < numWalls; i++)
            {
                GameObject wall = Instantiate(wallPrefab);
                walls[i] = wall;
                wall.GetComponent<MeshRenderer>().enabled = false;
                wall.transform.parent = transform;

                float rad = deltaRad * i;
                float x = Mathf.Cos(rad) * radius;
                float z = Mathf.Sin(rad) * radius;

                // wall.transform.LookAt(transform);

                wall.transform.localPosition = new Vector3(x, y, z);
                wall.transform.Rotate(0, -Mathf.Rad2Deg * (rad), 0);

                ColorPulse cp = wall.GetComponent<ColorPulse>();
                masterLoop.onNewLoop.AddListener(delegate ()
                {
                    cp.NewLoop();
                });
            }
            wasPlaced = true;
        }
    }

    private void Update()
    {
        if (interactive)
        {
            // Right hand
            RaycastHit hitR;
            Ray rayR = new Ray(rightHand.transform.position, rightHand.transform.forward);

            if (Physics.Raycast(rayR, out hitR, Mathf.Infinity, ~(1 << 2)))
            {
                if (string.Equals(hitR.collider.tag, "BeatWall"))
                {
                    hitR.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }

            // Left hand
            RaycastHit hitL;
            Ray rayL = new Ray(leftHand.transform.position, leftHand.transform.forward);

            if (Physics.Raycast(rayL, out hitL, Mathf.Infinity, ~(1 << 2)))
            {
                if (string.Equals(hitL.collider.tag, "BeatWall"))
                {
                    hitL.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    public void StopPulse()
    {
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<ColorPulse>().StopBlink();
        }
    }

    public void PausePulse()
    {
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<ColorPulse>().PauseBlink();
        }
    }

    public void ResumePulse()
    {
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<ColorPulse>().ResumeBlink();
        }
    }

    public bool Interactive
    {
        get
        {
            return interactive;
        }
        set
        {
            interactive = value;
        }
    }

}
