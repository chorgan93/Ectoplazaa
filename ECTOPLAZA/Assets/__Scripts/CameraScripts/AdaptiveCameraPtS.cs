using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdaptiveCameraPtS : MonoBehaviour {

	public List<Transform> playerPositions;
	public List<Transform> hitPositions;
	private Vector3 hitCenter;
	public Transform centerPt;

	public float playerWeight;
	public float centerWeight;
	public float hitWeight;
	private float currentHitWeight;

	public float sizeAddMult = 0.1f;
	private float largestDistance;

	public static AdaptiveCameraPtS A;

	public bool dontDoCameraThing = false;

	void Awake () {
		A = this;
	}

	void Start () {
		playerPositions.Clear();

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < players.Length; i++){
			playerPositions.Add(players[i].transform);
		}
	}


	
	// Update is called once per frame
	void Update () {

		// create var for changing pos
		Vector3 adaptPt = centerPt.transform.position;

		CalculateHitPos();

		// create player centerpt

		if (playerPositions.Count > 0){
			Vector3 playerCenterPos = Vector3.zero;
			for (int i = 0; i < playerPositions.Count; i++){
				playerCenterPos += playerPositions[i].position;
			}
			playerCenterPos/=playerPositions.Count;

			// add two values together and divide by total weight

			adaptPt = (centerPt.transform.position*centerWeight + playerCenterPos*playerWeight
			           + hitCenter*currentHitWeight)/(centerWeight+playerWeight+currentHitWeight);
		}

		// set pos to adaptPt
		adaptPt.z = transform.position.z;

		transform.position = adaptPt;


	
	}

	void FixedUpdate () {

		SetCamMult();

	}

	void SetCamMult () {

		largestDistance = 0;
		Vector2 centerPos2d = Vector2.zero;
		centerPos2d.x = centerPt.position.x;
		centerPos2d.y = centerPt.position.y;

		if (playerPositions.Count > 0){
			for (int i = 0; i < playerPositions.Count; i++){
				Vector2 playerPos2D = Vector2.zero;
				playerPos2D.x = playerPositions[i].position.x;
				playerPos2D.y = playerPositions[i].position.y;

				float newDistance = Vector2.Distance(playerPos2D,centerPos2d);
				if (newDistance > largestDistance){
					largestDistance = newDistance;
				}
			}
		}

		if (!dontDoCameraThing){
			CameraFollowS.F.SetCamMult(largestDistance*sizeAddMult);
		}

	}

	public float GetXDiff () {

		float xDiff = transform.position.x-centerPt.transform.position.x;

		return(xDiff);

	}

	void CalculateHitPos () {
		hitCenter = Vector3.zero;
		if (hitPositions.Count > 0){

			for (int i = 0; i < hitPositions.Count-1; i++){
				hitCenter += hitPositions[i].position;
			}
			hitCenter/=hitPositions.Count;
			currentHitWeight = hitWeight;
		}
		else{
			currentHitWeight = 0;
		}
	}

}
