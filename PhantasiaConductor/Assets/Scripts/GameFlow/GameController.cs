using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class GameController : MonoBehaviour
{
    public GameObject[] puzzleOrder;
    public int[] numActive;
    public GameObject[] fadeObjects;

    public GameObject podiumPosition;
    public float returnDelay;
    public float fadeDelay;

    public UnityEvent onPuzzlesComplete;

    private Fade fade;
    private int puzzleIndex;
    private int activeIndex;

    private void Awake()
    {
        fade = GetComponent<Fade>();
    }

    private void Start()
    {
        puzzleIndex = 0;
        activeIndex = 0;
        for (int i = puzzleIndex; i < numActive[activeIndex]; i++)
        {
            puzzleOrder[i].SetActive(true);
            puzzleIndex++;
        }
        activeIndex++;
    }

    public void SetNextActive()
    {
        if (puzzleIndex == puzzleOrder.Length)
        {
            onPuzzlesComplete.Invoke();
        }
        else
        {
            for (int i = 0; i < numActive[activeIndex]; i++)
            {
                puzzleOrder[puzzleIndex].SetActive(true);
                puzzleIndex++;
            }
            activeIndex++;
        }
    }

    public void FadeInAll()
    {
        foreach (GameObject obj in fadeObjects)
        {
            // obj.SetActive(true);
            if (obj.activeInHierarchy)
            {
                fade.FadeIn(obj);
            }
        }
    }

    public void FadeOutAll()
    {
        foreach (GameObject obj in fadeObjects)
        {
            if (obj.activeInHierarchy)
            {
                // obj.SetActive(false);
                fade.FadeOut(obj);
            }
        }
    }

    public void GoBackToPodium()
    {
        podiumPosition.SetActive(true);
        podiumPosition.GetComponent<Glow>().GlowOn();
        StartCoroutine(TeleportBackWithDelay());
    }

    private IEnumerator TeleportBackWithDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        // Player.instance.GetComponent<PerspectiveShift>().TeleportTo(podiumPosition);
        AgnosticPlayer.GetPlayer().GetComponent<PerspectiveShift>().TeleportTo(podiumPosition);
        podiumPosition.GetComponent<Glow>().GlowOff();
        podiumPosition.SetActive(false);

        yield return new WaitForSeconds(fadeDelay);

        // Player.instance.GetComponent<PerspectiveShift>().teleportEnabled = true;
        AgnosticPlayer.GetPlayer().GetComponent<PerspectiveShift>().teleportEnabled = true;

        // Fade In
        FadeInAll();

        // Set next puzzles active
        SetNextActive();
    }
}
