using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform orbitCenter;

    public float minorAxisDist = 2f;

    public float speed = 1.0f;

    [Range(0, 360)]
    public float rotationDegrees = 0;

    float majorAxisDist;

    Vector3 startPosition;

    Vector3 minorAxis;
    Vector3 majorAxis;

    private float acc;

    void Awake()
    {
        startPosition = transform.localPosition;
        majorAxisDist = Vector3.Distance(startPosition, orbitCenter.localPosition);
        acc = 0.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        majorAxis = (startPosition - orbitCenter.transform.localPosition).normalized;
        SetMinorAxis();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = orbitCenter.transform.localPosition +
        majorAxis * majorAxisDist * Mathf.Cos(acc * speed) +
         minorAxis * minorAxisDist * Mathf.Sin(acc * speed);

        acc += Time.deltaTime;
    }

    void SetMinorAxis()
    {
        Quaternion rot = Quaternion.FromToRotation(Vector3.right, majorAxis);
        Quaternion axisRot = Quaternion.AngleAxis(rotationDegrees, Vector3.right);
        
        Vector3 basis = axisRot * Vector3.up;
        minorAxis = (rot * basis).normalized;
    }

    void OnValidate()
    {
        SetMinorAxis();
    }
}
