using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewGhostballGoalS : MonoBehaviour {

	private ScoreKeeperS 			scoreKeeper;
	private GhostballS				ghostBall;

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
			AddScore(ghostBall.GetCurrentPlayer());
			ghostBall.MakeSlashEffect(transform.position);
			goalScored = true;
		}
	}
	

	void AddScore(int playerToScore){

		scoreKeeper.AddPoints(playerToScore);

	}
	public void Disable(){

		this.gameObject.SetActive(false);
	}
}
