using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @AJSTEAMVR

namespace Valve.VR.InteractionSystem
{
    public class RadialController : MonoBehaviour
    {

        public float radius = 10f;

        public float heightOffset = 0f;

        public GameObject netObj;

        public GameObject leftNetObj;

        public Hand rightHand;

        public Hand leftHand;


        public Transform originTransform;

        private bool mouseMode = true;

        // Start is called before the first frame update
        void Start()
        {
            mouseMode = rightHand == null && leftHand == null;
        }

        void Update()
        {
            // we have to use world positions since this is not necessarily a child of originTransform
            if (mouseMode)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                ray.direction = new Vector3(ray.direction.x, 0, ray.direction.z).normalized;
                // this doesn't really make sense unless it originTransform is the camera
                ray.origin = originTransform.position;
                Vector3 pos = ray.origin + (ray.direction * radius);

                netObj.transform.position = new Vector3(pos.x, originTransform.position.y + heightOffset, pos.z);
            }
            else
            {
                {
                    Vector3 transformPos = originTransform.position;
                    Vector3 dir = (rightHand.transform.forward).normalized;

                    Ray ray = new Ray();
                    ray.origin = transformPos;
                    ray.direction = new Vector3(dir.x, 0, dir.z).normalized;


                    Vector3 pos = ray.origin + (ray.direction * radius);
                    // netObj.transform.position = new Vector3(pos.x, transform.position.y + heightOffset, pos.z);
                    netObj.transform.position = new Vector3(pos.x, rightHand.transform.position.y + heightOffset, pos.z);
                }
                {
                    Vector3 transformPos = originTransform.position;
                    Vector3 dir = (leftHand.transform.forward).normalized;

                    Ray ray = new Ray();
                    ray.origin = transformPos;
                    ray.direction = new Vector3(dir.x, 0, dir.z).normalized;

                    Vector3 pos = ray.origin + (ray.direction * radius);
                    // netObj.transform.position = new Vector3(pos.x, transform.position.y + heightOffset, pos.z);
                    leftNetObj.transform.position = new Vector3(pos.x, leftHand.transform.position.y + heightOffset, pos.z);
                }
            }
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }
    }
}