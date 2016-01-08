﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeperS : MonoBehaviour {

	// keeps track of player scores and initializes scoreboard ui
	// currently only works with ecto mode, will need to be repurposed for multiple modes


	public GameObject 	scoreBarObj; 
	public GameObject	uiObj; 
	public GameObject 	ghostballGoalParentPrefab,
						ghostballPrefab;
				
	private int 		winningPlayerNum;
	public GameObject 	winningPlayerTail, winningPlayerSprite; 

	public Sprite [] 	playerHighResSprites; 

	public int 			scoreThresholdCollectoplaza;

	private int 			numberLives = 7;								//stock mode
	private int 		numPlayersLeft = 0; 					//stock mode
	public int 			scorePerGoalGhostball = 1,
						scoreThresholdGhostball = 10;
	private bool []		playersPlaying = new bool[4]{false,false,false,false}; //Stock mode
	public static bool 	gameEnd = false;

	public GameObject 	endGameObj;
	public TextMesh 	winText;

	bool 				spawnedScoreboard = false;

	public static bool 	gameStarted = false;

	public GameObject 	timer3;
	public GameObject 	timer2;
	public GameObject 	timer1;
	public GameObject 	goText;
	private int 		currentText = 4;

	public GameObject 	statText1, statText2, statText3, statText4; 

	private float 		countdownRateMax = .5f;
	private float 		startCountdown;

	public GameObject 	endFlash;

	//public GameObject endGameSound;

	// don't allow exit of game until this time is up
	public float 		gameEndMinTime = 2f;

	float 				cameraSizeSmall = 20f;

	Vector3 			worldPos = Vector3.zero;

	private int 		currentMode =-1;
	private int 		numberRounds;
	private bool		bNumRoundIntroComplete = false;
	private bool 		bRoundIntroRunnning = false;

	// for "out special" check
	private int playersOut;
	private int lastRemaining;

	public List <GameObject> mapLayoutsForMode;

	void Start () 
	{
		endGameObj.SetActive(false);
		gameStarted = false;
		gameEnd = false;




		currentMode = CurrentModeS.currentMode;						//get what mode to use.


		//SetupMode();
		
		startCountdown = countdownRateMax;

		foreach (GameObject layout in mapLayoutsForMode){
			layout.SetActive(false);
		}
	}



	
	// Update is called once per frame
	void Update () {
		 if(!bNumRoundIntroComplete)
		{
			if(!bRoundIntroRunnning)
			{
				//Setup intro sequence
				SetupRoundIntroSequence();
				//Call Intro sequence
				RoundIntroSequence();
				bRoundIntroRunnning = true;
			}

		}
		if (!spawnedScoreboard)
		{
			SetupMode (); 
		}


		if(gameEnd)
		{
			UpdateEndScreen();
		}
		else
		{
			UpdateScoreboard(); 
		}

		RepositionScoreboard(); 

		// start game stuff
		if (!gameStarted){
			startCountdown -= Time.deltaTime;
			if (startCountdown <= 0){
				currentText --;
				startCountdown = countdownRateMax;

				if (currentText == 0){
					gameStarted = true;
					goText.SetActive(true);
					CameraShakeS.C.SmallShake();
				}
				if (currentText == 1){
					timer1.SetActive(true);
				}
				if (currentText == 2){
					timer2.SetActive(true);
				}
				if (currentText == 3){
					timer3.SetActive(true);
				}
			}
		}

	}
	void SetupRoundIntroSequence()
	{
		numberRounds = CurrentModeS.GetNumberRounds();
		print ("Number of rounds: " + numberRounds);
	}

	void RoundIntroSequence()
	{
		// do the thing.
		//When you're done, set this flag to true
		bNumRoundIntroComplete = true;
		bRoundIntroRunnning = false;
	}



	void SetupMode()
	{
		currentMode = CurrentModeS.currentMode;
		
		//Begin by disabling all (for safety);
		/*		for (int i = 0; i < modeObjectOwners.Count-1; i++)
		{
			modeObjectOwners[i].SetActive(false);
		}
*/		
		// turn on mode stage layout
		mapLayoutsForMode[currentMode].SetActive(true);
		
		
		switch (currentMode)										//Find parent object for desired mode and enable
		{
		case 0: // Ecto
			//enable Ecto Mode
			//modeObjectOwners[0].SetActive (true);
			scoreBarObj.GetComponent<ScoreBar>().scoreThreshold = scoreThresholdCollectoplaza; 
			scoreBarObj.GetComponent<ScoreBar>().SpawnScoreboard(); 
			break;
			
		case 1: //Stock
			print("ScoreKeeper setting up stock mode");
			scoreBarObj.GetComponent<ScoreBar>().SpawnScoreboard(); 
			
			
			for (int i = 0; i < 4; i++) {						//Tell players how many lives they have
				if (GlobalVars.characterNumber [i] != 0) {
					PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
					currentPlayer.numLives = numberLives;
					print ("Set lives to " + numberLives);
					numPlayersLeft ++;								//Add to list of players remaining
					playersPlaying[i] = true;
				}
				
			}
			
			
			break;
			
		case 2: //Ghostball

			//Make Scorebar
			scoreBarObj.GetComponent<ScoreBar>().scoreThreshold = scoreThresholdGhostball; 
			scoreBarObj.GetComponent<ScoreBar>().SpawnScoreboard(); 

			//Instantiate Goal  objects
			Instantiate(ghostballGoalParentPrefab, Vector3.zero, Quaternion.identity);

			//Instantiate Ghostball
			Instantiate(ghostballPrefab, Vector3.zero, Quaternion.identity);

			break;
			
		case 3:
			break;
			
		case -1:
			print (" CurrMode = -1");
			break;
			
		default:
			print("Default = " + currentMode);
			
			break;
		}
		
		
		
		spawnedScoreboard = true; 
		//Have mode owners set up everything?	
		
	}

	void SpawnScoreboard()
	{

		//Spawn depending on which mode is active
		if(currentMode == 0) //Collectoplaza mode
		{

			print ("(ScoreKeeperS)Spawning Scoreboard for Collectoplaza mode");
		}
		else if(currentMode == 1) //Stock Mode
		{
			print ("(ScoreKeeperS)Spawning Scoreboard for Stock mode");
		}
		else if(currentMode == 2) //Ghostball Mode
		{
			print ("(ScoreKeeperS)Spawning Scoreboard for Ghostball mode");
		}

		spawnedScoreboard = true; 
		//print ("trying to set up scoreboard"); 
	}

	public void PlayerDied(PlayerS p, bool hasMoreLives)
	{
		
		
		if(currentMode == 1)
		{	
			
			scoreBarObj.GetComponent<ScoreBar>().UpdateScoreboardStockMode(p);					//Update Scorebar
			
			if ( !hasMoreLives )																//If Eliminated...
			{										
				numPlayersLeft--;																//Decrement counter
				playersPlaying[p.playerNum-1] = false;											//Update list of remaining players
				print ("Player " + p.playerNum + " died!");
				if (numPlayersLeft  == 1)														//If only one player remains left
				{
					for(int i = 0; i < 4 ; i ++)												//Find Winner
					{
						if (playersPlaying[i] == true)
							winningPlayerNum = i + 1;											//Record Winning Player Number
					}

					SpawnRoundEndScreen();															//Spawn  End Screen
				}
				else{
					print ("Number of players left: " + numPlayersLeft);
				}
				
				
				//end the game
				//SpawnEndScreen();
			}
			else
			{
				//Update Scorebar ??
				
			}
			
			
		}
	}
	
	void UpdateScoreboard()
	{

		playersOut = 0;

		for (int i = 0; i < 4; i++) {
			if (GlobalVars.characterNumber [i] != 0) {
				PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();

				// check for out player condition
				if (currentPlayer.numLives == 0){
					playersOut++;
				}
				else{
					lastRemaining = i+1;
				}
				
				// Endgame for Collectoplaza mode
				if(currentMode == 0)
				{
					if (currentPlayer.score >= scoreThresholdCollectoplaza){
						
						gameEnd = true;
						winningPlayerNum = i + 1;
						
						SpawnRoundEndScreen();
						
					}
				}
				// Stock Mode
				else if(currentMode == 1)
				{
					//if player loses life
				}
				//Ghostball Mode
				else if(currentMode == 2)
				{
					if (currentPlayer.score >= scoreThresholdGhostball ) {
						gameEnd = true;
						winningPlayerNum = i + 1;
						SpawnRoundEndScreen();
						
					}
				}
			} 
			else{
				playersOut++;
			}

			// if only one player remains, they win
			if (playersOut == 3){
				gameEnd = true;
				winningPlayerNum = lastRemaining;
				SpawnRoundEndScreen();
			}
		}
	}

	void RepositionScoreboard()
	{
		worldPos = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 1f, 0f));
		//print("worldPos: " + worldPos.x + " " + worldPos.y + " " + worldPos.z); 	
		
		Vector3 newPos = new Vector3 (worldPos.x, worldPos.y -0.0f, 0f); 
		uiObj.transform.position = newPos; 

		float cameraScale = Camera.main.orthographicSize / cameraSizeSmall;

		Vector3 newScale = new Vector3 (1f * cameraScale, 1f * cameraScale, 1f); 

		uiObj.transform.localScale = newScale; 
	}

	void SpawnRoundEndScreen()
	{
		//Do some coroutine to spawn the UI stuffs?
		//Check number of rounds
		CurrentModeS.AddToRoundsCompleted( winningPlayerNum - 1);
		/*if(CurrentModeS.DoAnotherRound() == true)
		{
			//Replay
			print("Restarting level");
			//HOTFIX FOR TIME ISSUE WITH CAMERASHAKE.C.HALFTIMESLEEP
			//Time.timeScale = 1;
			StartCoroutine("RoundEndScreen");
		
		}
		else
		{

			SpawnEndScreen();
		}*/

		SpawnEndScreen();
	}
	IEnumerator RoundEndScreen()
	{

	
		yield return new WaitForSeconds(3);
		yield return StartCoroutine("CheckEndButtonPress");
		Application.LoadLevel(Application.loadedLevel);
		
	}

	IEnumerator CheckEndButtonPress()
	{
		bool foo = true;
		float fooTimer =0;
		while(foo)
		{
			//NOT WORKING YET
			if (Input.GetButton ("AButtonAllPlayers" + PlatformS.GetPlatform ())) 
			{
				break;
			}
			else 
				fooTimer += Time.deltaTime;

			if(fooTimer > 10)
				break;
		}
		yield return new WaitForEndOfFrame();
	}

	void SpawnEndScreen()
	{
		gameEnd = true;
		
		int characterNum = GlobalVars.playerList [winningPlayerNum - 1].GetComponent<PlayerS> ().characterNum-1;

		scoreBarObj.SetActive (false); 

		endGameObj.SetActive (true);

		CameraShakeS.C.LargeShake();
		CameraShakeS.C.HalfTimeSleep(1.5f);

		Vector3 newPos = endGameObj.transform.position; 
		newPos.x = Camera.main.ViewportToWorldPoint (new Vector3 (1f, .5f, 0f)).x + 50f;

		endGameObj.transform.position = newPos; 

		GlobalVars.playerList[winningPlayerNum-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();

		GlobalVars.lastWinningPlayer = winningPlayerNum;

		endFlash.GetComponent<Renderer>().material.color = GlobalVars.playerList 
			[winningPlayerNum - 1].GetComponent<PlayerS> ().playerParticleMats [characterNum].GetColor("_TintColor"); 


		if (CurrentModeS.DoAnotherRound()){
			winText.text = "P" + winningPlayerNum + "\nwins Round " + CurrentModeS.GetRoundsCurrent() + "!";
		}
		else{
			winText.text = "P" + winningPlayerNum + "\nwins!";
		}

		winningPlayerSprite.GetComponent<SpriteRenderer> ().sprite = playerHighResSprites [characterNum];
		winningPlayerTail.GetComponent<Renderer> ().material = GlobalVars.playerList [winningPlayerNum - 1].GetComponent<PlayerS> ().playerMats [characterNum]; 
	
		SpawnEndText(); 

		foreach (GameObject p in GlobalVars.playerList) {

			if(p != null)
			{
				p.SetActive (false); 
				//GameObject.Destroy(p.gameObject); 
			}

		}

	}

	void SpawnEndText()
	{
		int maxVal = 0; 
		int maxIndex = 0; 
	
		for(int i = 0; i < 4; i++)
		{
			if(GlobalVars.totalGlobsEaten[i] > maxVal)
			{
				maxVal = GlobalVars.totalGlobsEaten[i]; 
				maxIndex = i; 
			}
		}

		statText1.GetComponent<TextMesh>().text = "P" + (maxIndex+1) + " had a huge appetite"; 

		maxVal = 0;
		maxIndex = 0; 

		for(int i = 0; i < 4; i++)
		{
			if(GlobalVars.totalDeaths[i] > maxVal)
			{
				maxVal = GlobalVars.totalDeaths[i]; 
				maxIndex = i; 
			}
		}
		
		statText2.GetComponent<TextMesh>().text = "P" + (maxIndex+1) + " was eviscerated most"; 
		
		maxVal = 0;
		maxIndex = 0; 

		for(int i = 0; i < 4; i++)
		{
			if(GlobalVars.totalFlings[i] > maxVal)
			{
				maxVal = GlobalVars.totalFlings[i]; 
				maxIndex = i; 
			}
		}
		
		statText3.GetComponent<TextMesh>().text = "P" + (maxIndex+1) + " fling king"; 
		
		maxVal = 0;
		maxIndex = 0; 

		for(int i = 0; i < 4; i++)
		{
			if(GlobalVars.totalKills[i] > maxVal)
			{
				maxVal = GlobalVars.totalKills[i]; 
				maxIndex = i; 
			}
		}
		
		statText4.GetComponent<TextMesh>().text = "P" + (maxIndex+1) + " at large"; 
		
		maxVal = 0;
		maxIndex = 0; 

		GlobalVars.ResetGameStats(); 
	}
	
	void UpdateEndScreen()
	{
	

		// once gameEnd is on, 
		gameEndMinTime -= Time.deltaTime;

		Vector3 newPos = endGameObj.transform.position; 
		newPos.x = Camera.main.ViewportToWorldPoint (new Vector3 (0.0f, .5f, 0f)).x ;

		endGameObj.transform.position = Vector3.Lerp (endGameObj.transform.position, newPos, 0.2f
		                                              *Time.deltaTime*TimeManagerS.timeMult*50f); 

		if (Input.GetButton ("BButtonAllPlayers"+	PlatformS.GetPlatform ()) && gameEndMinTime < 0) 
		{
			gameEnd = false;
			// if rounds left, reset
			if (CurrentModeS.DoAnotherRound()){
				Application.LoadLevel(Application.loadedLevelName);
			}
			else{
			// hard coding in return to map select
				Application.LoadLevel("2MapSelect");
			}
		}
		if (Input.GetButton ("AButtonAllPlayers" + PlatformS.GetPlatform ()) && gameEndMinTime < 0) {

			if (CurrentModeS.DoAnotherRound()){
				Application.LoadLevel(Application.loadedLevelName);
			}
			else{
			print("RESETTINGLEVEL"); 
			//GlobalVars.ResetVariables(); 
			CurrentModeS.ResetWinRecord();
			Application.LoadLevel(Application.loadedLevel);
			}
		}

	}

	public void AddPoints(int playerNumber)						//Used by Ghostball Goal
	{	//Should probably be generalized - depending on the mode...?
		if (GlobalVars.characterIsPlaying[playerNumber])	//check if active
		{
			PlayerS p = GlobalVars.playerList[playerNumber].GetComponent<PlayerS>();	//Get Player
			p.score += scorePerGoalGhostball;				//Add score
		}
	}
}
