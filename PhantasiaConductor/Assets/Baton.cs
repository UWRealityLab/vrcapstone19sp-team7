using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baton : MonoBehaviour
{

    public float batonLevel = 0;
    // Start is called before the first frame update
    public Renderer renderer;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.SetFloat("_Completion", 0.0f);
    }

    public void setCompletion(float completion)
    {
        Debug.Log("HI");
        renderer.material.SetFloat("_Completion", 0.0f);
        if (completion == 1.0f)
        {
            //some particle thing
        }
    }
}
