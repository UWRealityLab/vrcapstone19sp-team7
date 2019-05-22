using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(VelocityEstimator))]
    public class CustumThrowable : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

        [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
        public Transform attachmentOffset;

        [Tooltip("How fast must this object be moving to attach due to a trigger hold instead of a trigger press? (-1 to disable)")]
        public float catchingSpeedThreshold = -1;

        public ReleaseStyle releaseVelocityStyle = ReleaseStyle.GetFromHand;

        [Tooltip("The time offset used when releasing the object with the RawFromHand option")]
        public float releaseVelocityTimeOffset = -0.011f;

        public float scaleReleaseVelocity = 1.1f;

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;



        protected VelocityEstimator velocityEstimator;
        protected bool attached = false;
        protected float attachTime;
        protected Vector3 attachPosition;
        protected Quaternion attachRotation;
        protected Transform attachEaseInTransform;

        public UnityEvent onPickUp;
        public UnityEvent onDetachFromHand;
        public UnityEvent<Hand> onHeldUpdate;


        protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;

        protected new Rigidbody rigidbody;

        [HideInInspector]
        public Interactable interactable;


        //-------------------------------------------------
        protected virtual void Awake()
        {
            velocityEstimator = GetComponent<VelocityEstimator>();
            interactable = GetComponent<Interactable>();



            rigidbody = GetComponent<Rigidbody>();
            rigidbody.maxAngularVelocity = 50.0f;


            if (attachmentOffset != null)
            {
                // remove?
                //interactable.handFollowTransform = attachmentOffset;
            }

        }

        //-------------------------------------------------
        protected virtual void OnAttachedToHand(Hand hand)
        {
            //Debug.Log("<b>[SteamVR Interaction]</b> Pickup: " + hand.GetGrabStarting().ToString());
            hadInterpolation = this.rigidbody.interpolation;

            attached = true;

            onPickUp.Invoke();

            hand.HoverLock(null);

            rigidbody.interpolation = RigidbodyInterpolation.None;

            velocityEstimator.BeginEstimatingVelocity();

            attachTime = Time.time;
            attachPosition = transform.position;
            attachRotation = transform.rotation;

        }


        //-------------------------------------------------
        protected virtual void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            onDetachFromHand.Invoke();

            hand.HoverUnlock(null);

            rigidbody.interpolation = hadInterpolation;

            Vector3 velocity;
            Vector3 angularVelocity;

            GetReleaseVelocities(hand, out velocity, out angularVelocity);

            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = angularVelocity;
            // Debug.Log(transform.forward);
            // rigidbody.velocity = transform.forward * velocity.magnitude;
            // rigidbody.angularVelocity = transform.forward * angularVelocity.magnitude;
        }


        public virtual void GetReleaseVelocities(Hand hand, out Vector3 velocity, out Vector3 angularVelocity)
        {
            if (hand.noSteamVRFallbackCamera && releaseVelocityStyle != ReleaseStyle.NoChange)
                releaseVelocityStyle = ReleaseStyle.ShortEstimation; // only type that works with fallback hand is short estimation.

            switch (releaseVelocityStyle)
            {
                case ReleaseStyle.ShortEstimation:
                    velocityEstimator.FinishEstimatingVelocity();
                    velocity = velocityEstimator.GetVelocityEstimate();
                    angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
                    break;
                case ReleaseStyle.AdvancedEstimation:
                    hand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
                    break;
                case ReleaseStyle.GetFromHand:
                    velocity = hand.transform.forward * hand.GetTrackedObjectVelocity(releaseVelocityTimeOffset).magnitude;
                    angularVelocity = hand.GetTrackedObjectAngularVelocity(releaseVelocityTimeOffset);
                    break;
                default:
                case ReleaseStyle.NoChange:
                    velocity = rigidbody.velocity;
                    angularVelocity = rigidbody.angularVelocity;
                    break;
            }

            if (releaseVelocityStyle != ReleaseStyle.NoChange)
                velocity *= scaleReleaseVelocity;
        }

        //-------------------------------------------------
        protected virtual IEnumerator LateDetach(Hand hand)
        {
            yield return new WaitForEndOfFrame();

            hand.DetachObject(gameObject, restoreOriginalParent);
        }
    }
}
