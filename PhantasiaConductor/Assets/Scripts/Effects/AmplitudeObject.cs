using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplitudeObject : MonoBehaviour
{

    public float angleRad;

    public Vector3 originalPos;

    public Ray ourRay;

    public float maxHeight = 10f;

    public float frequencyScale = 3.0f;

    public float distCoeff = 0.2f;

    public float distFromCenter;

    private float waveSpan;


    private Renderer renderer;

    void Start()
    {
        waveSpan = Mathf.PI / 2.0f / frequencyScale;
        renderer = GetComponent<Renderer>();
        
        originalPos.y += distFromCenter * distCoeff;
        // Debug.Log(maxHeight + " " + originalPos.y);
        maxHeight = maxHeight + originalPos.y;

        
    }


    public void UpdateAmplitude(Ray ray, float handRad)
    {
        transform.localPosition = originalPos;

        float deg = Mathf.DeltaAngle(Mathf.Rad2Deg * handRad, Mathf.Rad2Deg * angleRad);

        float distRad = Mathf.Deg2Rad * deg;
        if (distRad > waveSpan || distRad < -waveSpan)
        {
            return;
        }

        float k = Mathf.Cos(distRad * frequencyScale);

        float height = Mathf.Clamp(maxHeight * k + originalPos.y, 0.0f, maxHeight);
        transform.localPosition = originalPos + new Vector3(0, height, 0);
    }


}
