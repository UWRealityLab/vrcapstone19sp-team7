using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baton : MonoBehaviour
{
    
    // Start is called before the first frame update
    public Renderer rend;
    public Baton other;
    void Awake()
    {
        rend = GetComponent<Renderer>();
    }


    public void setCompletion(float completion, float time)
    {
        //Debug.Log("HI" + other);
        //other.setCompletion(completion, time);
        
        rend.materials[1].SetFloat("_Completion", completion);
        
        if (completion == 1.0f)
        {
            //some particle thing
            Invoke("Reset", .25f);
        }
    }

    public void Reset()
    {
        rend.materials[1].SetFloat("_Completion", 0);
        other.Reset();
    }
}
