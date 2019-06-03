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
        if (other != null)
        {
            other.setCompletion(completion, time);
        }
        rend.materials[1].SetFloat("_Completion", scaled(completion));
        
        if (completion == 1.0f)
        {
            //some particle thing
            Invoke("Reset", .5f);
        }
    }

    public void Reset()
    {
        rend.materials[1].SetFloat("_Completion", 0);
        if (other != null)
        {
            other.Reset();
        }
    }

    private float scaled(float input)
    {
        if (input < .5)
        {
            return input * .73f; 
        } else
        {
            return .365f + (input - .5f) * 1.27f;
        }
    }
}
