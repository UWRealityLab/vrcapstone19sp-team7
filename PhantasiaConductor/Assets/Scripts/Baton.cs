using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baton : MonoBehaviour
{
    
    // Start is called before the first frame update
    public Renderer rend;
    public Baton other;
    public AudioSource audio;

    void Awake()
    {
    	audio = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        audio.volume = 0;
    }


    public void SetCompletion(float completion, float time)
    {
        if (other != null)
        {
            Debug.Log("CCCC: " + completion);
        	audio.pitch = 1.5f + (1.5f * completion);
	    	audio.volume = .1f;
	    	CancelInvoke();
			Invoke("VolumeDown", .1f);
            other.SetCompletion(completion, time);
        }
        rend.materials[1].SetFloat("_Completion", Scaled(completion));
        
        if (completion == 1.0f)
        {
            //some particle thing
            Invoke("ResetCompletion", .5f);
        }
    }
    
    public void VolumeDown(){
    	audio.volume -= .01f;

    	if (audio.volume > 0){

            Invoke("VolumeDown", .1f);
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
