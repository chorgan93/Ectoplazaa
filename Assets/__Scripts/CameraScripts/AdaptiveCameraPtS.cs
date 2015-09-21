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

	Vector3 playerCenterPos;

	float camMult;

	
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	
	public float xConstraintSizeAdjustMult = 0.1f;
	public float yConstraintSizeAdjustMult = 0.05f;

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
	void FixedUpdate () {

		// create var for changing pos
		Vector3 adaptPt = centerPt.transform.position;

		CalculateHitPos();

		// create player centerpt

		if (playerPositions.Count > 0){
			playerCenterPos = Vector3.zero;
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

		if (adaptPt.x < minX+camMult*xConstraintSizeAdjustMult){
			adaptPt.x = minX;
		}
		if (adaptPt.x > maxX-camMult*xConstraintSizeAdjustMult){
			adaptPt.x = maxX;
		}
		
		if (adaptPt.y < minY+camMult*yConstraintSizeAdjustMult){
			adaptPt.y = minY;
		}
		if (adaptPt.y > maxY-camMult*yConstraintSizeAdjustMult){
			adaptPt.y = maxY;
		}
		if (!ScoreKeeperS.gameStarted){
			adaptPt = centerPt.transform.position;
		}

		transform.position = adaptPt;

		
		
		SetCamMult();


	
	}

	void SetCamMult () {

		largestDistance = 0;
		Vector2 centerPos2d = Vector2.zero;
		centerPos2d.x = centerPt.position.x;
		centerPos2d.y = centerPt.position.y;

		Vector2 playerCenter2d = Vector2.zero;
		playerCenter2d.x = playerCenterPos.x;
		playerCenter2d.y = playerCenterPos.y;

		if (playerPositions.Count > 0){
			for (int i = 0; i < playerPositions.Count; i++){
				Vector2 playerPos2D = Vector2.zero;
				playerPos2D.x = playerPositions[i].position.x;
				playerPos2D.y = playerPositions[i].position.y;

				float newDistance = Vector2.Distance(playerPos2D,playerCenter2d);
				if (newDistance > largestDistance){
					largestDistance = newDistance;
				}
			}
		}

		if (!dontDoCameraThing){
			CameraFollowS.F.SetCamMult(largestDistance*sizeAddMult);
			camMult = largestDistance*sizeAddMult;
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
