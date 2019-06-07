using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipUI : MonoBehaviour
{
    public float holdTime = 3f;
    public CountDown cd;
    private float holdTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.deltaTime;
            //Debug.Log(holdTimer);

            if (holdTimer > holdTime)
            {
                Debug.Log(holdTimer);
                cd.enabled = true;
                Invoke("Reset", 5.5f);
            }
        }
    }
    private void Reset()
    {
        cd.enabled = false;
        holdTimer = 0f;
    }
}
