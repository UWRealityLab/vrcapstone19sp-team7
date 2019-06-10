using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawn : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    public float spawnInterval = 0.25f;
    public GameObject prefab;

    private GameObject leftObj;
    private GameObject rightObj;

    private void OnEnable()
    {
        StartCoroutine(RunStarSpawn());
    }

    private IEnumerator RunStarSpawn()
    {
        while (gameObject.activeInHierarchy)
        {
            DetachObjects();

            yield return new WaitForSeconds(0.1f);

            leftObj = Instantiate(prefab);
            Debug.Log(leftObj);
            rightObj = Instantiate(prefab);
            Debug.Log(rightObj);

            leftObj.transform.parent = leftHand.transform;
            leftObj.transform.position = leftHand.transform.position;
            rightObj.transform.parent = rightHand.transform;
            rightObj.transform.position = rightObj.transform.position; 

            yield return new WaitForSeconds(spawnInterval);
        }
        yield return null;
    }


    private void OnDisable()
    {
        DetachObjects();
    }

    private void DetachObjects()
    {
        if (leftObj != null)
        {
            leftObj.GetComponent<StarSpawnObject>().Detach();
        }
        if (rightObj != null)
        {
            rightObj.GetComponent<StarSpawnObject>().Detach();
        }
    }
}
