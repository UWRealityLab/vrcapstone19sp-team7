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
    void OnEnabled()
    {
        Invoke("Dim", 0);
        countdown.GetComponent<Text>().enabled = true;
        StartCoroutine("LoseTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }
    void Update()
    {
        countdown.text = ("Skip puzzle in " + timeLeft); 
        if (timeLeft == 0)
        {
            countdown.GetComponent<Text>().enabled = true;
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
