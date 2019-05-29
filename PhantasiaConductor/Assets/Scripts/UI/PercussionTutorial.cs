using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercussionTutorial : MonoBehaviour
{
    public GameObject rhythmObject;
    public GameObject drumstick;
    public Material unlockMaterial;
    public Material originalMaterial;
    public float delay = 0.5f;
    public float timeInterval = 1f;
    public float speed = 10f;
    public bool showTutorial = true;

    private Blink blink;
    private Quaternion materialChangeAngle;

    private void Awake()
    {
        blink = rhythmObject.GetComponent<Blink>();
        materialChangeAngle = Quaternion.Euler(0, 0, 60);
    }

    private void Start()
    {
        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial()
    {
        while (showTutorial)
        {
            blink.BlinkOnOnce();
            Quaternion original = drumstick.transform.rotation;
            Quaternion final = Quaternion.Euler(0, 0, 65);

            while (showTutorial && drumstick.transform.rotation != final)
            {
                float step = speed * Time.deltaTime;
                drumstick.transform.rotation = Quaternion.RotateTowards(drumstick.transform.rotation, final, step);
                if (drumstick.transform.rotation.z >= materialChangeAngle.z)
                {
                    rhythmObject.GetComponent<Renderer>().material = unlockMaterial;
                }
                yield return null;
            }

            if (!showTutorial)
            {
                break;
            }

            yield return new WaitForSeconds(delay);

            rhythmObject.GetComponent<Renderer>().material = originalMaterial; 
            blink.BlinkOffOnce();
            drumstick.transform.rotation = original;

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
