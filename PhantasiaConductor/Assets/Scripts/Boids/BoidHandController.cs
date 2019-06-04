using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class BoidHandController : MonoBehaviour
    {

        public Hand leftHand;
        public Hand rightHand;

        public GameObject haptics;
        public GameObject rightTarget;
        public GameObject leftTarget;

        public float placementRadius = 35;

       

        void Start()
            {
                haptics = GameObject.Find("/Haptics");
            }

        // Update is called once per frame
        void Update()
        {
            if (rightHand != null && rightTarget != null)
            {
                PlaceTarget(rightTarget, rightHand);
                
            }

            if (leftHand != null && leftTarget != null)
            {
                PlaceTarget(leftTarget, leftHand);
            }

        }

        void PlaceTarget(GameObject targ, Hand hand)
        {
            var dir = hand.transform.rotation * transform.forward;
            var pos = placementRadius * dir + hand.transform.position;

            targ.transform.position = pos;
        }

        void OnEnable() {
            InvokeRepeating("goodVibes", 0, 4f);
        }

        void goodVibes () {
            haptics.GetComponent<Haptics>().sweepHandRight();
            haptics.GetComponent<Haptics>().sweepHandLeft();
        }
    }
}