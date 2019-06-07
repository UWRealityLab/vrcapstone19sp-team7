using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipUI : MonoBehaviour
{
    public float holdTime = 3f;
    private float holdTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer > holdTime)
            {
                FindObjectOfType<CountDown>().enabled = true;
                Invoke("Reset", 5.5f);
            }
        }
    }
    private void Reset()
    {
        FindObjectOfType<CountDown>().enabled = false;
        holdTimer = 0f;
    }
}
