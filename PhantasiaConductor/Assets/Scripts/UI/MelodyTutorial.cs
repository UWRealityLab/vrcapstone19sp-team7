using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyTutorial : MonoBehaviour
{
    public GameObject obj;
    public GameObject hand;
    public Material normal;
    public Material tracking;
    public Material unlocked;
    public Vector3 start;
    public Vector3 end;
    public float speed;
    public float delay;
    public float rotateSpeed = 70f;
    public bool showTutorial;

    private LineRenderer lineRend;
    private Quaternion initialAngle;
    private Quaternion finalAngle;
    private Quaternion startAngle;
    private float rSpeed;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        showTutorial = false;
        startAngle = Quaternion.Euler(-25, 20, 0);
        initialAngle = Quaternion.Euler (-25, 0 , 0);
        finalAngle = Quaternion.Euler(-12, 70, 0);

        float time = 2.0f / speed;
        rSpeed = Quaternion.Angle(finalAngle, startAngle) / time;
    }

    private void OnEnable()
    {
        lineRend.SetPosition(0, start);
        lineRend.SetPosition(1, end);
        showTutorial = true;
        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial()
    {
        while (showTutorial)
        {
            Color c = obj.GetComponent<Renderer>().material.color;
            obj.GetComponent<Renderer>().material.color = new Color(c.r, c.g, c.b, 0.3f);
            yield return new WaitForSeconds(0.5f);

            c = obj.GetComponent<Renderer>().material.color;
            obj.GetComponent<Renderer>().material.color = new Color(c.r, c.g, c.b, 1f);
            yield return new WaitForSeconds(0.2f);

            while(showTutorial && hand.transform.localRotation != startAngle) {
                float step = rotateSpeed * Time.deltaTime;
                hand.transform.localRotation = Quaternion.RotateTowards(hand.transform.localRotation, startAngle, step);
                yield return null;
            }
            obj.GetComponent<Renderer>().material = tracking;

            while (showTutorial && obj.transform.localPosition != end) {
                float step = speed * Time.deltaTime;
                obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, end, step);

                Vector3 newDir = obj.transform.localPosition - hand.transform.localPosition;
                hand.transform.localRotation = Quaternion.LookRotation(newDir);
                yield return null;
            }
            obj.GetComponent<Renderer>().material = unlocked;

            if (!showTutorial) {
                break;
            }

            yield return new WaitForSeconds(delay);
            hand.transform.localRotation = initialAngle;
            obj.transform.localPosition = start;
            obj.GetComponent<Renderer>().material = normal; 
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
