using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewGhostballGoalS : MonoBehaviour {

	private ScoreKeeperS 			scoreKeeper;
	private GhostballS				ghostBall;

	public GameObject sfxObj;

	public bool goalScored = false;

	// Use this for initialization
	void Start () 
	{
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeperS>() as ScoreKeeperS;
		ghostBall = GameObject.FindObjectOfType<GhostballS>() as GhostballS;
	}

	void OnTriggerEnter(Collider c)
	{
		if (c == ghostBall.GetComponent<Collider>())
		{
			if (!CurrentModeS.isTeamMode){
				AddScore(ghostBall.GetCurrentPlayer());
			}
			else{
				AddScoreTeam(ghostBall.GetCurrentTeam());
			}
			ghostBall.MakeSlashEffect(transform.position);
			goalScored = true;

			ghostBall.PlayHitSound();

			Instantiate(sfxObj);
		}
	}
	

	void AddScore(int playerToScore){

		scoreKeeper.AddPoints(playerToScore);

	}
	void AddScoreTeam(int teamToScore){
		scoreKeeper.AddPointsTeam(teamToScore);
	}
	public void Disable(){

		this.gameObject.SetActive(false);
	}
}
