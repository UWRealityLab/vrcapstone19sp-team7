using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class HandController : MonoBehaviour
    {
        public bool debugMode = true;

        public Hand leftHand;
        public Hand rightHand;

        // private int lastInstanceIdLeft;
        // private bool interactedLastFrameLeft;

        public bool renderingLines = true;

        private Dictionary<Hand, int> lastInstanceIds = new Dictionary<Hand, int>();

        private Dictionary<Hand, bool> interactedLastFrame = new Dictionary<Hand, bool>();

        // private int lastInstanceIdRight;
        // private bool interactedLastFrameRight;

        private List<Hand> hands = new List<Hand>();


        public LineRenderer leftLineRenderer;
        public LineRenderer rightLineRenderer;

        public bool extraRays = false;

        // for extra help with raycasting
        private List<LineRenderer> leftExtraLineRenderers;
        private List<LineRenderer> rightExtraLineRenderers;


        private SteamVR_Action_Boolean gripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
        private SteamVR_Action_Boolean pinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

        private SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");

        // [EnumFlags]
        // public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.
        // Start is called before the first frame update

        void Start()
        {
            hands.Add(rightHand);
            hands.Add(leftHand);

            lastInstanceIds[rightHand] = -1;
            lastInstanceIds[leftHand] = -1;

            interactedLastFrame[rightHand] = false;
            interactedLastFrame[leftHand] = false;

            leftExtraLineRenderers = new List<LineRenderer>();
            rightExtraLineRenderers = new List<LineRenderer>();

            for (var i = 0; i < 4; i++)
            {
                // leftExtraLineRenderers.Add(gameObject.AddComponent<LineRenderer>());
                // rightExtraLineRenderers.Add(gameObject.AddComponent<LineRenderer>());
            }
        }


        // Update is called once per frame
        void Update()
        {
            // TODO figure out why raycast doesn't ignore layers

            // if (Physics.Raycast(leftHand.transform.position, leftHand.transform.rotation * transform.forward, Mathf.Infinity, ~(1 << 2)))
            // Debug.Log("we have hit");
            foreach (var hand in hands)
            {
                // Raycast only once for each hand
                GameObject obj = PerformRaycast(hand);
                if (obj == null)
                {
                    continue;
                }

                if (pinchAction.GetStateDown(hand.handType))
                {
                    obj.SendMessage("OnPinched", SendMessageOptions.DontRequireReceiver);
                }

                if (gripAction.GetStateDown(hand.handType))
                {
                    obj.SendMessage("OnGripped", SendMessageOptions.DontRequireReceiver);
                }

                if (!interactedLastFrame[hand])
                {
                    obj.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                    lastInstanceIds[hand] = obj.GetInstanceID();
                    interactedLastFrame[hand] = true;
                }
                else
                {
                    lastInstanceIds[hand] = -1;
                    interactedLastFrame[hand] = false;
                }

                obj.SendMessage("OnTracked", SendMessageOptions.DontRequireReceiver);
            }


            if (debugMode)
            {
                Debug.DrawRay(leftHand.transform.position, leftHand.transform.forward * 1000, Color.green);
                Debug.DrawRay(rightHand.transform.position, rightHand.transform.forward * 1000, Color.blue);
            }
            
            if (renderingLines)
            {
                if (leftLineRenderer != null)
                {
                    Vector3 start = leftHand.transform.position;
                    // Vector3 end = leftHand.transform.rotation * transform.forward * 1000 + start;
                    Vector3 end = leftHand.transform.forward * 1000 + start;

                    leftLineRenderer.SetPosition(0, start);
                    leftLineRenderer.SetPosition(1, end);
                }

                if (rightLineRenderer != null)
                {
                    Vector3 start = rightHand.transform.position;
                    // Vector3 end = rightHand.transform.rotation * transform.forward * 1000 + start;
                    Vector3 end = rightHand.transform.forward * 1000 + start;

                    rightLineRenderer.SetPosition(0, start);
                    rightLineRenderer.SetPosition(1, end);
                }
            }
        }

        private GameObject PerformRaycast(Hand hand)
        {
            RaycastHit hit;
            // LayerMask layerMask = ~(1 << 2);
            LayerMask layerMask = LayerMask.GetMask("Interactable");
            // if (Physics.Raycast(hand.transform.position, hand.transform.rotation * transform.forward, out hit, Mathf.Infinity, layerMask))
            if (Physics.Raycast(hand.transform.position, hand.transform.forward, out hit, Mathf.Infinity, layerMask))
            {
                GameObject obj = hit.collider.gameObject;
                return obj;
            }
            return null;
        }

    }
}

