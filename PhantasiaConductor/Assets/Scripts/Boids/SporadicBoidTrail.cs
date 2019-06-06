using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class SporadicBoidTrail : MonoBehaviour
{
    public float trailChance = 0.2f;

    public float minTrailTime = 0.3f;
    public float maxTrailTime = 0.8f;

    public float minTrailWidth = 0.5f;
    public float maxTrailWidth = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.value < trailChance)
        {
            TrailRenderer tr = GetComponent<TrailRenderer>();


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

            float trailTime = Random.Range(minTrailTime, maxTrailTime);
            tr.time = trailTime;

            tr.widthMultiplier = Random.Range(minTrailWidth, maxTrailWidth);



            tr.enabled = true;
        }

    }
}
