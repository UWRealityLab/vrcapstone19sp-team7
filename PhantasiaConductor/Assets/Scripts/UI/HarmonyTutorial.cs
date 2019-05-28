using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonyTutorial : MonoBehaviour
{
    public GameObject harmObject;
    public GameObject hand;
    public float delay = 0.5f;
    public float timeInterval = 1f;
    public float speed = 1f;
    public bool showTutorial = true;

    private Vector3 secondPosition;
    private Vector3 objectStart;
    private Vector3 handStart;

    private void Start()
    {
        if (showTutorial)
        {
            handStart = hand.transform.position;
            objectStart = harmObject.transform.position;
            secondPosition = harmObject.transform.position;
            secondPosition.y += 0.1f;

            StartCoroutine(RunTutorial());
        }

    }

    private IEnumerator RunTutorial()
    {
        while (showTutorial)
        {
            yield return new WaitForSeconds(timeInterval);

            while (showTutorial && hand.transform.position != objectStart)
            {
                float step = speed * Time.deltaTime;
                hand.transform.position = Vector3.Lerp(hand.transform.position, objectStart, step);

                if ((hand.transform.position - objectStart).magnitude < 0.01f)
                {
                    harmObject.GetComponent<Renderer>().material.color = Color.green;
                }
                yield return null;
            }
            
            while (showTutorial && harmObject.transform.position != secondPosition)
            {
                float step = speed * Time.deltaTime;
                harmObject.transform.position = Vector3.Lerp(harmObject.transform.position, secondPosition, step);
                hand.transform.position = Vector3.Lerp(hand.transform.position, secondPosition, step);
                if ((hand.transform.position - secondPosition).magnitude < 0.01f)
                {
                    harmObject.GetComponent<Renderer>().material.color = Color.blue;
                }

                yield return null;
            }
            
            if (!showTutorial)
            {
                break;
            }

            yield return new WaitForSeconds(delay);

            harmObject.GetComponent<Renderer>().material.color = Color.white;
            hand.transform.position = handStart;
            harmObject.transform.position = objectStart;
        }
        gameObject.SetActive(false);
    }

    public bool ShowTutorial
    {
        set
        {
            showTutorial = value;
        }
        get
        {
            return showTutorial;
        }
    }
}
