using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
    public class PuzzleSequence : MonoBehaviour
    {

        public GameObject[] puzzles;
        private AudioSource winSource;
        public AudioClip winClip;
        public int cheat = 0;
        public int currentPuzzle;
        public bool handColliders;
        public bool isRhythm;
        public bool isMelody;
        
        public GameObject leftHand;
        public GameObject rightHand;

        
        public UnityEvent onNextPuzzle;
        public UnityEvent onPuzzleComplete;
        private bool waitingToEnableNext = true;
        
        // Start is called before the first frame update
        void Awake()
        {
            
            if (handColliders)
            {
                leftHand.GetComponent<Collider>().enabled = true;
                rightHand.GetComponent<Collider>().enabled = true;
            } else
            {
                leftHand.GetComponent<Collider>().enabled = false;
                rightHand.GetComponent<Collider>().enabled = false;
            }
            

            winSource = GetComponent<AudioSource>();
            winSource.clip = winClip;
            winSource.volume = .75f;
            currentPuzzle = 0;
            puzzles[0].SetActive(true);
            for (int i = 0; i < puzzles.Length; i++)
            {
                puzzles[i].SetActive(false);
            }
        }

        public void NextPuzzle() {
            winSource.Play();
            currentPuzzle++;
            onNextPuzzle.Invoke();
            if (currentPuzzle < puzzles.Length - cheat)
            {
                waitingToEnableNext = true;
                
            } else {
                onPuzzleComplete.Invoke();
            }
        }

        public void NewLoop(){

            if (gameObject.activeInHierarchy && waitingToEnableNext)
            {
                waitingToEnableNext = false;
                if (currentPuzzle < puzzles.Length)
                {
                    puzzles[currentPuzzle].SetActive(true);
                }
            }
            for (int i = 0; i < puzzles.Length; i++) {

                if (isRhythm) {
                    puzzles[i].transform.Find("RhythmObject").GetComponent<PercussionObject>().NewLoop();
                } else if (isMelody) {
                    puzzles[i].transform.Find("MelodyObject").GetComponent<MelodyObject>().NewLoop();
                } else {
                    puzzles[i].GetComponent<HarmonyObject>().NewLoop();
                }
            }
        }

        public void FantasiaOn()
        {
            for (int i = 0; i < puzzles.Length; i++)
            {
                if (isRhythm)
                {
                    puzzles[i].transform.Find("RhythmObject").GetComponent<PercussionObject>().fantasiaOn = true;
                } else if (isMelody)
                {
                    puzzles[i].transform.Find("MelodyObject").GetComponent<MelodyObject>().fantasiaOn = true;
                }
                else
                {
                    puzzles[i].GetComponent<HarmonyObject>().fantasiaOn = true;
                }
            }
        }

        public void PauseFantasia()
        {
            for (int i = 0; i < puzzles.Length; i++)
            {
                if (isRhythm)
                {
                    puzzles[i].transform.Find("RhythmObject").GetComponent<BeatBlinkController>().PauseFantasia();
                }
                else if (isMelody)
                {
                    puzzles[i].GetComponent<PathBeat>().PauseFantasia();
                }
                else
                {
                    puzzles[i].GetComponent<HarmonyObject>().PauseFantasia();
                }
            }
        }

        public void ResumeeFantasia()
        {
            for (int i = 0; i < puzzles.Length; i++)
            {
                if (isRhythm)
                {
                    puzzles[i].transform.Find("RhythmObject").GetComponent<BeatBlinkController>().ResumeFantasia();
                }
                else if (isMelody)
                {
                    puzzles[i].GetComponent<PathBeat>().ResumeFantasia();
                }
                else
                {
                    puzzles[i].GetComponent<HarmonyObject>().ResumeFantasia();
                }
            }
        }
        
    }	
}
