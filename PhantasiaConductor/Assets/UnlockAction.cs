using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAction : MonoBehaviour
{
	public Vector3 centerPosition;
	public Vector3 centerScale;
	public bool inCenter = false;
	public float radius;
	public bool counterClockwise;

	public GameObject rhythmObject;

	void Awake () {
		centerPosition = new Vector3(0,0,0);
		centerScale = new Vector3(1,1,1);
		inCenter = false;
		//rhythmObject = transform.parent.Find("RhythmObject");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (inCenter) {
			//transform.rotation.y += 1;
		} else {
			Vector3 delta = centerPosition - transform.position;
			if (delta.magnitude < .01f) {
				inCenter = true;
				transform.position = centerPosition;
			} else {
				transform.position += delta / 75;
			}
		}


		float deltaRadius = radius - rhythmObject.transform.localPosition.x;
		if (deltaRadius < .01f ) {
			rhythmObject.transform.localPosition = new Vector3(radius, 0, 0);
		} else {
			rhythmObject.transform.localPosition += new Vector3(deltaRadius / 75, 0, 0);
		}


		Vector3 deltaScale = centerScale - transform.localScale;
		if (deltaScale.magnitude < .01f) {
			transform.localScale = centerScale;
		} else {
			transform.localScale += deltaScale / 75;
		}
	}

	public void OnEnable(){
		GameObject center = GameObject.Find("/CenterArea");
		transform.SetParent(center.transform);
	}
}
