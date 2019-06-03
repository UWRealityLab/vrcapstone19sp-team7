using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class DestinationActions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject puzzleObj;
    public GameObject[] makeColorful;
    public Material[] colorMaterials;
    public float loadDelay = 2f;

    public GameObject leftHandObject;
    public GameObject rightHandObject;

    public UnityEvent LoadPuzzle;
    public UnityEvent OnDepart;

    public bool fantasiaOn = false;

    private GameController gameController;

    void Awake()
    {
        gameController = transform.parent.transform.GetComponent<GameController>();
    }

    public void onArrive()
    {
        if (puzzleObj != null)
        {
            gameController.FadeOutAll();

            AgnosticPlayer.GetPlayer().GetComponent<PerspectiveShift>().teleportEnabled = false;

            // Player.instance.GetComponent<PerspectiveShift>().teleportEnabled = false;
            StartCoroutine(DelayedLoad());
        }
        else // At Podium
        {
            SetHands(true);
            if (!fantasiaOn)
            {
                // Player.instance.GetComponent<PerspectiveShift>().LasersOn();
                AgnosticPlayer.GetPlayer().GetComponent<PerspectiveShift>().LasersOn();
            }
        }
    }

    public void onPrepareToLeave()
    {
        if (puzzleObj != null)
        {
            // Make instruments colorful
            for (int i = 0; i < makeColorful.Length; i++)
            {
                Color orig = colorMaterials[i].color;
                Color newColor = new Color(orig.r, orig.g, orig.b, 0.0f);
                Material newMaterial = colorMaterials[i];
                newMaterial.color = newColor;

                makeColorful[i].GetComponent<Renderer>().material = newMaterial;
            }

            OnDepart.Invoke();
        }
        SetHands(false);
    }

    private void SetHands(bool active)
    {
        if (leftHandObject != null)
        {
            leftHandObject.SetActive(active);
        }
        if (rightHandObject != null)
        {
            rightHandObject.SetActive(active);
        }
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(loadDelay);
        Debug.Log("loading puzzle");
        LoadPuzzle.Invoke();
        SetHands(true);
    }

    public bool FantasiaOn
    {
        set
        {
            fantasiaOn = value;
        }
        get
        {
            return fantasiaOn;
        }
    }
}

