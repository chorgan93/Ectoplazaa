using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdaptiveCameraPtS : MonoBehaviour {

	public List<Transform> playerPositions;
	public Transform centerPt;

	public float playerWeight;
	public float centerWeight;


	
	// Update is called once per frame
	void Update () {

		// create var for changing pos
		Vector3 adaptPt = centerPt.transform.position;

		// create player centerpt

		if (playerPositions.Count > 0){
			Vector3 playerCenterPos = Vector3.zero;
			for (int i = 0; i < playerPositions.Count; i++){
				playerCenterPos += playerPositions[i].position;
			}
			playerCenterPos/=playerPositions.Count;

			// add two values together and divide by total weight

			adaptPt = (centerPt.transform.position*centerWeight + playerCenterPos*playerWeight)/(centerWeight+playerWeight);
		}

		// set pos to adaptPt
		adaptPt.z = transform.position.z;

		transform.position = adaptPt;


	
	}

	public float GetXDiff () {

		float xDiff = transform.position.x-centerPt.transform.position.x;

		return(xDiff);

	}

}
