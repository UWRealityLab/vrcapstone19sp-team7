using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
    public class PerspectiveShift : MonoBehaviour
    {
        public bool MOUSE_DEBUG = false;

        public SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");
       
        public Hand leftHand;
        public Hand rightHand;
        public Camera playerCamera;

        public float speed;
        public bool teleportEnabled;
        public float fadeTime = 1.0f;
        public float teleportDelay = 0.5f;

        // to prevent motion sickness
        public GameObject motionOverlay;

        public UnityEvent onLeavePodium;

        private bool teleporting;
        private float rotateSpeed;
        private bool initializingTeleport = false;

        private GameObject target;
        private Vector3 targetPosition;
        private CustomLaserPointer leftLaser;
        private CustomLaserPointer rightLaser;
      
        void Awake()
        {
            leftLaser = leftHand.GetComponent<CustomLaserPointer>();
            rightLaser = rightHand.GetComponent<CustomLaserPointer>();

            leftLaser.active = true;
            leftLaser.PointerIn += onPointerIn;
            leftLaser.PointerOut += onPointerOut;

            rightLaser.active = true;
            rightLaser.PointerIn += onPointerIn;
            rightLaser.PointerOut += onPointerOut;
        } 

        // Start is called before the first frame update
        void Start()
        {
            teleporting = false;
            target = null;
        }

        // Update is called once per frame
        void Update()
        {
            if (teleporting)
            {
                if (transform.position == targetPosition && transform.rotation == target.transform.rotation) // reached destination
                {
                    teleporting = false;
                    StartCoroutine(FadeThenFinish());
                }
                else // still moving
                {
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                    float rotateStep = rotateSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, rotateStep);

                    Vector3 overlayPos = playerCamera.transform.forward * 2 + playerCamera.transform.position;

                    motionOverlay.transform.position = overlayPos;

                }
            }

            /*
            else if (teleportEnabled)
            {
                if (WasTeleportButtonReleased(leftHand))
                {
                    teleport(leftHand);
                    leftLaser.active = false;
                }
                else if (WasTeleportButtonReleased(rightHand))
                {
                    teleport(rightHand);
                    rightLaser.active = false;
                }
                else if (IsTeleportButtonDown(leftHand) && !IsTeleportButtonDown(rightHand))
                {
                    leftLaser.active = true; 
                }
                else if (IsTeleportButtonDown(rightHand) && !IsTeleportButtonDown(leftHand))
                {
                    rightLaser.active = true;
                }
            }
            */
        }

        /*
        private void teleport(Hand hand)
        {
            RaycastHit hit;
            Ray ray = new Ray(hand.transform.position, hand.transform.forward);
            
            if (MOUSE_DEBUG)
            {
                ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            }

            Debug.Log("ray " + ray);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 2)))
            {
                if (string.Equals(hit.collider.tag, "teleportDest"))
                {
                    target = hit.collider.gameObject;
                    targetPosition = new Vector3(target.transform.position.x, target.transform.position.y - 1.5f, target.transform.position.z);
                    
                    // hide the target when start moving
                    target.gameObject.SetActive(false);

                    Debug.Log("hit " + hit.collider.gameObject.name);

                    StartCoroutine(FadeThenMove());

                    onLeavePodium.Invoke();
                }

            }
        } */

        private IEnumerator FadeThenFinish()
        {
            yield return StartCoroutine(FadeCanvas(motionOverlay.GetComponent<CanvasGroup>(), 1.0f, 0.0f, fadeTime));
            teleportEnabled = true;

            motionOverlay.SetActive(false);

            target.transform.parent.transform.GetComponent<DestinationActions>().onArrive();
            /* if (string.Equals(target.name, "PodimTeleportInd"))
            {
                leftLaser.active = true;
                rightLaser.active = true;
            }*/
        }

        private IEnumerator FadeThenMove()
        {
            teleportEnabled = false;

            float time = 1.0f * Vector3.Distance(transform.position, target.transform.position) / speed;
            rotateSpeed = Quaternion.Angle(transform.rotation, target.transform.rotation) / time;

            yield return StartCoroutine(FadeCanvas(motionOverlay.GetComponent<CanvasGroup>(), 0.0f, 1.0f, fadeTime));

            teleporting = true;
        }

        private IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
        {
            // keep track of when the fading started, when it should finish, and how long it has been running&lt;/p&gt; &lt;p&gt;&a
            var startTime = Time.time;
            var endTime = Time.time + duration;
            var elapsedTime = 0f;

            // set the canvas to the start alpha – this ensures that the canvas is ‘reset’ if you fade it multiple times
            canvas.alpha = startAlpha;
            motionOverlay.SetActive(true);
            // loop repeatedly until the previously calculated end time
            while (Time.time <= endTime)
            {
                elapsedTime = Time.time - startTime; // update the elapsed time
                var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
                if (startAlpha > endAlpha) // if we are fading out/down 
                {
                    canvas.alpha = startAlpha - percentage; // calculate the new alpha
                }
                else // if we are fading in/up
                {
                    canvas.alpha = startAlpha + percentage; // calculate the new alpha
                }

                yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
            }
            canvas.alpha = endAlpha; // force the alpha to the end alpha before finishing – this is here to mitigate any rounding errors, e.g. leaving the alpha at 0.01 instead of 0
        }

        /*
        private bool IsTeleportButtonDown(Hand hand)
        {
            if (MOUSE_DEBUG)
            {
                return Input.GetKeyDown(KeyCode.T);
            }
            else
            {
                return teleportAction.GetStateDown(hand.handType);
            }
        }

        private bool WasTeleportButtonReleased(Hand hand)
        {
            if (MOUSE_DEBUG)
            {
                return Input.GetKeyUp(KeyCode.T);
            }
            else
            {
                return teleportAction.GetStateUp(hand.handType);
            }
        }

        // Laser Pointer Methods

        private void onPointerIn(object sender, PointerEventArgs e)
        {
            if (string.Equals(e.target.tag, "teleportDest"))
            {
                e.target.gameObject.GetComponent<Pulse>().enablePulse = false;
                e.target.gameObject.GetComponent<Glow>().GlowOn();
            }
        }

        private void onPointerOut(object sender, PointerEventArgs e)
        {
            if (string.Equals(e.target.tag, "teleportDest"))
            {
                e.target.gameObject.GetComponent<Glow>().GlowOff();
                e.target.gameObject.GetComponent<Pulse>().enablePulse = true;
            }
        }

    */

        // Laser Pointer methods 
        private void onPointerIn(object sender, PointerEventArgs e)
        {
            if (string.Equals(e.target.tag, "teleportDest"))
            {
                StartCoroutine(startTeleport(e.target.gameObject));
            }
        }

        private void onPointerOut(object sender, PointerEventArgs e)
        {
            if (string.Equals(e.target.tag, "teleportDest"))
            {
                initializingTeleport = false;
                e.target.gameObject.GetComponent<Grow>().StopGrow();
            }
        }

        private IEnumerator startTeleport(GameObject target)
        {
            initializingTeleport = true;
            yield return StartCoroutine(target.GetComponent<Grow>().GrowObject());

            if (initializingTeleport)
            {
                yield return new WaitForSeconds(teleportDelay);
                TeleportTo(target);
                onLeavePodium.Invoke();
            }
        }

        public void TeleportTo(GameObject destination) 
        {
            LasersOff();

            target = destination;
            targetPosition = new Vector3(target.transform.position.x, target.transform.position.y - 1.5f, target.transform.position.z);
            target.SetActive(false);

            StartCoroutine(FadeThenMove());
        }

        public void LasersOn()
        {
            leftLaser.active = true;
            rightLaser.active = true;
        }

        public void LasersOff()
        {
            leftLaser.active = false;
            rightLaser.active = false;
        }
    }
}
