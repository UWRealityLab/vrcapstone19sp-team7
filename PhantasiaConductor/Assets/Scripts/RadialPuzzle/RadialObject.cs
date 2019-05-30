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

    private RadialSequence ownerSequence;

    private int groupId_;

    private bool hasBeenCaught = false;


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

    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (!hasBeenCaught)
        {
            if (particleSystemPrefab != null)
            {
                ParticleSystem ps = Instantiate(particleSystemPrefab).GetComponent<ParticleSystem>();
                ps.gameObject.transform.position = transform.position;
                ps.Play();
            }

            // we caught it
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
