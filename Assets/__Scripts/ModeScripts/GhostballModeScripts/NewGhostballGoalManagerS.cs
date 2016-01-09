using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewGhostballGoalManagerS : MonoBehaviour {

	// Use this for initialization

	public List<BallRespawnPosS> ballPositions; // position 0 will be start position
	private NewGhostballGoalS goal;
	private GhostballS ghostBall;
	

	void Start () {
		goal = GetComponentInChildren<NewGhostballGoalS>();
		ghostBall = GetComponentInChildren<GhostballS>();
		ghostBall.transform.position = ballPositions[0].transform.position;

	}



	// Update is called once per frame
	void FixedUpdate () {
	
		if (goal.goalScored){
			//Reset ball
			ghostBall.ResetBall(ballPositions[GetFreeBallRespawn()].transform.position);
			goal.goalScored =false;
		}

	}

	private int GetFreeBallRespawn(){

		List<int> availablePos = new List<int>();

		foreach (BallRespawnPosS ballPos in ballPositions){

			if (ballPos.SpotIsFree()){
				availablePos.Add(ballPositions.IndexOf(ballPos));
			}

		}

		if (availablePos.Count > 0){

			int chosenPos = Mathf.FloorToInt(Random.Range(0, availablePos.Count));
			return(availablePos[chosenPos]);

		}
		else{
			return 0;
		}

	}




}
