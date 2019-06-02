using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{

    public static bool confettiOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfettiOn() {
        confettiOn = true;
    }

    public void ConfettiOff()
    {
        confettiOn = false;
    }
}
