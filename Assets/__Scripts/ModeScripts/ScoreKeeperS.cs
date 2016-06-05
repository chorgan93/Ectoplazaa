using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeperS : MonoBehaviour {

	// keeps track of player scores and initializes scoreboard ui
	// currently only works with ecto mode, will need to be repurposed for multiple modes


	public GameObject 	scoreBarObj; 
	public GameObject	uiObj; 
	//private GameObject 	ghostballGoalParentPrefab;
						//ghostballPrefab;
				
	private int 		winningPlayerNum;
	public GameObject 	winningPlayerTail, winningPlayerSprite; 
	
	private int 		winningTeamNum;

	public Sprite [] 	playerHighResSprites; 

	public static int 			scoreThresholdCollectoplaza = 40;
	public static int 			scoreThresholdCollectoplazaTeam = 90;

	public static int 			numberLives = 4;
	public static int 			numberLivesTeam = 4;								//stock mode
	private int 		numPlayersLeft = 0; 					//stock mode
	public static int 			scorePerGoalGhostball = 1,
						scoreThresholdGhostball = 3,
						scoreThresholdGhostballTeam = 5;
	private bool []		playersPlaying = new bool[4]{false,false,false,false}; //Stock mode
	public static bool 	gameEnd = false;

	public GameObject 	endGameObj;
	public TextMesh 	winText;

	bool 				spawnedScoreboard = false;

	public static int lastPlayerToScore = 1;

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

	private List <GameObject> mapLayoutsForMode = new List<GameObject>();

	private int redTeamScore = 0;
	private int blueTeamScore = 0;

	private int redTeamScoreCalc = 0;
	private int blueTeamScoreCalc = 0;

	private int numPlayersRed = 0;
	private int numPlayersBlue = 0;

	private int numPlayersRedStart = 0;
	private int numPlayersBlueStart = 0;
	private int numRedPlayersOut;
	private int numBluePlayersOut;

	public Material teamWinRedMat;
	public Material teamWinBlueMat;

	private bool roundEnded = false;

	public StageLoaderS levelLoad;

	void Awake () 
	{
		levelLoad.LoadLevel();

		mapLayoutsForMode.Add(GameObject.FindGameObjectWithTag("CollectoplazaLayout"));
		mapLayoutsForMode.Add(GameObject.FindGameObjectWithTag("AfterlivesLayout"));
		mapLayoutsForMode.Add(GameObject.FindGameObjectWithTag("GhostballLayout"));

		endGameObj.SetActive(false);
		gameStarted = false;
		gameEnd = false;




		currentMode = CurrentModeS.currentMode;						//get what mode to use.


		startCountdown = countdownRateMax;

		foreach (GameObject layout in mapLayoutsForMode){
			layout.SetActive(false);
		}
		SetupLevel();


		if (CurrentModeS.isTeamMode){
			numPlayersRed = numPlayersRedStart = GlobalVars.NumPlayersRedTeam();
			numPlayersBlue = numPlayersBlueStart = GlobalVars.NumPlayersBlueTeam();
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
			SetupCharacters (); 
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
	}

	void RoundIntroSequence()
	{
		// do the thing.
		//When you're done, set this flag to true
		bNumRoundIntroComplete = true;
		bRoundIntroRunnning = false;
	}



	void SetupLevel()
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
	}

	void SetupCharacters(){
		
		
		switch (currentMode)										//Find parent object for desired mode and enable
		{
		case 0: // Ecto
			//enable Ecto Mode
			//modeObjectOwners[0].SetActive (true);
			scoreBarObj.GetComponent<ScoreBar>().scoreThreshold = scoreThresholdCollectoplaza; 
			//scoreBarObj.GetComponent<ScoreBar>().SpawnScoreboard(); 

			for (int i = 0; i < 4; i++) {						//Tell players how many lives they have
				if (GlobalVars.characterNumber [i] != 0) {
					PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
					if (CurrentModeS.isTeamMode){
						currentPlayer.numLives = numberLivesTeam;
					}
					else{
						currentPlayer.numLives = numberLives;
					}
				}
				
			}
			break;
			
		case 1: //Stock
			print("ScoreKeeper setting up stock mode");
			//scoreBarObj.GetComponent<ScoreBar>().SpawnScoreboard(); 
			
			
			for (int i = 0; i < 4; i++) {						//Tell players how many lives they have
				if (GlobalVars.characterNumber [i] != 0) {
					PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
					if (CurrentModeS.isTeamMode){
						currentPlayer.numLives = numberLivesTeam;
					}
					else{
						currentPlayer.numLives = numberLives;
					}
					print ("Set lives to " + numberLives);
					numPlayersLeft ++;								//Add to list of players remaining
					playersPlaying[i] = true;
				}
				
			}
			
			
			break;
			
		case 2: //Ghostball

			//Make Scorebar
			scoreBarObj.GetComponent<ScoreBar>().scoreThreshold = scoreThresholdGhostball; 
			//scoreBarObj.GetComponent<ScoreBar>().SpawnScoreboard(); 

			for (int i = 0; i < 4; i++) {						//Tell players how many lives they have
				if (GlobalVars.characterNumber [i] != 0) {
					PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
					currentPlayer.numLives = numberLives;
				}
				
			}

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

			if (!CurrentModeS.isTeamMode){
			
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
			else{
				//  in team mode
				if ( !hasMoreLives )																//If Eliminated...
				{			
					if (GlobalVars.teamNumber[p.playerNum-1] == 1){
						numPlayersRed--;					
					}
					else{
						numPlayersBlue--;
					}

					//Decrement counter
					playersPlaying[p.playerNum-1] = false;											//Update list of remaining players
					print ("Player " + p.playerNum + " died!");
					if (numPlayersRed  == 0)														//If only one player remains left
					{
						// BLUE TEAM WINS
						winningTeamNum = 2;
						
						SpawnRoundEndScreen();															//Spawn  End Screen
					}
					else if (numPlayersBlue  == 0)														//If only one player remains left
					{
						// RED TEAM WINS
						winningTeamNum = 1;
						
						SpawnRoundEndScreen();															//Spawn  End Screen
					}
					else{
						print ("Number of players left: " + numPlayersLeft);
					}

				}
			}
			
			
		}
	}
	
	void UpdateScoreboard()
	{

		// non team ver
		if (!CurrentModeS.isTeamMode){

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
		// team mode ver
		else{
			numRedPlayersOut = 0;
			numBluePlayersOut = 0;
			
			redTeamScoreCalc = 0;
			blueTeamScoreCalc = 0;
			
			for (int i = 0; i < 4; i++) {


				if (GlobalVars.characterNumber [i] != 0) {
					PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
					
					// check for out player condition
					if (currentPlayer.numLives == 0){
						if (GlobalVars.teamNumber[i] == 1){
							numRedPlayersOut++;
						}
						else{
							numBluePlayersOut++;
						}
					}
					else{
						winningPlayerNum = i+1;
					}
					
					// Endgame for Collectoplaza mode
					if(currentMode == 0)
					{
						// calculate score on frame
						if (GlobalVars.teamNumber[i] == 1){
							redTeamScoreCalc += currentPlayer.score;

							if (redTeamScoreCalc >= scoreThresholdCollectoplazaTeam){
								
								gameEnd = true;
								winningTeamNum = 1;
								winningPlayerNum = lastPlayerToScore;
								
								SpawnRoundEndScreen();
								
							}
						}
						else{
							blueTeamScoreCalc += currentPlayer.score;

							if (blueTeamScoreCalc >= scoreThresholdCollectoplazaTeam){
								
								gameEnd = true;
								winningTeamNum = 2;
								winningPlayerNum = lastPlayerToScore;
								
								SpawnRoundEndScreen();
								
							}
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
						// calculate score on frame
						if (GlobalVars.teamNumber[i] == 1){
							redTeamScoreCalc = redTeamScore;
							
							if (redTeamScoreCalc >= scoreThresholdGhostballTeam){
								
								gameEnd = true;
								winningTeamNum = 1;
								winningPlayerNum = lastPlayerToScore;
								
								SpawnRoundEndScreen();
								
							}
						}
						else{
							blueTeamScoreCalc  = blueTeamScore;
							
							if (blueTeamScoreCalc >= scoreThresholdGhostballTeam){
								
								gameEnd = true;
								winningTeamNum = 2;
								winningPlayerNum = lastPlayerToScore;
								
								SpawnRoundEndScreen();
								
							}
						}
					}
				} 
				
				// if only one team remains, they win
				if (numRedPlayersOut == numPlayersRedStart){
					// blue team win
					gameEnd = true;
					winningTeamNum = 2;
					SpawnRoundEndScreen();
				}
				if (numBluePlayersOut == numPlayersBlueStart){
					// red team win
					gameEnd = true;
					winningTeamNum = 1;
					SpawnRoundEndScreen();
				}

			}

			if (redTeamScoreCalc != redTeamScore){
				redTeamScore = redTeamScoreCalc;
			}
			if (blueTeamScoreCalc != blueTeamScore){
				blueTeamScore = blueTeamScoreCalc;
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
		if (!roundEnded){
		//Do some coroutine to spawn the UI stuffs?
		//Check number of rounds
		if (CurrentModeS.isTeamMode){
			CurrentModeS.AddToRoundsCompletedTeam( winningTeamNum);
		}
		else{
			CurrentModeS.AddToRoundsCompleted( winningPlayerNum - 1);
		}
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
			roundEnded = true;
		}
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

		// for non-team win settings
		int characterNum = 0;
		characterNum = GlobalVars.playerList [winningPlayerNum - 1].GetComponent<PlayerS> ().characterNum-1;


		scoreBarObj.SetActive (false); 

		endGameObj.SetActive (true);

		CameraShakeS.C.LargeShake();
		CameraShakeS.C.HalfTimeSleep(1.5f);

		Vector3 newPos = endGameObj.transform.position; 
		newPos.x = Camera.main.ViewportToWorldPoint (new Vector3 (1f, .5f, 0f)).x + 50f;

		endGameObj.transform.position = newPos; 

		if (CurrentModeS.isTeamMode){

			//winningPlayerSprite.SetActive(false);

			// red team win
			if (winningTeamNum == 1){
				endFlash.GetComponent<Renderer>().material.color = Color.red;
				winningPlayerSprite.GetComponent<SpriteRenderer>().material = teamWinRedMat;
				Debug.Log(CurrentModeS.DoAnotherRound());
				if (CurrentModeS.DoAnotherRound()){
					winningPlayerSprite.gameObject.SetActive(false);
					winningPlayerTail.gameObject.SetActive(false);
					CameraFollowS.F.StartSpecialCam(GlobalVars.playerList[winningPlayerNum-1], 3f);
					winText.text = "RED TEAM\nwins Round " + CurrentModeS.GetRoundsCurrent() + "!";
				}
				else{
					winText.text = "RED TEAM\nwins!";
				}
			}
			// blue team win
			else{
				endFlash.GetComponent<Renderer>().material.color = Color.blue;
				winningPlayerSprite.GetComponent<SpriteRenderer>().material = teamWinBlueMat;
				if (CurrentModeS.DoAnotherRound()){
					winningPlayerSprite.gameObject.SetActive(false);
					winningPlayerTail.gameObject.SetActive(false);
					CameraFollowS.F.StartSpecialCam(GlobalVars.playerList[winningPlayerNum-1], 3f);
					winText.text = "BLUE TEAM\nwins Round " + CurrentModeS.GetRoundsCurrent() + "!";
				}
				else{
					winText.text = "BLUE TEAM\nwins!";
				}
			}

			winningPlayerSprite.GetComponent<SpriteRenderer> ().sprite = playerHighResSprites [characterNum];
			winningPlayerTail.GetComponent<Renderer>().material = winningPlayerSprite.GetComponent<SpriteRenderer>().material;

			
			GlobalVars.playerList[winningPlayerNum-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();
			GlobalVars.lastWinningPlayer = winningPlayerNum;
		}
		else{
		GlobalVars.playerList[winningPlayerNum-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();

		GlobalVars.lastWinningPlayer = winningPlayerNum;

		endFlash.GetComponent<Renderer>().material.color = GlobalVars.playerList 
			[winningPlayerNum - 1].GetComponent<PlayerS> ().playerParticleMats [characterNum].GetColor("_TintColor"); 


		if (CurrentModeS.DoAnotherRound()){
				
				winningPlayerSprite.gameObject.SetActive(false);
				winningPlayerTail.gameObject.SetActive(false);
				CameraFollowS.F.StartSpecialCam(GlobalVars.playerList[winningPlayerNum-1], 3f);
			winText.text = "P" + winningPlayerNum + "\nwins Round " + CurrentModeS.GetRoundsCurrent() + "!";
		}
		else{
			winText.text = "P" + winningPlayerNum + "\nwins!";
		}

		winningPlayerSprite.GetComponent<SpriteRenderer> ().sprite = playerHighResSprites [characterNum];
		winningPlayerTail.GetComponent<Renderer> ().material = GlobalVars.playerList [winningPlayerNum - 1].GetComponent<PlayerS> ().playerMats [characterNum]; 
		}
	
		SpawnEndText(); 

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
				CurrentModeS.ResetWinRecord();
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
		if (playerNumber > 0){
		if (GlobalVars.playerList.Length>=playerNumber)	//check if active
		{
			PlayerS p = GlobalVars.playerList[playerNumber-1].GetComponent<PlayerS>();	//Get Player
			p.score += scorePerGoalGhostball;				//Add score
		}
		}
	}

	public void AddPointsTeam(int teamNumber, int playerNumber)						//Used by Ghostball Goal
	{	//Should probably be generalized - depending on the mode...?

		if (teamNumber == 1){
			redTeamScore += scorePerGoalGhostball;
		}
		if (teamNumber == 2){
			blueTeamScore += scorePerGoalGhostball;
		}

		lastPlayerToScore = playerNumber;
	}

	public int GetWinningPlayer(){
		return winningPlayerNum;
	}

	public  float GetRedScore(){

		return redTeamScore;

	}

	public  float GetBlueScore(){
		
		return blueTeamScore;
		
	}
}
