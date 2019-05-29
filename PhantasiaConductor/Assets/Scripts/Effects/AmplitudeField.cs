using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @AJVRHAND

public class AmplitudeField : MonoBehaviour
{
    public GameObject objPrefab;

    public Valve.VR.InteractionSystem.Hand rightHand;
    public Valve.VR.InteractionSystem.Hand leftHand;

    public float radius;

    public int objectsPerSector = 5;

    public int numSectors = 16;

    List<AmplitudeObject> amplitudeObjects;

    private bool mouseMode;

    // Start is called before the first frame update
    void Start()
    {
        mouseMode = rightHand == null && leftHand == null;
        amplitudeObjects = new List<AmplitudeObject>();

        var center = Vector3.zero;
        float radialSeparation = radius / objectsPerSector;
        float radDelta = Mathf.PI * 2 / numSectors;
        float rad = 0.0f;
        for (var sector = 0; sector < numSectors; sector++)
        {
            var dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
            for (var i = 0; i < objectsPerSector; i++)
            {
                float distFromCenter = radialSeparation * (i + 1);
                GameObject go = Instantiate(objPrefab);
                go.transform.parent = transform;
                go.transform.localPosition = center + dir * distFromCenter;

                AmplitudeObject ao = go.GetComponent<AmplitudeObject>();
                amplitudeObjects.Add(ao);
                ao.angleRad = rad;
                ao.originalPos = go.transform.localPosition;
                ao.ourRay = new Ray(transform.localPosition, dir);
                ao.distFromCenter = Vector3.Distance(go.transform.localPosition, center);
            }

            rad += radDelta;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (mouseMode)
        {
            float t = Time.time;
            float rad = Mathf.Repeat(t, Mathf.PI * 2);
            Vector3 dir = new Vector3(Mathf.Cos(rad), 0.0f, Mathf.Sin(rad));
            Ray ray = new Ray(transform.localPosition, dir);
            foreach (var a in amplitudeObjects)
            {
                a.UpdateAmplitude(ray, rad);
            }
        } else {
            Ray rightRay = new Ray(transform.localPosition, rightHand.transform.rotation * transform.forward);
            Ray leftRay = new Ray(transform.localPosition, leftHand.transform.rotation * transform.forward);

            foreach (var a in amplitudeObjects)
            {
                float rad = Mathf.Atan2(rightRay.direction.z, rightRay.direction.x);
                a.UpdateAmplitude(rightRay, rad);
            }
        }


    }
}
