using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BoidFactory : ScriptableObject
{
    [SerializeField]
    GameObject[] prefabs;

    public GameObject GetRandomBoid() {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        return Instantiate(prefab);
    }

    
}
