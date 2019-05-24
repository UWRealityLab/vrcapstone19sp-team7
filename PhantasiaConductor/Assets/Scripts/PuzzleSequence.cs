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

        public int currentPuzzle;
        public bool handColliders;

        public GameObject leftHand;
        public GameObject rightHand;

        
        public UnityEvent onPuzzleComplete;
        
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
            for (int i = 1; i < puzzles.Length; i++)
            {
                puzzles[i].SetActive(false);
            }
        }

        public void NextPuzzle() {
            winSource.Play();
            currentPuzzle++;
            if (currentPuzzle < puzzles.Length)
            {
                puzzles[currentPuzzle].SetActive(true);

            } else {
                onPuzzleComplete.Invoke();
            }
        }

        public void NewLoop(){
            for (int i = 0; i < puzzles.Length; i++) {
                puzzles[i].transform.Find("RhythmObject").GetComponent<PercussionObject>().NewLoop();
            }
        }

        public void MoveAllToCenter(){
            for (int i = 0; i < puzzles.Length; i++) {
                // puzzles[i].GetComponent<UnlockAction>().Unlock();
            }
        }
    }
}
