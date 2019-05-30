using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGuide : MonoBehaviour
{
    public GameObject sphere;
    public GameObject net;
    public float speed = 2f;
    public float delay = 1f;
    public bool showTutorial = true;

    private float initialSPhereHeight;
    private float[] xPos;
    private int index;

    private void Awake()
    {
        index = 1;
        initialSPhereHeight = sphere.transform.position.y;
        xPos = new float[2] { 0, 0.3f };
    }

    private void OnEnable()
    {
        if (showTutorial)
        {
            StartCoroutine(RunTutorial());
        }
    }

    private IEnumerator RunTutorial()
    {
        while (showTutorial)
        {
            sphere.GetComponent<Renderer>().enabled = true;
            while (showTutorial && sphere.transform.position.y > net.transform.position.y + 0.01)
            {
                if (net.transform.position.x != sphere.transform.position.x)
                {
                    Debug.Log("moving x");
                    float netStep = speed * 2 * Time.deltaTime;
                    Vector3 newPos = net.transform.position;
                    newPos.x = sphere.transform.position.x;
                    net.transform.position = Vector3.Lerp(net.transform.position, newPos, netStep);
                }

                Debug.Log("moving y");
                float step = speed * Time.deltaTime;
                Vector3 newSpherePos = sphere.transform.position;
                newSpherePos.y = net.transform.position.y;
                sphere.transform.position = Vector3.Lerp(sphere.transform.position, newSpherePos, step);
                index++;
                yield return null;
            }
            if (!showTutorial)
            {
                break;
            }
            sphere.GetComponent<Renderer>().enabled = false;
            Vector3 nextPos = new Vector3(xPos[index % xPos.Length], initialSPhereHeight, sphere.transform.position.z);
            sphere.transform.position = nextPos;

            yield return new WaitForSeconds(delay);
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
