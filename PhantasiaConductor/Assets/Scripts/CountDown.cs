using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public int timeLeft = 5;
    public Canvas darkCanvas;
    public Text countdown;
    public GameController controller;
    public GameObject rhythm;
    public bool trig = false;
    public int cSwitch = 0;
    void OnEnable()
    {
        Invoke("Dim", 0);
        countdown.GetComponent<Text>().enabled = true;
        StartCoroutine("LoseTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }
    void Update()
    {
        countdown.text = ("Keep holding the trigger to skip puzzle in " + timeLeft);
        if (Input.GetKey(KeyCode.Backspace) && timeLeft != 0)
        {
            darkCanvas.enabled = false;
            timeLeft = 5;
        }

        if (timeLeft == 0)
        {
            countdown.GetComponent<Text>().enabled = false;
            
            //case 1 = rhythm, case 2 = bass, case 3 = melody, case 4 = harmony

            switch (cSwitch)
            {
                case 1:
                    trig = true;
                    break;

                case 2:
                    FindObjectOfType<RadialSequence>().onSuccess.Invoke();
                    break;

                case 3:
                    break;

                case 4:
                    trig = true;
                    break;

            }

            controller.SetNextActive();
            darkCanvas.enabled = false;
            timeLeft = 5;

        }
    }
    
    IEnumerator LoseTime()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

    }

    void Dim()
    {
        darkCanvas.enabled = true;
    }
}
