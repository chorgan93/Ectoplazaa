using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdaptiveCameraPtS : MonoBehaviour {

	public List<Transform> playerPositions;
	public List<PlayerS> playerRefs;
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

	private GameObject ghostBall;
	private float ghostBallWeight = 0.25f;

	
	public float xConstraintSizeAdjustMult = 0.1f;
	public float yConstraintSizeAdjustMult = 0.05f;

	public bool snapCamera = false;

	void Awake () {
		A = this;
	}

	void Start () {
		playerPositions.Clear();
		if (CurrentModeS.currentMode == 2){
		ghostBall = GameObject.Find("NewGhostball");
		}

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < players.Length; i++){
			playerPositions.Add(players[i].transform);
			playerRefs.Add(players[i].GetComponent<PlayerS>());
		}

	}


	
	// Update is called once per frame
	void FixedUpdate () {

		// create var for changing pos
		Vector3 adaptPt = centerPt.transform.position;

		CalculateHitPos();

		// create player centerpt

		float minXPlayer = 0;
		float maxXPlayer = 0;
		float minYPlayer = 0;
		float maxYPlayer = 0;

		if (playerPositions.Count > 0){


			playerCenterPos = Vector3.zero;
			for (int i = 0; i < playerPositions.Count; i++){

				if (i == 0){
					minXPlayer = maxXPlayer = playerPositions[i].position.x;
					minYPlayer = maxYPlayer = playerPositions[i].position.y;
				}
				else{
					if (playerPositions[i].position.x < minXPlayer){
						minXPlayer = playerPositions[i].position.x;
					}
					if (playerPositions[i].position.y < minYPlayer){
						minYPlayer = playerPositions[i].position.y;
					}
					if (playerPositions[i].position.x > maxXPlayer){
						maxXPlayer = playerPositions[i].position.x;
					}
					if (playerPositions[i].position.y > maxYPlayer){
						maxYPlayer = playerPositions[i].position.y;
					}
				}


			}

			// factor in ball in ball mode
			if (CurrentModeS.currentMode == 2){
				if (ghostBall.transform.position.x < minXPlayer){
					minXPlayer = ghostBall.transform.position.x;
				}
				if (ghostBall.transform.position.y < minYPlayer){
					minYPlayer = ghostBall.transform.position.y;
				}
				if (ghostBall.transform.position.x > maxXPlayer){
					maxXPlayer = ghostBall.transform.position.x;
				}
				if (ghostBall.transform.position.y > maxYPlayer){
					maxYPlayer = ghostBall.transform.position.y;
				}
			}

			playerCenterPos.x = (minXPlayer + maxXPlayer)/2f;
			playerCenterPos.y = (minYPlayer + maxYPlayer)/2f;

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

	void LateUpdate () {

		// remove players who are out of lives or out of the game
			for (int i = 0; i < playerRefs.Count; i++){
				if (playerRefs[i].numLives == 0){
					playerRefs.RemoveAt(i);
					playerPositions.RemoveAt(i);
				}
			}


	}

	void SetCamMult () {

		largestDistance = 0;

		float currentDistance = 0;

		Vector2 playerPos2d = Vector2.zero;
		Vector2 checkPos2d = Vector2.zero;

		foreach (Transform player in playerPositions){
			playerPos2d.x = player.position.x;
			playerPos2d.y = player.position.y;

			for (int i = 0; i < playerPositions.Count; i++){
				if (playerPositions[i] != player){
					checkPos2d.x = playerPositions[i].transform.position.x;
					checkPos2d.y = playerPositions[i].transform.position.y;

					currentDistance = Vector2.Distance(playerPos2d, checkPos2d);
					if (currentDistance > largestDistance){
						largestDistance = currentDistance;
					}
				}
			}

			// if ball mode, add ball into equation
			if (CurrentModeS.currentMode ==2){
			checkPos2d.x = ghostBall.transform.position.x;
			checkPos2d.y = ghostBall.transform.position.y;
			}

			currentDistance = Vector2.Distance(playerPos2d, checkPos2d);
			if (currentDistance > largestDistance){
				largestDistance = currentDistance;
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
