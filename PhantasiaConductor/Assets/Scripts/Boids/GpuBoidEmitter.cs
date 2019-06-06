using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpuBoidEmitter : MonoBehaviour
{
    // Start is called before the first frame update
    public GpuFlock flock;

    public float emitRadius = 1.5f;


    public void EmitBoids(int n) {
        Vector3[] positions = new Vector3[n];
        for (int i = 0; i < n; i++) {
            positions[i] = GetRandomPosition();
        }
        flock.AddBoids(n, positions);
    }

    public Vector3 GetRandomPosition() {
        return transform.position + Random.insideUnitSphere * emitRadius;
    }


}
