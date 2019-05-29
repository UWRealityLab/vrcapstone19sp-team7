using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    // number of boids to spawn
    public uint flockSize = 20;

    public float minVelocity = 5;
    public float maxVelocity = 20;

    public float randomness = 1;

    public GameObject[] boidPrefabs;

    public GameObject chasee;

    public Vector3 flockCenter;
    public Vector3 flockAlignment;

    // public Vector3 flockVelocity;
    // public float flockVelocity = 6.0f;

    public float flockSpeed = 6.0f;

    public float spawnRadius = 10;


    public float neighborRadius = 3;

    public float velocityVariation = 0.5f;

    public float rotationCoef = 4.0f;

    public LayerMask searchLayer;

    private List<Boid> boids = new List<Boid>();

    private Vector3 targetAlignment;



    // Start is called before the first frame update
    void Start()
    {
        neighborRadius = 3;
        searchLayer = LayerMask.GetMask("Boid");
        rotationCoef = 5.0f;

        for (int i = 0; i < flockSize; i++)
        {
            SpawnBoid();
        }
    }

    Boid SpawnBoid()
    {
        Vector3 pos = transform.position + Random.insideUnitSphere * 10;
        Quaternion rot = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
        int i = Random.Range(0, boidPrefabs.Length);
        Boid boid = Instantiate(boidPrefabs[i], transform.position, rot).GetComponent<Boid>();
        boid.transform.parent = transform;
        boid.transform.position = pos;
        boids.Add(boid);
        boid.SetFlock(this);

        return boid;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 center = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        // Vector3 velocity = Vector3.zero;

        // update center of mass
        foreach (Boid boid in boids)
        {
            // center += boid.transform.localPosition;
            center += boid.transform.position;
            alignment += boid.transform.forward;
            // velocity += boid.rigidBody.velocity;
        }

        var avg = 1.0f / flockSize;
        center /= flockSize;
        alignment /= flockSize;
        // velocity /= flockSize;

        flockCenter = center;
        flockAlignment = alignment;
        // flockVelocity = velocity;
        // transform.localPosition = flockCenter;
    }

    public void AddBoid(Vector3 pos)
    {
        Boid boid = SpawnBoid();
        flockSize++;
        boid.transform.position = pos;
    }
}
