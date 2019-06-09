using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    public Baton baton;
    public Material glowMaterial;
    public Material normalMaterial;
    public float growDuration;
    public float growScale = 0.75f;

    private Vector3 normalScale;
    private bool growing = false;

    private void Awake()
    {
        normalScale = transform.localScale;
    }

    private void Start()
    {
        // StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        StartCoroutine(GrowObject());
        yield return new WaitForSeconds(1);
        StopGrow();
    }

    public IEnumerator GrowObject()
    {
        GetComponent<Renderer>().material = glowMaterial;
        float counter = 0;
        float initialScale = transform.localScale.x;
        growing = true;
        if (counter >= growDuration)
        {
            Debug.Log("DAM");
            GetComponent<AudioSource>().Play();
        }
        while (growing && counter < growDuration)
        {
            counter += Time.deltaTime;
            float newScalePoint = (initialScale + (counter / growDuration)) * growScale;
            baton.SetCompletion(counter / growDuration, 0);
            Vector3 newScale = new Vector3(newScalePoint, newScalePoint, newScalePoint);
            transform.localScale = newScale;
            yield return null;
        }

    }

    public void StopGrow()
    {
        growing = false;
        StopAllCoroutines();
        GetComponent<Renderer>().material = normalMaterial;
        transform.localScale = normalScale;
        baton.SetCompletion(0, 0);
    }
}
