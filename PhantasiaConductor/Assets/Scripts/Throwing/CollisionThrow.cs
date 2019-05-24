using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CollisionThrow : MonoBehaviour
{

    Rigidbody rigidBody;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Vector3.zero, Vector3.up, Color.red);
    }

    void OnCollisionEnter(Collision col) {
        Debug.Log(string.Format("contact count {0}", col.contactCount));

        ContactPoint[] contactPoints = new ContactPoint[col.contactCount];
        int numContacts = col.GetContacts(contactPoints);

        for (var i = 0; i < numContacts; i++) {
            ContactPoint cp = contactPoints[i];
            // Debug.Log("contact point " + cp.normal + " point" + cp.point);
            rigidBody.AddForce(cp.normal * 100, ForceMode.Acceleration);
            Debug.DrawRay(cp.point, cp.normal * 100, Color.red, 10f);
        }
        
    }
}
