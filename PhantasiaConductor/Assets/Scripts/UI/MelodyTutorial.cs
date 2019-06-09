using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyTutorial : MonoBehaviour
{
    public GameObject obj;
    public GameObject hand;
    public Vector3 start;
    public Vector3 end;
    public float speed;
    public float delay;
    public bool showTutorial;

    private LineRenderer lineRend;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        showTutorial = false;
    }

    private void OnEnable()
    {
        lineRend.SetPosition(0, start);
        lineRend.SetPosition(1, end);
        showTutorial = true;
    }

    private IEnumerator RunTutorial()
    {
        while (showTutorial)
        {
            Color c = obj.GetComponent<Renderer>().material.color;
            obj.GetComponent<Renderer>().material.color = new Color(c.r, c.g, c.b, 0.3f);
            yield return new WaitForSeconds(0.2f);

            c = obj.GetComponent<Renderer>().material.color;
            obj.GetComponent<Renderer>().material.color = new Color(c.r, c.g, c.b, 1f);
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
