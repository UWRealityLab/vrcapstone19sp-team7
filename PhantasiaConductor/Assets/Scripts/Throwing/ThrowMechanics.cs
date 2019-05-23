using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class ThrowMechanics : MonoBehaviour
    {
        public SteamVR_Action_Boolean releaseAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
        public Hand leftHand;
        public Hand rightHand;

        public float releaseVelocityTimeOffset = -0.011f;

        public float scaleReleaseVelocity = 1.1f;
        public bool throwEnabled;

        public bool MOUSE_DEBUG = false;

        private CustomSpawnAndAttachToHand spawn;
        private BeatInfo beatInfo;
        private GameObject leftThrowable = null;
        private GameObject rightThrowable = null;

        private bool startup = true;


        public float velocityThreshhold = 10f;
        private bool leftAboveThreshhold = false;
        private bool rightAboveThreshhold = false;

        private void Awake()
        {
            spawn = GetComponent<CustomSpawnAndAttachToHand>();
            // beatInfo = GetComponent<BeatInfo>();
        }

        /*
        void Start() 
        {
            if (throwEnabled)
            {
                leftThrowable = spawn.SpawnAndAttach(leftHand);
                rightThrowable = spawn.SpawnAndAttach(rightHand);
            }
        }
        */

            /*
        private void FixedUpdate()
        {

            Debug.Log(rightHand.GetTrackedObjectVelocity(0f).magnitude);
            if (leftHand.GetTrackedObjectVelocity(0f).magnitude > velocityThreshhold)
            {
                leftAboveThreshhold = true;
            }
            else if (leftAboveThreshhold && leftHand.GetTrackedObjectVelocity(0f).magnitude < velocityThreshhold)
            {
                if (leftThrowable != null)
                {
                    leftHand.DetachObject(leftThrowable);
                    StartCoroutine(delaySpawn(false));
                }
                leftThrowable = null;
                leftAboveThreshhold = false;
            }

            if (rightHand.GetTrackedObjectVelocity(0f).magnitude > velocityThreshhold)
            {
                rightAboveThreshhold = true;
            }
            else if (rightAboveThreshhold && rightHand.GetTrackedObjectVelocity(0f).magnitude < velocityThreshhold)
            {
                if (rightThrowable != null)
                {
                    rightHand.DetachObject(rightThrowable);
                    StartCoroutine(delaySpawn(true));
                }
                rightThrowable = null;
                rightAboveThreshhold = false;
                
            }
        }
        
        private IEnumerator delaySpawn(bool right)
        {
            yield return new WaitForSeconds(0.25f);
            if (right)
            {
                rightThrowable = spawn.SpawnAndAttach(rightHand);
            } else
            {
                leftThrowable = spawn.SpawnAndAttach(leftHand);
            }
        }
        */
        /*
        public void NewLoop()
        {
            if (startup && gameObject.activeInHierarchy)
            {
                startup = false;
                StartCoroutine(ReleasePerFrame());
            }
        }

        private IEnumerator ReleasePerFrame()
        {
            while(true)
            {
                if (throwEnabled)
                {
                    leftHand.DetachObject(leftThrowable);
                    rightHand.DetachObject(rightThrowable);

                    leftThrowable = spawn.SpawnAndAttach(leftHand);
                    rightThrowable = spawn.SpawnAndAttach(rightHand);
                }
                yield return new WaitForSeconds(beatInfo.beatTime);
            }
        }

        private void ReleaseOnBeat()
        {
            if (throwEnabled)
            {
                leftHand.DetachObject(leftThrowable);
                rightHand.DetachObject(rightThrowable);

                leftThrowable = spawn.SpawnAndAttach(leftHand);
                rightThrowable = spawn.SpawnAndAttach(rightHand);
            }
            Invoke("ReleaseOnBeat", beatInfo.beatTime);
        }
        */

                void FixedUpdate()
                {
                    if (throwEnabled)
                    {


                        if (leftThrowable == null && IsButtonDown(leftHand))
                        {
                            leftThrowable = spawn.SpawnAndAttach(leftHand);
                        } 
                        if (rightThrowable == null && IsButtonDown(rightHand))
                        {
                            rightThrowable = spawn.SpawnAndAttach(rightHand);
                        }
                        if (WasButtonReleased(leftHand))
                        {
                            leftHand.DetachObject(leftThrowable);
                            leftThrowable = null;
                        }
                        if (WasButtonReleased(rightHand))
                        {
                            rightHand.DetachObject(rightThrowable);
                            rightThrowable = null;
                        }

                    }
                }

        private bool IsButtonDown(Hand hand) {
            if (MOUSE_DEBUG)
            {
                return Input.GetKeyDown(KeyCode.T);
            }
            else
            {
                return releaseAction.GetStateDown(hand.handType);
            }
        }

        private bool WasButtonReleased(Hand hand)
        {
            if (MOUSE_DEBUG)
            {
                return Input.GetKeyUp(KeyCode.T);
            }
            else
            {
                return releaseAction.GetStateUp(hand.handType);
            }
        }

        public void DisableThrow()
        {
            throwEnabled = false;
            leftHand.DetachObject(leftThrowable);
            rightHand.DetachObject(rightThrowable);
        }

        public bool ThrowEnabled
        {
            set
            {
                throwEnabled = value;
            }
            get
            {
                return throwEnabled;
            }
        }
    }
}
