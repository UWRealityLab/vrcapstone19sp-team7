using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonyTutorial : MonoBehaviour
{
    public GameObject harmonyObject;
    public GameObject harmonyHand;
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
            handStart = harmonyHand.transform.position;
            objectStart = harmonyObject.transform.position;
            secondPosition = harmonyObject.transform.position;
            secondPosition.y += 0.1f;

            StartCoroutine(RunTutorial());
        }

    }
    private IEnumerator RunTutorial()
    {
        while (showTutorial)
        {
            while (showTutorial && harmonyHand.transform.position != harmonyObject.transform.position)
            {
                float step = speed * Time.deltaTime;
                harmonyHand.transform.position = Vector3.MoveTowards(harmonyHand.transform.position, harmonyObject.transform.position, step);
                yield return null;
            }
            harmonyObject.GetComponent<Renderer>().material.color = Color.yellow;

            while (showTutorial && harmonyHand.transform.position != secondPosition) ;
            {
                float step = speed * Time.deltaTime;
                harmonyHand.transform.position = Vector3.MoveTowards(harmonyHand.transform.position, secondPosition, step);
                harmonyObject.transform.position = Vector3.MoveTowards(harmonyObject.transform.position, secondPosition, step);
                yield return null;
            }

            if (!showTutorial)
            {
                break;
            }

            harmonyObject.GetComponent<Renderer>().material.color = Color.green;
            yield return new WaitForSeconds(delay);

            harmonyHand.transform.position = handStart;
            harmonyObject.transform.position = objectStart;
            harmonyObject.GetComponent<Renderer>().material.color = Color.white;

            yield return new WaitForSeconds(timeInterval);
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
