using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PercussionDestinationActions : MonoBehaviour
{
    public Vector3 puzzleCompleteOffset;
    public GameObject center;

    public void Arrive()
    {
        gameObject.SetActive(true);
    }

    public void Depart()
    {
        StartCoroutine(MoveDelay(GetComponent<Fade>().fadeTime));
    }

    private IEnumerator MoveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 newPos = transform.position;
        newPos += puzzleCompleteOffset;
        center.transform.position = newPos;
    }
}
