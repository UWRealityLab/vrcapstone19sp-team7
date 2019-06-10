﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    // Start is called before the first frame update
	public void Move(){
		StartCoroutine(MoveDownSteps());
	}   

    public IEnumerator MoveDownSteps(){
		for (int i = 0; i < 100; i++) {
	    	yield return new WaitForSeconds(.025f);
	    	transform.position += new Vector3(0, -.004f, 0);
    	}
    }

    
    
}