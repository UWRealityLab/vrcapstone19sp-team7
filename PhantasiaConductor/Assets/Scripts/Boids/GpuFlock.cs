using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GpuBoid
{
    // treat this as local position
    public Vector3 position;
    public Vector3 direction;
    public float noiseOffset;
}

public class GpuFlock : MonoBehaviour
{
    public ComputeShader computeShader;

    public int numThreadGroups = 6;

    public BoidFactory factory;

    public GameObject boidPrefab;

    public int numBoids = 100;
    public float spawnRadius;
    
    public GpuBoid[] boidsData;
    public Transform target;

    public float noiseScale = 10f;
    

    private GameObject[] allBoids;
    private int kernelHandle;

    public float rotationSpeed = 4f;
    public float boidSpeed = 10f;
    public float neighborDistance = 2f;
    public float boidSpeedVariation = .9f;
    
    void Start()
    {
        allBoids = new GameObject[numBoids];
        boidsData = new GpuBoid[numBoids];
        kernelHandle = computeShader.FindKernel("CSMain");

        for (int i = 0; i < numBoids; i++)
        {
            boidsData[i] = CreateBoidData();
            allBoids[i] = factory.GetRandomBoid();
            allBoids[i].transform.position = boidsData[i].position;
            allBoids[i].transform.rotation = Quaternion.Euler(boidsData[i].direction);
            allBoids[i].transform.parent = transform;

            boidsData[i].direction = allBoids[i].transform.forward;
            boidsData[i].noiseOffset = GetNoiseOffset();
        }
    }

    GpuBoid CreateBoidData()
    {
        GpuBoid boidData = new GpuBoid();
        boidData.position = Random.insideUnitSphere * spawnRadius;
        return boidData;
    }

    private GameObject InstantiateBoid() {
        GameObject boid = factory.GetRandomBoid();
        boid.transform.parent = transform;
        return boid;
    }

    // world positions
    public void AddBoids(int n, Vector3[] positions) {
        numBoids += n;
        GpuBoid[] newBoidData = new GpuBoid[numBoids];
        GameObject[] newAllBoids = new GameObject[numBoids];

        boidsData.CopyTo(newBoidData, 0);
        allBoids.CopyTo(newAllBoids, 0);

        for (int i = boidsData.Length; i < numBoids; i++) {
            Vector3 p = positions[i - boidsData.Length];
            
            GameObject boid = InstantiateBoid();
            newAllBoids[i] = boid;
            
            boid.transform.position = p;
            boid.transform.rotation = Quaternion.Euler(newBoidData[i].direction);
            
            newBoidData[i].position = transform.InverseTransformPoint(p);
            newBoidData[i].direction = boid.transform.forward;
            newBoidData[i].noiseOffset = GetNoiseOffset();
        }

        boidsData = newBoidData;
        allBoids = newAllBoids;
    }
    
    float GetNoiseOffset() {
        return Random.value * noiseScale;
    }

    void Update()
    {
        ComputeBuffer buffer = new ComputeBuffer(numBoids, 28);
        buffer.SetData(boidsData);

        computeShader.SetBuffer(kernelHandle, "boidBuffer", buffer);
        computeShader.SetFloat("DeltaTime", Time.deltaTime);
        computeShader.SetFloat("RotationSpeed", rotationSpeed);
        computeShader.SetFloat("BoidSpeed", boidSpeed);
        computeShader.SetFloat("BoidSpeedVariation", boidSpeedVariation);
        computeShader.SetVector("FlockPosition", transform.InverseTransformPoint(target.transform.position));
        computeShader.SetFloat("NeighborDistance", neighborDistance);
        computeShader.SetInt("BoidsCount", numBoids);

        // computeShader.Dispatch(kernelHandle, numBoids, 1, 1);
        // threadGroups * threadGroupSize = total elements processed
        computeShader.Dispatch(kernelHandle, numThreadGroups, 1, 1);

        // For future reference: Here we decided between using a maxBoids property and just setting the array to that
        // vs just copying array values into a new array when boids are emitted
        // buffer.GetData(boidsData, 0, 0, numBoids);
        buffer.GetData(boidsData);
        buffer.Release();

        for (int i = 0; i < boidsData.Length; i++) {
            allBoids[i].transform.localPosition = boidsData[i].position;
            if (!boidsData[i].direction.Equals(Vector3.zero)) {
                allBoids[i].transform.rotation = Quaternion.LookRotation(boidsData[i].direction);
            }
        }
    }
}
