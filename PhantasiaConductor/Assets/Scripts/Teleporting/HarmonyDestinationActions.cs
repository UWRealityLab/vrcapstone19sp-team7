using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonyDestinationActions : MonoBehaviour
{
    public float delayDepart = 4f;
    public Vector3 newLocalPosition;
    public Vector3 newLocalScale;
    public float transitionSpeed = 2f;

    public void Arrive()
    {
        gameObject.SetActive(true);
    }

    public void Depart()
    {
        StartCoroutine(DelayDepart());
    }

    private IEnumerator DelayDepart() {
        yield return new WaitForSeconds(delayDepart);

        while (transform.localPosition != newLocalPosition)
        {
            float step = transitionSpeed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, newLocalPosition, step);

            transform.localScale = Vector3.Lerp(transform.localScale, newLocalScale, step);
            yield return null;
        }
    }
}
