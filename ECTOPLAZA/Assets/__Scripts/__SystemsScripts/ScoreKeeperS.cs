﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeperS : MonoBehaviour {


	public GameObject scoreBarPrefab; 
	public GameObject uiObj; 
	private int winningPlayerNum;

	public int scoreThreshold;

	public static bool gameEnd = false;

	public GameObject endGameObj;
	public TextMesh  winText;

	bool scoreboardExists= false;

	// don't allow exit of game until this time is up
	public float gameEndMinTime = 2f;

	float cameraSizeSmall = 20f;

	Vector3 worldPos = Vector3.zero;

	void Start () {
		endGameObj.SetActive(false);

	}

	
	// Update is called once per frame
	void Update () {

		if(!scoreboardExists)
			SpawnScoreboard (); 

		
		if(gameEnd)
		{
			UpdateEndScreen();
		}
		else
		{
			UpdateScoreboard(); 
			RepositionScoreboard(); 
		}
	}


	void UpdateScoreboard()
	{

		for (int i = 0; i < 4; i++) {
			if (GlobalVars.characterNumber [i] != 0) {
				PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
				
				if (currentPlayer.health >= scoreThreshold) {

					gameEnd = true;
					winningPlayerNum = i + 1;

					SpawnEndScreen();
				
				}
			} 
		}
	}

	void SpawnScoreboard()
	{
		scoreboardExists = true; 
		print ("trying to spawn scoreboard"); 
		float totalPlayers = 0; 
		for (int i = 0; i < 4; i++) {
			if (GlobalVars.characterNumber [i] != 0) {

				totalPlayers++; 

				PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();

				Vector3 spawnPos = new Vector3(0f,19f - (2f*totalPlayers), 0f); 

				GameObject newScore = Instantiate(scoreBarPrefab,spawnPos,Quaternion.identity) as GameObject; 
				newScore.GetComponent<ScoreBar>().scoreThreshold = scoreThreshold; 
				newScore.GetComponent<ScoreBar>().playerNum = currentPlayer.playerNum;
				newScore.GetComponent<ScoreBar>().scoreNumber = totalPlayers; 
				newScore.transform.parent = uiObj.transform; 


			} 
		}
	}

	void RepositionScoreboard()
	{
		worldPos = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 1f, 0f));
		print("worldPos: " + worldPos.x + " " + worldPos.y + " " + worldPos.z); 	
		
		Vector3 newPos = new Vector3 (worldPos.x, worldPos.y -2.5f, 0f); 
		uiObj.transform.position = newPos; 

		float cameraScale = Camera.main.orthographicSize / cameraSizeSmall;

		Vector3 newScale = new Vector3 (1f * cameraScale, 1f * cameraScale, 1f); 

		uiObj.transform.localScale = newScale; 
	}
	
	void SpawnEndScreen()
	{
		endGameObj.SetActive (true);
		winText.text = "Player " + winningPlayerNum + " wins!!";
	}
	
	void UpdateEndScreen()
	{

		// once gameEnd is on, start countdown
		// restart map on countdown (replace this once menu infrastructure is in place

		gameEndMinTime -= Time.deltaTime;
		
		if (Input.GetButton ("StartButtonAllPlayers"+	PlatformS.GetPlatform ()) && gameEndMinTime < 0) 
		{
			gameEnd = false;
			// hard coding in return to character select
			Application.LoadLevel("3CharacterSelect");
		}
	}
}
