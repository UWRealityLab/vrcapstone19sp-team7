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


    public void SetCompletion(float completion, float time)
    {
        //Debug.Log("HI" + other);
        if (other != null)
        {
            other.SetCompletion(completion, time);
        }
        rend.materials[1].SetFloat("_Completion", Scaled(completion));
        
        if (completion == 1.0f)
        {
            //some particle thing
            Invoke("ResetCompletion", .25f);
        }
    }
    

    public void ResetCompletion()
    {
        rend.materials[1].SetFloat("_Completion", 0);
        if (other != null)
        {
            other.ResetCompletion();
        }
    }

    private float Scaled(float input)
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
