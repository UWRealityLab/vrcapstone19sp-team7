using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonyMovement : MonoBehaviour
{
    // Start is called before the first frame update
	public void MoveDown(){
		StartCoroutine(MoveDownSteps());
	}   

    public IEnumerator MoveDownSteps(){
		for (int i = 0; i < 100; i++) {
	    	yield return new WaitForSeconds(.025f);
	    	transform.position += new Vector3(0, -.005f, 0);
    	}
    }

    
    
}
