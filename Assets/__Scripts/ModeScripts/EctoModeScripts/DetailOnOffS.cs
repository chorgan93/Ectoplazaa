using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetailOnOffS : MonoBehaviour {

	// place on any ui object that should not appear in a certain mode
	public List<int> myModes; // modes element should appear in

	// Use this for initialization
	void Start () {

		bool dontDestroy = false;

		for (int i = 0; i < myModes.Count; i++){
			if (CurrentModeS.currentMode == myModes[i]){
				dontDestroy = true;
			}
		}

		if (!dontDestroy){
			gameObject.SetActive(false);
		}

	
	}
}
