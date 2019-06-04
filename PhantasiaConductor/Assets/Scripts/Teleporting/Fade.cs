using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float fadeTime = 2f;
    public bool done;
  


    private IEnumerator FadeTo(Material material, float targetOpacity, float duration)
    {

        // Cache the current color of the material, and its initiql opacity.
        Color color = material.color;
        float startOpacity = color.a;

        // Track how many seconds we've been fading.
        float t = 0;

        while (t < duration)
        {
            // Step the fade forward one frame.
            t += Time.deltaTime;
            // Turn the time into an interpolation factor between 0 and 1.
            float blend = Mathf.Clamp01(t / duration);

            // Blend to the corresponding opacity between start & target.
            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            // Apply the resulting color to the material.
            material.color = color;

            // Wait one frame, and repeat.
            yield return null;
        }
        done = true;
    }

    
    public void FadeIn(GameObject obj)
    {
        done = false;
        StartCoroutine(FadeTo(obj.GetComponent<Renderer>().material, 1.0f, fadeTime));
    }

    public void FadeOut(GameObject obj)
    {
        done = false;
        StartCoroutine(FadeTo(obj.GetComponent<Renderer>().material, 0.0f, fadeTime));
    }

    public void FadeInAll(GameObject[] objs, float[] alphaValues)
    {
        done = false;
        for (int i = 0; i < objs.Length; i++)
        {
            Debug.Log("fading in to " + alphaValues[i]);
            StartCoroutine(FadeTo(objs[i].GetComponent<Renderer>().material, alphaValues[i], fadeTime));
        }
    }

    public void FadeOutAll(GameObject[] objs)
    {
        done = false;
        for (int i = 0; i < objs.Length; i++)
        {
            StartCoroutine(FadeTo(objs[i].GetComponent<Renderer>().material, 0.0f, fadeTime));
        }
    }
}
