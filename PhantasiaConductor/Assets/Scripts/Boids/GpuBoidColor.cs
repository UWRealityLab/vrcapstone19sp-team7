    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpuBoidColor : MonoBehaviour
{
    public float intensityScale = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        Color color = ColorGenerator.GenerateColor();
        float alpha = Random.Range(0.35f, .9f);
        color.a = alpha;

        Vector4 v = color;
        float mag = v.magnitude;

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = color;
        // renderer.material.SetColor("_EmissionColor", color * (1.7f - (v.magnitude / 10f)));
        renderer.material.SetColor("_EmissionColor", color * intensityScale);
    }
}
