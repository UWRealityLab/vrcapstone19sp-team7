using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidScale : MonoBehaviour
{
    public float minScale = 0.1f;
    public float maxScale = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        float s = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(s, s, s);
    }


}
