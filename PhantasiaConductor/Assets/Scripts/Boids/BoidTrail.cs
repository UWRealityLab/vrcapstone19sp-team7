using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class BoidTrail : MonoBehaviour
{
    public bool setWidth = true;

    [SerializeField]
    private float widthScale = 1.0f;
    private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();

        int numColorKeys = Random.Range(2, 8);
        Color[] colors = ColorGenerator.GenerateColors(numColorKeys);
        // Color[] colors = GenerateColorsHsv(numColorKeys);
        // Color[] colors = ColorGenerator.GenerateColorsHsv(numColorKeys);

        GradientAlphaKey[] alphaKeys = { new GradientAlphaKey(Random.Range(.4f, 1f), 0.0f), new GradientAlphaKey(0.0f, 1.0f) };
        GradientColorKey[] colorKeys = new GradientColorKey[numColorKeys];
        {
            float delta = 1.0f / numColorKeys;
            for (var i = 0; i < numColorKeys; i++)
            {
                colorKeys[i] = new GradientColorKey(colors[i], delta * (i + 1));
            }
        }

        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);

        tr.colorGradient = gradient;

        float trailTime = Random.Range(.15f, .6f);
        tr.time = trailTime;

        // handle the width curve
        if (setWidth)
        {
            AnimationCurve curve = new AnimationCurve();
            int numCurveKeys = Random.Range(2, 8);
            float[] widths = new float[numCurveKeys];

            float seed = Random.Range(0f, 1000f);
            for (var i = 0; i < numCurveKeys; i++)
            {
                widths[i] = Mathf.PerlinNoise(seed + (0.5f * i), seed + (0.25f * i));
            }

            {
                float delta = 1.0f / numCurveKeys;
                for (var i = 0; i < numCurveKeys; i++)
                {
                    curve.AddKey((1 + i) * delta, widths[i]);
                }
            }

            tr.widthCurve = curve;
        }

        tr.widthMultiplier = widthScale;

        // TODO maybe average the trail color with the mesh color for more visual coherence
    }


}
