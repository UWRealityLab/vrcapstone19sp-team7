using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class RadialObject : MonoBehaviour
{
    public UnityEvent onSuccess;

    public UnityEvent onFailed;


    public float lifetime = 4f;

    public float fallSpeed = 1.0f;

    public GameObject particleSystemPrefab;


    [HideInInspector]
    public AudioSource audioSource;


    [HideInInspector]
    public bool isLastObject;

    [HideInInspector]
    public float emissionIntensity = 1.0f;

    private RadialSequence ownerSequence;

    private int groupId_;

    private bool hasBeenCaught = false;

    private CaptureType captureType;


    void Start()
    {

        Invoke("EndOfLifetime", lifetime);
    }

    void EndOfLifetime()
    {
        onFailed.Invoke();
        Destroy(gameObject);
    }

    // sequence this reports to for finishing
    public void BindSequence(RadialSequence seq)
    {
        ownerSequence = seq;
    }

    public void SetColor(Color c)
    {
        Material mat = GetComponent<Renderer>().material;
        mat.color = c;
        Vector4 v = c;
        // mat.SetColor("_EmissionColor", v * emissionIntensity);
        mat.SetColor("_EmissionColor", v);
    }

    public void SetCaptureType(CaptureType type)
    {
        captureType = type;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (!hasBeenCaught)
        {
            GameObject net = collision.gameObject;
            if (net.tag == "RightNet" && captureType == CaptureType.LEFT ||
                net.tag == "LeftNet" && captureType == CaptureType.RIGHT)
            {
                return;
            }


            // we caught it
            if (particleSystemPrefab != null)
            {
                ParticleSystem ps = Instantiate(particleSystemPrefab).GetComponent<ParticleSystem>();
                ps.gameObject.transform.position = transform.position;
                ps.Play();
            }


            onSuccess.Invoke();
            ownerSequence.ObjectCaught(groupId);
            Destroy(gameObject);
            hasBeenCaught = true;
        }


    }

    void Fadeout()
    {

    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    void OnDestroy()
    {
        if (isLastObject)
        {
            ownerSequence.LastObjectDestroyed(groupId);
        }
    }

    public int groupId
    {
        get
        {
            return groupId_;
        }

        set
        {
            groupId_ = value;
        }
    }

}
